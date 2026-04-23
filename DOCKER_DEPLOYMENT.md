# 🐳 WEBZINE DOCKER DEPLOYMENT GUIDE

**This is the single source of truth for deploying the Webzine stack with Docker
Compose.**

---

## 📋 Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Prerequisites](#prerequisites)
3. [Quick Start (5 minutes)](#quick-start-5-minutes)
4. [Detailed Setup](#detailed-setup)
5. [Configuration](#configuration)
6. [Commands Reference](#commands-reference)
7. [Troubleshooting](#troubleshooting)
8. [Production Deployment](#production-deployment)

---

## 🏗️ Architecture Overview

The Webzine stack consists of **3 containerized services** orchestrated by
Docker Compose:

```
┌─────────────────────────────────────────────────────┐
│            Docker Compose Network                   │
│          (webzine_network)                          │
├─────────────────────────────────────────────────────┤
│                                                     │
│  ┌─────────────────┐  ┌──────────────┐             │
│  │  Webzine        │  │  RAG API     │             │
│  │  .NET 10        │←→│  Python      │             │
│  │  :7110          │  │  :8000       │             │
│  └────────┬────────┘  └──────┬───────┘             │
│           │                  │                     │
│           └──────────────┬───┘                     │
│                          ↓                         │
│            ┌──────────────────────────┐            │
│            │   PostgreSQL 16          │            │
│            │   :5432                  │            │
│            │   (webzine database)     │            │
│            └──────────────────────────┘            │
│                                                     │
│  External: Ollama LLM :11434 (on host)             │
│            via host.docker.internal                │
└─────────────────────────────────────────────────────┘
```

### Service Details

| Service        | Port  | Technology           | Purpose                            |
| -------------- | ----- | -------------------- | ---------------------------------- |
| **Webzine**    | 7110  | .NET 10 Razor Pages  | Web UI with chatbot widget         |
| **RAG API**    | 8000  | Python FastAPI       | Retrieval-Augmented Generation API |
| **PostgreSQL** | 5432  | PostgreSQL 16 Alpine | Conversation history & user data   |
| **Ollama**     | 11434 | External (host)      | LLM inference engine               |

---

## ✅ Prerequisites

### System Requirements

- **OS**: Windows / macOS / Linux
- **Docker Desktop**: v24.0 or newer
- **Docker Compose**: v2.20 or newer
- **RAM**: 4GB minimum (6GB recommended)
- **Disk Space**: 3GB minimum
- **Ollama**: Running on host (v0.1.0+)

### Installation

```bash
# Windows
# Download Docker Desktop from https://www.docker.com/products/docker-desktop

# macOS
brew install docker

# Linux (Ubuntu/Debian)
sudo apt-get install docker-ce docker-compose-plugin

# Verify installation
docker --version
docker-compose --version
```

### Starting Ollama

```bash
# Download from https://ollama.ai
# Then run (will serve on localhost:11434)
ollama serve

# In another terminal, pull a model
ollama pull llama2  # or llama3.2, mistral, etc.
```

---

## 🚀 Quick Start (5 minutes)

### Step 1: Prepare Environment

```bash
cd /path/to/Webzine
cp Webzine/.env.example .env
```

### Step 2: Build & Launch

```bash
# Build all images
docker-compose build

# Start all services
docker-compose up -d

# Verify containers are running
docker-compose ps
```

### Step 3: Verify Services

```bash
# Check health
curl http://localhost:7110/health
curl http://localhost:8000/health

# Access applications
# Webzine: http://localhost:7110
# RAG API Docs: http://localhost:8000/docs
```

✅ **Done!** Your stack is now running.

---

## 🔧 Detailed Setup

### 1. Initial Configuration

**Clone or prepare your project:**

```bash
cd /path/to/Webzine
```

**Verify directory structure:**

```
Webzine/
├── Dockerfile              # .NET application
├── docker-compose.yml      # Development
├── docker-compose.prod.yml # Production
├── init-db.sql            # Database schema
├── .env.example           # Environment template
├── rag/
│   ├── Dockerfile         # Python RAG service
│   ├── requirements.txt    # Python dependencies
│   ├── api.py            # FastAPI application
│   └── chroma_db/        # Vector database
├── Webzine.WebApplication/
└── ... (other projects)
```

### 2. Create Environment File

**Copy the template:**

```bash
cp Webzine/.env.example .env
```

**Edit `.env` for your setup:**

```bash
# PostgreSQL
POSTGRES_DB=webzine
POSTGRES_USER=webzine
POSTGRES_PASSWORD=your_secure_password  # Change this!

# .NET
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:7110
RAG_API_URL=http://rag:8000

# Connection string (must match PostgreSQL settings)
POSTGRES_CONNECTION_STRING=Server=postgres;Port=5432;Database=webzine;User Id=webzine;Password=your_secure_password;

# Python
PYTHONUNBUFFERED=1
OLLAMA_HOST=http://host.docker.internal:11434

# Logging
LOGLEVEL=Information
```

### 3. Build Images

```bash
# Build all images
docker-compose build

# Or build specific services
docker-compose build webzine
docker-compose build rag
docker-compose build postgres
```

### 4. Start Services

```bash
# Start in background
docker-compose up -d

# Or start and watch logs
docker-compose up
```

### 5. Verify Health

```bash
# Check running containers
docker-compose ps

# View combined logs
docker-compose logs -f

# Check individual services
docker-compose logs -f webzine
docker-compose logs -f rag
docker-compose logs -f postgres
```

---

## ⚙️ Configuration

### Environment Variables

**Key Variables:**

| Variable                     | Value                               | Description                   |
| ---------------------------- | ----------------------------------- | ----------------------------- |
| `ASPNETCORE_ENVIRONMENT`     | `Production`                        | .NET environment mode         |
| `RAG_API_URL`                | `http://rag:8000`                   | RAG API endpoint (Docker DNS) |
| `POSTGRES_CONNECTION_STRING` | See below                           | Database connection           |
| `OLLAMA_HOST`                | `http://host.docker.internal:11434` | LLM service endpoint          |

**PostgreSQL Connection String:**

```
Server=postgres;Port=5432;Database=webzine;User Id=webzine;Password=webzine_password;
```

**Ollama Endpoint:**

- **Windows/macOS**: `http://host.docker.internal:11434` (Docker Desktop)
- **Linux**: `http://localhost:11434` (needs Ollama on host) or
  `http://ollama:11434` (if containerized)

### Ollama Setup

**Start Ollama on host:**

```bash
# Download from https://ollama.ai
ollama serve
```

**Pull a model (in another terminal):**

```bash
ollama pull llama2      # Small, fast
ollama pull llama3.2    # Medium
ollama pull mistral     # Fast, decent quality
ollama pull neural-chat # Good for chat
```

**Check available models:**

```bash
ollama list
```

### Database Initialization

The database is automatically initialized from [init-db.sql](init-db.sql):

```bash
# View initialization script
cat init-db.sql

# Manually initialize (if needed)
docker-compose exec postgres psql -U webzine -d webzine -f /docker-entrypoint-initdb.d/01-init.sql
```

---

## 📚 Commands Reference

### Container Management

```bash
# Start services
docker-compose up -d

# Stop services
docker-compose down

# Stop and remove volumes (⚠️ destructive)
docker-compose down -v

# Restart services
docker-compose restart

# Restart specific service
docker-compose restart webzine

# Rebuild images
docker-compose build --no-cache

# Full rebuild and restart
docker-compose down -v
docker-compose build --no-cache
docker-compose up -d
```

### Logs & Debugging

```bash
# View all logs
docker-compose logs

# Follow logs in real-time
docker-compose logs -f

# Last 100 lines
docker-compose logs --tail=100

# Specific service
docker-compose logs -f webzine
docker-compose logs -f rag
docker-compose logs -f postgres

# Logs since specific time
docker-compose logs --since 2024-01-15T10:00:00
```

### Container Access

```bash
# Bash shell in container
docker-compose exec webzine bash
docker-compose exec rag bash

# PostgreSQL CLI
docker-compose exec postgres psql -U webzine -d webzine

# Python shell in RAG container
docker-compose exec rag python
```

### Health & Validation

```bash
# Check container status
docker-compose ps

# Health status
docker-compose exec webzine curl http://localhost:7110/health
docker-compose exec rag curl http://localhost:8000/health

# Database connection test
docker-compose exec postgres psql -U webzine -d webzine -c "SELECT version();"

# View container resource usage
docker stats

# Inspect service details
docker-compose exec webzine dotnet --info
docker-compose exec rag pip list
```

### Volumes & Data

```bash
# List volumes
docker volume ls

# Inspect volume
docker volume inspect webzine_postgres_data

# Clean unused volumes
docker volume prune

# Backup PostgreSQL
docker-compose exec postgres pg_dump -U webzine -d webzine > backup.sql

# Restore PostgreSQL
docker-compose exec -T postgres psql -U webzine -d webzine < backup.sql
```

---

## 🐛 Troubleshooting

### Common Issues

#### 1. **Connection Refused on localhost:7110**

```bash
# Check if container is running
docker-compose ps

# Check logs
docker-compose logs webzine

# Verify port mapping
docker-compose port webzine
```

**Solution:**

- Wait 30-60 seconds for startup
- Check health: `curl http://localhost:7110/health`
- Verify port not in use: `netstat -ano | findstr :7110` (Windows)

#### 2. **RAG API Not Responding**

```bash
# Check RAG service
docker-compose logs rag

# Verify Ollama is running
curl http://host.docker.internal:11434/api/tags

# Test RAG health
curl http://localhost:8000/health
```

**Solution:**

- Ensure Ollama is running on host
- Check OLLAMA_HOST env var
- Rebuild RAG image: `docker-compose build --no-cache rag`

#### 3. **PostgreSQL Connection Error**

```bash
# Check Postgres logs
docker-compose logs postgres

# Test connection
docker-compose exec postgres psql -U webzine -d webzine -c "SELECT 1;"
```

**Solution:**

- Verify POSTGRES_PASSWORD in .env
- Check connection string format
- Ensure database exists: `docker-compose exec postgres psql -U webzine -c "\l"`

#### 4. **Out of Memory**

```bash
# Check resource usage
docker stats

# Reduce container limits
# Edit docker-compose.yml and add resource limits
```

**Solution:**

- Increase Docker memory allocation
- Use production config: `docker-compose -f docker-compose.prod.yml up -d`

#### 5. **Image Build Failures**

```bash
# Clear build cache
docker builder prune

# Rebuild with verbose output
docker-compose build --no-cache --verbose

# Check Dockerfile syntax
docker buildx build --dry-run .
```

### Debug Commands

```bash
# Inspect a running container
docker inspect webzine_app

# Check network connectivity
docker-compose exec webzine ping rag
docker-compose exec rag ping postgres

# View Docker daemon logs (macOS/Linux)
docker logs <container-id>

# Validate docker-compose syntax
docker-compose config
```

---

## 🏭 Production Deployment

### Using Production Configuration

```bash
# Use production docker-compose file
docker-compose -f docker-compose.prod.yml up -d

# Or set env for consistency
docker-compose -f docker-compose.prod.yml -f .env.prod up -d
```

### Production `.env` File

Create `.env.prod`:

```bash
# 🔒 Security-First Configuration
POSTGRES_PASSWORD=<VERY_STRONG_PASSWORD_HERE>
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:7110
RAG_API_URL=http://rag:8000
POSTGRES_CONNECTION_STRING=Server=postgres;Port=5432;Database=webzine;User Id=webzine;Password=<VERY_STRONG_PASSWORD_HERE>;

# Logging
LOGLEVEL=Warning

# Performance
PYTHONUNBUFFERED=1
OLLAMA_HOST=http://host.docker.internal:11434
```

### Production Best Practices

1. **Use Strong Credentials**

    ```bash
    # Generate strong password
    openssl rand -base64 32
    ```

2. **Enable SSL/TLS**

    ```
    # Configure reverse proxy (nginx/traefik)
    # Use HTTPS for external access
    ```

3. **Resource Limits**

    ```yaml
    # docker-compose.prod.yml includes limits
    deploy:
        resources:
            limits:
                cpus: "2"
                memory: 2G
    ```

4. **Monitoring & Logging**

    ```bash
    # Enable Docker logs driver
    logging:
      driver: "json-file"
      options:
        max-size: "10m"
        max-file: "3"
    ```

5. **Backup Strategy**

    ```bash
    # Regular PostgreSQL backups
    docker-compose -f docker-compose.prod.yml exec postgres \
      pg_dump -U webzine -d webzine > backup-$(date +%Y%m%d).sql
    ```

6. **Health Monitoring**
    ```bash
    # Check health regularly
    watch -n 5 'docker-compose ps'
    ```

---

## 📞 Support & Resources

- **Docker Docs**: https://docs.docker.com
- **Docker Compose**: https://docs.docker.com/compose/reference/
- **Ollama**: https://ollama.ai
- **FastAPI**: https://fastapi.tiangolo.com
- **PostgreSQL**: https://www.postgresql.org/docs/

---

## ✨ Key Takeaways

✅ **Use this file as the single source of truth for Docker deployment**

✅ **Quick Start**: 3 commands in 5 minutes

✅ **Production Ready**: Use `docker-compose.prod.yml` for production

✅ **Monitoring**: Use `docker-compose logs -f` to watch services

✅ **Troubleshooting**: Check logs first, use debug commands for deep issues

---

**Last Updated**: 2024 **Version**: 1.0 **Status**: Production Ready ✅
