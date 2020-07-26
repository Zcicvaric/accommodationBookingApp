using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AccommodationBookingApp.DataAccess.DataContext
{
    public class AppConfiguration
    {
        public AppConfiguration()
        {
            //creats a new configuration builder and path to the configuration file
            var configBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configBuilder.AddJsonFile(path, false);
            var root = configBuilder.Build();
            var appSetting = root.GetSection("ConnectionStrings:DefaultConnection");
            Console.WriteLine(appSetting.Value);
            sqlConnectionString = appSetting.Value;
        }
        public static string sqlConnectionString { get; set; }
    }
}
