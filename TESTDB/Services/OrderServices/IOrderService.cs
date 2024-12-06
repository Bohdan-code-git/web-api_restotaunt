using TESTDB.Models;
using WebApplication1.Dtos;

namespace TESTDB.Services.OrderServices
{
    public interface IOrderService
    {
       Task CreateOrder(CreateOrderDto order, int id);
       Task<List<Order?>> GetOrders();
    }
}
