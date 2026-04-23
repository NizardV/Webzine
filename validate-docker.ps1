# ============================================================
# 🐳 WEBZINE DOCKER VALIDATION & DIAGNOSTIC TOOL (PowerShell)
# ============================================================
# Usage: .\validate-docker.ps1
# Checks Docker configuration, containers, and connectivity
# ============================================================

# Set error handling
$ErrorActionPreference = "Continue"

# Colors
$colors = @{
    Red = "Red"
    Green = "Green"
    Yellow = "Yellow"
    Blue = "Cyan"
    Gray = "Gray"
}

# Counters
$checksPassed = 0
$checksFailed = 0

# Helper functions
function Write-Header {
    param([string]$Text)
    Write-Host ""
    Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor $colors.Blue
    Write-Host $Text -ForegroundColor $colors.Blue
    Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor $colors.Blue
    Write-Host ""
}

function Write-CheckPass {
    param([string]$Text)
    Write-Host "✓ $Text" -ForegroundColor $colors.Green
    $script:checksPassed++
}

function Write-CheckFail {
    param([string]$Text)
    Write-Host "✗ $Text" -ForegroundColor $colors.Red
    $script:checksFailed++
}

function Write-CheckWarning {
    param([string]$Text)
    Write-Host "⚠ $Text" -ForegroundColor $colors.Yellow
}

function Write-Info {
    param([string]$Text)
    Write-Host "  $Text" -ForegroundColor $colors.Blue
}

function Write-InfoValue {
    param([string]$Label, [string]$Value)
    Write-Host "    $Label $Value" -ForegroundColor $colors.Gray
}

function Write-Summary {
    Write-Host ""
    Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor $colors.Blue
    if ($checksFailed -eq 0) {
        Write-Host "✓ ALL CHECKS PASSED" -ForegroundColor $colors.Green
    }
    else {
        Write-Host "✗ $checksFailed CHECK(S) FAILED" -ForegroundColor $colors.Red
    }
    Write-Host "Passed: $checksPassed | Failed: $checksFailed" -ForegroundColor $colors.Blue
    Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor $colors.Blue
    Write-Host ""
}

# ============================================================
# 1. ENVIRONMENT & SYSTEM CHECKS
# ============================================================
Write-Header "1️⃣  ENVIRONMENT & SYSTEM CHECKS"

# Docker installation
$docker = Get-Command docker -ErrorAction SilentlyContinue
if ($docker) {
    $dockerVersion = & docker --version
    Write-CheckPass "Docker installed: $dockerVersion"
}
else {
    Write-CheckFail "Docker not found - Install from https://docker.com"
    exit 1
}

# Docker Compose installation
$compose = Get-Command docker-compose -ErrorAction SilentlyContinue
if ($compose) {
    $composeVersion = & docker-compose --version
    Write-CheckPass "Docker Compose installed: $composeVersion"
}
else {
    Write-CheckFail "Docker Compose not found"
    exit 1
}

# Docker daemon running
try {
    $null = & docker ps 2>&1
    Write-CheckPass "Docker daemon is running"
}
catch {
    Write-CheckFail "Docker daemon is not running - Start Docker Desktop"
    exit 1
}

# ============================================================
# 2. PROJECT STRUCTURE CHECKS
# ============================================================
Write-Header "2️⃣  PROJECT STRUCTURE CHECKS"

$requiredFiles = @(
    "Dockerfile",
    "docker-compose.yml",
    "docker-compose.prod.yml",
    ".env.example",
    "init-db.sql",
    "rag/Dockerfile",
    "rag/requirements.txt",
    "rag/api.py"
)

foreach ($file in $requiredFiles) {
    if (Test-Path $file) {
        Write-CheckPass "Found: $file"
    }
    else {
        Write-CheckFail "Missing: $file"
    }
}

# ============================================================
# 3. DOCKER-COMPOSE CONFIGURATION
# ============================================================
Write-Header "3️⃣  DOCKER-COMPOSE CONFIGURATION"

