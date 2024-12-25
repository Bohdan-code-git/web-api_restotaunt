namespace TESTDB.Models
{
    public class User 
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Adress { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Order>? Orders { get; set; }

    }
}
