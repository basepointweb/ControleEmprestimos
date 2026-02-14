# Mudanças - Sistema de Múltiplos Itens e Recebimentos Parciais

## Resumo
Implementação completa de sistema para permitir múltiplos itens por empréstimo e recebimentos parciais, com controle granular de quantidades e status dinâmico.

---

## 1. NOVA ESTRUTURA DE DADOS

### 1.1. Modelo EmprestimoItem (NOVO)

**Localização:** `Models\EmprestimoItem.cs`

```csharp
public class EmprestimoItem
{
    public int Id { get; set; }
    public int EmprestimoId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantidade { get; set; }
    public int QuantidadeRecebida { get; set; } = 0;
    
    // Propriedades calculadas
    public int QuantidadePendente => Quantidade - QuantidadeRecebida;
    public bool TotalmenteRecebido => QuantidadeRecebida >= Quantidade;
    
    // Auditoria
    public DateTime DataCriacao { get; set; }
    public DateTime DataAlteracao { get; set; }
}
```

**Características:**
- ? Representa **um item** dentro de um empréstimo
- ? Rastreia **quantidade emprestada** vs **quantidade recebida**
- ? Calcula **quantidade pendente** automaticamente
- ? Indica se item foi **totalmente recebido**

**Exemplo de Dados:**
```csharp
EmprestimoId: 1
[
    { Id: 1, ItemId: 1, ItemName: "Cadeira", Quantidade: 10, QuantidadeRecebida: 5 },
    { Id: 2, ItemId: 3, ItemName: "Projetor", Quantidade: 2, QuantidadeRecebida: 0 }
]
```

### 1.2. Modelo RecebimentoItem (NOVO)

**Localização:** `Models\RecebimentoItem.cs`

```csharp
public class RecebimentoItem
{
    public int Id { get; set; }
    public int RecebimentoEmprestimoId { get; set; }
    public int EmprestimoItemId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int QuantidadeRecebida { get; set; }
    
    // Auditoria
    public DateTime DataCriacao { get; set; }
    public DateTime DataAlteracao { get; set; }
}
```

**Características:**
- ? Representa **um item recebido** em um recebimento
- ? Vincula ao **EmprestimoItem** original
- ? Registra **quantidade parcial** recebida
- ? Permite **múltiplos recebimentos** do mesmo item

**Exemplo de Dados:**
```csharp
RecebimentoEmprestimoId: 1
[
    { Id: 1, EmprestimoItemId: 1, ItemName: "Cadeira", QuantidadeRecebida: 5 }
]

RecebimentoEmprestimoId: 2 (segundo recebimento)
[
    { Id: 2, EmprestimoItemId: 1, ItemName: "Cadeira", QuantidadeRecebida: 5 }
]
```

---

## 2. MODELO EMPRESTIMO ATUALIZADO

### 2.1. Estrutura Nova

```csharp
public class Emprestimo
{
    // Campos existentes
    public int Id { get; set; }
    public string Name { get; set; }         // Recebedor
    public string Motivo { get; set; }
    public int? CongregacaoId { get; set; }
    public string CongregacaoName { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public StatusEmprestimo Status { get; set; }
    
    // NOVO: Lista de itens
    public List<EmprestimoItem> Itens { get; set; } = new();
    
    // NOVO: Propriedades calculadas
    public int TotalItens => Itens.Sum(i => i.Quantidade);
    public int TotalRecebido => Itens.Sum(i => i.QuantidadeRecebida);
    public int TotalPendente => TotalItens - TotalRecebido;
    public bool TodosItensRecebidos => Itens.All(i => i.TotalmenteRecebido);
    
    // Campos antigos (deprecated para compatibilidade)
    [Obsolete] public int? ItemId { get; set; }
    [Obsolete] public string ItemName { get; set; }
    [Obsolete] public int QuantityInStock { get; set; }
}
```

### 2.2. Mudanças Principais

#### Antes:
```csharp
Emprestimo
{
    ItemId: 3,
    ItemName: "Projetor",
    QuantityInStock: 2
}
```

