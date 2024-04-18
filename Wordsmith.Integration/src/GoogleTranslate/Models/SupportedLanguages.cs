using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Integration.GoogleTranslate.Models;

public class SupportedLanguages
{
    [SwaggerSchema("List of supported translation languages")]
    public IEnumerable<Language> Languages { get; set; }
}