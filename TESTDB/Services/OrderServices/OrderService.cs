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

            if (order == null)
                throw new ArgumentNullException(nameof(order), "Order data cannot be null");

            if (_httpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("HTTP context is not available");

            // Получаем пользователя из токена
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                throw new UnauthorizedAccessException("User email not found in token");

            var user = await postgreSqlContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail) ?? throw new Exception("User not found");

            // Валидация данных заказа
            if (order.OrderItems == null || order.OrderItems.Count == 0)
                throw new ArgumentException("Order must contain at least one item");

            if (order.TotalPrice < 0  )
                throw new ArgumentException("Order must contain positive price");

            using var transaction = await postgreSqlContext.Database.BeginTransactionAsync();
            try
            {
                var newOrder = new Order
                {
                    TotalPrice = order.TotalPrice,
                    User = user,
                    UserId = user.Id,
                    State = State.New,
                    Adress = order.Adress,
                    OrderItems = new List<OrderItem>()
                };

                postgreSqlContext.Orders.Add(newOrder);

                foreach (var item in order.OrderItems)
                {
                    // Проверка существования блюда
                    var dish = await postgreSqlContext.Items.FindAsync(item.ItemId);
                    if (dish == null)
                        throw new Exception($"Item with ID {item.ItemId} not found");

                    // Добавление позиции заказа
                    var orderItem = new OrderItem
                    {
                        Quantity = item.Quantity,
                        Order = newOrder,
                        Item = dish
                    };

                    newOrder.OrderItems.Add(orderItem);
                    postgreSqlContext.OrderItems.Add(orderItem);
                }

                // Сохранение всех изменений
                await postgreSqlContext.SaveChangesAsync();

                // Фиксация транзакции
                await transaction.CommitAsync();

                // Логирование успешного создания заказа (можно заменить на реальный логгер)
                Console.WriteLine($"Order for user {user.Email} created successfully with ID: {newOrder.Id}");
            }
            catch (Exception ex)
            {
                // Откат транзакции в случае ошибки
                await transaction.RollbackAsync();

                // Логирование ошибки
                Console.WriteLine($"Error creating order: {ex.Message}");

                throw; // Повторно выбрасываем исключение для обработки на более высоком уровне
            }
        }

        public async Task<List<Order?>> GetOrders()
        {
            var items = await postgreSqlContext.Orders.Include(i => i.OrderItems).ToListAsync();
            return items;
        }
        public async Task<List<Order?>> GetUserOrders()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("HTTP context is not available");

            // Получаем email пользователя из токена
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                throw new UnauthorizedAccessException("User email not found in token");

            // Ищем пользователя в базе данных
            var user = await postgreSqlContext.Users
                .Include(u => u.Orders!) 
                .ThenInclude(o => o.OrderItems) 
                .FirstOrDefaultAsync( u=> u.Email == userEmail) ?? throw new Exception("User not found");

            // Проверяем, есть ли у пользователя заказы
            if (user.Orders == null || !user.Orders.Any())
                return new List<Order?>();

            return user.Orders.ToList();
        }
    }
}
