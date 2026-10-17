using FinLedger.Domain.Entities;
using FinLedger.Domain.Interfaces;
using FinLedger.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinLedger.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly FinLedgerDbContext _context;

    public AccountRepository(FinLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public Task UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        return Task.CompletedTask;
    }
}