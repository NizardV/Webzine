.PHONY: help build up down logs clean restart status

help:
    @echo "🐳 Webzine Docker Stack - Available Commands"
    @echo ""
    @echo "Development:"
    @echo "  make build        - Build Docker images"
    @echo "  make up           - Start containers (development)"
    @echo "  make down         - Stop containers"
    @echo "  make logs         - View logs (all services)"
    @echo "  make status       - Check container status"
    @echo "  make restart      - Restart containers"
    @echo "  make clean        - Stop and remove containers + volumes"
    @echo ""
    @echo "Production:"
    @echo "  make build-prod   - Build production images"
    @echo "  make up-prod      - Start containers (production)"
    @echo "  make down-prod    - Stop production containers"
    @echo ""
    @echo "Database:"
    @echo "  make db-shell     - Access PostgreSQL shell"
    @echo "  make db-backup    - Backup PostgreSQL database"
    @echo "  make db-restore   - Restore PostgreSQL database"
    @echo ""
    @echo "Utilities:"
    @echo "  make fetch-deezer - Fetch Deezer data"
    @echo "  make test-api     - Test API health"
    @echo ""

# Development
build:
    docker-compose build

up:
    docker-compose up -d
    @echo "✅ Services started"
    @echo "🌐 Webzine:     http://localhost:7110"
    @echo "📡 RAG API:     http://localhost:8000"
    @echo "🗄️  PostgreSQL:  localhost:5432"

down:
    docker-compose down
    @echo "✅ Services stopped"

logs:
    docker-compose logs -f

logs-webzine:
    docker-compose logs -f webzine

logs-rag:
    docker-compose logs -f rag

logs-postgres:
    docker-compose logs -f postgres

status:
    docker-compose ps

restart:
    docker-compose restart
    @echo "✅ Services restarted"

clean:
    docker-compose down -v
    @echo "✅ Containers and volumes removed"

# Production
build-prod:
    docker-compose -f docker-compose.prod.yml build

up-prod:
    docker-compose -f docker-compose.prod.yml --env-file .env.prod up -d
    @echo "✅ Production services started"

down-prod:
    docker-compose -f docker-compose.prod.yml down

# Database
db-shell:
    docker-compose exec postgres psql -U webzine -d webzine

db-backup:
    @mkdir -p backups
    docker-compose exec postgres pg_dump -U webzine webzine > backups/webzine_$(shell date +%Y%m%d_%H%M%S).sql
    @echo "✅ Database backed up"

db-restore:
    @read -p "Enter backup file path: " backup_file; \
    docker-compose exec -T postgres psql -U webzine webzine < $$backup_file; \
    echo "✅ Database restored"

# Utilities
fetch-deezer:
    docker-compose exec rag python fetch_deezer_data.py

test-api:
    @echo "Testing Webzine health..."
    @curl -s http://localhost:7110/health | jq .
    @echo ""
    @echo "Testing RAG API health..."
    @curl -s http://localhost:8000/health | jq .
    @echo ""
    @echo "Testing PostgreSQL..."
    @docker-compose exec -T postgres psql -U webzine -d webzine -c "SELECT 'PostgreSQL OK' AS status;"

# Information
info:
    @echo "📦 Docker Images:"
    @docker images | grep webzine || echo "No images built yet"
    @echo ""
    @echo "🐳 Running Containers:"
    @docker-compose ps
    @echo ""
    @echo "📊 Volume Usage:"
    @docker volume ls | grep webzine || echo "No volumes created yet"
