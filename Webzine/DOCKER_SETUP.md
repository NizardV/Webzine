# 🐳 Guide de Containerisation - Webzine Stack

## Architecture Docker

La stack complète est composée de **3 services orchestrés**:

```
┌─────────────────────────────────────────────────────────┐
│                Docker Compose Network                   │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │  Webzine     │  │   RAG API    │  │  PostgreSQL  │  │
│  │  .NET 10     │─→│   Python     │  │  Database    │  │
│  │  :7110       │  │   :8000      │  │  :5432       │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
│                            ↓                            │
│                    ┌────────────────┐                   │
│                    │  Ollama LLM    │                   │
│                    │  :11434        │                   │
│                    │  (host)        │                   │
│                    └────────────────┘                   │
└─────────────────────────────────────────────────────────┘
```

### Services

1. **PostgreSQL** (Port 5432)
   - Database pour historique conversationnel
   - Persistance des préférences utilisateur
   - Tables pré-initialisées via `init-db.sql`

2. **RAG API** (Port 8000)
   - FastAPI Python
   - Support dual-source (Wikipedia/Deezer)
   - Index vectoriel Chroma
   - Volumes persistants pour données

3. **Webzine** (Port 7110)
   - Application .NET 10 Razor Pages
   - Interface web avec chatbot widget
   - Appels RAG via réseau Docker

4. **Ollama** (Port 11434 - optionnel)
   - LLM local (llama3.2 par défaut)
   - Via `localhost:11434` depuis host

---

## Installation & Setup

### Prérequis

- **Docker Desktop** v24.0+
- **Docker Compose** v2.20+
- **Ollama** en cours d'exécution (sur host ou en conteneur)
- **2GB RAM libre** minimum
- **2GB disk space** pour DB + vectors

### 1. Préparation - Génération requirements.txt Python

```bash
cd rag
pip freeze > requirements.txt
cd ..
```

### 2. Build des images

```bash
# Build tout
docker-compose build

# Ou build spécifique
docker-compose build webzine
docker-compose build rag
docker-compose build postgres
```

### 3. Démarrer la stack

```bash
# Démarrer tout
docker-compose up -d

# Vérifier les conteneurs
docker-compose ps

# Voir les logs
docker-compose logs -f
docker-compose logs -f rag      # Logs spécifiques
docker-compose logs -f webzine
```

### 4. Accès aux services

| Service       | URL                    | Description              |
|---------------|------------------------|--------------------------|
| Webzine       | http://localhost:7110  | Application web          |
| RAG API       | http://localhost:8000  | API FastAPI              |
| PostgreSQL    | localhost:5432         | Database                 |
| Ollama        | http://localhost:11434 | LLM (sur host)           |

---

## Configuration

### Variables d'environnement

#### .env (créer à la racine)

```bash
# PostgreSQL
POSTGRES_DB=webzine
POSTGRES_USER=webzine
POSTGRES_PASSWORD=webzine_password  # ⚠️ À changer en production

# .NET
ASPNETCORE_ENVIRONMENT=Production
RAG_API_URL=http://rag:8000
POSTGRES_CONNECTION_STRING=Server=postgres;Port=5432;Database=webzine;User Id=webzine;Password=webzine_password;

# Python
PYTHONUNBUFFERED=1
OLLAMA_HOST=http://host.docker.internal:11434  # Pour Docker Desktop Win/Mac
# OLLAMA_HOST=http://ollama:11434  # Si Ollama en conteneur

# Optionnel: Pour logs
LOGLEVEL=INFO
```

### Connection Strings

#### .NET (Webzine)
```
Server=postgres;Port=5432;Database=webzine;User Id=webzine;Password=webzine_password;
```

#### Python (RAG)
```python
import psycopg2
conn = psycopg2.connect(
    host="postgres",
    database="webzine",
    user="webzine",
    password="webzine_password"
)
```

---

## Commandes Utiles

### Gestion des conteneurs

```bash
# Démarrer
docker-compose up -d

# Arrêter
docker-compose down

# Arrêter + supprimer volumes
docker-compose down -v

# Redémarrer
docker-compose restart

# Reconstruire
docker-compose build --no-cache
docker-compose up -d
```

### Logs & Debugging

```bash
# Logs en temps réel
docker-compose logs -f

# Logs spécifique service
docker-compose logs -f webzine
docker-compose logs -f rag
docker-compose logs -f postgres

# Entrée shell dans conteneur
docker-compose exec webzine bash
docker-compose exec rag bash
docker-compose exec postgres psql -U webzine -d webzine
```

### Health Checks

```bash
# Vérifier status
docker-compose ps

# Test manuel des services
curl http://localhost:7110/health
curl http://localhost:8000/health
curl http://localhost:5432 || echo "PostgreSQL OK"
```

---

## Initialisation base de données

### Schéma automatique (via init-db.sql)

À la création du conteneur `postgres`, le fichier `init-db.sql` s'exécute automatiquement et crée:

