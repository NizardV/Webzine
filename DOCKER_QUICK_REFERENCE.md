# 🐳 DOCKER QUICK REFERENCE CARD

**Print this page or keep it handy for quick command reference**

---

## 🚀 Quick Start (Copy & Paste)

```bash
# Setup (one-time)
cp Webzine/.env.example .env
docker-compose build
docker-compose up -d

# Verify
docker-compose ps
curl http://localhost:7110/health
curl http://localhost:8000/health
```

---

## 📋 Using Make Commands (Linux/macOS)

```bash
# Start
make docker-up           # Start services
make docker-down         # Stop services
make docker-restart      # Restart everything
make docker-build        # Build images

# Monitoring
make docker-ps           # Show containers
make docker-logs         # Watch logs
make docker-health       # Check health
make docker-validate     # Run diagnostics

# Debugging
make docker-shell-webzine   # Bash in .NET container
make docker-shell-rag       # Bash in Python container
make docker-shell-db        # PostgreSQL CLI

# Help
make help                # Show all commands
```

---

## 🏗️ Direct Docker Compose Commands

| Command                          | Purpose          |
| -------------------------------- | ---------------- |
| `docker-compose build`           | Build images     |
| `docker-compose up -d`           | Start services   |
| `docker-compose down`            | Stop services    |
| `docker-compose restart`         | Restart all      |
| `docker-compose ps`              | Show status      |
| `docker-compose logs -f`         | View logs        |
| `docker-compose logs -f webzine` | Logs for service |

---

## 🔍 Debugging

### View Logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f webzine
docker-compose logs -f rag
docker-compose logs -f postgres

# Last 50 lines
docker-compose logs --tail=50
```

### Access Container Shell

```bash
# .NET Webzine
docker-compose exec webzine bash

# Python RAG
docker-compose exec rag bash

# PostgreSQL
docker-compose exec postgres psql -U webzine -d webzine
```

### Check Health

```bash
# HTTP endpoints
curl http://localhost:7110/health
curl http://localhost:8000/health

# Database
docker-compose exec postgres pg_isready -U webzine -d webzine

# Containers
docker-compose ps
```

---

## ⚙️ Configuration

### .env File (Create from example)

```bash
cp Webzine/.env.example .env
```

### Key Variables

```bash
POSTGRES_PASSWORD=webzine_password
ASPNETCORE_ENVIRONMENT=Production
RAG_API_URL=http://rag:8000
OLLAMA_HOST=http://host.docker.internal:11434
```

### Ollama Setup

```bash
# Terminal 1: Start server
ollama serve

# Terminal 2: Pull model
ollama pull llama2
ollama list
```

---

## 🆘 Common Issues

### "Connection refused" on port 7110

```bash
# Wait for startup (30-60 seconds)
sleep 30
curl http://localhost:7110/health

# Or check logs
docker-compose logs webzine
```

### Ollama not responding

```bash
# Ensure running on host
ollama serve

# Verify connectivity
curl http://localhost:11434/api/tags
```

### PostgreSQL won't connect

```bash
# Check logs
docker-compose logs postgres

# Verify connection
docker-compose exec postgres psql -U webzine -d webzine -c "SELECT 1;"
```

### Container build fails

```bash
# Rebuild without cache
docker-compose build --no-cache

# Full restart
docker-compose down -v
docker-compose build --no-cache
docker-compose up -d
```

---

## 📊 Service Ports & URLs

| Service    | Port  | URL                        |
| ---------- | ----- | -------------------------- |
| Webzine    | 7110  | http://localhost:7110      |
| RAG API    | 8000  | http://localhost:8000/docs |
| PostgreSQL | 5432  | localhost:5432             |
| Ollama     | 11434 | localhost:11434 (host)     |

---

## 🏭 Production Commands

```bash
# Start production stack
docker-compose -f docker-compose.prod.yml up -d

# View logs
docker-compose -f docker-compose.prod.yml logs -f

# Stop production
docker-compose -f docker-compose.prod.yml down
```

---

## 💾 Database Operations

```bash
# Backup
docker-compose exec -T postgres pg_dump -U webzine -d webzine > backup.sql

# Restore
docker-compose exec -T postgres psql -U webzine -d webzine < backup.sql

# Check tables
docker-compose exec postgres psql -U webzine -d webzine -c "\dt"

# View logs
docker-compose logs postgres
```

---

## 🧹 Cleanup

```bash
# Stop containers
docker-compose down

# Remove containers & volumes (⚠️ DATA LOSS)
docker-compose down -v

# Clean unused resources
docker system prune -f

# Remove specific volume
docker volume rm webzine_postgres_data
```

---

## 🔗 Docker Networking (Internal)

These names work INSIDE Docker containers:

- `postgres` → PostgreSQL server
- `rag` → RAG API service
- `webzine` → Webzine application

Example from inside a container:

```bash
# From webzine container
curl http://rag:8000/health
psql -h postgres -U webzine -d webzine
```

---

## 🆘 Quick Troubleshooting Flow

1. **Check container status**

    ```bash
    docker-compose ps
    ```

2. **View recent logs**

    ```bash
    docker-compose logs --tail=50 -f
    ```

3. **Test service health**

    ```bash
    curl http://localhost:7110/health
    ```

4. **Run full diagnostics**

    ```bash
    # Windows
    .\validate-docker.ps1

    # Linux/macOS
    bash validate-docker.sh
    ```

5. **Restart if needed**
    ```bash
    docker-compose restart
    ```

---

## 📚 Full Documentation

- **Quick Start**: [DOCKER_README.md](DOCKER_README.md)
- **Complete Guide**: [DOCKER_DEPLOYMENT.md](DOCKER_DEPLOYMENT.md)
- **Migration Path**: [DOCKER_CONSOLIDATION.md](DOCKER_CONSOLIDATION.md)

---

## ⏱️ Command Reference by Time

| Need            | Command                           | Time     |
| --------------- | --------------------------------- | -------- |
| Quick start     | `docker-compose up -d`            | 1 min    |
| Full rebuild    | `docker-compose build --no-cache` | 5-10 min |
| Check status    | `docker-compose ps`               | 10 sec   |
| View logs       | `docker-compose logs -f`          | 10 sec   |
| Full validation | `validate-docker.ps1`             | 30 sec   |

---

**TIP**: Pin this file in your editor or print it out! 📌

**Last Updated**: 2024-04-22
