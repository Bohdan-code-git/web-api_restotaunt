using TESTDB.Models;
using WebApplication1.Dtos;

namespace TESTDB.Services.OrderServices
{
    public interface IOrderService
    {
       Task CreateOrder(CreateOrderDto order);
       Task<List<Order?>> GetOrders();
       Task<List<Order?>> GetUserOrders();
    }
}
