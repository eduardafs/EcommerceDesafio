# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia o arquivo de solução e os arquivos .csproj
COPY *.sln ./
COPY ECommerceBem/*.csproj ./ECommerceBem/
COPY ECommerceBem.Core/*.csproj ./ECommerceBem.Core/
COPY ECommerceBem.Application/*.csproj ./ECommerceBem.Application/
COPY ECommerceBem.Infrastructure/*.csproj ./ECommerceBem.Infrastructure/
COPY ECommerceBem.Exception/*.csproj ./ECommerceBem.Exception/
COPY ECommerceBem.Tests/*.csproj ./ECommerceBem.Tests/

# Restaura as dependências
RUN dotnet restore

# Copia o restante do código-fonte
COPY . .

# Publica a aplicação
RUN dotnet publish ECommerceBem/ECommerceBem.csproj -c Release -o /app/publish

# Etapa de execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ECommerceBem.dll"]