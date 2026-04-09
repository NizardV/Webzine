## Equipe 3 - dossier de configuration

### Résumé du projet

- Ce projet est un webzine utilisant Asp.Net Core MVC

### Etape de configuration
1. Pour exécuter l'environnement de développement il faut :
	- Modifier la variable environnement dans le appSettings.json, dans Environnement et mettre "Development"
	- Se rendre dans le appSettings.Development.json
	- Pour utiliser une base de données mettre "Database" dans Repository, sinon mettre "Local"
	- Pour utiliser les factories mettre "Factory" dans Seeder, sinon mettre "Deezer".

> {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "version": "2.0",
  "AppConfig": {
    "Environment": "Development", //"Production", "Development"
    "Repository": "Database", // "Local" "Database"
    "Seeder": "Deezer" // "Factories", "Deezer",
    "DeezerApi": "https://api.deezer.com"
  },
  "ConnectionStrings": {
    "Postgres": "", //  Ex : "Host=;Database=;Username=;Password="
    "Sqlite": "Data Source=./Data/Webzine.sqlite" // Ex : "Data Source=webzine.db"
  }
}
