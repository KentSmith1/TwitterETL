using Microsoft.EntityFrameworkCore;
using TwitterETL.Models;

namespace TwitterETL.Context
{
    public class TweetContext: DbContext
    {
        protected override void OnConfiguring
            (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "TwitterDb");
        }
        public DbSet<TweetDbModel> Tweets { get; set; }
    }
}
