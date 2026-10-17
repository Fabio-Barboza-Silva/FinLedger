namespace FinLedger.Application.DTOs;

public record TransferDto(
    string SourceAccountNumber,
    string DestinationAccountNumber,
    decimal Amount,
    string Description
);