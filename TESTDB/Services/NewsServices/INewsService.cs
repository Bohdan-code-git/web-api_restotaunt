using TESTDB.Models;

namespace TESTDB.Services.NewsServices
{
    public interface INewsService
    {
        Task<List<News?>> GetItems();
        Task<List<News?>> GetItemsByNewsType(string NewsType);
    }
}
