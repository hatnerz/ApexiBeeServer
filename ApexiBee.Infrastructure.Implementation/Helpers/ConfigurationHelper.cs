using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.Helpers
{
    public class ConfigurationHelper
    {
        private readonly IConfigurationRoot configuration;

        public ConfigurationHelper()
        {
            configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
        }

        public string? GetConnectionString(string connectionStringName)
        {
            string? connectionString = configuration.GetConnectionString(connectionStringName);
            return connectionString;
        }
    }
}
