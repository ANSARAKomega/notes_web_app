using Microsoft.EntityFrameworkCore;

namespace NotesWebApp.Models
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options)
           : base(options)
        {
        }

        public DbSet<Notes> Notes { get; set; }
        public DbSet<Users> Users { get; set; }

    }
}
