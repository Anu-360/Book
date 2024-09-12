using Microsoft.EntityFrameworkCore;
using static Book_Coding_Challenge.Constant.Types;
namespace Book_Coding_Challenge.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
              .Property(b => b.User_Type)
              .HasConversion(
                  v => v.ToString(),
                  v => Enum.Parse<UserType>(v));

            base.OnModelCreating(modelBuilder);

        }
    }
}
