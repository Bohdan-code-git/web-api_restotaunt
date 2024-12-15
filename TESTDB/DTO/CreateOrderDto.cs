using TESTDB.Models;

namespace WebApplication1.Dtos
{
    public class CreateOrderDto
    {
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string Adress { get; set; }
    }
}
