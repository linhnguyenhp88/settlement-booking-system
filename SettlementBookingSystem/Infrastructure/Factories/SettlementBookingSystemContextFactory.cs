using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SettlementBookingSystem.Infrastructure.EntityFrameworkCore;
using System.IO;

namespace SettlementBookingSystem.Infrastructure.Factories
{
    public class SettlementBookingSystemContextFactory : IDesignTimeDbContextFactory<SettlementBookingSystemContext>
    {
        public SettlementBookingSystemContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SettlementBookingSystemContext>();        
            return new SettlementBookingSystemContext(optionsBuilder.Options);
        } 
    }
}