- ✅ `conversation_history` - Historique des messages
- ✅ `user_sessions` - Sessions utilisateur
- ✅ `user_preferences` - Préférences utilisateur
- ✅ `conversation_feedback` - Notes et commentaires
- ✅ Index et views pour optimisation

### Vérifier l'initialisation

```bash
# Accès à PostgreSQL
docker-compose exec postgres psql -U webzine -d webzine

# Lister les tables
\dt

# Vérifier une table
SELECT * FROM conversation_history;

# Quitter
\q
```

---

## Données persistantes

### Volumes

| Volume              | Chemin Conteneur      | Description                   |
|---------------------|-----------------------|-------------------------------|
| `postgres_data`     | `/var/lib/postgresql` | Database PostgreSQL           |
| `webzine_data`      | `/app/data`           | Données Webzine (if needed)   |
| ./rag/chroma_db     | `/app/chroma_db`      | Vector DB Wikipedia           |
| ./rag/chroma_db_deezer | `/app/chroma_db_deezer` | Vector DB Deezer          |
| ./rag/data_deezer   | `/app/data_deezer`    | Données Deezer (CSV/TXT)      |

### Sauvegarde PostgreSQL

```bash
# Export
docker-compose exec postgres pg_dump -U webzine webzine > backup.sql

# Import
docker-compose exec -T postgres psql -U webzine webzine < backup.sql
```

---

## Troubleshooting

### Le conteneur WebZine ne démarre pas

```bash
# Vérifier les logs
docker-compose logs webzine

# Possible: Connection string incorrecte
# Solution: Vérifier postgres_connection_string dans docker-compose.yml
```

### RAG API retourne 502 Bad Gateway

```bash
# Vérifier si Ollama est en cours d'exécution
curl http://localhost:11434/api/tags

# Si erreur "connection refused":
# 1. Démarrer Ollama: ollama serve
# 2. Ou commenter ollama_data et décommenter service ollama dans docker-compose.yml
```

### PostgreSQL ne se connecte pas

```bash
# Vérifier les logs PostgreSQL
docker-compose logs postgres

# Vérifier si le port 5432 est en utilisation
netstat -an | findstr 5432  # Windows
lsof -i :5432              # Linux/Mac
```

### Les volumes ne sont pas créés

```bash
# Vérifier les volumes
docker volume ls

# Nettoyer les volumes stale
docker volume prune

# Recréer
docker-compose down -v
docker-compose up -d
```

---

## Production

### Checklist Sécurité

- [ ] Changer `POSTGRES_PASSWORD` dans docker-compose.yml
- [ ] Utiliser `.env` pour secrets (non committer)
- [ ] Activer HTTPS/TLS (Nginx reverse proxy)
- [ ] Configurer `ASPNETCORE_ENVIRONMENT=Production`
- [ ] Ajouter `docker-compose.prod.yml` avec ressources limitées
- [ ] Implémenter backup automatique PostgreSQL
- [ ] Ajouter monitoring (Prometheus, Grafana)
- [ ] Configurer logging centralisé (ELK, Splunk)

### Ressources limite (docker-compose.prod.yml)

```yaml
services:
  postgres:
    deploy:
      resources:
        limits:
          cpus: '1'
          memory: 1G
  rag:
    deploy:
      resources:
        limits:
          cpus: '2'
          memory: 3G
  webzine:
    deploy:
      resources:
        limits:
          cpus: '1'
          memory: 1G
```

---

## Intégration Données Deezer

### Récupérer les données Deezer

```bash
# Via docker
docker-compose exec rag python fetch_deezer_data.py

# Ou local
cd rag
python fetch_deezer_data.py
cd ..
```

### Initialiser l'index Deezer

Dans le RAG (`api.py`), à la startup:
```python
build_deezer_index()  # S'exécute automatiquement
```

---

## Next Steps

1. **Personnaliser appsettings.Production.json**
   ```bash
   cp Webzine/appsettings.json Webzine/appsettings.Production.json
   # Éditer et configurer pour Production
   ```

2. **Initialiser données Deezer**
   ```bash
   docker-compose exec rag python fetch_deezer_data.py
   ```

3. **Vérifier l'interface chatbot**
   - Ouvrir http://localhost:7110
   - Tester le widget flottant en bas à droite
   - Basculer entre Wikipedia et Deezer

4. **Monitorer les logs**
   ```bash
   docker-compose logs -f
   ```

5. **Personnaliser PostgreSQL**
   - Adapter le schéma dans `init-db.sql`
   - Ajouter des migrations via C#

---

## Ressources

- Docker Compose: https://docs.docker.com/compose/
- .NET Docker: https://github.com/dotnet/dotnet-docker
- FastAPI Docker: https://fastapi.tiangolo.com/deployment/docker/
- PostgreSQL Docker: https://hub.docker.com/_/postgres
- Ollama: https://ollama.com

---

**Questions?** Vérifier les logs: `docker-compose logs -f`
