using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        //Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //DataSets
        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Addresses { get; set; }

    }
}
