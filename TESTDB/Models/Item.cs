using System.Runtime.CompilerServices;

namespace TESTDB.Models
{
    public class Item: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }  
        public string ImageURL { get; set; }
        public virtual Type Type { get; set; }
    }
}
