using TESTDB.Models;

namespace TESTDB.Services.ItemServices
{
    public interface IItemservices
    {
        Task<List<Item?>> GetItems();
    }
}
