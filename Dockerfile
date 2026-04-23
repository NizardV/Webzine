# Multi-stage build pour Webzine (.NET 10)
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copier fichiers solution et projets
COPY ["Webzine/Webzine.WebApplication/Webzine.WebApplication.csproj", "Webzine.WebApplication/"]
COPY ["Webzine/Webzine.Business/Webzine.Business.csproj", "Webzine.Business/"]
COPY ["Webzine/Webzine.Business.Contracts/Webzine.Business.Contracts.csproj", "Webzine.Business.Contracts/"]
COPY ["Webzine/Webzine.Entity/Webzine.Entity.csproj", "Webzine.Entity/"]
COPY ["Webzine/Webzine.Entity.Tests/Webzine.Entity.Tests.csproj", "Webzine.Entity.Tests/"]
COPY ["Webzine/Webzine.EntitiesContext/Webzine.EntitiesContext.csproj", "Webzine.EntitiesContext/"]
COPY ["Webzine/Webzine.Repository/Webzine.Repository.csproj", "Webzine.Repository/"]
COPY ["Webzine/Webzine.Repository.Contracts/Webzine.Repository.Contracts.csproj", "Webzine.Repository.Contracts/"]
COPY ["Webzine/Webzine.Documentation/Webzine.Documentation.csproj", "Webzine.Documentation/"]

# Restaurer dépendances NuGet
RUN dotnet restore "Webzine.WebApplication/Webzine.WebApplication.csproj"

# Copier tout le code source
COPY Webzine/ .

# Build en Release
RUN dotnet build "Webzine.WebApplication/Webzine.WebApplication.csproj" -c Release -o /app/build

# Publier
FROM build AS publish
RUN dotnet publish "Webzine.WebApplication/Webzine.WebApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copier depuis publish stage
COPY --from=publish /app/publish .

# Créer dossier pour données persistantes
RUN mkdir -p /app/data

# Variables d'environnement par défaut
ENV ASPNETCORE_URLS=http://+:7110
ENV ASPNETCORE_ENVIRONMENT=Production
ENV RAG_API_URL=http://rag:8000
ENV POSTGRES_CONNECTION_STRING=Server=postgres;Port=5432;Database=webzine;User Id=webzine;Password=webzine_password;

EXPOSE 7110

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:7110/health || exit 1

ENTRYPOINT ["dotnet", "Webzine.WebApplication.dll"]

# Publier
FROM build AS publish
RUN dotnet publish "Webzine.WebApplication/Webzine.WebApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copier depuis publish stage
COPY --from=publish /app/publish .

# Créer dossier pour données persistantes
RUN mkdir -p /app/data

# Variables d'environnement par défaut
ENV ASPNETCORE_URLS=http://+:7110
ENV ASPNETCORE_ENVIRONMENT=Production
ENV RAG_API_URL=http://rag:8000
ENV POSTGRES_CONNECTION_STRING=Server=postgres;Port=5432;Database=webzine;User Id=webzine;Password=webzine_password;

EXPOSE 7110

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:7110/health || exit 1

ENTRYPOINT ["dotnet", "Webzine.WebApplication.dll"]
