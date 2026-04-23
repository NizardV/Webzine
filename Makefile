.PHONY: help docker-build docker-up docker-down docker-logs docker-ps docker-restart \
        docker-clean docker-validate docker-shell-webzine docker-shell-rag docker-shell-db \
        docker-health docker-prod-up docker-prod-down docker-volumes-clean

# ============================================================
# 🐳 WEBZINE DOCKER MAKE TARGETS
# ============================================================

## help: Show this help message
help:
	@echo "🐳 WEBZINE DOCKER COMMANDS"
	@echo ""
	@echo "Usage: make <target>"
	@echo ""
	@echo "Quick Start:"
	@echo "  make docker-build      Build all containers"
	@echo "  make docker-up         Start all services"
	@echo "  make docker-down       Stop all services"
	@echo "  make docker-ps         Show running containers"
	@echo "  make docker-logs       Stream logs from all services"
	@echo ""
	@echo "Debugging:"
	@echo "  make docker-validate   Run validation checks"
	@echo "  make docker-health     Check service health"
	@echo "  make docker-shell-*    Open shell in container"
	@echo ""
	@echo "Production:"
	@echo "  make docker-prod-up    Start production stack"
	@echo "  make docker-prod-down  Stop production stack"
	@echo ""
	@echo "Maintenance:"
	@echo "  make docker-clean      Stop and remove containers"
	@echo "  make docker-volumes-clean  Remove all volumes (⚠️ DATA LOSS)"
	@echo ""

# ============================================================
# 🚀 BASIC COMMANDS
# ============================================================

## docker-build: Build all Docker images
docker-build:
	@echo "🏗️  Building Docker images..."
	docker-compose build

## docker-build-no-cache: Build without cache
docker-build-no-cache:
	@echo "🏗️  Building Docker images (no cache)..."
	docker-compose build --no-cache

## docker-up: Start all services in background
docker-up:
	@echo "🚀 Starting services..."
	docker-compose up -d
	@echo "✅ Services started!"
	@echo "   Webzine:   http://localhost:7110"
	@echo "   RAG API:   http://localhost:8000/docs"
	@echo "   PostgreSQL: localhost:5432"

## docker-down: Stop all services
docker-down:
	@echo "⏹️  Stopping services..."
	docker-compose down

## docker-restart: Restart all services
docker-restart: docker-down docker-up

## docker-ps: Show status of all containers
docker-ps:
	@docker-compose ps

## docker-logs: Stream logs from all services
docker-logs:
	@docker-compose logs -f

## docker-logs-webzine: Stream logs from Webzine only
docker-logs-webzine:
	@docker-compose logs -f webzine

## docker-logs-rag: Stream logs from RAG API only
docker-logs-rag:
	@docker-compose logs -f rag

## docker-logs-postgres: Stream logs from PostgreSQL only
docker-logs-postgres:
	@docker-compose logs -f postgres

# ============================================================
# 🔧 DEBUGGING & DIAGNOSTICS
# ============================================================

## docker-validate: Run validation script
docker-validate:
	@echo "🔍 Running Docker validation..."
	@if command -v pwsh &> /dev/null; then \
		pwsh -NoProfile -File validate-docker.ps1; \
	elif command -v powershell &> /dev/null; then \
		powershell -NoProfile -File validate-docker.ps1; \
	else \
		bash validate-docker.sh; \
	fi

## docker-health: Check health of all services
docker-health:
	@echo "🏥 Checking service health..."
	@echo ""
	@echo "Webzine:"
	@curl -s -o /dev/null -w "  Status: %{http_code}\n" http://localhost:7110/health || echo "  ❌ Not responding"
	@echo ""
	@echo "RAG API:"
	@curl -s -o /dev/null -w "  Status: %{http_code}\n" http://localhost:8000/health || echo "  ❌ Not responding"
	@echo ""
	@echo "PostgreSQL:"
	@docker-compose exec -T postgres pg_isready -U webzine -d webzine 2>/dev/null || echo "  ❌ Not responding"
	@echo ""
	@echo "Ollama:"
	@curl -s -o /dev/null -w "  Status: %{http_code}\n" http://localhost:11434/api/tags || echo "  ❌ Not responding"

## docker-shell-webzine: Open bash shell in Webzine container
docker-shell-webzine:
	@docker-compose exec webzine bash

## docker-shell-rag: Open bash shell in RAG API container
docker-shell-rag:
	@docker-compose exec rag bash

## docker-shell-db: Open PostgreSQL CLI in database container
docker-shell-db:
	@docker-compose exec postgres psql -U webzine -d webzine

# ============================================================
# 🧹 CLEANUP & MAINTENANCE
# ============================================================

## docker-clean: Stop services and remove containers
docker-clean:
	@echo "🧹 Cleaning up Docker services..."
	docker-compose down