# Validate docker-compose syntax
try {
    $null = & docker-compose config 2>&1
    Write-CheckPass "docker-compose.yml syntax is valid"
}
catch {
    Write-CheckFail "docker-compose.yml has syntax errors"
}

# Check for .env file
if (Test-Path ".env") {
    Write-CheckPass ".env file exists"
    Write-Info "Environment Variables:"

    $envContent = Get-Content .env | Where-Object { $_ -notmatch '^#' -and $_ -notmatch '^\s*$' }
    $envContent | Select-Object -First 10 | ForEach-Object { Write-Host "      $_" }
}
else {
    Write-CheckWarning ".env file not found - Run: Copy-Item .env.example .env"
}

# ============================================================
# 4. RUNNING CONTAINERS
# ============================================================
Write-Header "4️⃣  RUNNING CONTAINERS"

# Get container status
$containers = & docker-compose ps 2>$null
$webzineRunning = $containers -like "*webzine_app*"
$ragRunning = $containers -like "*webzine_rag*"
$postgresRunning = $containers -like "*webzine_postgres*"

if ($webzineRunning) {
    Write-CheckPass "Webzine container exists"
}
else {
    Write-CheckWarning "Webzine container not running"
}

if ($ragRunning) {
    Write-CheckPass "RAG API container exists"
}
else {
    Write-CheckWarning "RAG API container not running"
}

if ($postgresRunning) {
    Write-CheckPass "PostgreSQL container exists"
}
else {
    Write-CheckWarning "PostgreSQL container not running"
}

Write-Info "Container Status:"
$containers | Select-Object -Skip 1 | ForEach-Object { Write-Host "      $_" }

# ============================================================
# 5. PORT AVAILABILITY
# ============================================================
Write-Header "5️⃣  PORT AVAILABILITY"

function Test-Port {
    param([int]$Port, [string]$Service)

    try {
        $tcp = New-Object System.Net.Sockets.TcpClient
        $tcp.Connect("127.0.0.1", $Port)
        $tcp.Close()
        Write-CheckPass "Port $Port ($Service) is accessible"
    }
    catch {
        Write-CheckWarning "Port $Port ($Service) is not responding (services may not be running)"
    }
}

Test-Port 7110 "Webzine"
Test-Port 8000 "RAG API"
Test-Port 5432 "PostgreSQL"
Test-Port 11434 "Ollama (on host)"

# ============================================================
# 6. SERVICE HEALTH CHECKS
# ============================================================
Write-Header "6️⃣  SERVICE HEALTH CHECKS"

# Webzine health
try {
    $response = Invoke-WebRequest -Uri "http://localhost:7110/health" -UseBasicParsing -ErrorAction SilentlyContinue
    if ($response.StatusCode -eq 200) {
        Write-CheckPass "Webzine health endpoint responding"
    }
}
catch {
    Write-CheckWarning "Webzine health endpoint not responding (http://localhost:7110/health)"
}

# RAG API health
try {
    $response = Invoke-WebRequest -Uri "http://localhost:8000/health" -UseBasicParsing -ErrorAction SilentlyContinue
    if ($response.StatusCode -eq 200) {
        Write-CheckPass "RAG API health endpoint responding"
    }
}
catch {
    Write-CheckWarning "RAG API health endpoint not responding (http://localhost:8000/health)"
}

# PostgreSQL connection
try {
    $null = & docker-compose exec -T postgres pg_isready -U webzine -d webzine 2>&1
    Write-CheckPass "PostgreSQL is accepting connections"
}
catch {
    Write-CheckWarning "PostgreSQL is not accepting connections"
}

# Ollama availability
try {
    $response = Invoke-WebRequest -Uri "http://localhost:11434/api/tags" -UseBasicParsing -ErrorAction SilentlyContinue
    if ($response.StatusCode -eq 200) {
        Write-CheckPass "Ollama is accessible on localhost:11434"

        # Parse models
        $content = $response.Content | ConvertFrom-Json
        if ($content.models.Count -gt 0) {
            Write-Info "Available Models:"
            $content.models | Select-Object -First 3 | ForEach-Object { Write-Host "      - $($_.name)" }
        }
    }
}
catch {
    Write-CheckWarning "Ollama not responding - Ensure: ollama serve is running"
}

