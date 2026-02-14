# Alterações - Botão Imprimir Recibo e Validações de Exclusão

## Data: 2025-01-XX

## Descrição
1. **Botão "Imprimir Recibo"** adicionado na listagem de recebimentos
2. **Validações de exclusão** para impedir excluir Bens e Congregações que possuem empréstimos

## Alterações Realizadas

### 1. Forms\RecebimentoListForm.Designer.cs
**Novo Controle:**
- `btnImprimirRecibo`: Botão "Imprimir Recibo" (cor azul ciano - #17A2B8)

**Layout:**
- Posicionado após o botão "Excluir" (posição X: 330)
- Largura: 120px
- Altura: 30px
- Cor de fundo: `Color.FromArgb(23, 162, 184)` (azul ciano)
- Texto branco

### 2. Forms\RecebimentoListForm.cs
**Novo Método:**
```csharp
private void BtnImprimirRecibo_Click(object sender, EventArgs e)
```

**Funcionalidade:**
- Verifica se há um recebimento selecionado
- Busca o empréstimo relacionado (se houver)
- Cria instância de `ReciboRecebimentoPrinter`
- Abre visualização de impressão
- Mostra mensagem se nenhum recebimento estiver selecionado

**Import Adicionado:**
```csharp
using ControleEmprestimos.Reports;
```

### 3. Forms\ItemListForm.cs
**Método Modificado:** `BtnDelete_Click`

**Nova Validação:**
```csharp
// Verificar se o bem possui empréstimos (independente do status)
var emprestimosComItem = _repository.EmprestimoItens
    .Where(ei => ei.ItemId == item.Id)
    .ToList();

if (emprestimosComItem.Any())
{
    // Bloqueia exclusão e mostra mensagem detalhada
}
```

**Mensagem de Bloqueio:**
- Lista quantidade de empréstimos por status
- Informa que é necessário excluir os empréstimos primeiro
- Formato: "X empréstimo(s) Em Andamento\nY empréstimo(s) Devolvido(s)"

**Mensagem de Sucesso:**
- Adicionada ao confirmar exclusão bem-sucedida

### 4. Forms\CongregacaoListForm.cs
**Método Modificado:** `BtnDelete_Click`

**Nova Validação:**
```csharp
// Verificar se há empréstimos para esta congregação (independente do status)
var emprestimos = _repository.Emprestimos
    .Where(e => e.CongregacaoId == item.Id)
    .ToList();

if (emprestimos.Any())
{
    // Bloqueia exclusão e mostra mensagem detalhada
}
```

**Mensagem de Bloqueio:**
- Lista quantidade de empréstimos por status
- Informa que é necessário excluir os empréstimos primeiro
- Formato: "X empréstimo(s) Em Andamento\nY empréstimo(s) Devolvido(s)"

**Mensagem de Sucesso:**
- Adicionada ao confirmar exclusão bem-sucedida

## Funcionalidades Implementadas

### 1. Botão Imprimir Recibo (Recebimentos)

**Localização:**
- Painel inferior da listagem de recebimentos
- Ao lado do botão "Excluir"

**Comportamento:**
1. Usuário seleciona um recebimento no grid
2. Clica em "Imprimir Recibo"
3. Sistema busca o empréstimo relacionado
4. Abre visualização de impressão do recibo
5. Se nenhum recebimento selecionado, mostra aviso

**Mensagens:**
- Sucesso: Abre preview de impressão
- Erro: "Por favor, selecione um recebimento para imprimir o recibo."

### 2. Validação de Exclusão - Bem (Item)

**Regra de Negócio:**
- **NÃO** pode excluir bem que possui empréstimos
- Validação independente do status do empréstimo
- Verifica tabela `EmprestimoItens`

**Cenários Bloqueados:**
1. Bem com empréstimos "Em Andamento"
2. Bem com empréstimos "Devolvido"
3. Bem com empréstimos em ambos os status

**Mensagem de Bloqueio:**
```
Não é possível excluir o bem '[Nome]' porque ele possui empréstimos registrados:

X empréstimo(s) Em Andamento
Y empréstimo(s) Devolvido(s)

Para excluir este bem, primeiro exclua todos os empréstimos relacionados a ele.
```

**Fluxo:**
1. Usuário tenta excluir bem
2. Sistema verifica EmprestimoItens
3. Se houver registros: bloqueia e mostra mensagem detalhada
4. Se não houver: pede confirmação e exclui

### 3. Validação de Exclusão - Congregação

**Regra de Negócio:**
- **NÃO** pode excluir congregação que possui empréstimos
- Validação independente do status do empréstimo
- Verifica tabela `Emprestimos`

**Cenários Bloqueados:**
1. Congregação com empréstimos "Em Andamento"
2. Congregação com empréstimos "Devolvido"
3. Congregação com empréstimos em ambos os status

**Mensagem de Bloqueio:**
```
Não é possível excluir a congregação '[Nome]' porque ela possui empréstimos registrados:

X empréstimo(s) Em Andamento
Y empréstimo(s) Devolvido(s)

Para excluir esta congregação, primeiro exclua todos os empréstimos relacionados a ela.
```

**Fluxo:**
1. Usuário tenta excluir congregação
2. Sistema verifica Emprestimos
3. Se houver registros: bloqueia e mostra mensagem detalhada
4. Se não houver: pede confirmação e exclui

## Benefícios

### Botão Imprimir Recibo
1. **Facilidade de Acesso**: Impressão direta da listagem
2. **Menos Cliques**: Não precisa abrir detalhes para imprimir
3. **Consistência**: Mesmo padrão da tela de empréstimos
4. **Experiência do Usuário**: Interface mais completa e profissional

### Validações de Exclusão
1. **Integridade de Dados**: Evita perda de histórico
2. **Rastreabilidade**: Mantém registro completo de movimentações
3. **Prevenção de Erros**: Impede exclusões acidentais com impacto no histórico
4. **Informação Clara**: Usuário entende exatamente por que não pode excluir
5. **Orientação**: Informa o que fazer para conseguir excluir

## Observações Técnicas

### Botão Imprimir Recibo
- Cor: `#17A2B8` (Bootstrap info color)
- Usa classe `ReciboRecebimentoPrinter` existente
- Busca empréstimo relacionado via `EmprestimoId`
- Trata caso de recebimento sem empréstimo vinculado

### Validações de Exclusão

**Bem (Item):**
- Verifica `EmprestimoItens` (tabela de relação)
- Agrupa por status do empréstimo pai
- Mensagem dinâmica baseada nos status encontrados

**Congregação:**
- Verifica `Emprestimos` diretamente
- Filtra por `CongregacaoId`
- Agrupa por status
- Mensagem dinâmica baseada nos status encontrados

**Status Tratados:**
- `StatusEmprestimo.EmAndamento` ? "Em Andamento"
- `StatusEmprestimo.Devolvido` ? "Devolvido(s)"
- Outros status ? Exibe o nome do enum

## Exemplos de Uso

### Imprimir Recibo
1. Abrir "Recebimento de Emprestimo"
2. Selecionar um recebimento na lista
3. Clicar em "Imprimir Recibo" (botão azul)
4. Visualizar e imprimir

### Tentar Excluir Bem com Empréstimos
1. Abrir "Bem"
2. Selecionar bem que foi emprestado
3. Clicar em "Excluir"
4. Sistema mostra: "Não é possível excluir... possui 2 empréstimo(s) Em Andamento"
5. Usuário deve excluir os empréstimos primeiro

### Tentar Excluir Congregação com Empréstimos
1. Abrir "Congregação"
2. Selecionar congregação que possui empréstimos
3. Clicar em "Excluir"
4. Sistema mostra: "Não é possível excluir... possui 5 empréstimo(s) Devolvido(s)"
5. Usuário deve excluir os empréstimos primeiro

### Excluir Bem sem Empréstimos (Sucesso)
1. Abrir "Bem"
2. Selecionar bem nunca emprestado
3. Clicar em "Excluir"
4. Confirmar exclusão
5. Sistema mostra: "Bem excluído com sucesso!"

## Testes Sugeridos

### Botão Imprimir Recibo
1. ? Selecionar recebimento e imprimir
2. ? Tentar imprimir sem selecionar (deve mostrar aviso)
3. ? Imprimir recebimento com empréstimo vinculado
4. ? Imprimir recebimento sem empréstimo vinculado
5. ? Verificar preview de impressão

### Validação - Bem
1. ? Tentar excluir bem com empréstimo "Em Andamento"
2. ? Tentar excluir bem com empréstimo "Devolvido"
3. ? Tentar excluir bem com ambos os status
4. ? Excluir bem sem empréstimos (deve permitir)
5. ? Verificar mensagem de bloqueio detalhada
6. ? Verificar mensagem de sucesso

### Validação - Congregação
1. ? Tentar excluir congregação com empréstimo "Em Andamento"
2. ? Tentar excluir congregação com empréstimo "Devolvido"
3. ? Tentar excluir congregação com ambos os status
4. ? Excluir congregação sem empréstimos (deve permitir)
5. ? Verificar mensagem de bloqueio detalhada
6. ? Verificar mensagem de sucesso

## Melhorias Futuras Sugeridas

1. **Opção de Exclusão em Cascata**: Permitir excluir bem/congregação e todos os empréstimos relacionados (com confirmação)
2. **Filtro de Status**: Adicionar filtro na mensagem de bloqueio para mostrar apenas empréstimos ativos
3. **Relatório de Dependências**: Tela para visualizar todas as dependências antes de excluir
4. **Arquivo/Inativação**: Opção de arquivar/inativar ao invés de excluir
5. **Log de Tentativas**: Registrar tentativas de exclusão bloqueadas para auditoria
