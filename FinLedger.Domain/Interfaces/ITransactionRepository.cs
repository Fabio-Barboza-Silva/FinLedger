using FinLedger.Domain.Entities;

namespace FinLedger.Domain.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
}