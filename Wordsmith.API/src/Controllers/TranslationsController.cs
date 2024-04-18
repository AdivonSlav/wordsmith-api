using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.Integration.GoogleTranslate;
using Wordsmith.Integration.GoogleTranslate.Models;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("translate")]
public class TranslationsController
{
    private readonly IGoogleTranslateService _translateService;

    public TranslationsController(IGoogleTranslateService translateService)
    {
        _translateService = translateService;
    }

    [SwaggerOperation("Get supported translation languages")]
    [Authorize("All")]
    [HttpGet("languages")]
    public async Task<ActionResult<QueryResult<SupportedLanguages>>> GetSupportedLanguages()
    {
        return await _translateService.GetSupportedLanguages();
    }

    [SwaggerOperation("Translate text into a specified language")]
    [Authorize("All")]
    [HttpGet]
    public async Task<ActionResult<QueryResult<TranslationResponse>>> Translate([FromQuery]TranslationRequest request)
    {
        return await _translateService.Translate(request);
    }
}