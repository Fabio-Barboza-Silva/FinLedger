using FinLedger.Domain.Entities;

namespace FinLedger.Tests;

public class AccountTests
{
    [Fact]
    public void Credit_ShouldIncreaseBalance_WhenAmountIsPositive()
    {
        // 1. Arrange
        var account = new Account("1001-X", "11122233344", 1000m);

        // 2. Act
        account.Credit(500m);

        // 3. Assert
        Assert.Equal(1500m, account.Balance);
    }

    [Fact]
    public void Debit_ShouldDecreaseBalance_WhenFundsAreSufficient()
    {
        // 1. Arrange
        var account = new Account("1001-X", "11122233344", 1000m);

        // 2. Act
        account.Debit(400m);

        // 3. Assert
        Assert.Equal(600m, account.Balance);
    }

    [Fact]
    public void Debit_ShouldThrowInvalidOperationException_WhenInsufficientFunds()
    {
        // 1. Arrange
        var account = new Account("1001-X", "11122233344", 100m);

        // 2. Act & Assert
        // Valida se a exceção correta foi lançada quando o saldo é insuficiente
        Assert.Throws<InvalidOperationException>(() =>
        {
            account.Debit(250m);
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Credit_ShouldThrowInvalidOperationException_WhenAmountIsZeroOrNegative(decimal invalidAmount)
    {
        // 1. Arrange
        var account = new Account("1001-X", "11122233344", 1000m);

        // 2. Act & Assert
        // Ajustado para esperar InvalidOperationException conforme a regra de domínio implementada
        Assert.Throws<InvalidOperationException>(() =>
        {
            account.Credit(invalidAmount);
        });
    }
}