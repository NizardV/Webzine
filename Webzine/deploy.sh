#!/usr/bin/env bash
# Webzine Docker Stack - One-line deployment
# Run this script to deploy everything

set -e

echo "🐳 Webzine Docker Stack - Full Deployment"
echo "=========================================="
echo ""

# Colors
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m'

# Step 1: Validation
echo -e "${BLUE}Step 1: Validating environment...${NC}"
if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "win32" ]]; then
    echo "Running on Windows - use: powershell -ExecutionPolicy Bypass -File validate.ps1"
else
    bash validate.sh || echo "⚠️  Some checks may have failed"
fi
echo ""

# Step 2: Build
echo -e "${BLUE}Step 2: Building Docker images...${NC}"
docker-compose build
echo ""

# Step 3: Start services
echo -e "${BLUE}Step 3: Starting services...${NC}"
docker-compose up -d
echo ""

# Step 4: Wait for services
echo -e "${BLUE}Step 4: Waiting for services to start (30-40 seconds)...${NC}"
sleep 10
for i in {1..4}; do
    echo "⏳ Waiting... ($((i*10)) seconds)"
    sleep 10
done
echo ""

# Step 5: Verify
echo -e "${BLUE}Step 5: Verifying deployment...${NC}"
echo ""
echo "Service Status:"
docker-compose ps
echo ""

echo "Health Checks:"
echo -n "Webzine: "
curl -s http://localhost:7110/health | grep -q "healthy" && echo "✅ Healthy" || echo "⏳ Starting..."
echo -n "RAG API: "
curl -s http://localhost:8000/health | grep -q "healthy" && echo "✅ Healthy" || echo "⏳ Starting..."
echo ""

echo -e "${GREEN}✅ Deployment Complete!${NC}"
echo ""
echo "📍 Access Your Application:"
echo "   • Webzine Web: http://localhost:7110"
echo "   • RAG API: http://localhost:8000/docs"
echo "   • Database: localhost:5432 (webzine / webzine_password)"
echo ""
echo "📚 Documentation:"
echo "   • Quick Start: QUICKSTART.md"
echo "   • Full Guide: DOCKER_SETUP.md"
echo "   • Commands: make help"
echo ""
echo "🔍 Useful Commands:"
echo "   • Logs: docker-compose logs -f"
echo "   • Stop: docker-compose down"
echo "   • Database: docker-compose exec postgres psql -U webzine -d webzine"
echo ""
echo "🚀 Ready to use! Visit http://localhost:7110"
