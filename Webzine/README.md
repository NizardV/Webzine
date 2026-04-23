# 🐳 Webzine Docker Stack - Complete Documentation Index

## 📋 Files Created

### 📌 Core Docker Files

| File | Purpose | Status |
|------|---------|--------|
| **Dockerfile** | Multi-stage build pour .NET 10 application | ✅ |
| **rag/Dockerfile** | Image Python FastAPI pour le service RAG | ✅ |
| **docker-compose.yml** | Orchestration des 3 services (dev) | ✅ |
| **docker-compose.prod.yml** | Configuration production avec limites ressources | ✅ |
| **.dockerignore** | Fichiers à exclure du build Docker | ✅ |

### 📝 Configuration Files

| File | Purpose | Status |
|------|---------|--------|
| **.env.example** | Modèle de variables d'environnement (dev) | ✅ |
| **.env.prod.example** | Modèle de variables d'environnement (prod) | ✅ |
| **appsettings.Production.json** | Configuration .NET pour production | ✅ |
| **rag/requirements.txt** | Dépendances Python | ✅ |
| **init-db.sql** | Script d'initialisation PostgreSQL | ✅ |

### 📚 Documentation

| File | Description | Status |
|------|-------------|--------|
| **DOCKER_SETUP.md** | Guide complet de containerisation (116 KB) | ✅ |
| **QUICKSTART.md** | Démarrage rapide en 3 étapes | ✅ |
| **POSTGRES_GUIDE.md** | Intégration PostgreSQL détaillée | ✅ |
| **README.md** | Ce fichier (index) | ✅ |

### 🛠️ Utilities

| File | Purpose | Status |
|------|---------|--------|
| **Makefile** | Commandes pratiques pour Docker | ✅ |
| **validate.sh** | Script de validation (Linux/Mac) | ✅ |
| **validate.ps1** | Script de validation PowerShell (Windows) | ✅ |

### 💻 Code Changes

| File | Changes | Status |
|------|---------|--------|
| **Webzine.WebApplication/Controllers/HealthController.cs** | Nouveau controller pour health checks | ✅ |
| **Webzine.WebApplication/Extensions/WebApplicationExtensions.cs** | Ajout route `/health` | ✅ |
| **Webzine.WebApplication/appsettings.json** | Connexion PostgreSQL | ✅ |
| **rag/api.py** | Ajout endpoint `/health` | ✅ |

---

## 🚀 Quick Start

### Option 1: Via Makefile (Recommandé)

```bash
make build
make up
make status
make test-api
```

### Option 2: Via Docker Compose Dirct

```bash
docker-compose build
docker-compose up -d
docker-compose ps
```

### Option 3: Via Scripts

```bash
# Windows
powershell -ExecutionPolicy Bypass -File validate.ps1
docker-compose build
docker-compose up -d

# Linux/Mac
bash validate.sh
docker-compose build
docker-compose up -d
```

---

## 📖 Documentation Flow

### For Developers 👨‍💻

1. **Start Here**: [QUICKSTART.md](QUICKSTART.md)
   - 3 étapes pour lancer l'application
   - Commandes essentielles
   - Troubleshooting basique

2. **Then Read**: [DOCKER_SETUP.md](DOCKER_SETUP.md)
   - Architecture détaillée
   - Configuration avancée
   - Production checklist

3. **For Database**: [POSTGRES_GUIDE.md](POSTGRES_GUIDE.md)
   - Schéma et tables
   - Intégration .NET
   - Backup & recovery

### For DevOps/SRE 🏗️

1. **Production Deployment**: [docker-compose.prod.yml](docker-compose.prod.yml)
   - Limites de ressources CPU/RAM
   - Logging structuré
   - Health checks

2. **Configuration**: [.env.prod.example](.env.prod.example)
   - Secrets management
   - Variable d'environnement

