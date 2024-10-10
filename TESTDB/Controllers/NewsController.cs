using Microsoft.AspNetCore.Mvc;
using TESTDB.Models;
using TESTDB.Services.NewsServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<ValuesController>
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

        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
