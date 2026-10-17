using FinLedger.Application.DTOs;
using FinLedger.Domain.Entities;

namespace FinLedger.Application.Services;

public interface IAccountService
{
    Task<Account> CreateAccountAsync(CreateAccountDto dto);
    Task<bool> TransferAsync(TransferDto dto);
    Task<Account?> GetAccountByNumberAsync(string accountNumber);
}