3. **Maintenance**: [DOCKER_SETUP.md](DOCKER_SETUP.md) → Section "Production"

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────┐
│           Docker Compose Network                     │
├─────────────────────────────────────────────────────┤
│                                                      │
│  ┌──────────────────┐  ┌──────────────────┐        │
│  │  Webzine         │  │  RAG API         │        │
│  │  .NET 10         │→ │  Python FastAPI  │        │
│  │  Port: 7110      │  │  Port: 8000      │        │
│  │                  │  │                  │        │
│  │ • ChatController │  │ • /query         │        │
│  │ • HealthChecker  │  │ • /initialize    │        │
│  │ • Widget UI      │  │ • /status        │        │
│  │                  │  │ • /health        │        │
│  └────────┬─────────┘  └────────┬─────────┘        │
│           │                     │                   │
│           └─────────┬───────────┘                   │
│                     │                                │
│           ┌─────────▼──────────┐                    │
│           │  PostgreSQL 16     │                    │
│           │  Port: 5432        │                    │
│           │                    │                    │
│           │ • conversation_h.  │                    │
│           │ • user_sessions    │                    │
│           │ • user_preferences │                    │
│           │ • feedback         │                    │
│           └────────────────────┘                    │
│                                                      │
│  ┌────────────────────────────┐                    │
│  │  Ollama (Host)              │                    │
│  │  Port: 11434 (external)    │                    │
│  │  LLM: llama3.2              │                    │
│  └────────────────────────────┘                    │
│                                                      │
└─────────────────────────────────────────────────────┘

Volumes:
├── postgres_data/        (PostgreSQL persistence)
├── webzine_data/         (Application data)
├── rag/chroma_db/        (Wikipedia vectors)
└── rag/chroma_db_deezer/ (Deezer vectors)
```

---

## ✅ Deployment Checklist

### Pre-Deployment

- [ ] Run `validate.ps1` or `validate.sh`
- [ ] Copy `.env.example` to `.env`
- [ ] Update `POSTGRES_PASSWORD` in `.env`
- [ ] Verify `docker --version` ≥ 24.0
- [ ] Verify `docker-compose --version` ≥ 2.20
- [ ] Verify Ollama is running on host
- [ ] Ensure ports 7110, 8000, 5432 are free

### Build & Deploy

- [ ] Run `docker-compose build`
- [ ] Run `docker-compose up -d`
- [ ] Wait 30-40 seconds for services startup
- [ ] Check `docker-compose ps` - all running
- [ ] Test health: `curl http://localhost:7110/health`
- [ ] Test RAG: `curl http://localhost:8000/health`
- [ ] Access Webzine: `http://localhost:7110`

### Post-Deployment

- [ ] Check database initialized: `docker-compose exec postgres psql -U webzine -d webzine -c "\dt"`
- [ ] Test chatbot widget in UI
- [ ] Verify logs: `docker-compose logs webzine`
- [ ] Set up automated backups (see POSTGRES_GUIDE.md)
- [ ] Monitor resource usage: `docker stats`

---

## 📊 Service Specifications

### Webzine (.NET 10)
- **Image**: mcr.microsoft.com/dotnet/aspnet:10.0
- **Port**: 7110
- **Endpoints**:
  - `GET /health` - Simple health check
  - `GET /api/health/ready` - Readiness check
  - `GET /api/health/detailed` - Detailed health
  - `POST /Chat/Query` - Query RAG
  - `POST /Chat/SetSource` - Change source
  - `GET /Chat/Status` - Get status
- **Volumes**: appsettings.Production.json (ro)
- **Depends On**: postgres, rag

### RAG API (Python FastAPI)
- **Image**: python:3.11-slim
- **Port**: 8000
- **Endpoints**:
  - `GET /health` - Health check
  - `GET /status` - Service status
  - `POST /query` - Query RAG
  - `POST /initialize` - Initialize source
- **Volumes**: chroma_db, data_deezer, cache
- **Depends On**: postgres, ollama (host)

### PostgreSQL 16
- **Image**: postgres:16-alpine
- **Port**: 5432
- **Database**: webzine
- **User**: webzine
- **Volumes**: postgres_data
- **Init Script**: init-db.sql
- **Tables**:
  - conversation_history
  - user_sessions
  - user_preferences
  - conversation_feedback

---

## 🔧 Common Commands

### Development

```bash
make up              # Start all services
make down            # Stop all services
make logs            # View all logs
make status          # Check container status
make restart         # Restart containers
make test-api        # Test health endpoints
```

### Database

```bash
make db-shell        # PostgreSQL shell
make db-backup       # Backup database
make db-restore      # Restore database
```

### Production

