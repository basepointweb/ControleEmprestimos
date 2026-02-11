# ControleEmprestimos

Sistema de gerenciamento de inventário em Windows Forms para controle de bens e empréstimos.

## Funcionalidades

O sistema possui um menu principal com as seguintes opções:

1. **Listagem de Bens** - Gerencia itens do inventário
2. **Emprestimo** - Controla empréstimos de itens
3. **Recebimento de Emprestimo** - Registra recebimentos/devoluções de empréstimos
4. **Congregacoes** - Gerencia congregações

### Operações Disponíveis

Cada módulo permite:
- **Criar** - Adicionar novo item
- **Editar** - Modificar item existente
- **Excluir** - Remover item do sistema

### Informações dos Itens

Cada item possui:
- **Nome** - Nome do item
- **Quantidade em Estoque** - Quantidade disponível no estoque

## Requisitos

- .NET 10.0 ou superior
- Windows (para execução do Windows Forms)

## Como Executar

```bash
dotnet build
dotnet run
```

## Estrutura do Projeto

```
ControleEmprestimos/
├── Models/              # Modelos de dados (Item, Emprestimo, RecebimentoEmprestimo, Congregacao)
├── Forms/               # Formulários da aplicação
│   ├── MainForm         # Formulário principal com menu
│   ├── *ListForm        # Formulários de listagem
│   └── *DetailForm      # Formulários de detalhes (criar/editar)
├── Data/                # Repositório de dados em memória
└── Program.cs           # Ponto de entrada da aplicação
```