#### Depois:
```csharp
Emprestimo
{
    Itens: [
        { ItemId: 1, ItemName: "Cadeira", Quantidade: 10, QuantidadeRecebida: 5 },
        { ItemId: 3, ItemName: "Projetor", Quantidade: 2, QuantidadeRecebida: 0 }
    ],
    TotalItens: 12,
    TotalRecebido: 5,
    TotalPendente: 7,
    TodosItensRecebidos: false
}
```

### 2.3. Propriedades Calculadas

| Propriedade | Cálculo | Uso |
|-------------|---------|-----|
| **TotalItens** | Soma de todas as quantidades | Total emprestado |
| **TotalRecebido** | Soma das quantidades recebidas | Total já devolvido |
| **TotalPendente** | TotalItens - TotalRecebido | Resta devolver |
| **TodosItensRecebidos** | Todos itens 100% recebidos | Status Devolvido |

---

## 3. MODELO RECEBIMENTO ATUALIZADO

### 3.1. Estrutura Nova

```csharp
public class RecebimentoEmprestimo
{
    // Campos existentes
    public int Id { get; set; }
    public string Name { get; set; }
    public string NomeRecebedor { get; set; }        // Quem pegou
    public string NomeQuemRecebeu { get; set; }      // Quem recebeu
    public int? EmprestimoId { get; set; }
    public DateTime? DataEmprestimo { get; set; }
    public DateTime DataRecebimento { get; set; }
    
    // NOVO: Lista de itens recebidos
    public List<RecebimentoItem> ItensRecebidos { get; set; } = new();
    
    // NOVO: Propriedades
    public int TotalItensRecebidos => ItensRecebidos.Sum(i => i.QuantidadeRecebida);
    public bool RecebimentoParcial { get; set; }
    
    // Campo antigo (deprecated)
    [Obsolete] public int QuantityInStock { get; set; }
}
```

### 3.2. Recebimentos Múltiplos

**Empréstimo com 3 itens:**
```csharp
Emprestimo ID: 1
Itens:
  - Cadeira: 10 (0 recebidas)
  - Mesa: 5 (0 recebidas)
  - Projetor: 2 (0 recebidas)
Status: Em Andamento
```

**Primeiro Recebimento (Parcial):**
```csharp
RecebimentoEmprestimo ID: 1
EmprestimoId: 1
ItensRecebidos:
  - Cadeira: 5
  - Projetor: 2
RecebimentoParcial: true

Emprestimo ID: 1 (atualizado)
Itens:
  - Cadeira: 10 (5 recebidas) ? 5 pendentes
  - Mesa: 5 (0 recebidas) ? 5 pendentes
  - Projetor: 2 (2 recebidas) ? 0 pendentes (completo)
Status: Em Andamento ? Ainda tem itens pendentes
```

**Segundo Recebimento (Parcial):**
```csharp
RecebimentoEmprestimo ID: 2
EmprestimoId: 1
ItensRecebidos:
  - Cadeira: 3
RecebimentoParcial: true

Emprestimo ID: 1 (atualizado)
Itens:
  - Cadeira: 10 (8 recebidas) ? 2 pendentes
  - Mesa: 5 (0 recebidas) ? 5 pendentes
  - Projetor: 2 (2 recebidas) ? completo
Status: Em Andamento ? Ainda tem itens pendentes
```

**Terceiro Recebimento (Final):**
```csharp
RecebimentoEmprestimo ID: 3
EmprestimoId: 1
ItensRecebidos:
  - Cadeira: 2
  - Mesa: 5
RecebimentoParcial: false

Emprestimo ID: 1 (atualizado)
Itens:
  - Cadeira: 10 (10 recebidas) ? completo
  - Mesa: 5 (5 recebidas) ? completo
  - Projetor: 2 (2 recebidas) ? completo
Status: Devolvido ? Todos itens recebidos!
```

---

## 4. DATAREPOSITORY ATUALIZADO

### 4.1. Novas Coleções

```csharp
public List<EmprestimoItem> EmprestimoItens { get; } = new();
public List<RecebimentoItem> RecebimentoItens { get; } = new();
```

