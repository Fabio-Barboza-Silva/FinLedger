using FinLedger.Application.DTOs;
using FinLedger.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinLedger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
    {
        try
        {
            var account = await _accountService.CreateAccountAsync(dto);
            return CreatedAtAction(nameof(GetAccount), new { accountNumber = account.AccountNumber }, account);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpGet("{accountNumber}")]
    public async Task<IActionResult> GetAccount(string accountNumber)
    {
        var account = await _accountService.GetAccountByNumberAsync(accountNumber);
        if (account == null)
            return NotFound(new { message = $"Account {accountNumber} not found." });

        return Ok(account);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferDto dto)
    {
        try
        {
            var success = await _accountService.TransferAsync(dto);
            if (!success)
                return StatusCode(500, new { message = "Failed to complete transaction." });

            return Ok(new { message = "Transfer completed successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
    }
}