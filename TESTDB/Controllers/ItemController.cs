using Microsoft.AspNetCore.Mvc;
using TESTDB.Models;
using TESTDB.Services.ItemServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TESTDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemservices itemservices;

        public ItemController(IItemservices itemservices)
        {
            this.itemservices = itemservices;
        }
        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetAsync()
        {
            var result = await itemservices.GetItems();
            return Ok(result);
        }
        [HttpGet("{NameOfDish}")]
        public async Task<ActionResult<Item>> GetItemsByDishType(string TypeNameOfDish)
        {
            var result = await itemservices.GetItemsByDishType(TypeNameOfDish);

            if (result is [])
                return NotFound("no dishes");

            return Ok(result);
        }
        //// GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<ValuesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
