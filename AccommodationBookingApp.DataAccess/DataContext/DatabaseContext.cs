using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccommodationBookingApp.DataAccess.DataContext
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public class OptionsBuild
        {
            public OptionsBuild()
            {
                settings = new AppConfiguration();
                optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlServer(AppConfiguration.sqlConnectionString);
                dbContextOptions = optionsBuilder.Options;
            }
            public DbContextOptionsBuilder<DatabaseContext> optionsBuilder { get; set; }
            public DbContextOptions<DatabaseContext> dbContextOptions { get; set; }
            private AppConfiguration settings { get; set; }
        }

        public static OptionsBuild optionsBuild = new OptionsBuild();

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<AccommodationType> AccommodationType { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}
