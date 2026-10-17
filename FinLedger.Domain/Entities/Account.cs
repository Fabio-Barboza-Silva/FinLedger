namespace FinLedger.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; } = string.Empty;
    public string OwnerDocument { get; private set; } = string.Empty; // CPF ou CNPJ
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Construtor sem parâmetros exigido pelo Entity Framework futuramente
    protected Account() { }

    public Account(string accountNumber, string ownerDocument, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number cannot be empty.", nameof(accountNumber));

        if (string.IsNullOrWhiteSpace(ownerDocument))
            throw new ArgumentException("Owner document cannot be empty.", nameof(ownerDocument));

        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.", nameof(initialBalance));

        Id = Guid.NewGuid();
        AccountNumber = accountNumber;
        OwnerDocument = ownerDocument;
        Balance = initialBalance;
        CreatedAt = DateTime.UtcNow;
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Credit amount must be greater than zero.");

        Balance += amount;
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Debit amount must be greater than zero.");

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds for this transaction.");

        Balance -= amount;
    }
}