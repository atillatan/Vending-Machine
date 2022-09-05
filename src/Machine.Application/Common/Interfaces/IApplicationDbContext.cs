using Machine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Machine.Application.Common.Interfaces;

public interface IApplicationDbContext : IDisposable
{
    DbSet<Product> Product { get; set; }

    DbSet<Coin> Coin { get; set; }

    DbSet<Language> Language { get; set; }

    DbSet<ProductPrice> ProductPrice { get; set; } 

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    public DbConnection Connection { get; }
}