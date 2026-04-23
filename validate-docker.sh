#!/bin/bash
# ============================================================
# 🐳 WEBZINE DOCKER VALIDATION & DIAGNOSTIC TOOL
# ============================================================
# Usage: ./validate-docker.sh
# Checks Docker configuration, containers, and connectivity
# ============================================================

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Counters
CHECKS_PASSED=0
CHECKS_FAILED=0

# Helper functions
print_header() {
    echo -e "\n${BLUE}═══════════════════════════════════════════════════════${NC}"
    echo -e "${BLUE}$1${NC}"
    echo -e "${BLUE}═══════════════════════════════════════════════════════${NC}\n"
}

check_passed() {
    echo -e "${GREEN}✓${NC} $1"
    ((CHECKS_PASSED++))
}

check_failed() {
    echo -e "${RED}✗${NC} $1"
    ((CHECKS_FAILED++))
}

check_warning() {
    echo -e "${YELLOW}⚠${NC} $1"
}

print_summary() {
    echo -e "\n${BLUE}═══════════════════════════════════════════════════════${NC}"
    if [ $CHECKS_FAILED -eq 0 ]; then
        echo -e "${GREEN}✓ ALL CHECKS PASSED${NC}"
    else
        echo -e "${RED}✗ $CHECKS_FAILED CHECK(S) FAILED${NC}"
    fi
    echo -e "${BLUE}Passed: ${GREEN}$CHECKS_PASSED${NC} | Failed: ${RED}$CHECKS_FAILED${NC}"
    echo -e "${BLUE}═══════════════════════════════════════════════════════${NC}\n"
}

# ============================================================
# 1. ENVIRONMENT & SYSTEM CHECKS
# ============================================================
print_header "1️⃣  ENVIRONMENT & SYSTEM CHECKS"

# Docker installation
if command -v docker &> /dev/null; then
    DOCKER_VERSION=$(docker --version)
    check_passed "Docker installed: $DOCKER_VERSION"
else
    check_failed "Docker not found - Install from https://docker.com"
    exit 1
fi

# Docker Compose installation
if command -v docker-compose &> /dev/null; then
    COMPOSE_VERSION=$(docker-compose --version)
    check_passed "Docker Compose installed: $COMPOSE_VERSION"
else
    check_failed "Docker Compose not found"
    exit 1
fi

# Docker daemon running
if docker ps &> /dev/null; then
    check_passed "Docker daemon is running"
else
    check_failed "Docker daemon is not running - Start Docker Desktop"
    exit 1
fi

# ============================================================
# 2. PROJECT STRUCTURE CHECKS
# ============================================================
print_header "2️⃣  PROJECT STRUCTURE CHECKS"

# Check required files
REQUIRED_FILES=(
    "Dockerfile"
    "docker-compose.yml"
    "docker-compose.prod.yml"
    ".env.example"
    "init-db.sql"
    "rag/Dockerfile"
    "rag/requirements.txt"
    "rag/api.py"
)

for file in "${REQUIRED_FILES[@]}"; do
    if [ -f "$file" ]; then
        check_passed "Found: $file"
    else
        check_failed "Missing: $file"
    fi
done

# ============================================================
# 3. DOCKER-COMPOSE CONFIGURATION
# ============================================================
print_header "3️⃣  DOCKER-COMPOSE CONFIGURATION"

# Validate docker-compose syntax
if docker-compose config > /dev/null 2>&1; then
    check_passed "docker-compose.yml syntax is valid"
else
    check_failed "docker-compose.yml has syntax errors"
fi

# Check for .env file
if [ -f ".env" ]; then
    check_passed ".env file exists"
    # Parse and display key variables
    echo -e "${BLUE}  Environment Variables:${NC}"
    grep -v '^#' .env | grep -v '^$' | sed 's/^/    /' | head -10
else
    check_warning ".env file not found - Run: cp .env.example .env"
fi

# ============================================================
# 4. RUNNING CONTAINERS
# ============================================================
print_header "4️⃣  RUNNING CONTAINERS"

# Check if containers exist
if docker-compose ps 2>/dev/null | grep -q webzine_app; then
    check_passed "Webzine container exists"
else
    check_warning "Webzine container not running"
fi

if docker-compose ps 2>/dev/null | grep -q webzine_rag; then
    check_passed "RAG API container exists"
else
    check_warning "RAG API container not running"
fi

if docker-compose ps 2>/dev/null | grep -q webzine_postgres; then
    check_passed "PostgreSQL container exists"
else
    check_warning "PostgreSQL container not running"
fi

# Show container status
echo -e "${BLUE}  Container Status:${NC}"
docker-compose ps 2>/dev/null | tail -n +2 | sed 's/^/    /' || echo "    No containers running"

# ============================================================
# 5. PORT AVAILABILITY
# ============================================================
print_header "5️⃣  PORT AVAILABILITY"

check_port() {
    local port=$1
    local service=$2

    if nc -z localhost "$port" 2>/dev/null; then
        check_passed "Port $port ($service) is accessible"
    else
        check_warning "Port $port ($service) is not responding (services may not be running)"
    fi
}

