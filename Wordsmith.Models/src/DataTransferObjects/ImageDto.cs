namespace Wordsmith.Models.DataTransferObjects;

public class ImageDto
{
    public string? ImagePath { get; set; }
    
    public string? EncodedImage { get; set; }
    
    public string Format { get; set; }
    
    public int Size { get; set; }
}