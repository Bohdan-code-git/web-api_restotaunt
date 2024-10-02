using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TESTDB.DATA;
using TESTDB.Models;

namespace TESTDB.Services.ItemServices
{
    public class ItemServices : IItemservices
    {
        private readonly PostgreSqlContext postgreSqlContext;

        public ItemServices(PostgreSqlContext context)
        {
            postgreSqlContext = context;
        }

        public async Task<List<Item?>> GetItems()
        {
            var items = await postgreSqlContext.Items.ToListAsync();
            return items;
        }
    }
}
