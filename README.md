# FinLedger - Core Banking & Double-Entry Ledger API

[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED.svg)](https://www.docker.com/)
[![Tests](https://img.shields.io/badge/Tests-xUnit-green.svg)](https://xunit.net/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-orange.svg)]()

Uma API bancária robusta concebida segundo os princípios de **Clean Architecture**, **Domain-Driven Design (DDD)** e **Contabilidade por Partidas Dobradas (*Double-Entry Bookkeeping*)**, garantindo rastreabilidade, imutabilidade financeira e consistência transacional.

---

## 🏛️ Arquitetura da Solução

O sistema foi estruturado em quatro camadas estritamente desacopladas:

```
FinLedger/
├── src/
│   ├── FinLedger.Domain/          # Entidades ricas, regras de negócio puras e contratos
│   ├── FinLedger.Application/     # Casos de uso, DTOs e orquestração de serviços
│   ├── FinLedger.Infrastructure/  # EF Core, mapeamentos relacionais, migrations e repositórios
│   └── FinLedger.Api/             # Controladores REST, injeção de dependências e Swagger
└── tests/
    └── FinLedger.Tests/           # Testes unitários de domínio com xUnit
```

---

## 💡 Princípios e Padrões Aplicados

* **Clean Architecture & SOLID:** Regras de negócio no Domínio completamente independentes de frameworks, banco de dados ou bibliotecas externas.
* **Double-Entry Bookkeeping:** Toda a movimentação entre contas gera duas entradas contábeis imutáveis (um débito na origem e um crédito no destino), garantindo auditoria contábil matemática.
* **Unit of Work & Repository Pattern:** Garante que débitos e créditos sejam executados de forma atómica dentro de uma mesma transação no banco de dados.
* **Entidades Ricas:** Propriedades fechadas para alteração externa direta (`private set`), com mutações de estado protegidas por métodos de domínio (`Credit`, `Debit`).
* **Testes Automatizados:** Testes unitários isolados com xUnit cobrindo validações de saldo, prevenção de saques a descoberto e valores inválidos.

---

## 🛠️ Tecnologias Utilizadas

* **Linguagem & Framework:** C# 12, .NET 8, ASP.NET Core Web API
* **Persistência & ORM:** Entity Framework Core 8, SQL Server, EF Core Migrations
* **Testes:** xUnit
* **Documentação de API:** Swagger / OpenAPI
* **Contentorização:** Docker (Multi-stage build)

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [SQL Server](https://www.microsoft.com/sql-server) ou LocalDB

### Execução via Linha de Comando (.NET CLI)

1. Clone o repositório:
   ```bash
   git clone [https://github.com/SEU_USUARIO/FinLedger.git](https://github.com/SEU_USUARIO/FinLedger.git)
   cd FinLedger
   ```

2. Restaure as dependências e aplique as migrações no banco:
   ```bash
   dotnet restore
   dotnet ef database update --project FinLedger.Infrastructure --startup-project FinLedger.Api
   ```

3. Execute a API:
   ```bash
   dotnet run --project FinLedger.Api
   ```
4. Aceda ao Swagger em: `https://localhost:7xxx/swagger`

### Execução via Docker

1. Construir a imagem otimizada:
   ```bash
   docker build -t finledger-api:v1 .
   ```

2. Executar os testes automatizados:
   ```bash
   dotnet test
   ```