using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Api.Entities;

namespace Sat.Recruitment.Api
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)   
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
