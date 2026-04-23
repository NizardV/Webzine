# 🐳 Docker Deployment - Webzine Stack

**👉 START HERE: [DOCKER_DEPLOYMENT.md](DOCKER_DEPLOYMENT.md)** ← Complete guide

---

## 🚀 Quick Start (2 minutes)

```bash
# 1. Copy environment configuration
cp Webzine/.env.example .env

# 2. Build and start services
docker-compose build
docker-compose up -d

# 3. Verify setup
.\validate-docker.ps1           # Windows PowerShell
# OR
bash validate-docker.sh         # Linux/macOS

# 4. Access applications
# Webzine: http://localhost:7110
# RAG API: http://localhost:8000/docs
```

---

## 📚 Documentation Index

| Document                                           | Purpose                                                     |
| -------------------------------------------------- | ----------------------------------------------------------- |
| **[DOCKER_DEPLOYMENT.md](DOCKER_DEPLOYMENT.md)**   | 📖 **PRIMARY** - Complete deployment guide (go here first!) |
| [docker-compose.yml](docker-compose.yml)           | Development environment configuration                       |
| [docker-compose.prod.yml](docker-compose.prod.yml) | Production environment configuration                        |
| [.env.example](.env.example)                       | Environment variables template                              |
| [init-db.sql](init-db.sql)                         | PostgreSQL database schema                                  |

---

## 🔧 Validation & Diagnostics

### Automated Validation

```bash
# Windows (PowerShell)
.\validate-docker.ps1

# Linux/macOS (Bash)
bash validate-docker.sh
```

These scripts check:

- ✅ Docker & Docker Compose installation
- ✅ Project structure & required files
- ✅ Container status & health
- ✅ Port availability
- ✅ Service connectivity
- ✅ Database access
- ✅ Ollama availability
- ✅ Recent logs

---

## 📦 Stack Architecture

```
┌────────────────────────────────────────┐
│     Docker Compose Network             │
├────────────────────────────────────────┤
│                                        │
│  Webzine (.NET)   RAG API (Python)    │
│      :7110              :8000          │
│         │                  │           │
│         └──────────┬───────┘           │
│                    ↓                   │
│           PostgreSQL :5432             │
│                                        │
│  Ollama :11434 (on host)              │
│  via host.docker.internal              │
└────────────────────────────────────────┘
```

---

## 🎯 Common Commands

```bash
# Start/Stop
docker-compose up -d       # Start in background
docker-compose down        # Stop containers
docker-compose restart     # Restart all services

# View Logs
docker-compose logs -f                    # All services
docker-compose logs -f webzine            # Specific service
docker-compose logs --tail=50             # Last 50 lines

# Access Containers
docker-compose exec webzine bash          # .NET container
docker-compose exec rag bash              # Python container
docker-compose exec postgres psql -U webzine -d webzine  # Database

# Health Checks
curl http://localhost:7110/health         # Webzine
curl http://localhost:8000/health         # RAG API
docker-compose ps                         # Container status
```

---

## ⚙️ Configuration

### Environment Variables (`.env`)

```bash
POSTGRES_PASSWORD=your_password
ASPNETCORE_ENVIRONMENT=Production
RAG_API_URL=http://rag:8000
OLLAMA_HOST=http://host.docker.internal:11434
```

### Ollama Setup

```bash
# Terminal 1: Start Ollama
ollama serve

# Terminal 2: Pull a model
ollama pull llama2          # Small & fast
ollama pull llama3.2        # Better quality
ollama list                 # Check available models
```

---

## 🆘 Troubleshooting

### Containers not starting?

```bash
docker-compose logs -f
docker-compose build --no-cache
docker-compose up -d
```

### Connection refused?

```bash
# Wait 30-60 seconds for services to start
docker-compose ps
curl http://localhost:7110/health
```

### Ollama not responding?

```bash
# Ensure Ollama is running:
ollama serve

# Verify accessibility:
curl http://localhost:11434/api/tags
```

### Database connection error?

```bash
# Check PostgreSQL logs:
docker-compose logs postgres

# Test connection:
docker-compose exec postgres psql -U webzine -d webzine -c "SELECT 1;"
```

---

## 🚀 Production Deployment

For production, use the dedicated configuration:

```bash
docker-compose -f docker-compose.prod.yml up -d
```

Key differences:

- Resource limits applied
- Stronger security settings
- Persistent logging configuration
- Production environment variables

See
[DOCKER_DEPLOYMENT.md → Production Deployment](DOCKER_DEPLOYMENT.md#-production-deployment)
for details.

---

## 📞 Need Help?

1. **Read**: [DOCKER_DEPLOYMENT.md](DOCKER_DEPLOYMENT.md)
2. **Check Logs**: `docker-compose logs -f`
3. **Run Validation**: `.\validate-docker.ps1` or `bash validate-docker.sh`
4. **Review Troubleshooting**: See section above

---

**Version**: 1.0 | **Status**: Production Ready ✅