check_port 7110 "Webzine"
check_port 8000 "RAG API"
check_port 5432 "PostgreSQL"
check_port 11434 "Ollama (on host)"

# ============================================================
# 6. SERVICE HEALTH CHECKS
# ============================================================
print_header "6️⃣  SERVICE HEALTH CHECKS"

# Webzine health
if curl -s -o /dev/null -w "%{http_code}" http://localhost:7110/health 2>/dev/null | grep -q "200"; then
    check_passed "Webzine health endpoint responding"
else
    check_warning "Webzine health endpoint not responding (http://localhost:7110/health)"
fi

# RAG API health
if curl -s -o /dev/null -w "%{http_code}" http://localhost:8000/health 2>/dev/null | grep -q "200"; then
    check_passed "RAG API health endpoint responding"
else
    check_warning "RAG API health endpoint not responding (http://localhost:8000/health)"
fi

# PostgreSQL connection
if docker-compose exec -T postgres pg_isready -U webzine -d webzine 2>/dev/null | grep -q "accepting"; then
    check_passed "PostgreSQL is accepting connections"
else
    check_warning "PostgreSQL is not accepting connections"
fi

# Ollama availability
if curl -s -o /dev/null -w "%{http_code}" http://host.docker.internal:11434/api/tags 2>/dev/null | grep -q "200"; then
    check_passed "Ollama is accessible on host.docker.internal:11434"

    # Check available models
    MODELS=$(curl -s http://host.docker.internal:11434/api/tags 2>/dev/null | grep -o '"name":"[^"]*' | cut -d'"' -f4 | head -3)
    if [ ! -z "$MODELS" ]; then
        echo -e "${BLUE}  Available Models:${NC}"
        echo "$MODELS" | sed 's/^/    - /'
    fi
else
    check_warning "Ollama not responding - Ensure: ollama serve is running"
fi

# ============================================================
# 7. DATABASE CHECKS
# ============================================================
print_header "7️⃣  DATABASE CHECKS"

# Check database exists
if docker-compose exec -T postgres psql -U webzine -d webzine -c "\q" 2>/dev/null; then
    check_passed "Database 'webzine' exists and accessible"

    # Count tables
    TABLE_COUNT=$(docker-compose exec -T postgres psql -U webzine -d webzine -c "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public';" 2>/dev/null | tail -n 2 | head -n 1 | xargs)
    echo -e "${BLUE}  Database Info:${NC}"
    echo -e "    Tables: $TABLE_COUNT"
else
    check_warning "Cannot access database (containers may not be running)"
fi

# ============================================================
# 8. DISK USAGE
# ============================================================
print_header "8️⃣  DISK USAGE"

# Docker volumes
if command -v docker &> /dev/null; then
    VOLUME_COUNT=$(docker volume ls --quiet | wc -l)
    check_passed "Docker volumes: $VOLUME_COUNT"

    # Show webzine volumes
    echo -e "${BLUE}  Webzine Volumes:${NC}"
    docker volume ls --filter name=webzine | tail -n +2 | sed 's/^/    /'
fi

# ============================================================
# 9. LOGS ANALYSIS
# ============================================================
print_header "9️⃣  RECENT CONTAINER LOGS"

echo -e "${BLUE}  Last 5 Webzine logs:${NC}"
docker-compose logs --tail=5 webzine 2>/dev/null | tail -n 5 | sed 's/^/    /' || echo "    (No logs available)"

echo -e "\n${BLUE}  Last 5 RAG API logs:${NC}"
docker-compose logs --tail=5 rag 2>/dev/null | tail -n 5 | sed 's/^/    /' || echo "    (No logs available)"

echo -e "\n${BLUE}  Last 5 PostgreSQL logs:${NC}"
docker-compose logs --tail=5 postgres 2>/dev/null | tail -n 5 | sed 's/^/    /' || echo "    (No logs available)"

# ============================================================
# 10. RECOMMENDATIONS
# ============================================================
print_header "🔧 RECOMMENDATIONS"

if [ $CHECKS_FAILED -eq 0 ]; then
    echo "✅ Your Docker setup appears to be working correctly!"
    echo ""
    echo "Access your services:"
    echo "  • Webzine: http://localhost:7110"
    echo "  • RAG API Docs: http://localhost:8000/docs"
    echo ""
    echo "Useful commands:"
    echo "  • View logs: docker-compose logs -f"
    echo "  • Restart services: docker-compose restart"
    echo "  • Access database: docker-compose exec postgres psql -U webzine -d webzine"
else
    echo "⚠️  Please address the failing checks above."
    echo ""
    echo "Common solutions:"
    echo "  1. Ensure Ollama is running: ollama serve"
    echo "  2. Start containers: docker-compose up -d"
    echo "  3. Rebuild images: docker-compose build --no-cache"
    echo "  4. Check logs: docker-compose logs -f"
    echo "  5. Ensure ports are available (not blocked by firewall)"
fi

# ============================================================
# FINAL SUMMARY
# ============================================================
print_summary

# Exit with appropriate code
if [ $CHECKS_FAILED -eq 0 ]; then
    exit 0
else
    exit 1
fi