## docker-volumes-clean: Remove ALL volumes (⚠️ DATA LOSS)
docker-volumes-clean:
	@echo "⚠️  WARNING: This will DELETE all data in volumes!"
	@read -p "Are you sure? (y/N) " -n 1 -r; \
	echo; \
	if [[ $$REPLY =~ ^[Yy]$$ ]]; then \
		echo "🗑️  Removing volumes..."; \
		docker-compose down -v; \
		echo "✅ Volumes removed"; \
	else \
		echo "❌ Cancelled"; \
	fi

## docker-prune: Clean up unused Docker resources
docker-prune:
	@echo "🧹 Pruning unused Docker resources..."
	docker system prune -f

## docker-logs-clear: Clear logs from containers
docker-logs-clear:
	@echo "🗑️  Clearing container logs..."
	@docker-compose logs -t --no-log-prefix --tail=0 > /dev/null 2>&1 || true
	@echo "✅ Logs cleared"

# ============================================================
# 🏭 PRODUCTION
# ============================================================

## docker-prod-up: Start production stack
docker-prod-up:
	@echo "🚀 Starting production stack..."
	docker-compose -f docker-compose.prod.yml up -d
	@echo "✅ Production services started!"

## docker-prod-down: Stop production stack
docker-prod-down:
	@echo "⏹️  Stopping production stack..."
	docker-compose -f docker-compose.prod.yml down

## docker-prod-logs: Stream logs from production stack
docker-prod-logs:
	@docker-compose -f docker-compose.prod.yml logs -f

## docker-prod-ps: Show production containers status
docker-prod-ps:
	@docker-compose -f docker-compose.prod.yml ps

# ============================================================
# 📊 MONITORING & STATS
# ============================================================

## docker-stats: Show real-time container resource usage
docker-stats:
	@docker stats

## docker-inspect-webzine: Show detailed info about Webzine container
docker-inspect-webzine:
	@docker-compose exec -T webzine dotnet --info

## docker-inspect-rag: Show detailed info about RAG API
docker-inspect-rag:
	@docker-compose exec -T rag pip list

## docker-backup-db: Backup PostgreSQL database
docker-backup-db:
	@echo "💾 Backing up PostgreSQL database..."
	@mkdir -p ./backups
	docker-compose exec -T postgres pg_dump -U webzine -d webzine > ./backups/backup-$$(date +%Y%m%d-%H%M%S).sql
	@echo "✅ Backup created in ./backups/"

## docker-restore-db: Restore PostgreSQL database from backup
docker-restore-db:
	@echo "⚠️  Enter backup file path:"
	@read BACKUP_FILE; \
	docker-compose exec -T postgres psql -U webzine -d webzine < $$BACKUP_FILE
	@echo "✅ Database restored"

# ============================================================
# 🧪 TESTING & VALIDATION
# ============================================================

## docker-test-endpoints: Test all service endpoints
docker-test-endpoints:
	@echo "🧪 Testing service endpoints..."
	@echo ""
	@echo "Testing Webzine (http://localhost:7110):"
	@curl -v http://localhost:7110/health 2>&1 | grep -E "HTTP|health"
	@echo ""
	@echo "Testing RAG API (http://localhost:8000):"
	@curl -v http://localhost:8000/health 2>&1 | grep -E "HTTP|health"
	@echo ""
	@echo "Testing PostgreSQL (localhost:5432):"
	@docker-compose exec -T postgres pg_isready -U webzine -d webzine
	@echo ""

## docker-ping-services: Verify inter-service connectivity
docker-ping-services:
	@echo "🔗 Testing inter-service connectivity..."
	@echo ""
	@echo "Webzine → RAG API:"
	@docker-compose exec webzine ping -c 1 rag || echo "Failed"
	@echo ""
	@echo "Webzine → PostgreSQL:"
	@docker-compose exec webzine ping -c 1 postgres || echo "Failed"
	@echo ""
	@echo "RAG API → PostgreSQL:"
	@docker-compose exec rag ping -c 1 postgres || echo "Failed"
	@echo ""

# ============================================================
# 📚 DOCUMENTATION
# ============================================================

## docker-docs: Open Docker deployment documentation
docker-docs:
	@echo "📖 Docker Documentation:"
	@echo "  Quick Start:    DOCKER_README.md"
	@echo "  Complete Guide: DOCKER_DEPLOYMENT.md"
	@echo "  Consolidation:  DOCKER_CONSOLIDATION.md"
	@echo ""

# ============================================================
# 🆘 TROUBLESHOOTING
# ============================================================

## docker-troubleshoot: Run full troubleshooting check
docker-troubleshoot: docker-validate docker-health docker-ps
	@echo ""
	@echo "✅ Troubleshooting complete!"
	@echo ""
	@echo "Next steps:"
	@echo "  1. Check logs: make docker-logs"
	@echo "  2. See documentation: make docker-docs"
	@echo ""

.DEFAULT_GOAL := help
