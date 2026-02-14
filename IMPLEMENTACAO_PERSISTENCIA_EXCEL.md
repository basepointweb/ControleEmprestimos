# Implementação de Persistência em Excel

## Resumo
Implementada persistência de dados em arquivo Excel (.xlsx) para todas as entidades do sistema. Os dados são salvos automaticamente após cada operação e carregados na inicialização do aplicativo.

## Mudanças Implementadas

### 1. Pacote NuGet Adicionado
- **EPPlus 7.0.0** - Biblioteca para manipulação de arquivos Excel (formato .xlsx - Excel 2007+)

### 2. Novo Arquivo: `Data\ExcelDataRepository.cs`

Classe responsável pela leitura e escrita de dados no arquivo Excel.

#### Características:
- **Arquivo único**: `ControleEmprestimos.xlsx`
- **Localização**: **Pasta do executável** (mesma pasta onde está o .exe)
- **6 Abas (uma para cada entidade)**:
  1. **Itens** - Bens disponíveis para empréstimo
  2. **Congregacoes** - Congregações cadastradas
  3. **Emprestimos** - Registros de empréstimos realizados
  4. **EmprestimoItens** - Itens de cada empréstimo (relacionamento)
  5. **Recebimentos** - Registros de recebimentos/devoluções
  6. **RecebimentoItens** - Itens de cada recebimento (relacionamento)

#### Funcionalidades:

##### CreateEmptyExcelFile()
- Cria arquivo Excel vazio com estrutura inicial
- Define cabeçalhos para cada aba
- Aplica formatação (negrito, auto-filtro)

##### SaveData()
- Salva todas as entidades no Excel
- Substitui dados existentes (exceto cabeçalhos)
- Formata datas no padrão ISO (yyyy-MM-dd HH:mm:ss)
- Ajusta largura das colunas automaticamente

##### LoadData()
- Carrega todos os dados do Excel
- Reconstrói relacionamentos (Emprestimos ? EmprestimoItens, Recebimentos ? RecebimentoItens)
- Calcula próximos IDs automaticamente
- Trata valores nulos/vazios com valores padrão

### 3. Modificações em `Data\DataRepository.cs`

#### Novas Propriedades:
```csharp
private readonly ExcelDataRepository _excelRepository;
private readonly string _dataFilePath;
```

#### Caminho do Arquivo:
```csharp
var exePath = AppDomain.CurrentDomain.BaseDirectory;
_dataFilePath = Path.Combine(exePath, "ControleEmprestimos.xlsx");
```

#### Método: `LoadFromExcel()`
- Carrega dados do Excel na inicialização
- Carrega **TODAS** as entidades, incluindo congregações
- Trata exceções e registra erros no Debug
- Atualiza contadores de IDs

#### Método: `SaveToExcel()`
- Salva dados após cada operação
- Chamado automaticamente em:
  - AddItem()
  - UpdateItem()
  - AddEmprestimo()
  - UpdateEmprestimo()
  - DevolverEmprestimo()
  - AddRecebimento()
  - AddCongregacao()
  - UpdateCongregacao()
  - RemoverEmprestimo()
  - RemoverRecebimento()

#### ?? Seed Inicial Removido
- **Não há mais seed automático de congregações**
- Todas as congregações devem ser cadastradas pelo usuário ou importadas via Excel
- O sistema inicia com planilha vazia (apenas cabeçalhos)

## Estrutura das Abas do Excel

### Aba: Itens
| Id | Nome | QuantidadeEstoque | DataCriacao | DataAlteracao |
|----|------|-------------------|-------------|---------------|

### Aba: Congregacoes
| Id | Nome | Setor | DataCriacao | DataAlteracao |
|----|------|-------|-------------|---------------|

### Aba: Emprestimos
| Id | Nome | Motivo | CongregacaoId | CongregacaoNome | DataEmprestimo | Status | DataCriacao | DataAlteracao |
|----|------|--------|---------------|-----------------|----------------|--------|-------------|---------------|

### Aba: EmprestimoItens
| Id | EmprestimoId | ItemId | ItemNome | Quantidade | QuantidadeRecebida | DataCriacao | DataAlteracao |
|----|--------------|--------|----------|------------|--------------------|-------------|---------------|

### Aba: Recebimentos
| Id | Nome | NomeRecebedor | NomeQuemRecebeu | EmprestimoId | DataEmprestimo | DataRecebimento | RecebimentoParcial | DataCriacao | DataAlteracao |
|----|------|---------------|-----------------|--------------|----------------|-----------------|--------------------|--------------|-----------------|

### Aba: RecebimentoItens
| Id | RecebimentoEmprestimoId | EmprestimoItemId | ItemId | ItemNome | QuantidadeRecebida | DataCriacao | DataAlteracao |
|----|-------------------------|------------------|--------|----------|--------------------|-------------|---------------|

## Benefícios da Implementação

### 1. Persistência Automática
- Dados são salvos automaticamente após cada operação
- Não há risco de perda de dados por fechamento inesperado

### 2. Backup Facilitado
- Arquivo único facilita backup
- **Localização na pasta do executável** facilita cópia junto com o programa
- Pode ser copiado manualmente para backup

### 3. Formato Acessível
- Dados podem ser visualizados/editados diretamente no Excel
- Útil para análises, relatórios ou correções manuais
- Formato padrão da indústria (Excel 2007+)

### 4. Integridade de Dados
- Relacionamentos são mantidos
- IDs são calculados automaticamente
- Datas padronizadas

