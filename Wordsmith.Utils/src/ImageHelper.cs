using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using Wordsmith.Models.Exceptions;

namespace Wordsmith.Utils;

/// <summary>
/// Represents information on an image save operation
/// </summary>
public struct SaveInfo
{
    public string Path;
    public string Format;
    public int Size;
}

public static class ImageHelper
{
    private static string _webRootPath = "";
    private static uint _allowedImageSize = 0;
    private static List<string> _allowedFormats = new List<string>();

    /// <summary>
    /// Initializes the ImageHelper by providing the necessary settings
    /// </summary>
    /// <param name="webRootPath">Path to the wwwroot directory</param>
    /// <param name="allowedSize">The maximum allowed size for an image in bytes</param>
    /// <param name="allowedFormats">Comma-separated list of allowed image extensions</param>
    /// <exception cref="Exception">Invalid settings passed</exception>
    public static void Init(string? webRootPath, string? allowedSize, string? allowedFormats)
    {
        if (webRootPath == null || allowedSize == null || allowedFormats == null)
        {
            throw new Exception("Not all image settings are set!");
        }

        if (!Directory.Exists(webRootPath))
        {
            throw new Exception("wwwroot is not configured, cannot configure ImageHelper!");
        }

        _webRootPath = webRootPath;

        Directory.CreateDirectory(Path.Combine(webRootPath, "images", "users"));
        Directory.CreateDirectory(Path.Combine(webRootPath, "images", "ebooks"));
        
        if (!uint.TryParse(allowedSize, out _allowedImageSize))
        {
            throw new Exception("Allowed size for images cannot be parsed as a positive integer!");
        }

        var passedFormats = allowedFormats.Split(',');

        foreach (var format in passedFormats)
        {
            _allowedFormats.Add(format);
        }
    }
    
    /// <summary>
    /// Saves an image converted from Base64 to disk
    /// </summary>
    /// <param name="encodedImage">The Base64 encoding of the image</param>
    /// <param name="format">The image extension</param>
    /// <param name="filepath">Path to save</param>
    /// <returns>Information on the image path, size and format after saving</returns>
    public static SaveInfo SaveFromBase64(string encodedImage, string? format, string filepath)
    {
        const int bitsEncodedPerChar = 6;
        // Dividing the bits by 8 to get the byte value. Right-shifting by 3 is generally faster than doing / 8
        var bytesExpected = (encodedImage.Length * bitsEncodedPerChar) >> 3;
        var imageBuffer = new Span<byte>(new byte[bytesExpected]);

        ConvertAndValidate(encodedImage, format, ref imageBuffer, out var identifiedFormat);
        
        var pathToSave = Path.Combine(_webRootPath, filepath);

        // If the extension isn't written, write one
        if (!filepath.Contains('.'))
        {
            var formatExtension = identifiedFormat!.FileExtensions.First();
            pathToSave += $".{formatExtension}";
            filepath += $".{formatExtension}";
        }

        using var fileStream = File.OpenWrite(pathToSave);
        using var image = Image.Load(imageBuffer);

        image.Save(fileStream, identifiedFormat!); // The identified format will never be null here since that case is handled in ConvertAndValidate

        return new SaveInfo()
        {
            Path = filepath,
            Format = identifiedFormat!.Name,
            Size = imageBuffer.Length,
        };
    }
    
    /// <summary>
    /// Deletes an image residing on disk
    /// </summary>
    /// <param name="imageName">Name of the image, including the subfolders</param>
    public static void DeleteImage(string imageName)
    {
        var filepath = Path.Combine(_webRootPath, imageName);

        if (!File.Exists(filepath))
        {
            throw new Exception($"Image with path {filepath} was not found on disk, but was scheduled for deletion!");
        }
        
        File.Delete(filepath);
    }

    /// <summary>
    /// Takes in a Base64 encoding of an image, converts it to a byte array and validates it
    /// </summary>
    /// <param name="encodedImage">Base64 encoding of the image</param>
    /// <param name="format">The image extension</param>
    /// <param name="imageBuffer">A reference to a byte Span for storing of the image data</param>
    /// <param name="identifiedFormat">The identified format of the image after conversion</param>
    /// <exception cref="AppException">Validation failed for the image</exception>
    private static void ConvertAndValidate(string encodedImage, string? format, ref Span<byte> imageBuffer, out IImageFormat? identifiedFormat)
    {
        if (!Convert.TryFromBase64String(encodedImage, imageBuffer, out var bytesWritten))
        {
            throw new AppException("Failed to convert image from Base64");
        }

        if (imageBuffer.Length > _allowedImageSize)
        {
            throw new AppException("Image size is disallowed",
                new Dictionary<string, object>()
                {
                    { "passedSize", imageBuffer.Length.ToString() },
                    { "allowedSize", _allowedImageSize.ToString() }
                });
        }
        
        identifiedFormat = Image.Identify(imageBuffer).Metadata.DecodedImageFormat;
        
        if (identifiedFormat == null)
        {
            throw new AppException("Image format is disallowed",
                new Dictionary<string, object>()
                {
                    { "passedFormat", format ?? "" },
                    { "allowedFormats", string.Join(',', _allowedFormats) }
                });
        }

        // If the common file extensions for the identified format are not found within the allowed formats, stop execution
        if (!identifiedFormat.FileExtensions.Intersect(_allowedFormats).Any())
        {
            throw new AppException("Image format is disallowed",
                new Dictionary<string, object>()
                {
                    { "passedFormat", format ?? "" },
                    { "allowedFormats", string.Join(',', _allowedFormats) }
                });
        }
    }
}