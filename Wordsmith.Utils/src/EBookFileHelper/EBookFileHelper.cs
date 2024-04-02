using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using VersOne.Epub;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;

namespace Wordsmith.Utils.EBookFileHelper;

/// <summary>
/// A static helper class for operations regarding EPUB files
/// </summary>
public static class EBookFileHelper
{
    private static string _savePath;

    /// <summary>
    /// Initializes the EBookFileHelper
    /// </summary>
    /// <param name="savePath">Path where to save ebook files</param>
    /// <exception cref="Exception">If the savePath is invalid</exception>
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

    /// <summary>
    /// Parses the passed EPUB file and returns data associated with it
    /// </summary>
    /// <param name="file">A file received with an HTTP request</param>
    /// <returns>The parsed data</returns>
    /// <exception cref="AppException">If the EPUB file could not be read, its TOC is invalid or it does not contain any cover art</exception>
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
            Logger.LogWarn($"Could not read EPUB: {e.Message}");
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
        Logger.LogDebug($"Parsed {htmlEncodedFilename} successfully");
        
        
        return ebookData;
    }

    /// <summary>
    /// Parses the passed EPUB file and returns data associated with it. Primarily used for seeding
    /// </summary>
    /// <param name="filepath">Path to the EPUB file</param>
    /// <returns>The parsed data</returns>
    /// <exception cref="Exception">If the EPUB file could not be read, its TOC is invalid or it does not contain any cover art</exception>
    public static async Task<EBookParseDto> ParseEpub(string filepath)
    {
        EpubBookRef epub;

        try
        {
            epub = await EpubReader.OpenBookAsync(filepath);
        }
        catch (Exception e)
        {
            Logger.LogWarn($"Could not read EPUB: {e.Message}");
            throw;
        }
        
        var ebookData = new EBookParseDto()
        {
            Title = StripHtml(epub.Title),
            Description = StripHtml(epub.Description ?? ""),
        };
        
        epub.Dispose();

        await ParseCoverArt(ebookData, filepath);
        await ParseChapters(ebookData, filepath);
        
        Logger.LogDebug($"Parsed {filepath} successfully");

        return ebookData;
    }
    
    /// <summary>
    /// Saves the EPUB file to the designated save path
    /// </summary>
    /// <param name="file">A file received with an HTTP request</param>
    /// <returns>The new filename of the EPUB file</returns>
    public static async Task<string> SaveFile(IFormFile file)
    {
        var randomFilename = $"eBook-{Guid.NewGuid()}.epub";
        var filePath = Path.Combine(_savePath, randomFilename);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        Logger.LogDebug($"Saved {filePath}");

        return randomFilename;
    }
    
    /// <summary>
    /// Saves the EPUB file to the designated save path. Primarily used for seeding
    /// </summary>
    /// <param name="filepath">Path to the EPUB file</param>
    /// <returns>The new filename of the EPUB file</returns>
    public static string SaveFile(string filepath)
    {
        var randomFilename = $"eBook-{Guid.NewGuid()}.epub";
        var savePath = Path.Combine(_savePath, randomFilename);
        
        File.Copy(filepath, savePath, overwrite: true);

        Logger.LogDebug($"Saved {savePath}");

        return randomFilename;
    }

    /// <summary>
    /// Returns an EPUB file present on disk
    /// </summary>
    /// <param name="filename">Filename of the EPUB</param>
    /// <returns>The byte array and filename of the EPUB</returns>
    /// <exception cref="Exception">If the requested EPUB file does not exist or reading it fails</exception>
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

    /// <summary>
    /// Returns whether the specified EPUB file exists
    /// </summary>
    /// <param name="filename">Filename of the EPUB file</param>
    /// <returns>True if it exists, otherwise false</returns>
    public static bool Saved(string filename)
    {
        return File.Exists(Path.Combine(_savePath, filename));
    }

    /// <summary>
    /// Strips an input string of any HTML markup
    /// </summary>
    /// <param name="input">The string that needs to be stripped</param>
    /// <returns>The string without any HTML</returns>
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

    /// <summary>
    /// Parses an EPUB file and fills the provided dto with the chapters it finds
    /// </summary>
    /// <param name="dto">The dto containing previous EPUB parse data</param>
    /// <param name="file">A file received with an HTTP request</param>
    /// <exception cref="AppException">If reading the EPUB file fails or the ebook contains no TOC</exception>
    private static async Task ParseChapters(EBookParseDto dto, IFormFile file)
    {
        EpubBookRef epubRef;
        
        try
        {
            epubRef = await EpubReader.OpenBookAsync(file.OpenReadStream());
        }
        catch (Exception e)
        {
            Logger.LogWarn($"Could not read EPUB: {e.Message}");
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
    
    /// <summary>
    /// Parses an EPUB file and fills the provided dto with the chapters it finds. Primary used for seeding
    /// </summary>
    /// <param name="dto">The dto containing previous EPUB parse data</param>
    /// <param name="filepath">Path to the EPUB file</param>
    /// <exception cref="Exception">If reading the EPUB file fails or the ebook contains no TOC</exception>
    private static async Task ParseChapters(EBookParseDto dto, string filepath)
    {
        EpubBookRef epubRef;
        
        try
        {
            epubRef = await EpubReader.OpenBookAsync(filepath);
        }
        catch (Exception e)
        {
            Logger.LogWarn($"Could not read EPUB: {e.Message}");
            throw;
        }
        
        var navigation = await epubRef.GetNavigationAsync();

        if (navigation == null)
        {
            throw new Exception($"Ebook {filepath} does not contain any TOC!");
        }

        dto.Chapters = new List<string>();
        
        foreach (var navItem in navigation)
        {
            dto.Chapters.Add(navItem.Title);
        }
        
        epubRef.Dispose();
    }

    /// <summary>
    /// Parses an EPUB file and fills the provided dto with the cover art it finds.
    /// </summary>
    /// <param name="dto">The dto containing previous EPUB parse data</param>
    /// <param name="file">A file received with an HTTP request</param>
    /// <exception cref="AppException">If reading the EPUB file fails or the ebook contains no cover art</exception>
    private static async Task ParseCoverArt(EBookParseDto dto, IFormFile file)
    {
        EpubBookRef epubRef;
        
        try
        {
            epubRef = await EpubReader.OpenBookAsync(file.OpenReadStream());
        }
        catch (Exception e)
        {
            Logger.LogWarn($"Could not read EPUB: {e.Message}");
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
    
    /// <summary>
    /// Parses an EPUB file and fills the provided dto with the cover art it finds.
    /// </summary>
    /// <param name="dto">The dto containing previous EPUB parse data</param>
    /// <param name="filepath">Path to the EPUB file</param>
    /// <exception cref="Exception">If reading the EPUB file fails or the ebook contains no cover art</exception>
    private static async Task ParseCoverArt(EBookParseDto dto, string filepath)
    {
        EpubBookRef epubRef;
        
        try
        {
            epubRef = await EpubReader.OpenBookAsync(filepath);
        }
        catch (Exception e)
        {
            Logger.LogWarn($"Could not read EPUB: {e.Message}");
            throw;
        }
        
        var coverImageBytes = await epubRef.ReadCoverAsync();

        if (coverImageBytes == null)
        {
            throw new Exception($"Ebook {filepath} does not have a cover image!");
        }

        dto.EncodedCoverArt = Convert.ToBase64String(coverImageBytes);
        epubRef.Dispose();
    }
}