### 4.2. AddEmprestimo() Atualizado

```csharp
public int AddEmprestimo(Emprestimo emprestimo)
{
    // Gerar ID do empréstimo
    emprestimo.Id = _nextEmprestimoId++;
    
    // Processar cada item
    foreach (var emprestimoItem in emprestimo.Itens)
    {
        emprestimoItem.Id = _nextEmprestimoItemId++;
        emprestimoItem.EmprestimoId = emprestimo.Id;
        
        // Obter item do estoque
        var item = Items.FirstOrDefault(i => i.Id == emprestimoItem.ItemId);
        if (item != null)
        {
            emprestimoItem.ItemName = item.Name;
            
            // Reduzir estoque
            item.QuantityInStock -= emprestimoItem.Quantidade;
        }
        
        EmprestimoItens.Add(emprestimoItem);
    }
    
    Emprestimos.Add(emprestimo);
    return emprestimo.Id;
}
```

**Fluxo:**
1. ? Gera ID para empréstimo
2. ? Processa cada item da lista
3. ? Gera ID para cada EmprestimoItem
4. ? Obtém nome do item
5. ? **Reduz estoque pela quantidade**
6. ? Adiciona à coleção EmprestimoItens

### 4.3. AddRecebimento() Atualizado

```csharp
public int AddRecebimento(RecebimentoEmprestimo recebimento)
{
    // Gerar ID do recebimento
    recebimento.Id = _nextRecebimentoId++;
    
    // Processar cada item recebido
    foreach (var recebimentoItem in recebimento.ItensRecebidos)
    {
        recebimentoItem.Id = _nextRecebimentoItemId++;
        recebimentoItem.RecebimentoEmprestimoId = recebimento.Id;
        
        // Atualizar quantidade recebida no EmprestimoItem
        var emprestimoItem = EmprestimoItens
            .FirstOrDefault(ei => ei.Id == recebimentoItem.EmprestimoItemId);
        
        if (emprestimoItem != null)
        {
            emprestimoItem.QuantidadeRecebida += recebimentoItem.QuantidadeRecebida;
            
            // Repor estoque
            var item = Items.FirstOrDefault(i => i.Id == emprestimoItem.ItemId);
            if (item != null)
            {
                item.QuantityInStock += recebimentoItem.QuantidadeRecebida;
            }
        }
        
        RecebimentoItens.Add(recebimentoItem);
    }
    
    RecebimentoEmprestimos.Add(recebimento);
    return recebimento.Id;
}
```

**Fluxo:**
1. ? Gera ID para recebimento
2. ? Processa cada item recebido
3. ? Atualiza **QuantidadeRecebida** no EmprestimoItem
4. ? **Repõe estoque pela quantidade recebida**
5. ? Adiciona à coleção RecebimentoItens

### 4.4. DevolverEmprestimo() Atualizado

```csharp
public void DevolverEmprestimo(Emprestimo emprestimo)
{
    // Verificar se todos os itens foram recebidos
    if (emprestimo.TodosItensRecebidos)
    {
        emprestimo.Status = StatusEmprestimo.Devolvido;
    }
    else
    {
        emprestimo.Status = StatusEmprestimo.EmAndamento;
    }
    
    emprestimo.DataAlteracao = DateTime.Now;
}
```

**Lógica:**
- ? Se **TodosItensRecebidos** = true ? Status = **Devolvido**
- ? Se ainda tem itens pendentes ? Status = **Em Andamento**

### 4.5. RemoverEmprestimo() (NOVO)

```csharp
public void RemoverEmprestimo(Emprestimo emprestimo)
{
    // Repor estoque de todos os itens não recebidos
    foreach (var emprestimoItem in emprestimo.Itens)
    {
        var quantidadePendente = emprestimoItem.QuantidadePendente;
        if (quantidadePendente > 0)
        {
            var item = Items.FirstOrDefault(i => i.Id == emprestimoItem.ItemId);
            if (item != null)
            {
                item.QuantityInStock += quantidadePendente;
            }
        }
        
        EmprestimoItens.Remove(emprestimoItem);
    }
    
    Emprestimos.Remove(emprestimo);
}
```

