using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TESTDB.DATA;
using TESTDB.Models;
using WebApplication1.Dtos;

namespace TESTDB.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly PostgreSqlContext postgreSqlContext;
        private readonly IMapper _mapper;

        public OrderService(PostgreSqlContext context, IMapper mapper)
        {
            postgreSqlContext = context;
            _mapper = mapper;
        }
        public async Task CreateOrder(CreateOrderDto order, int id)
        {
            var user = postgreSqlContext.Users.Find(id);

            Order newOrder = new Order();
            newOrder.TotalPrice = order.TotalPrice;
            newOrder.User = user;
            newOrder.UserId = user.Id;
            newOrder.State= State.New;
            newOrder.Adress = order.Adress;

            postgreSqlContext.Orders.Add(newOrder);

            foreach (OrderItem item in order.OrderItems)
            {
                var dish = postgreSqlContext.Items.Find(item.ItemId);

                OrderItem orderItem = new OrderItem();
                orderItem.Quantity = item.Quantity;
                orderItem.Order = newOrder;
                orderItem.Item = dish;

                newOrder.OrderItems.Add(orderItem);
                postgreSqlContext.OrderItems.Add(orderItem);
            }
            await postgreSqlContext.SaveChangesAsync();
        }

        public async Task<List<Order?>> GetOrders()
        {
            var items = await postgreSqlContext.Orders.Include(i => i.OrderItems).ToListAsync();
            return items;
        }
    }
}
