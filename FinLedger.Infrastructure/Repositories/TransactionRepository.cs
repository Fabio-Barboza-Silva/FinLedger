using FinLedger.Domain.Entities;
using FinLedger.Domain.Interfaces;
using FinLedger.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinLedger.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly FinLedgerDbContext _context;

    public TransactionRepository(FinLedgerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Timestamp)
            .ToListAsync();
    }
}