**Lógica:**
- ? Para cada item, calcula **QuantidadePendente**
- ? Repõe estoque **apenas da quantidade não recebida**
- ? Remove todos os EmprestimoItens
- ? Remove o Emprestimo

**Exemplo:**
```
Emprestimo:
  - Cadeira: 10 emprestadas, 5 recebidas
    ? Repor: 5 (10 - 5)
  - Projetor: 2 emprestadas, 2 recebidas
    ? Repor: 0 (já recebido totalmente)
```

### 4.6. RemoverRecebimento() (NOVO)

```csharp
public void RemoverRecebimento(RecebimentoEmprestimo recebimento)
{
    if (recebimento.EmprestimoId.HasValue)
    {
        var emprestimo = Emprestimos
            .FirstOrDefault(e => e.Id == recebimento.EmprestimoId.Value);
        
        if (emprestimo != null)
        {
            // Reverter quantidades recebidas e estoque
            foreach (var recebimentoItem in recebimento.ItensRecebidos)
            {
                var emprestimoItem = EmprestimoItens
                    .FirstOrDefault(ei => ei.Id == recebimentoItem.EmprestimoItemId);
                
                if (emprestimoItem != null)
                {
                    // Reverter quantidade recebida
                    emprestimoItem.QuantidadeRecebida -= recebimentoItem.QuantidadeRecebida;
                }
                
                // Reduzir estoque novamente
                var item = Items.FirstOrDefault(i => i.Id == recebimentoItem.ItemId);
                if (item != null)
                {
                    item.QuantityInStock -= recebimentoItem.QuantidadeRecebida;
                }
                
                RecebimentoItens.Remove(recebimentoItem);
            }
            
            // Atualizar status do empréstimo
            emprestimo.Status = emprestimo.TodosItensRecebidos 
                ? StatusEmprestimo.Devolvido 
                : StatusEmprestimo.EmAndamento;
        }
    }
    
    RecebimentoEmprestimos.Remove(recebimento);
}
```

**Lógica:**
- ? Para cada item recebido, **reverte QuantidadeRecebida**
- ? **Reduz estoque** novamente (desfaz reposição)
- ? Remove todos os RecebimentoItens
- ? Recalcula status do empréstimo
- ? Remove o RecebimentoEmprestimo

---

## 5. CONTROLE DE ESTOQUE

### 5.1. Ao Emprestar

```csharp
Estoque Inicial:
  Cadeira: 50
  Projetor: 5

Empréstimo criado:
  - Cadeira: 10
  - Projetor: 2

Estoque Atualizado:
  Cadeira: 40 (50 - 10) ?
  Projetor: 3 (5 - 2) ?
```

### 5.2. Ao Receber Parcialmente

```csharp
Recebimento 1:
  - Cadeira: 5

Estoque Atualizado:
  Cadeira: 45 (40 + 5) ?
  Projetor: 3 (sem mudança)

Recebimento 2:
  - Cadeira: 5
  - Projetor: 2

Estoque Atualizado:
  Cadeira: 50 (45 + 5) ? (volta ao original)
  Projetor: 5 (3 + 2) ? (volta ao original)
```

### 5.3. Ao Excluir Empréstimo

```csharp
Emprestimo com itens parcialmente recebidos:
  - Cadeira: 10 (5 recebidas)
  - Projetor: 2 (2 recebidas)

Ao excluir:
  Cadeira: Repor 5 (10 - 5) ?
  Projetor: Repor 0 (2 - 2) ?

Estoque Final:
  Cadeira: +5 unidades
  Projetor: +0 unidades (já estava completo)
```

### 5.4. Ao Excluir Recebimento

```csharp
Recebimento contém:
  - Cadeira: 5 recebidas

Ao excluir:
  1. Reverter QuantidadeRecebida no EmprestimoItem:
     Cadeira: 5 recebidas ? 0 recebidas ?
  
  2. Reduzir estoque novamente:
     Estoque Cadeira: -5 unidades ?
  
  3. Recalcular status do empréstimo:
     Se ainda tem itens pendentes ? Em Andamento ?
```

