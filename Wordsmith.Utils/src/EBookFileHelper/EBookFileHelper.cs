using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using VersOne.Epub;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;

namespace Wordsmith.Utils.EBookFileHelper;

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
        EpubBookRef epub;

        try
        {
            await using var stream = file.OpenReadStream();
            epub = await EpubReader.OpenBookAsync(stream);
        }
        catch (Exception e)
        {
            Logger.LogInfo($"Could not read EPUB: {e.Message}");
            throw new AppException($"Could not read EPUB!");
        }
        
        var ebookData = new EBookParseDto()
        {
            Title = StripHtml(epub.Title),
            Description = StripHtml(epub.Description ?? ""),
        };

        epub.Dispose();
        
        await ParseCoverArt(ebookData, file);
        await ParseChapters(ebookData, file);

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

        Logger.LogDebug($"Saved {filePath}");

        return randomFilename;
    }

    public static async Task<EBookFile> GetFile(string filename)
    {
        if (!Saved(filename))
        {
            throw new Exception($"File with name {filename} does not exist!");
        }

        var filepath = Path.Combine(_savePath, filename);
        using var memoryStream = new MemoryStream();
        
        await using var fileStream = new FileStream(filepath, FileMode.Open);
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        Logger.LogDebug($"Served {filepath}");
        
        return new EBookFile()
        {
            Bytes = memoryStream.ToArray(),
            Filename = filename
        };
    }

    public static bool Saved(string filename)
    {
        return File.Exists(Path.Combine(_savePath, filename));
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

    private static async Task ParseChapters(EBookParseDto dto, IFormFile file)
    {
        EpubBookRef epubRef;
        
        try
        {
            epubRef = await EpubReader.OpenBookAsync(file.OpenReadStream());
        }
        catch (Exception e)
        {
            Logger.LogInfo($"Could not read EPUB: {e.Message}");
            throw new AppException($"Could not read EPUB!");
        }
        
        var navigation = await epubRef.GetNavigationAsync();

        if (navigation == null)
        {
            throw new AppException("Your ebook does not contain any chapters! Make sure you have added a TOC");
        }

        dto.Chapters = new List<string>();
        
        foreach (var navItem in navigation)
        {
            dto.Chapters.Add(navItem.Title);
        }
        
        epubRef.Dispose();
    }

    private static async Task ParseCoverArt(EBookParseDto dto, IFormFile file)
    {
        EpubBookRef epubRef;
        
        try
        {
            epubRef = await EpubReader.OpenBookAsync(file.OpenReadStream());
        }
        catch (Exception e)
        {
            Logger.LogInfo($"Could not read EPUB: {e.Message}");
            throw new AppException($"Could not read EPUB!");
        }
        
        var coverImageBytes = await epubRef.ReadCoverAsync();

        if (coverImageBytes == null)
        {
            throw new AppException("Your ebook must have a cover image!");
        }

        dto.EncodedCoverArt = Convert.ToBase64String(coverImageBytes);
        epubRef.Dispose();
    }
}
