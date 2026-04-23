# Webzine Docker Stack - Pre-Deployment Validation (PowerShell)

$ErrorActionPreference = "SilentlyContinue"

# Color codes
$Green = @{ForegroundColor = 'Green'}
$Red = @{ForegroundColor = 'Red'}
$Yellow = @{ForegroundColor = 'Yellow'}
$Blue = @{ForegroundColor = 'Blue'}

Write-Host "🔍 Webzine Docker Stack - Pre-Deployment Validation`n" @Blue

$Errors = 0
$Warnings = 0
$Success = 0

# Check command exists
function CheckCommand {
    param([string]$Name, [string]$InstallUrl)
    $cmd = Get-Command $Name -ErrorAction SilentlyContinue
    if ($cmd) {
        Write-Host "✓ $Name installed" @Green
        $script:Success++
    } else {
        Write-Host "✗ $Name NOT FOUND - Install from: $InstallUrl" @Red
        $script:Errors++
    }
}

# Check file exists
function CheckFile {
    param([string]$Path)
    if (Test-Path $Path) {
        Write-Host "✓ $Path exists" @Green
        $script:Success++
    } else {
        Write-Host "✗ $Path MISSING" @Red
        $script:Errors++
    }
}

# Check port
function CheckPort {
    param([int]$Port)
    $connection = Test-NetConnection -ComputerName 127.0.0.1 -Port $Port -WarningAction SilentlyContinue
    if ($connection.TcpTestSucceeded) {
        Write-Host "⚠ Port $Port already in use" @Yellow
        $script:Warnings++
    } else {
        Write-Host "✓ Port $Port available" @Green
        $script:Success++
    }
}

# 1. Check Required Tools
Write-Host "1️⃣  Checking Required Tools" @Blue
CheckCommand "docker" "https://docs.docker.com/desktop/install/windows-install/"
CheckCommand "dotnet" "https://dotnet.microsoft.com/download"
CheckCommand "python" "https://www.python.org/downloads/"
Write-Host ""

# 2. Check Configuration Files
Write-Host "2️⃣  Checking Configuration Files" @Blue
CheckFile "Dockerfile"
CheckFile "rag\Dockerfile"
CheckFile "docker-compose.yml"
CheckFile "init-db.sql"
CheckFile ".dockerignore"
CheckFile "Webzine\appsettings.json"
CheckFile "Webzine\appsettings.Production.json"
Write-Host ""

# 3. Check .NET Project
Write-Host "3️⃣  Checking .NET Project" @Blue
CheckFile "Webzine\Webzine.WebApplication\Webzine.WebApplication.csproj"
CheckFile "Webzine\Webzine.WebApplication\Controllers\ChatController.cs"
CheckFile "Webzine\Webzine.WebApplication\Controllers\HealthController.cs"
Write-Host ""

# 4. Check Python RAG
Write-Host "4️⃣  Checking Python RAG" @Blue
CheckFile "rag\api.py"
CheckFile "rag\pipeline.py"
CheckFile "rag\pipeline_deezer.py"
CheckFile "rag\requirements.txt"
Write-Host ""

# 5. Check Documentation
Write-Host "5️⃣  Checking Documentation" @Blue
CheckFile "DOCKER_SETUP.md"
CheckFile "QUICKSTART.md"
CheckFile "POSTGRES_GUIDE.md"
Write-Host ""

# 6. Check Ports
Write-Host "6️⃣  Checking Ports" @Blue
CheckPort 7110
CheckPort 8000
CheckPort 5432
Write-Host ""

# 7. Check Configuration Content
Write-Host "7️⃣  Checking Configuration Content" @Blue
$appSettings = Get-Content "Webzine\appsettings.Production.json" | ConvertFrom-Json -ErrorAction SilentlyContinue
if ($appSettings.AppConfig.RagApi -like "*rag:8000*") {
    Write-Host "✓ Production RAG endpoint configured correctly" @Green
    $Success++
} else {
    Write-Host "✗ Production RAG endpoint NOT configured correctly" @Red
    $Errors++
}
Write-Host ""

# 8. Check .env Configuration
Write-Host "8️⃣  Checking Environment Configuration" @Blue
if (Test-Path ".env") {
    Write-Host "✓ .env file exists" @Green
    $Success++
} elseif (Test-Path ".env.example") {
    Write-Host "⚠ .env file missing (copy from .env.example)" @Yellow
    $Warnings++
} else {
    Write-Host "✗ Neither .env nor .env.example found" @Red
    $Errors++
}
Write-Host ""

# 9. Docker Configuration
Write-Host "9️⃣  Checking Docker Installation" @Blue
try {
    $dockerVersion = docker --version
    Write-Host "✓ Docker available: $dockerVersion" @Green
    $Success++
} catch {
    Write-Host "✗ Docker not accessible" @Red
    $Errors++
}
Write-Host ""

# Summary
Write-Host "═════════════════════════════════════════" @Blue
Write-Host "✓ Passed: $Success" @Green
Write-Host "⚠ Warnings: $Warnings" @Yellow
Write-Host "✗ Errors: $Errors" @Red
Write-Host "═════════════════════════════════════════`n" @Blue

if ($Errors -eq 0) {
    Write-Host "✓ All checks passed! Ready to deploy.`n" @Green
    Write-Host "Next steps:"
    Write-Host "  1. Copy .env.example to .env and configure"
    Write-Host "  2. docker-compose build"
    Write-Host "  3. docker-compose up -d"
    Write-Host "  4. Verify: curl http://localhost:7110/health`n"
    exit 0
} else {
    Write-Host "✗ Please fix the errors above before deploying.`n" @Red
    exit 1
}