# ============================================================
# 7. DATABASE CHECKS
# ============================================================
Write-Header "7️⃣  DATABASE CHECKS"

try {
    $null = & docker-compose exec -T postgres psql -U webzine -d webzine -c "\q" 2>&1
    Write-CheckPass "Database 'webzine' exists and accessible"

    Write-Info "Database Info:"
    Write-InfoValue "Status:" "Connected"
}
catch {
    Write-CheckWarning "Cannot access database (containers may not be running)"
}

# ============================================================
# 8. DISK USAGE
# ============================================================
Write-Header "8️⃣  DISK USAGE"

try {
    $volumes = & docker volume ls --quiet 2>&1
    $volumeCount = @($volumes).Count
    Write-CheckPass "Docker volumes: $volumeCount"

    Write-Info "Webzine Volumes:"
    $webzineVolumes = & docker volume ls --filter name=webzine 2>&1 | Select-Object -Skip 1
    $webzineVolumes | ForEach-Object { Write-Host "      $_" }
}
catch {
    Write-CheckWarning "Could not check volumes"
}

# ============================================================
# 9. LOGS ANALYSIS
# ============================================================
Write-Header "9️⃣  RECENT CONTAINER LOGS"

Write-Info "Last 5 Webzine logs:"
try {
    $logs = & docker-compose logs --tail=5 webzine 2>&1 | Select-Object -Last 5
    $logs | ForEach-Object { Write-Host "      $_" }
}
catch {
    Write-Host "      (No logs available)"
}

Write-Info "Last 5 RAG API logs:"
try {
    $logs = & docker-compose logs --tail=5 rag 2>&1 | Select-Object -Last 5
    $logs | ForEach-Object { Write-Host "      $_" }
}
catch {
    Write-Host "      (No logs available)"
}

Write-Info "Last 5 PostgreSQL logs:"
try {
    $logs = & docker-compose logs --tail=5 postgres 2>&1 | Select-Object -Last 5
    $logs | ForEach-Object { Write-Host "      $_" }
}
catch {
    Write-Host "      (No logs available)"
}

# ============================================================
# 10. RECOMMENDATIONS
# ============================================================
Write-Header "🔧 RECOMMENDATIONS"

if ($checksFailed -eq 0) {
    Write-Host "✅ Your Docker setup appears to be working correctly!" -ForegroundColor $colors.Green
    Write-Host ""
    Write-Host "Access your services:" -ForegroundColor $colors.Blue
    Write-Host "  • Webzine: http://localhost:7110"
    Write-Host "  • RAG API Docs: http://localhost:8000/docs"
    Write-Host ""
    Write-Host "Useful commands:" -ForegroundColor $colors.Blue
    Write-Host "  • View logs: docker-compose logs -f"
    Write-Host "  • Restart services: docker-compose restart"
    Write-Host "  • Access database: docker-compose exec postgres psql -U webzine -d webzine"
}
else {
    Write-Host "⚠️  Please address the failing checks above." -ForegroundColor $colors.Yellow
    Write-Host ""
    Write-Host "Common solutions:" -ForegroundColor $colors.Yellow
    Write-Host "  1. Ensure Ollama is running: ollama serve"
    Write-Host "  2. Start containers: docker-compose up -d"
    Write-Host "  3. Rebuild images: docker-compose build --no-cache"
    Write-Host "  4. Check logs: docker-compose logs -f"
    Write-Host "  5. Ensure ports are available (not blocked by firewall)"
}

# ============================================================
# FINAL SUMMARY
# ============================================================
Write-Summary

# Exit with appropriate code
if ($checksFailed -eq 0) {
    exit 0
}
else {
    exit 1
}
