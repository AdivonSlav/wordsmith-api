using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers
{
    [Route("[controller]")]
    public class ReadController<T, TSearch> : ControllerBase
        where T : class
        where TSearch : SearchObject
    {
        protected readonly IReadService<T, TSearch> ReadService;

        public ReadController(IReadService<T, TSearch> readService)
        {
            ReadService = readService;
        }

        [HttpGet()]
        public async Task<QueryResult<T>> Get([FromQuery] TSearch? search = null)
        {
            return await ReadService.Get(search);
        }

        [HttpGet("{id:int}")]
        public async Task<T> GetById(int id)
        {
            return await ReadService.GetById(id);
        }
    }
}