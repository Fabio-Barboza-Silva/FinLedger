namespace FinLedger.Application.DTOs;

public record CreateAccountDto(
    string AccountNumber,
    string OwnerDocument,
    decimal InitialBalance
);