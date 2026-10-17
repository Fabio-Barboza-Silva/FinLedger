# 1. Imagem base de execução (leve e optimizada)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. Imagem de compilação (contém o SDK do .NET 8)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os ficheiros de projeto para restaurar dependências (cache eficiente)
COPY ["FinLedger.Api/FinLedger.Api.csproj", "FinLedger.Api/"]
COPY ["FinLedger.Application/FinLedger.Application.csproj", "FinLedger.Application/"]
COPY ["FinLedger.Domain/FinLedger.Domain.csproj", "FinLedger.Domain/"]
COPY ["FinLedger.Infrastructure/FinLedger.Infrastructure.csproj", "FinLedger.Infrastructure/"]

RUN dotnet restore "FinLedger.Api/FinLedger.Api.csproj"

# Copia o código-fonte restante e executa o build
COPY . .
WORKDIR "/src/FinLedger.Api"
RUN dotnet build "FinLedger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publicação dos artefactos compilados
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FinLedger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Imagem final pronta para correr
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinLedger.Api.dll"]