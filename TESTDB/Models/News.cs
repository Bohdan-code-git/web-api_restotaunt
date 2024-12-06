namespace TESTDB.Models
{
    public class News :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public NewsType Type { get; set; }
    }
}
