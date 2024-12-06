using Microsoft.EntityFrameworkCore;
using TESTDB.DATA;
using TESTDB.Models;

namespace TESTDB.Services.NewsServices
{
    public class NewsService : INewsService
    {
        private readonly PostgreSqlContext postgreSqlContext;

        public NewsService(PostgreSqlContext context)
        {
            postgreSqlContext = context;
        }

        public async Task<List<News?>> GetItems()
        {
            var items = await postgreSqlContext.News.Include(x => x.Type).ToListAsync();
            return items;
        }
        public async Task<List<News?>> GetItemsByNewsType(string typename)
        {
            var items = await postgreSqlContext.News.Include(i => i.Type).ToListAsync();
            List<News> result = new List<News>();
            foreach (var item in items)
            {
                if (item.Type.Name == typename)
                {
                    result.Add(item);
                }
            }
            return result;
        }

    }
}
