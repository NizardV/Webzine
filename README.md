# 📚 WEBZINE - Docker Stack Deployment Documentation

**Complete project documentation and deployment guides for Webzine with Docker**

---

## 🚀 Quick Navigation

### 🐳 **DOCKER DEPLOYMENT** (👈 Start Here!)

| Document                                                   | Purpose                                 | Read Time |
| ---------------------------------------------------------- | --------------------------------------- | --------- |
| **[DOCKER_README.md](DOCKER_README.md)**                   | 👈 **Quick Start & Commands Reference** | 5 min     |
| **[DOCKER_DEPLOYMENT.md](DOCKER_DEPLOYMENT.md)**           | Complete Deployment Guide               | 15 min    |
| **[DOCKER_QUICK_REFERENCE.md](DOCKER_QUICK_REFERENCE.md)** | Command Cheat Sheet                     | 2 min     |
| **[DOCKER_CONSOLIDATION.md](DOCKER_CONSOLIDATION.md)**     | Documentation Organization Guide        | 5 min     |

### 🔧 **Tools & Scripts**

| Tool                      | Purpose                          |
| ------------------------- | -------------------------------- |
| **`validate-docker.ps1`** | Windows PowerShell diagnostics   |
| **`validate-docker.sh`**  | Linux/macOS Bash diagnostics     |
| **`Makefile`**            | Make targets for common commands |

---

## 📖 Getting Started

### 1️⃣ Read the Quick Start

👉 **[DOCKER_README.md](DOCKER_README.md)** - 5 minute overview with commands

### 2️⃣ Set Up Environment

```bash
cp Webzine/.env.example .env
```

### 3️⃣ Deploy

```bash
docker-compose build && docker-compose up -d
```

### 4️⃣ Verify

```bash
docker-compose ps
.\validate-docker.ps1  # Windows
bash validate-docker.sh  # Linux/macOS
```

### Access Services

- **Webzine**: http://localhost:7110
- **RAG API**: http://localhost:8000/docs
- **Database**: localhost:5432

---

## 🏗️ Stack Architecture

```
Webzine (.NET)  →  RAG API (Python)  →  PostgreSQL Database
    :7110              :8000               :5432
                         ↓
                    Ollama LLM
                   (on host)
```

---

## 💡 Quick Commands

```bash
docker-compose up -d       # Start
docker-compose down        # Stop
docker-compose logs -f     # Logs
docker-compose ps          # Status
make help                  # Make targets (Linux/macOS)
```

---

## ⚙️ Prerequisites

- **Docker Desktop** v24.0+
- **Ollama** (download: https://ollama.ai)
- 4GB+ RAM, 3GB+ disk space

---

## 📚 Documentation Index

**Docker & Deployment**:

- [DOCKER_README.md](DOCKER_README.md) - Quick start
- [DOCKER_DEPLOYMENT.md](DOCKER_DEPLOYMENT.md) - Complete guide
- [DOCKER_QUICK_REFERENCE.md](DOCKER_QUICK_REFERENCE.md) - Cheat sheet
- [DOCKER_CONSOLIDATION.md](DOCKER_CONSOLIDATION.md) - Organization guide

**Configuration**:

- [docker-compose.yml](docker-compose.yml)
- [docker-compose.prod.yml](docker-compose.prod.yml)
- [.env.example](.env.example)

**Architecture**:

- [Webzine/ARCHITECTURE.md](Webzine/ARCHITECTURE.md)

---

## 🆘 Troubleshooting

```bash
# View logs
docker-compose logs -f

# Full diagnostics
.\validate-docker.ps1           # Windows
bash validate-docker.sh         # Linux/macOS

# Rebuild
docker-compose build --no-cache
```

See
[DOCKER_DEPLOYMENT.md → Troubleshooting](DOCKER_DEPLOYMENT.md#-troubleshooting)
for detailed help.

---

## ✨ Status

✅ **Production Ready** | 📖 **Well Documented** | 🔧 **Easy to Deploy**

👉 **Start here**: [DOCKER_README.md](DOCKER_README.md)
