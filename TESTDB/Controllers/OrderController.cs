using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TESTDB.Models;
using TESTDB.Services.ItemServices;
using TESTDB.Services.OrderServices;
using WebApplication1.Dtos;

namespace TESTDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService service)
        {
            orderService = service;
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> AddOrder(CreateOrderDto order)
        {
            await orderService.CreateOrder(order);
            return Ok();
        }
        //[HttpGet]
        //public async Task<ActionResult<List<Order>>> GetAsync()
        //{
        //    var result = await orderService.GetOrders();
        //    return Ok(result);
        //}

        [HttpGet, Authorize]
        public async Task<ActionResult<List<Order>>> GetOrderByAuthorizedUser()
        {
            var result = await orderService.GetUserOrders();
            return Ok(result);
        }
    }
}
