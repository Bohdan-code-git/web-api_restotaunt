namespace TESTDB.Models
{
    public class News :BaseEntity
    {
        public string name { get; set; }
        public string description { get; set; }
        public string imageURL { get; set; }
        public NewsType type { get; set; }
    }
}
