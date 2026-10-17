using FinLedger.Domain.Enums;

namespace FinLedger.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime Timestamp { get; private set; }

    protected Transaction() { }

    public Transaction(Guid accountId, decimal amount, TransactionType type, string description)
    {
        if (accountId == Guid.Empty)
            throw new ArgumentException("Invalid account reference.", nameof(accountId));

        if (amount <= 0)
            throw new ArgumentException("Transaction amount must be positive.", nameof(amount));

        Id = Guid.NewGuid();
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Description = string.IsNullOrWhiteSpace(description) ? "Standard Transaction" : description;
        Timestamp = DateTime.UtcNow;
    }
}