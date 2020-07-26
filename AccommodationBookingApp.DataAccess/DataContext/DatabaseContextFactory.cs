using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccommodationBookingApp.DataAccess.DataContext
{
    class DatabaseContextFactory:IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string [] args)
        {
            AppConfiguration appConfiguration = new AppConfiguration();
            var opsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            opsBuilder.UseSqlServer(AppConfiguration.sqlConnectionString);
            return new DatabaseContext(opsBuilder.Options);
        }
    }
}
