namespace TESTDB.Models
{
    public class Order : BaseEntity
    {
        public State State { get; set; } = State.New;
        public decimal TotalPrice { get; set; }
        public string? Adress { get; set; } 
        public int UserId { get; set; }
        public User? User { get; set; }

        public List<OrderItem>? OrderItems { get; set; } = new();
    }
    public enum State
    {
        New,
        Accepted,
        Done,
        Canceled,
    }
}

