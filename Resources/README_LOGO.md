# Logo SEMIADET - Instruções de Instalação

## 📋 O que fazer agora

A funcionalidade de exibição da logo está **totalmente implementada** no código, mas você precisa adicionar o arquivo de imagem da logo.

## 🎯 Passos Rápidos

### 1. Salvar a Imagem da Logo
- A logo SEMIADET que você forneceu precisa ser salva como arquivo PNG
- Nome do arquivo: **`logo.png`**
- Formato recomendado: PNG com fundo transparente

### 2. Copiar para a Pasta Correta
Copie o arquivo `logo.png` para:
```
D:\Projetos\eliassilvadev\ControleEmprestimos\Resources\logo.png
```

### 3. Pronto!
- O sistema detectará automaticamente a logo
- Ela aparecerá em todos os recibos e relatórios

## 🖼️ Especificações da Imagem

### Formato
- **Recomendado**: PNG com fundo transparente
- **Alternativo**: JPG (mas terá fundo branco)

### Tamanho
- **Mínimo**: 256x256 pixels
- **Ideal**: 512x512 pixels ou maior
- **Proporção**: Quadrada (1:1) funciona melhor

### Qualidade
- Boa resolução para impressão
- Cores nítidas
- Sem pixelização

## 🛠️ Script Auxiliar

Execute o script PowerShell incluído para abrir a pasta automaticamente:

```powershell
.\preparar_logo.ps1
```

Este script:
- ✅ Verifica se a pasta Resources existe
- ✅ Abre a pasta no Windows Explorer
- ✅ Mostra instruções

## 📍 Onde a Logo Aparece

### Recibos
- ✅ Recibo de Empréstimo
- ✅ Recibo de Recebimento

### Relatórios
- ✅ Relatório de Empréstimos (primeira página)
- ✅ Relatório de Devoluções (primeira página)

### Posicionamento
- **Localização**: Canto superior direito
- **Alinhamento**: Com a linha do título
- **Tamanho**: Proporcional à altura do título (~2.5x)

## ✅ Como Testar

### Sem Logo (Estado Atual)
1. Compile e execute o sistema
2. Imprima qualquer recibo ou relatório
3. ✅ Apenas título aparece (sem logo)
4. ✅ Sistema funciona normalmente

### Com Logo (Após Adicionar)
1. Adicione o arquivo `logo.png` na pasta Resources
2. Reinicie o sistema (se estiver rodando)
3. Imprima qualquer recibo ou relatório
4. ✅ Logo aparece no canto superior direito

## 🎨 Exemplo Visual

```
┌────────────────────────────────────────────────┐
│  SEMIADET - EMPRÉSTIMO DE BENS       [LOGO]   │
│                                                │
│  Nº Empréstimo: 1                             │
│  Data: 15/12/2024                             │
│  ...                                          │
└────────────────────────────────────────────────┘
```

## 🚀 Para Distribuição

Ao distribuir o sistema, inclua:
```
ControleEmprestimos.exe
ControleEmprestimos.xlsx
Resources/
  └── logo.png    ← Não esquecer!
```

## ❓ Perguntas Frequentes

### A logo é obrigatória?
Não. O sistema funciona perfeitamente sem ela, apenas não exibirá a imagem.

### Posso usar JPG?
Sim, mas PNG com fundo transparente fica melhor visualmente.

### Como atualizar a logo?
Basta substituir o arquivo `Resources/logo.png` e reiniciar o sistema.

### A logo afeta a performance?
Não. O impacto é mínimo, apenas ao imprimir/visualizar documentos.

## 📞 Suporte

Se tiver dúvidas ou problemas:
1. Verifique se o arquivo está nomeado exatamente como `logo.png`
2. Verifique se está na pasta correta: `Resources/logo.png`
3. Verifique se o formato é PNG ou JPG válido
4. Reinicie o aplicativo após adicionar a logo

## ✨ Títulos Atualizados

Os títulos já estão atualizados com o prefixo SEMIADET:

- **Empréstimo**: "SEMIADET - EMPRÉSTIMO DE BENS"
- **Recebimento**: "SEMIADET - Recebimento de bens emprestados"
- **Relatório de Empréstimos**: "SEMIADET - RELATÓRIO DE EMPRÉSTIMOS"
- **Relatório de Devoluções**: "SEMIADET - Relatório de Devoluções"

---

**Pronto para usar!** Basta adicionar a imagem da logo. 🎉
