using FinLedger.Application.Services;
using FinLedger.Domain.Interfaces;
using FinLedger.Infrastructure.Data;
using FinLedger.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar o DbContext com SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FinLedgerDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Injeção de Dependências - Domínio / Infraestrutura
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<FinLedgerDbContext>());
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// 3. Injeção de Dependências - Aplicação
builder.Services.AddScoped<IAccountService, AccountService>();

// 4. Controladores e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();