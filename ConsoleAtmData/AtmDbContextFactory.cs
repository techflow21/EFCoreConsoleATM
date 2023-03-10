using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCoreATM_Data;

public class AtmDbContextFactory : IDesignTimeDbContextFactory<AtmDbContext>
{
    //static string _connectionString = ConfigurationSettings.AppSettings["DbConnection"];

    string _connectionString = @"Data Source=DESKTOP-APMJTIG;Initial Catalog = efCoreAtmDB; Integrated Security =True; Encrypt=false";
    public AtmDbContextFactory() { }

    public AtmDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AtmDbContext>();

        optionsBuilder.UseSqlServer(_connectionString);

        return new AtmDbContext(optionsBuilder.Options);
    }
}
