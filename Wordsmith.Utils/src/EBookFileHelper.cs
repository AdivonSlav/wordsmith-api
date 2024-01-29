using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using VersOne.Epub;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;

namespace Wordsmith.Utils;

public static class EBookFileHelper
{
    private static string _savePath;

    public static void Init(string? savePath)
    {
        if (string.IsNullOrWhiteSpace(savePath))
        {
            throw new Exception("Not all eBook settings are set!");
        }

        if (!Directory.Exists(savePath))
        {
            throw new Exception("The path for saving eBooks does not exist!");
        }
        
        _savePath = savePath;
    }

    public static async Task<EBookParseDto> ParseEpub(IFormFile file)
    {
        EpubBook epub;

        try
        {
            await using var stream = file.OpenReadStream();
            epub = await EpubReader.ReadBookAsync(stream);
        }
        catch (Exception e)
        {
            throw new AppException($"Could not read EPUB {e.Message}");
        }
        
        var ebookData = new EBookParseDto()
        {
            Title = StripHtml(epub.Title),
            Description = StripHtml(epub.Description ?? ""),
            EncodedCoverArt = (epub.CoverImage == null ? null : Convert.ToBase64String(epub.CoverImage)) ?? string.Empty,
            Chapters = new List<string>()
        };
        
        if (epub.Navigation != null)
        {
            foreach (var navItem in epub.Navigation.Where(navItem => navItem.Type == EpubNavigationItemType.LINK))
            {
                ebookData.Chapters.Add(navItem.Title);
            }
        }
        else
        {
            for (var i = 0; i < epub.ReadingOrder.Count; i++)
            {
                ebookData.Chapters.Add($"Chapter {i}");
            }
        }

        var htmlEncodedFilename = HttpUtility.HtmlEncode(file.FileName);
        Logger.LogDebug($"Parsed {htmlEncodedFilename} successfully", additionalArg: ebookData);
        
        return ebookData;
    }

    public static async Task<string> SaveFile(IFormFile file)
    {
        var randomFilename = $"eBook-{Guid.NewGuid()}.epub";
        var filePath = Path.Combine(_savePath, randomFilename);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var encodedFilename = HttpUtility.HtmlEncode(file.FileName);
        Logger.LogDebug($"Saved {encodedFilename} to {filePath}");

        return randomFilename;
    }

    public static bool Saved(string filename)
    {
        return File.Exists(Path.Combine(_savePath, filename));
    }

    public static async Task<bool> IsValidEpub(IFormFile file)
    {
        try
        {
            await using var stream = file.OpenReadStream();
            var epub = await EpubReader.OpenBookAsync(stream);
        }
        catch (Exception e)
        {
            Logger.LogInfo($"Invalid EPUB passed for parsing! {e.Message}");
            return false;
        }

        return true;
    }

    private static string StripHtml(string input)
    {
        // Create whitespaces between HTML elements
        input = input.Replace(">", "> ");
        
        // Parse HTML
        var document = new HtmlDocument();
        document.LoadHtml(input);

        // Strip HTML decoded text from the HTML
        var text = HttpUtility.HtmlDecode(document.DocumentNode.InnerText);

        // Replace all whitespaces with a single space and trim
        return Regex.Replace(text, @"\s+", " ").Trim();
    }
}
