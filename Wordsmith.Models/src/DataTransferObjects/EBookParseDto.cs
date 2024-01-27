namespace Wordsmith.Models.DataTransferObjects;

public class EBookParseDto
{
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string EncodedCoverArt { get; set; }
    
    public List<string> Chapters { get; set; }
}