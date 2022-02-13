using Microsoft.EntityFrameworkCore;

namespace BulkInsertDemo;

public sealed class BulkContext : DbContext
{
#pragma warning disable CS8618
    public BulkContext(DbContextOptions<BulkContext> options) : base(options)
#pragma warning restore CS8618
    {
        // ouch, but it will do for this MWE...
        Database.Migrate();
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public DbSet<StockRow> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entity = modelBuilder.Entity<StockRow>();
        entity.HasKey(e => new {e.Timestamp, e.StoreNo, e.ProductCode});
        entity.ToTable("Stock");
    }
}