---

## 6. GESTÃO DE STATUS

### 6.1. Status Dinâmico

| Situação | Status |
|----------|--------|
| **Todos itens pendentes** | Em Andamento |
| **Alguns itens recebidos** | Em Andamento |
| **Todos itens recebidos** | Devolvido |
| **Cancelado manualmente** | Cancelado |

### 6.2. Cálculo Automático

```csharp
public bool TodosItensRecebidos => Itens.All(i => i.TotalmenteRecebido);

// Verifica se CADA item foi 100% recebido
```

**Exemplos:**
```
Caso 1:
  Cadeira: 10/10 recebidas ?
  Projetor: 2/2 recebidas ?
  ? TodosItensRecebidos = true
  ? Status = Devolvido

Caso 2:
  Cadeira: 8/10 recebidas ?
  Projetor: 2/2 recebidas ?
  ? TodosItensRecebidos = false
  ? Status = Em Andamento

Caso 3:
  Cadeira: 0/10 recebidas ?
  Projetor: 0/2 recebidas ?
  ? TodosItensRecebidos = false
  ? Status = Em Andamento
```

---

## 7. PRÓXIMAS IMPLEMENTAÇÕES NECESSÁRIAS

### 7.1. EmprestimoDetailForm (PENDENTE)

**Mudanças Necessárias:**
- ? Remover campos únicos (ItemId, Quantidade)
- ? Adicionar DataGridView para itens
- ? Botão "Adicionar Item"
- ? Botão "Remover Item"
- ? Validação de estoque por item
- ? Campo quantidade por linha

**Layout Sugerido:**
```
?????????????????????????????????????????
? Recebedor: [________________]         ?
? Congregação: [_______________]        ?
? Motivo: [_______________________]    ?
? Data: [__________]                    ?
?                                       ?
? Itens do Empréstimo:                  ?
? ???????????????????????????????????  ?
? ? Bem       ? Qtd ? [Remover]     ?  ?
? ???????????????????????????????????  ?
? ? Cadeira   ? 10  ? [X]           ?  ?
? ? Projetor  ? 2   ? [X]           ?  ?
? ???????????????????????????????????  ?
?                                       ?
? [Adicionar Item]                      ?
?                                       ?
? Total de Itens: 12                    ?
?                                       ?
? [Salvar] [Cancelar]                   ?
?????????????????????????????????????????
```

### 7.2. RecebimentoDetailForm (PENDENTE)

**Mudanças Necessárias:**
- ? Carregar itens do empréstimo
- ? Mostrar QuantidadePendente por item
- ? DataGridView com checkboxes
- ? Campo quantidade a receber por item
- ? Validação de quantidade máxima
- ? Recálculo de status do empréstimo

**Layout Sugerido:**
```
??????????????????????????????????????????
? Empréstimo: [____________________]    ?
? Quem Pegou: [____________________]    ?
? Quem Recebeu: [__________________]    ?
? Data Recebimento: [__________]         ?
?                                        ?
? Itens a Receber:                       ?
? ????????????????????????????????????  ?
? ? ? ? Bem     ? Empr. ? Rec. ? Pend?  ?
? ????????????????????????????????????  ?
? ? ? ? Cadeira ? 10    ? 5    ? 5  ?  ?
? ? ? ? Projetor? 2     ? 0    ? 2  ?  ?
? ????????????????????????????????????  ?
?                                        ?
? Quantidade a Receber: [__]             ?
? (para item selecionado)                ?
?                                        ?
? [Salvar] [Cancelar] [Imprimir Recibo] ?
??????????????????????????????????????????
```

### 7.3. EmprestimoListForm (PENDENTE)

**Mudanças Necessárias:**
- ? Atualizar colunas do grid
- ? Remover coluna "Bem" (único)
- ? Adicionar coluna "Total Itens"
- ? Adicionar coluna "Total Recebido"
- ? Adicionar coluna "Pendente"
- ? Filtro funcionando com novos campos

