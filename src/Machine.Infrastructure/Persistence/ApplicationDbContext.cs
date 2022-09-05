using Machine.Application.Common.Interfaces;
using Machine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Machine.Application.Common;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Machine.Infrastructure.Persistence;


public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IConfiguration _configuration;
    public DbConnection Connection { get { return base.Database.GetDbConnection(); } }
    public DbSet<Product> Product { get; set; } = default!;
    public DbSet<Coin> Coin { get; set; } = default!;
    public DbSet<Language> Language { get; set; } = default!;
    public DbSet<ProductPrice> ProductPrice { get; set; } = default!;
 

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IConfiguration configuration )      
    {
        _configuration = configuration;       
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {     
        optionsBuilder.LogTo(Console.WriteLine); 
        string connectionString = _configuration.GetValue<string>("database.connectionString");

        _ = _configuration.GetValue<string>("database.providername") switch
        {            
            "System.Data.SQLite" => optionsBuilder.UseSqlite(connectionString),
            "InMemoryDatabase" => optionsBuilder.UseInMemoryDatabase(connectionString),
            _ => optionsBuilder.UseSqlite(connectionString)
        };
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        modelBuilder.Entity<Product>().HasKey(c => new { c.SlotId, c.ProductId });
        modelBuilder.Entity<Coin>().HasKey(c => new { c.CoinId });
        modelBuilder.Entity<Language>().HasKey(c => new { c.LanguageId, c.MessageKey });
        modelBuilder.Entity<ProductPrice>().HasKey(c => new { c.CurrencyId, c.ProductId });     
    }
}

 

