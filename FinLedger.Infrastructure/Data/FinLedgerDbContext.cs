using FinLedger.Domain.Entities;
using FinLedger.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinLedger.Infrastructure.Data;

public class FinLedgerDbContext : DbContext, IUnitOfWork
{
    public FinLedgerDbContext(DbContextOptions<FinLedgerDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public async Task<bool> CommitAsync()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.AccountNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(a => a.AccountNumber).IsUnique();
            builder.Property(a => a.OwnerDocument).IsRequired().HasMaxLength(14);
            builder.Property(a => a.Balance).HasPrecision(18, 2);
            builder.Property(a => a.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Transaction>(builder =>
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Amount).HasPrecision(18, 2);
            builder.Property(t => t.Type).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(250);
            builder.Property(t => t.Timestamp).IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(t => t.AccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }
}