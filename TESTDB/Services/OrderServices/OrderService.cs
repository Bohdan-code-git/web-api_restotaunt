using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TESTDB.DATA;
using TESTDB.DTO;
using TESTDB.Models;
using WebApplication1.Dtos;

namespace TESTDB.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly PostgreSqlContext postgreSqlContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(PostgreSqlContext context, IMapper mapper, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._configuration = configuration;
            this._httpContextAccessor= httpContextAccessor;
            postgreSqlContext = context;
            _mapper = mapper;
        }
        public async Task CreateOrder(CreateOrderDto order)
        {
            
            if (_httpContextAccessor.HttpContext is not null)
            {
                User user;
                var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

                user = postgreSqlContext.Users.FirstOrDefault(u => u.Email == result);
                Order newOrder = new Order();
                newOrder.TotalPrice = order.TotalPrice;
                newOrder.User = user ?? throw new Exception("User's not found");
                newOrder.UserId = user.Id;
                newOrder.State = State.New;
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
        }

        public async Task<List<Order?>> GetOrders()
        {
            var items = await postgreSqlContext.Orders.Include(i => i.OrderItems).ToListAsync();
            return items;
        }
    }
}