**Colunas Sugeridas:**
```
ID | Recebedor | Congr. | Tot.Itens | Recebido | Pendente | Status | Data
```

### 7.4. Recibos (PENDENTE)

**ReciboEmprestimoPrinter:**
- ? Listar todos os itens emprestados
- ? Mostrar quantidade de cada

**ReciboRecebimentoPrinter:**
- ? Listar apenas itens recebidos neste recebimento
- ? Indicar se é recebimento parcial
- ? Mostrar quantidades pendentes

---

## 8. COMPATIBILIDADE

### 8.1. Campos Deprecated

Para manter compatibilidade temporária:

```csharp
// Em Emprestimo
[Obsolete("Use Itens ao invés de ItemId")]
public int? ItemId { get; set; }

[Obsolete("Use Itens ao invés de ItemName")]
public string ItemName { get; set; }

[Obsolete("Use TotalItens ao invés de QuantityInStock")]
public int QuantityInStock { get; set; }

// Em RecebimentoEmprestimo
[Obsolete("Use ItensRecebidos ao invés de QuantityInStock")]
public int QuantityInStock { get; set; }
```

**Propósito:**
- ? Código antigo continua compilando
- ? Avisos do compilador indicam uso deprecated
- ? Facilita migração gradual
- ? Permite testar novas funcionalidades

### 8.2. Migração de Dados

**Dados existentes precisarão ser migrados:**

```csharp
// Emprestimo antigo
{
    ItemId: 3,
    ItemName: "Projetor",
    QuantityInStock: 2
}

// Migrar para novo formato
{
    Itens: [
        {
            ItemId: 3,
            ItemName: "Projetor",
            Quantidade: 2,
            QuantidadeRecebida: 0
        }
    ]
}
```

---

## 9. BENEFÍCIOS IMPLEMENTADOS

### 9.1. Flexibilidade
- ? **Múltiplos itens** por empréstimo
- ? **Diferentes quantidades** por item
- ? **Recebimentos parciais** de qualquer item
- ? **Múltiplos recebimentos** do mesmo empréstimo

### 9.2. Controle
- ? **Rastreamento granular** por item
- ? **Quantidades exatas** emprestadas vs recebidas
- ? **Status dinâmico** baseado em recebimentos
- ? **Histórico completo** de recebimentos

### 9.3. Estoque
- ? **Controle preciso** por item e quantidade
- ? **Reposição parcial** conforme recebimento
- ? **Reversão correta** ao excluir
- ? **Consistência** garantida

### 9.4. Relatórios
- ? **Recibos por recebimento** (parcial ou total)
- ? **Histórico de recebimentos** por empréstimo
- ? **Pendências** por item
- ? **Auditoria completa**

---

## 10. RESUMO TÉCNICO

### Modelos Criados:
1. ? **EmprestimoItem** - Item dentro de empréstimo
2. ? **RecebimentoItem** - Item recebido em recebimento

### Modelos Atualizados:
1. ? **Emprestimo** - Lista de itens, propriedades calculadas
2. ? **RecebimentoEmprestimo** - Lista de itens recebidos

### DataRepository Atualizado:
1. ? Novas coleções (EmprestimoItens, RecebimentoItens)
2. ? AddEmprestimo() - Processa múltiplos itens
3. ? AddRecebimento() - Recebimento parcial
4. ? DevolverEmprestimo() - Status dinâmico
5. ? RemoverEmprestimo() - Repor pendentes
6. ? RemoverRecebimento() - Reverter parciais

### Build Status:
- ? **Compilado com sucesso**
- ? **Estrutura de dados completa**
- ? **Lógica de negócio implementada**

### Pendente:
- ? **EmprestimoDetailForm** - Grid de itens
- ? **RecebimentoDetailForm** - Recebimento parcial
- ? **EmprestimoListForm** - Colunas atualizadas
- ? **Recibos** - Múltiplos itens
- ? **Filtros** - Novos campos

---

Esta documentação cobre a estrutura de dados e lógica de negócio. Os formulários precisam ser atualizados para utilizar essa nova estrutura.
