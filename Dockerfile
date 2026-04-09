# --- ÉTAPE 1 : BUILD ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. On copie les fichiers de projets pour restaurer les dépendances
# On recrée l'arborescence pour que le compilateur s'y retrouve
COPY ["Webzine/Webzine.WebApplication/Webzine.WebApplication.csproj", "Webzine/Webzine.WebApplication/"]
COPY ["Webzine/Webzine.Business/Webzine.Business.csproj", "Webzine/Webzine.Business/"]
COPY ["Webzine/Webzine.Repository/Webzine.Repository.csproj", "Webzine/Webzine.Repository/"]
COPY ["Webzine/Webzine.Entity/Webzine.Entity.csproj", "Webzine/Webzine.Entity/"]
# Ajoute ici les autres .csproj si la compilation bloque sur une référence manquante

RUN dotnet restore "Webzine/Webzine.WebApplication/Webzine.WebApplication.csproj"

# 2. On copie tout le reste du code
COPY . .

# 3. On compile spécifiquement l'application Web
WORKDIR "/src/Webzine/Webzine.WebApplication"
# Installation de l'outil LibMan
RUN dotnet tool install --global Microsoft.Web.LibraryManager.Cli
ENV PATH="$PATH:/root/.dotnet/tools"

# Restauration des bibliothèques (Bootstrap, etc.)
RUN libman restore
RUN dotnet publish "Webzine.WebApplication.csproj" -c Release -o /app/publish

# --- ÉTAPE 2 : RUNTIME ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
USER $APP_UID

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Webzine.WebApplication.dll"]
