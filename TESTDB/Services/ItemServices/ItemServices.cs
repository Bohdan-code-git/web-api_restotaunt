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
            var items = await postgreSqlContext.Items.Include(i => i.Type).ToListAsync();
            return items;
        }
         public async Task<List<Item?>> GetItemsByDishType(string name)
        {
            var items = await postgreSqlContext.Items.Include(i => i.Type).ToListAsync();
            List<Item> result = new List<Item>();
            foreach (var item in items)
            {
                if(item.Type.name == name)
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}
