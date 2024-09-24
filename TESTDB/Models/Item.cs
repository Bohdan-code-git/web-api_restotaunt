using System.Runtime.CompilerServices;

namespace TESTDB.Models
{
    public class Item: BaseEntity
    {
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }  
        public string imageURL { get; set; }


    }
}
