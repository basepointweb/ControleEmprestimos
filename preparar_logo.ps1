# Script para preparar logo SEMIADET

Write-Host "=== Preparando Logo SEMIADET ===" -ForegroundColor Cyan
Write-Host ""

# Caminho da pasta Resources
$projectPath = "D:\Projetos\eliassilvadev\ControleEmprestimos"
$resourcesPath = Join-Path $projectPath "Resources"
$logoPath = Join-Path $resourcesPath "logo.png"

# Verificar se pasta Resources existe
if (-not (Test-Path $resourcesPath)) {
    Write-Host "Criando pasta Resources..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $resourcesPath -Force | Out-Null
}

# Verificar se logo já existe
if (Test-Path $logoPath) {
    Write-Host "Logo já existe em: $logoPath" -ForegroundColor Green
    Write-Host ""
    Write-Host "Para substituir a logo:" -ForegroundColor Yellow
    Write-Host "1. Salve a nova imagem como 'logo.png'" -ForegroundColor White
    Write-Host "2. Copie para: $resourcesPath" -ForegroundColor White
    Write-Host ""
} else {
    Write-Host "Logo não encontrada!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Para adicionar a logo:" -ForegroundColor Yellow
    Write-Host "1. Salve a imagem da logo como 'logo.png'" -ForegroundColor White
    Write-Host "2. Copie para: $resourcesPath" -ForegroundColor White
    Write-Host ""
    Write-Host "Especificações recomendadas:" -ForegroundColor Cyan
    Write-Host "- Formato: PNG com fundo transparente" -ForegroundColor White
    Write-Host "- Tamanho: 512x512 pixels ou maior" -ForegroundColor White
    Write-Host "- Proporção: Quadrada (1:1)" -ForegroundColor White
    Write-Host ""
}

Write-Host "Caminho da pasta Resources: $resourcesPath" -ForegroundColor Cyan
Write-Host ""

# Abrir pasta Resources no Explorer
Write-Host "Abrindo pasta Resources..." -ForegroundColor Green
Start-Process explorer.exe $resourcesPath

Write-Host ""
Write-Host "=== Concluído ===" -ForegroundColor Green
