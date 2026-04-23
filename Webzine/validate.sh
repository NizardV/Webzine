#!/bin/bash

# Color codes
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}🔍 Webzine Docker Stack - Pre-Deployment Validation${NC}\n"

ERRORS=0
WARNINGS=0
SUCCESS=0

# Check functions
check_command() {
    if command -v $1 &> /dev/null; then
        echo -e "${GREEN}✓${NC} $1 installed"
        ((SUCCESS++))
    else
        echo -e "${RED}✗${NC} $1 NOT FOUND - Install it: $2"
        ((ERRORS++))
    fi
}

check_file() {
    if [ -f "$1" ]; then
        echo -e "${GREEN}✓${NC} $1 exists"
        ((SUCCESS++))
    else
        echo -e "${RED}✗${NC} $1 MISSING"
        ((ERRORS++))
    fi
}

check_port() {
    PORT=$1
    if lsof -Pi :$PORT -sTCP:LISTEN -t >/dev/null ; then
        echo -e "${YELLOW}⚠${NC} Port $PORT already in use"
        ((WARNINGS++))
    else
        echo -e "${GREEN}✓${NC} Port $PORT available"
        ((SUCCESS++))
    fi
}

# 1. Check Required Tools
echo -e "${BLUE}1️⃣  Checking Required Tools${NC}"
check_command "docker" "https://docs.docker.com/install/"
check_command "docker-compose" "https://docs.docker.com/compose/install/"
check_command "dotnet" "https://dotnet.microsoft.com/download"
check_command "python3" "https://www.python.org/downloads/"
echo ""

# 2. Check Configuration Files
echo -e "${BLUE}2️⃣  Checking Configuration Files${NC}"
check_file "Dockerfile"
check_file "rag/Dockerfile"
check_file "docker-compose.yml"
check_file "init-db.sql"
check_file ".dockerignore"
check_file "Webzine/appsettings.json"
check_file "Webzine/appsettings.Production.json"
echo ""

# 3. Check .NET Project
echo -e "${BLUE}3️⃣  Checking .NET Project${NC}"
check_file "Webzine/Webzine.WebApplication/Webzine.WebApplication.csproj"
check_file "Webzine/Webzine.WebApplication/Controllers/ChatController.cs"
check_file "Webzine/Webzine.WebApplication/Controllers/HealthController.cs"
echo ""

# 4. Check Python RAG
echo -e "${BLUE}4️⃣  Checking Python RAG${NC}"
check_file "rag/api.py"
check_file "rag/pipeline.py"
check_file "rag/pipeline_deezer.py"
check_file "rag/requirements.txt"
echo ""

# 5. Check Documentation
echo -e "${BLUE}5️⃣  Checking Documentation${NC}"
check_file "DOCKER_SETUP.md"
check_file "QUICKSTART.md"
check_file "POSTGRES_GUIDE.md"
echo ""

# 6. Check Ports
echo -e "${BLUE}6️⃣  Checking Ports${NC}"
check_port 7110
check_port 8000
check_port 5432
echo ""

# 7. Check .NET Build
echo -e "${BLUE}7️⃣  Checking .NET Build${NC}"
cd Webzine
if dotnet build -c Release >/dev/null 2>&1; then
    echo -e "${GREEN}✓${NC} .NET build successful"
    ((SUCCESS++))
else
    echo -e "${RED}✗${NC} .NET build failed"
    ((ERRORS++))
fi
cd ..
echo ""

# 8. Check Python Environment
echo -e "${BLUE}8️⃣  Checking Python Environment${NC}"
if python3 -m pip show fastapi >/dev/null 2>&1; then
    echo -e "${GREEN}✓${NC} FastAPI installed"
    ((SUCCESS++))
else
    echo -e "${YELLOW}⚠${NC} FastAPI not installed (will be installed in Docker)"
    ((WARNINGS++))
fi

if [ -f "rag/requirements.txt" ]; then
    REQUIRED_PACKAGES=("fastapi" "uvicorn" "pydantic" "langchain" "chromadb" "sentence-transformers")
    for package in "${REQUIRED_PACKAGES[@]}"; do
        if grep -q "^$package" rag/requirements.txt; then
            echo -e "${GREEN}✓${NC} $package in requirements.txt"
            ((SUCCESS++))
        else
            echo -e "${RED}✗${NC} $package MISSING in requirements.txt"
            ((ERRORS++))
        fi
    done
fi
echo ""

# 9. Check Docker Configuration
echo -e "${BLUE}9️⃣  Checking Docker Configuration${NC}"
if grep -q "RagApi.*:.*8000" Webzine/appsettings.json; then
    echo -e "${GREEN}✓${NC} RAG API endpoint configured"
    ((SUCCESS++))
else
    echo -e "${RED}✗${NC} RAG API endpoint NOT configured"
    ((ERRORS++))
fi

if grep -q "RagApi.*rag:8000" Webzine/appsettings.Production.json; then
    echo -e "${GREEN}✓${NC} Production RAG endpoint configured"
    ((SUCCESS++))
else
    echo -e "${RED}✗${NC} Production RAG endpoint NOT configured"
    ((ERRORS++))
fi
echo ""

# 10. Environment Configuration
echo -e "${BLUE}🔟 Checking Environment Configuration${NC}"
if [ -f ".env" ]; then
    echo -e "${GREEN}✓${NC} .env file exists"
    ((SUCCESS++))
elif [ -f ".env.example" ]; then
    echo -e "${YELLOW}⚠${NC} .env file missing (copy from .env.example)"
    ((WARNINGS++))
else
    echo -e "${RED}✗${NC} Neither .env nor .env.example found"
    ((ERRORS++))
fi
echo ""

# Summary
echo -e "${BLUE}═════════════════════════════════════════${NC}"
echo -e "✓ Passed: ${GREEN}$SUCCESS${NC}"
echo -e "⚠ Warnings: ${YELLOW}$WARNINGS${NC}"
echo -e "✗ Errors: ${RED}$ERRORS${NC}"
echo -e "${BLUE}═════════════════════════════════════════${NC}\n"

if [ $ERRORS -eq 0 ]; then
    echo -e "${GREEN}✓ All checks passed! Ready to deploy.${NC}\n"
    echo -e "Next steps:"
    echo -e "  1. Copy .env.example to .env and configure"
    echo -e "  2. docker-compose build"
    echo -e "  3. docker-compose up -d"
    echo -e "  4. Verify: curl http://localhost:7110/health\n"
    exit 0
else
    echo -e "${RED}✗ Please fix the errors above before deploying.${NC}\n"
    exit 1
fi
