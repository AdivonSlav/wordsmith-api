using System.Web;
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
            throw new AppException("Could not read EPUB");
        }

        var ebookData = new EBookParseDto()
        {
            Title = epub.Title,
            Description = epub.Description ?? "",
            EncodedCoverArt = epub.CoverImage == null ? null : Convert.ToBase64String(epub.CoverImage),
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
        var randomFilename = $"eBook_{Guid.NewGuid()}.epub";
        var filePath = Path.Combine(_savePath, randomFilename);
        await using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);

        var encodedFilename = HttpUtility.HtmlEncode(file.FileName);
        Logger.LogDebug($"Saved {encodedFilename} to {filePath}");

        return randomFilename;
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
            return false;
        }

        return true;
    }
}