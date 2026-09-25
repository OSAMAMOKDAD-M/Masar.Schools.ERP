using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Masar.Schools.ERP.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MasarDbContext>
{
    public MasarDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<MasarDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        optionsBuilder.UseSqlServer(connectionString);

        return new MasarDbContext(optionsBuilder.Options);
    }
}