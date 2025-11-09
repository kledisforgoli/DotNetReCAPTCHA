using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using siguria.Models;

namespace siguria.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
