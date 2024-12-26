using Microsoft.AspNetCore.Mvc;
using TESTDB.Models;
using TESTDB.Services.ItemServices;

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
        [HttpGet("{TypeNameOfDish}")]
        public async Task<ActionResult<Item>> GetItemsByDishType(string TypeNameOfDish)
        {
            var result = await itemservices.GetItemsByDishType(TypeNameOfDish);

            if (result is [])
                return NotFound("no dishes");

            return Ok(result);
        }
    }
}
