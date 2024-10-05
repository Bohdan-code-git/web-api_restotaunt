using TESTDB.Models;

namespace TESTDB.Services.ItemServices
{
    public interface IItemservices
    {
        Task<List<Item?>> GetItems();
        Task<List<Item?>> GetItemsByDishType(string name);
    }
}
