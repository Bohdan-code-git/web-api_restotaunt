using Microsoft.AspNetCore.Mvc;
using TESTDB.Models;
using TESTDB.Services.NewsServices;


namespace TESTDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService INewsService;
        public NewsController(INewsService iNewsService)
        {
            this.INewsService = iNewsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<News>>> GetAsync()
        {
            var result = await INewsService.GetItems();
            if(result is [])
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{typeName}")]
        public async Task<ActionResult<News>> Get(string typeName)
        {
            var result = await INewsService.GetItemsByNewsType(typeName);
            if (result is [])
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
