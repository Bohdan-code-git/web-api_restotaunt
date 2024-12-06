using System.ComponentModel.DataAnnotations.Schema;

namespace TESTDB.Models
{
    public class Type : BaseEntity
    {
        public string Name { get; set; }

        [NotMapped]
        public List<Item> Items { get; set; } = new();

    }
}
