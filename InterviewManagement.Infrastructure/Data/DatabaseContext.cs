using InterviewManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InterviewManagement.Infrastructure.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
