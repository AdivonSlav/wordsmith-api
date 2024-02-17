namespace Wordsmith.Utils.EBookFileHelper;

public class EBookFile
{
    public byte[] Bytes { get; init; }

    public string Filename { get; init; }

    public string MimeType { get; } = "application/epub+zip";
}