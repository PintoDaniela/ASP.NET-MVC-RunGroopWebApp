using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Data
{
    //I want to use Identity, so I installed the Microsoft.AspNetCore.Identity.EntityFrameworkCore package (same version as EntityFrameworkCore)
    //and changed DbContext to IdentityDbContext<AppUser>

    //public class ApplicationDbContext : DbContext
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
