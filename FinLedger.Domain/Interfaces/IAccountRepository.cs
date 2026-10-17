using FinLedger.Domain.Entities;

namespace FinLedger.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
}