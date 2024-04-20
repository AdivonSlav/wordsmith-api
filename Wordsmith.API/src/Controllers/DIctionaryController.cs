using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.Integration.MerriamWebster;
using Wordsmith.Integration.MerriamWebster.Models;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("dictionary")]
public class DictionaryController
{
    private readonly IMerriamWebsterService _merriamWebsterService;
    
    public DictionaryController(IMerriamWebsterService merriamWebsterService)
    {
        _merriamWebsterService = merriamWebsterService;
    }

    [SwaggerOperation("Return dictionary entries for a given search term")]
    [Authorize("All")]
    [HttpGet]
    public async Task<ActionResult<QueryResult<DictionaryResponse>>> GetDefinition([FromQuery] string searchTerm)
    {
        return await _merriamWebsterService.GetResults(searchTerm);
    }
}