using System.ComponentModel.DataAnnotations.Schema;

namespace TESTDB.Models
{
    public class Type : BaseEntity
    {
        public string name { get; set; }

        [NotMapped]
        public List<Item> items { get; set; } = new();

    }
}