### 5. Inicialização Flexível
- Cria arquivo automaticamente se não existir
- Inicia com planilha vazia (apenas estrutura)
- Permite importação de dados existentes

### 6. Portabilidade
- Arquivo na mesma pasta do executável
- Fácil distribuição (copiar pasta inteira)
- Ideal para backup e migração

## Uso da Licença EPPlus

A biblioteca EPPlus é configurada para uso **não comercial** através da linha:
```csharp
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
```

Para uso comercial, é necessário adquirir licença em: https://epplussoftware.com/

## Tratamento de Erros

### Carga de Dados (LoadFromExcel)
- Exceções são capturadas e registradas no Debug
- Sistema continua com dados vazios em caso de erro
- **Não há seed inicial** - sistema inicia vazio

### Salvamento de Dados (SaveToExcel)
- Exceções são capturadas e registradas no Debug
- Exceção é re-lançada para notificar o usuário
- Operação anterior é mantida em memória

## Compatibilidade

### Formato
- Excel 2007 ou superior (.xlsx)
- Compatível com LibreOffice Calc, Google Sheets

### Sistema Operacional
- Windows (testado)
- Usa `AppDomain.CurrentDomain.BaseDirectory` para localizar pasta do executável

### .NET
- .NET 8.0 (conforme projeto)
- EPPlus 7.0.0 compatível

## Importação Inicial de Dados

### Como Importar Congregações Iniciais:

**Opção 1: Via Interface do Sistema**
- Cadastrar manualmente através do menu "Congregações"

**Opção 2: Via Excel (Recomendado para importação em massa)**
1. Fechar o aplicativo
2. Abrir `ControleEmprestimos.xlsx` no Excel
3. Acessar aba "Congregacoes"
4. Adicionar dados a partir da linha 2 (manter cabeçalhos):
   ```
   Id | Nome          | Setor    | DataCriacao         | DataAlteracao
   1  | Sede          |          | 2024-01-01 10:00:00 | 2024-01-01 10:00:00
   2  | Bonsucesso    | SETOR E  | 2024-01-01 10:00:00 | 2024-01-01 10:00:00
   ...
   ```
5. Salvar arquivo
6. Abrir o aplicativo (dados serão carregados automaticamente)

### Template de Congregações (Exemplo):
```
1  | Sede              |          | 2024-01-01 10:00:00 | 2024-01-01 10:00:00
2  | Bonsucesso        | SETOR E  | 2024-01-01 10:00:00 | 2024-01-01 10:00:00
3  | Sub-sede          |          | 2024-01-01 10:00:00 | 2024-01-01 10:00:00
4  | Barroso           | SETOR A  | 2024-01-01 10:00:00 | 2024-01-01 10:00:00
5  | Rosario           | SETOR A  | 2024-01-01 10:00:00 | 2024-01-01 10:00:00
...
```

## Testagem Recomendada

1. ? Criar novo item e verificar Excel
2. ? Criar congregação e verificar Excel
3. ? Criar empréstimo e verificar relacionamentos
4. ? Registrar recebimento e verificar atualizações
5. ? Fechar e reabrir aplicativo (testar persistência)
6. ? Excluir arquivo e verificar criação automática
7. ? Editar dados diretamente no Excel e verificar carga
8. ? Copiar pasta inteira e verificar funcionamento (portabilidade)

## Manutenção Futura

### Adicionar Nova Entidade
1. Adicionar aba em `CreateEmptyExcelFile()`
2. Criar método `Save[Entidade]()`
3. Adicionar carregamento em `LoadData()`
4. Adicionar lista pública no DataRepository

### Modificar Estrutura Existente
1. Atualizar método `Create[Entidade]Sheet()`
2. Ajustar método `Save[Entidade]()`
3. Ajustar carregamento em `LoadData()`
4. **Atenção**: Pode quebrar compatibilidade com arquivos antigos

### Migração de Dados
Para manter compatibilidade, criar lógica de versão no Excel:
- Adicionar aba "Versão" com número de versão da estrutura
- Implementar migrações incrementais quando estrutura mudar

## Localização do Arquivo

O arquivo Excel é salvo em:
```
Pasta do Executável\ControleEmprestimos.xlsx
```

**Exemplos:**
- Debug: `D:\Projetos\eliassilvadev\ControleEmprestimos\bin\Debug\net8.0-windows7.0\ControleEmprestimos.xlsx`
- Release: `C:\Aplicativos\ControleEmprestimos\ControleEmprestimos.xlsx`

Para acessar:
1. Navegue até a pasta onde o executável está instalado
2. O arquivo `ControleEmprestimos.xlsx` estará junto ao `.exe`

## Distribuição do Sistema

### Como Distribuir:
1. Compilar em modo Release
2. Copiar toda a pasta `bin\Release\net8.0-windows7.0\`
3. O arquivo `ControleEmprestimos.xlsx` será criado automaticamente na primeira execução
4. Para distribuir com dados pré-cadastrados, incluir `ControleEmprestimos.xlsx` populado

### Vantagens desta Abordagem:
- ? Arquivo de dados sempre junto ao executável
- ? Fácil backup (copiar pasta inteira)
- ? Não depende de caminhos do sistema
- ? Portátil (pode rodar de pen drive)
- ? Múltiplas instalações independentes

## Conclusão

A implementação de persistência em Excel fornece uma solução simples, eficaz e acessível para armazenamento de dados do sistema. O arquivo na pasta do executável facilita distribuição, backup e portabilidade, enquanto o formato Excel permite fácil acesso, edição e análise dos dados, mantendo a integridade e os relacionamentos entre as entidades.
