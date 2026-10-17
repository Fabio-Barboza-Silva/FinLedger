using FinLedger.Application.DTOs;
using FinLedger.Domain.Entities;
using FinLedger.Domain.Enums;
using FinLedger.Domain.Interfaces;

namespace FinLedger.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    // Injeção de dependência via construtor
    public AccountService(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Account> CreateAccountAsync(CreateAccountDto dto)
    {
        var existingAccount = await _accountRepository.GetByAccountNumberAsync(dto.AccountNumber);
        if (existingAccount != null)
            throw new InvalidOperationException($"Account {dto.AccountNumber} already exists.");

        var account = new Account(dto.AccountNumber, dto.OwnerDocument, dto.InitialBalance);
        await _accountRepository.AddAsync(account);
        await _unitOfWork.CommitAsync();

        return account;
    }

    public async Task<bool> TransferAsync(TransferDto dto)
    {
        if (dto.Amount <= 0)
            throw new ArgumentException("Transfer amount must be positive.");

        if (dto.SourceAccountNumber == dto.DestinationAccountNumber)
            throw new InvalidOperationException("Source and destination accounts must be different.");

        // 1. Busca as duas contas
        var sourceAccount = await _accountRepository.GetByAccountNumberAsync(dto.SourceAccountNumber)
            ?? throw new KeyNotFoundException($"Source account {dto.SourceAccountNumber} not found.");

        var destinationAccount = await _accountRepository.GetByAccountNumberAsync(dto.DestinationAccountNumber)
            ?? throw new KeyNotFoundException($"Destination account {dto.DestinationAccountNumber} not found.");

        // 2. Aplica débito e crédito (regras de domínio encapsuladas)
        sourceAccount.Debit(dto.Amount);
        destinationAccount.Credit(dto.Amount);

        // 3. Cria o razão contábil (Audit trail / Ledger imutável)
        var debitTransaction = new Transaction(
            sourceAccount.Id,
            dto.Amount,
            TransactionType.Debit,
            $"Transfer to {dto.DestinationAccountNumber}: {dto.Description}"
        );

        var creditTransaction = new Transaction(
            destinationAccount.Id,
            dto.Amount,
            TransactionType.Credit,
            $"Transfer from {dto.SourceAccountNumber}: {dto.Description}"
        );

        // 4. Registra no repositório
        await _transactionRepository.AddAsync(debitTransaction);
        await _transactionRepository.AddAsync(creditTransaction);

        await _accountRepository.UpdateAsync(sourceAccount);
        await _accountRepository.UpdateAsync(destinationAccount);

        // 5. Salva tudo em uma única transação atômica (ACID)
        return await _unitOfWork.CommitAsync();
    }

    public async Task<Account?> GetAccountByNumberAsync(string accountNumber)
    {
        return await _accountRepository.GetByAccountNumberAsync(accountNumber);
    }
}