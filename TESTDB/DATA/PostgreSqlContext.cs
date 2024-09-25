using Microsoft.EntityFrameworkCore;
using TESTDB.Models;

namespace TESTDB.DATA
{
    public class PostgreSqlContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.UseNpgsql("Host=localhost;Database=restoraunt;Username=postgres;Password=admin1234;IncludeErrorDetail=true");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var addedEntries = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added).Select(e => e.Entity);

            foreach (var entry in addedEntries)
            {
                var item = entry as BaseEntity;

                if (item != null)
                    item.CteatedDate = DateTime.UtcNow;
            }

            var modifiedEntries = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified).Select(e => e.Entity);

            foreach (var entry in modifiedEntries)
            {
                var item = entry as BaseEntity;

                if (item != null)
                    item.UpdatedDate = DateTime.UtcNow;
            }


            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TESTDB.Models.Type> Types { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsType> NewsTypes { get; set; }
    }
}