```bash
make build-prod      # Build production images
make up-prod         # Start production stack
make down-prod       # Stop production stack
```

### Utilities

```bash
make fetch-deezer    # Fetch Deezer data for RAG
make info            # Show Docker info
make clean           # Remove containers + volumes
```

---

## 🐛 Troubleshooting Quick Links

| Issue | Solution |
|-------|----------|
| **Webzine won't start** | See DOCKER_SETUP.md → Troubleshooting → "Le conteneur WebZine ne démarre pas" |
| **RAG returns 502** | See DOCKER_SETUP.md → Troubleshooting → "RAG API retourne 502" |
| **PostgreSQL connection failed** | See DOCKER_SETUP.md → Troubleshooting → "PostgreSQL ne se connecte pas" |
| **Port already in use** | QUICKSTART.md → Troubleshooting → "Port occupé?" |
| **Database issues** | POSTGRES_GUIDE.md → Troubleshooting |

---

## 📚 Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Reference](https://docs.docker.com/compose/compose-file/)
- [.NET Docker Images](https://github.com/dotnet/dotnet-docker)
- [PostgreSQL Docker Hub](https://hub.docker.com/_/postgres)
- [FastAPI Deployment](https://fastapi.tiangolo.com/deployment/docker/)
- [Ollama Documentation](https://ollama.com)

---

## 🤝 Support & Contribution

### Getting Help

1. Check relevant documentation file (see table above)
2. Review DOCKER_SETUP.md Troubleshooting section
3. Check logs: `docker-compose logs -f [service]`
4. Verify ports: `netstat -an | findstr :PORT` (Windows)

### Reporting Issues

When reporting issues, include:
- Output of `docker-compose ps`
- Relevant logs: `docker-compose logs service_name`
- Output of `validate.ps1` or `validate.sh`
- Docker version: `docker --version`
- docker-compose version: `docker-compose --version`

---

## 📝 Version History

- **v1.0** (2024-01) - Initial Docker setup
  - Multi-stage .NET build
  - FastAPI with Chroma vectors
  - PostgreSQL for conversation history
  - Health check endpoints
  - Production configuration

---

## 🎯 Next Steps

1. **Now**: Read [QUICKSTART.md](QUICKSTART.md)
2. **Then**: Run `docker-compose build && docker-compose up -d`
3. **Test**: `curl http://localhost:7110/health`
4. **Explore**: Visit http://localhost:7110 and test the chatbot
5. **Deep Dive**: Read [DOCKER_SETUP.md](DOCKER_SETUP.md) for advanced topics

---

**Happy containerizing! 🚀**

---

## 📄 File Reference

```
.
├── Dockerfile                          # .NET 10 multi-stage build
├── rag/
│   ├── Dockerfile                      # Python FastAPI image
│   ├── requirements.txt                # Python dependencies
│   ├── api.py                          # FastAPI endpoints
│   ├── pipeline.py                     # Wikipedia RAG pipeline
│   ├── pipeline_deezer.py              # Deezer RAG pipeline
│   ├── fetch_deezer_data.py            # Data ingestion
│   └── chroma_db/                      # Vector database (auto-created)
├── docker-compose.yml                  # Dev orchestration
├── docker-compose.prod.yml             # Prod orchestration
├── init-db.sql                         # PostgreSQL initialization
├── .dockerignore                       # Docker build exclusions
├── .env.example                        # Dev environment template
├── .env.prod.example                   # Prod environment template
├── Makefile                            # Command shortcuts
├── validate.sh                         # Linux/Mac validation
├── validate.ps1                        # Windows validation
├── DOCKER_SETUP.md                     # Complete guide (116 KB)
├── QUICKSTART.md                       # 5-minute setup
├── POSTGRES_GUIDE.md                   # Database integration
├── README.md                           # This file
└── Webzine/
    ├── Webzine.WebApplication/
    │   ├── appsettings.json            # Dev configuration
    │   ├── appsettings.Production.json # Prod configuration
    │   ├── Controllers/
    │   │   ├── ChatController.cs       # RAG endpoints
    │   │   └── HealthController.cs     # Health check endpoints
    │   └── Extensions/
    │       └── WebApplicationExtensions.cs  # Routing config
    └── ...
```

---

Generated: 2024-01
Status: ✅ Production Ready
