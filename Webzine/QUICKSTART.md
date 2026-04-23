# 🚀 Quick Start - Docker Stack Deployment

## Démarrage Rapide en 3 Étapes

### 1️⃣ Préparation

```bash
# Copier la configuration d'environnement
cp .env.example .env

# Récupérer les données Deezer (optionnel)
cd rag
python fetch_deezer_data.py
cd ..
```

### 2️⃣ Build & Launch

```bash
# Construire les images
docker-compose build

# Démarrer la stack
docker-compose up -d

# Vérifier les conteneurs
docker-compose ps
```

### 3️⃣ Vérification

```bash
# Health checks
curl http://localhost:7110/health
curl http://localhost:8000/health
docker-compose exec postgres psql -U webzine -d webzine -c "SELECT * FROM user_sessions LIMIT 1;"

# Accéder à l'application
# Webzine: http://localhost:7110
# RAG API: http://localhost:8000/docs
```

---

## Services & Ports

| Service    | Port  | URL                          | Description          |
|------------|-------|------------------------------|----------------------|
| Webzine    | 7110  | http://localhost:7110        | Application web      |
| RAG API    | 8000  | http://localhost:8000        | FastAPI Python       |
| PostgreSQL | 5432  | localhost:5432               | Database             |
| Ollama     | 11434 | http://localhost:11434       | LLM (sur host)       |

---

## Commandes Essentielles

### Gestion

```bash
# Démarrer
docker-compose up -d

# Arrêter
docker-compose down

# Supprimer volumes (⚠️ supprime les données!)
docker-compose down -v

# Logs
docker-compose logs -f

# Logs spécifique
docker-compose logs -f webzine
docker-compose logs -f rag
docker-compose logs -f postgres
```

### Database

```bash
# Accès PostgreSQL
docker-compose exec postgres psql -U webzine -d webzine

# Commandes SQL
\dt                                    # Lister les tables
SELECT * FROM conversation_history;    # Voir l'historique
\q                                     # Quitter

# Backup
docker-compose exec postgres pg_dump -U webzine webzine > backup.sql

# Restore
docker-compose exec -T postgres psql -U webzine webzine < backup.sql
```

---

## Configuration

### Variables d'environnement (.env)

```bash
# PostgreSQL
POSTGRES_PASSWORD=votre_password_securise

# .NET
RAG_API_URL=http://rag:8000
POSTGRES_CONNECTION_STRING=Server=postgres;Port=5432;Database=webzine;User Id=webzine;Password=votre_password_securise;

# Python
OLLAMA_HOST=http://host.docker.internal:11434  # Windows/Mac
# OLLAMA_HOST=http://ollama:11434              # Linux
```

---

## Troubleshooting

### Webzine ne démarre pas

```bash
docker-compose logs webzine
# Vérifier: PostgreSQL connection string, RAG_API_URL, ports libres
```

### RAG API retourne erreur

```bash
docker-compose logs rag
# Vérifier: Ollama running (ollama serve), dépendances Python
```

### PostgreSQL ne se démarre pas

```bash
docker-compose logs postgres
# Vérifier: Port 5432 libre, permissions disque
```

---

## Données Persistantes

Les volumes suivants sont créés automatiquement:

- `postgres_data` - Base de données PostgreSQL
- `webzine_data` - Données application
- `./rag/chroma_db` - Vector DB Wikipedia
- `./rag/chroma_db_deezer` - Vector DB Deezer

Pour conserver les données lors d'un `docker-compose down`, ne pas utiliser `-v`:

```bash
docker-compose down      # ✅ Conserve les données
docker-compose down -v   # ⚠️  Supprime les volumes
```

---

## Documentation Complète

Voir **DOCKER_SETUP.md** pour:
- Architecture détaillée
- Configuration avancée
- Production checklist
- Intégration PostgreSQL
- Health checks
- Monitoring
- Et plus...

---

## Support

**Questions?** Vérifiez les logs:

```bash
docker-compose logs -f
```

**Port occupé?**

```bash
# Changer dans docker-compose.yml
ports:
  - "8110:7110"  # Utiliser 8110 au lieu de 7110
```

---

**Bon déploiement! 🚀**
