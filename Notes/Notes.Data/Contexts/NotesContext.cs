using Microsoft.EntityFrameworkCore;
using Notes.Data.Entities;
using System.Reflection;

namespace Notes.Data.Contexts
{
    public class NotesContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        public NotesContext(DbContextOptions<NotesContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
