using SixLabors.ImageSharp.Formats;
using Wordsmith.Models.Exceptions;

namespace Wordsmith.Utils;

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

    public static SaveInfo SaveFromBase64(string encodedImage, string format, string filepath)
    {
        const int bitsEncodedPerChar = 6;
        var bytesExpected = (encodedImage.Length * bitsEncodedPerChar) >> 3; // Divide by 8 bits in a byte
        var imageBuffer = new Span<byte>(new byte[bytesExpected]);

        ConvertAndValidate(encodedImage, format, ref imageBuffer, out var identifiedFormat);
        
        var pathToSave = Path.Combine(_webRootPath, filepath);

        using var fileStream = File.OpenWrite(pathToSave);
        using var image = Image.Load(imageBuffer);

        image.Save(fileStream, identifiedFormat!); // The identified format will never be null here since that case is handled in ConvertAndValidate

        return new SaveInfo()
        {
            Path = filepath,
            Format = format,
            Size = imageBuffer.Length,
        };
    }

    private static void ConvertAndValidate(string encodedImage, string format, ref Span<byte> imageBuffer, out IImageFormat? identifiedFormat)
    {
        if (!Convert.TryFromBase64String(encodedImage, imageBuffer, out var bytesWritten))
        {
            throw new AppException("Failed to convert image from Base64");
        }

        if (imageBuffer.Length > _allowedImageSize)
        {
            throw new AppException("Image size is disallowed",
                new Dictionary<string, string>()
                {
                    { "passedSize", imageBuffer.Length.ToString() },
                    { "allowedSize", _allowedImageSize.ToString() }
                });
        }
        
        identifiedFormat = Image.Identify(imageBuffer).Metadata.DecodedImageFormat;
        
        if (identifiedFormat == null)
        {
            throw new AppException("Image format is disallowed",
                new Dictionary<string, string>()
                {
                    { "passedFormat", format },
                    { "allowedFormats", string.Join(',', _allowedFormats) }
                });
        }

        // If the common file extensions for the identified format are not found within the allowed formats, stop execution
        if (!identifiedFormat.FileExtensions.Intersect(_allowedFormats).Any())
        {
            throw new AppException("Image format is disallowed",
                new Dictionary<string, string>()
                {
                    { "passedFormat", format },
                    { "allowedFormats", string.Join(',', _allowedFormats) }
                });
        }
    }
}