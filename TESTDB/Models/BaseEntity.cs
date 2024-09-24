namespace TESTDB.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CteatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
