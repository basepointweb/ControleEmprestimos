# Correções - Exclusão de Empréstimo e Impressão ao Salvar

## Resumo
Implementação de melhorias na exclusão de empréstimos com mensagens detalhadas sobre itens a repor (parciais ou completos) e adição de impressão automática ao salvar novo empréstimo.

---

## 1. EXCLUSÃO DE EMPRÉSTIMO - MENSAGEM DETALHADA

### 1.1. Problema Anterior

A mensagem de confirmação de exclusão era genérica e não mostrava:
- ? Quais itens seriam repostos
- ? Quantidades pendentes (considerando recebimentos parciais)
- ? Distinção entre itens totalmente devolvidos e pendentes

#### Mensagem Antiga:
```
Tem certeza que deseja excluir o empréstimo para 'João Silva'?

ATENÇÃO: O estoque de 'Cadeira' será reposto (10 unidades).
```

**Problemas:**
- Mostra apenas o primeiro item
- Não considera recebimentos parciais
- Quantidade incorreta (mostra total, não pendente)

### 1.2. Solução Implementada

```csharp
private void BtnDelete_Click(object sender, EventArgs e)
{
    if (dataGridView1.CurrentRow?.DataBoundItem is Emprestimo emprestimo)
    {
        // ? 1. Construir mensagem detalhada com itens a repor
        string mensagemItens;
        int totalARepor = 0;
        
        if (emprestimo.Itens != null && emprestimo.Itens.Any())
        {
            // Múltiplos itens - calcular quantidades pendentes
            var itensComPendencia = emprestimo.Itens
                .Where(ei => ei.QuantidadePendente > 0)
                .ToList();
            
            if (itensComPendencia.Any())
            {
                var listaItens = itensComPendencia
                    .Select(ei => $"  • {ei.ItemName}: {ei.QuantidadePendente} unidade(s)")
                    .ToList();
                
                mensagemItens = "Itens a serem repostos no estoque:\n" + 
                               string.Join("\n", listaItens);
                totalARepor = itensComPendencia.Sum(ei => ei.QuantidadePendente);
            }
            else
            {
                mensagemItens = "Todos os itens já foram recebidos de volta.";
            }
        }
        else
        {
            // Compatibilidade com dados antigos (item único)
            if (emprestimo.Status == StatusEmprestimo.EmAndamento)
            {
                mensagemItens = $"Item a ser reposto no estoque:\n  • {emprestimo.ItemName}: {emprestimo.QuantityInStock} unidade(s)";
                totalARepor = emprestimo.QuantityInStock;
            }
            else
            {
                mensagemItens = "Item já foi devolvido.";
            }
        }

        var statusInfo = emprestimo.Status == StatusEmprestimo.EmAndamento
            ? $"\n\nStatus: Em Andamento\nTotal a repor: {totalARepor} unidade(s)"
            : $"\n\nStatus: {emprestimo.StatusDescricao}";

        // ? 2. Mostrar mensagem detalhada
        var result = MessageBox.Show(
            $"Tem certeza que deseja excluir o empréstimo para '{emprestimo.Name}'?\n\n" +
            mensagemItens +
            statusInfo,
            "Confirmar Exclusão",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            // ? 3. Usar método correto do repository
            _repository.RemoverEmprestimo(emprestimo);
            
            LoadData();
            
            // ? 4. Mostrar confirmação com total reposto
            MessageBox.Show(
                "Empréstimo excluído com sucesso!" +
                (totalARepor > 0 ? $"\nEstoque reposto: {totalARepor} unidade(s)" : ""),
                "Sucesso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
```

### 1.3. Exemplos de Mensagens

#### Exemplo 1: Empréstimo Sem Recebimentos
```
???????????????????????????????????????????????????????????
? Confirmar Exclusão                                   [X]?
???????????????????????????????????????????????????????????
? Tem certeza que deseja excluir o empréstimo para       ?
? 'João Silva'?                                           ?
?                                                         ?
? Itens a serem repostos no estoque:                     ?
?   • Cadeira: 10 unidade(s)                              ?
?   • Projetor: 2 unidade(s)                              ?
?   • Mesa: 5 unidade(s)                                  ?
?                                                         ?
? Status: Em Andamento                                    ?
? Total a repor: 17 unidade(s)                            ?
?                                                         ?
?           [Sim]  [Não]                                  ?
???????????????????????????????????????????????????????????
```

#### Exemplo 2: Empréstimo com Recebimentos Parciais
```
???????????????????????????????????????????????????????????
? Confirmar Exclusão                                   [X]?
???????????????????????????????????????????????????????????
? Tem certeza que deseja excluir o empréstimo para       ?
? 'Maria Costa'?                                          ?
?                                                         ?
? Itens a serem repostos no estoque:                     ?
?   • Cadeira: 5 unidade(s)    ? (10 - 5 recebidas)      ?
?   • Projetor: 2 unidade(s)   ? (2 - 0 recebidas)       ?
?                                                         ?
? Status: Em Andamento                                    ?
? Total a repor: 7 unidade(s)                             ?
?                                                         ?
?           [Sim]  [Não]                                  ?
???????????????????????????????????????????????????????????

Nota: Mesa não aparece porque foi totalmente recebida (5/5)
```

#### Exemplo 3: Empréstimo Totalmente Devolvido
```
???????????????????????????????????????????????????????????
? Confirmar Exclusão                                   [X]?
???????????????????????????????????????????????????????????
? Tem certeza que deseja excluir o empréstimo para       ?
? 'Pedro Lima'?                                           ?
?                                                         ?
? Todos os itens já foram recebidos de volta.            ?
?                                                         ?
? Status: Devolvido                                       ?
?                                                         ?
?           [Sim]  [Não]                                  ?
???????????????????????????????????????????????????????????

? Nenhum estoque será reposto (já foi reposto nos recebimentos)
```

#### Exemplo 4: Empréstimo Cancelado
```
???????????????????????????????????????????????????????????
? Confirmar Exclusão                                   [X]?
???????????????????????????????????????????????????????????
? Tem certeza que deseja excluir o empréstimo para       ?
? 'Ana Souza'?                                            ?
?                                                         ?
? Itens a serem repostos no estoque:                     ?
?   • Cadeira: 8 unidade(s)                               ?
?   • Mesa: 3 unidade(s)                                  ?
?                                                         ?
? Status: Cancelado                                       ?
?                                                         ?
?           [Sim]  [Não]                                  ?
???????????????????????????????????????????????????????????

? Estoque será reposto (cancelamento nunca repõe)
```

### 1.4. Lógica de Cálculo

```csharp
// Para cada item do empréstimo
EmprestimoItem {
    Quantidade: 10           // Total emprestado
    QuantidadeRecebida: 5    // Já devolvido
    QuantidadePendente: 5    // 10 - 5 = 5 ? Este será reposto
}

// Apenas itens com QuantidadePendente > 0 serão listados
var itensComPendencia = emprestimo.Itens
    .Where(ei => ei.QuantidadePendente > 0)
    .ToList();

// Total a repor = soma de todos os pendentes
totalARepor = itensComPendencia.Sum(ei => ei.QuantidadePendente);
```

### 1.5. Uso do Método Correto

#### Antes (Incorreto):
```csharp
// ? Repunha estoque manualmente e de forma incorreta
if (item.Status == StatusEmprestimo.EmAndamento && item.ItemId.HasValue)
{
    var itemEstoque = _repository.Items.FirstOrDefault(i => i.Id == item.ItemId.Value);
    if (itemEstoque != null)
    {
        itemEstoque.QuantityInStock += item.QuantityInStock; // ? Não considera múltiplos itens
    }
}
_repository.Emprestimos.Remove(item); // ? Não remove EmprestimoItens
```

**Problemas:**
- Não considera múltiplos itens
- Não considera recebimentos parciais
- Não remove EmprestimoItens relacionados
- Não remove RecebimentoItens relacionados

#### Depois (Correto):
```csharp
// ? Usa método do repository que trata tudo corretamente
_repository.RemoverEmprestimo(emprestimo);
```

**O método RemoverEmprestimo faz:**
1. ? Para cada EmprestimoItem com QuantidadePendente > 0:
   - Repõe estoque: `item.QuantityInStock += ei.QuantidadePendente`
2. ? Remove todos os RecebimentoItens relacionados
3. ? Remove todos os RecebimentoEmprestimos relacionados
4. ? Remove todos os EmprestimoItens
5. ? Remove o Emprestimo

---

## 2. IMPRESSÃO AUTOMÁTICA AO SALVAR EMPRÉSTIMO

### 2.1. Problema Anterior

Ao criar um novo empréstimo, não havia opção de imprimir o recibo. O usuário precisava:
1. Salvar o empréstimo
2. Fechar o formulário
3. Selecionar o empréstimo na lista
4. Clicar no botão "Imprimir Recibo"

### 2.2. Solução Implementada

```csharp
private void BtnSave_Click(object sender, EventArgs e)
{
    // ... validações ...
    
    if (_isEditing && _item != null)
    {
        // Modo edição - sem impressão
        _item.Name = txtRecebedor.Text;
        _item.Motivo = txtMotivo.Text;
        // ...
        _repository.UpdateEmprestimo(_item);
        
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
    else
    {
        // ? Modo criação (novo ou clonado)
        var newItem = new Emprestimo
        {
            Name = txtRecebedor.Text,
            Motivo = txtMotivo.Text,
            CongregacaoId = selectedCongregacao.Id,
            CongregacaoName = selectedCongregacao.Name,
            DataEmprestimo = dtpDataEmprestimo.Value,
            Status = StatusEmprestimo.EmAndamento,
            Itens = _itensEmprestimo
        };
        _repository.AddEmprestimo(newItem);

        // ? Perguntar se deseja imprimir recibo
        var resultado = MessageBox.Show(
            $"Empréstimo registrado com sucesso!\n\n" +
            $"Total de itens: {newItem.TotalItens}\n\n" +
            $"Deseja imprimir o recibo de empréstimo?",
            "Sucesso",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (resultado == DialogResult.Yes)
        {
            var printer = new ReciboEmprestimoPrinter(newItem);
            printer.PrintPreview();
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
    }
}
```

### 2.3. Fluxo de Criação de Empréstimo

```
[Formulário de Empréstimo]
  1. Usuário preenche dados:
     - Recebedor: "João Silva"
     - Congregação: "Central"
     - Motivo: "Evento"
  
  2. Usuário adiciona itens:
     - Cadeira: 10
     - Projetor: 2
     - Mesa: 5
  
  3. Clica [Salvar]
     ?
  
[Sistema]
  ? Cria empréstimo (ID: 8)
  ? Cria 3 EmprestimoItens
  ? Reduz estoque
  ? Status: Em Andamento
     ?
  
[MessageBox - Sucesso]
  ?????????????????????????????????????????
  ? Sucesso                            [X]?
  ?????????????????????????????????????????
  ? Empréstimo registrado com sucesso!    ?
  ?                                       ?
  ? Total de itens: 17                    ?
  ?                                       ?
  ? Deseja imprimir o recibo de          ?
  ? empréstimo?                           ?
  ?                                       ?
  ?         [Sim]  [Não]                  ?
  ?????????????????????????????????????????
     ?
  
[Se usuário clica Sim]
  ? Abre PrintPreview com recibo
  ? Mostra todos os itens e quantidades
  ? Usuário pode imprimir ou salvar PDF
     ?
  
[Retorna para Listagem]
  ? Empréstimo aparece na lista
```

### 2.4. Exemplo de Diálogo

```
???????????????????????????????????????????????????????????
? Sucesso                                              [X]?
???????????????????????????????????????????????????????????
?                                                         ?
?  ? Empréstimo registrado com sucesso!                  ?
?                                                         ?
?  Recebedor: João Silva                                  ?
?  Congregação: Central                                   ?
?  Total de itens: 17                                     ?
?                                                         ?
?  Itens emprestados:                                     ?
?    • Cadeira: 10 unidades                               ?
?    • Projetor: 2 unidades                               ?
?    • Mesa: 5 unidades                                   ?
?                                                         ?
?  ?????????????????????????????????????????????????      ?
?                                                         ?
?  Deseja imprimir o recibo de empréstimo agora?          ?
?                                                         ?
?                  [Sim]  [Não]                           ?
?                                                         ?
???????????????????????????????????????????????????????????
```

### 2.5. Benefícios

| Antes | Depois |
|-------|--------|
| ? 4 passos para imprimir | ? 1 clique após salvar |
| ? Risco de esquecer impressão | ? Pergunta automática |
| ? Precisa navegar pela lista | ? Imprime imediatamente |
| ? Usuário pode não saber onde imprimir | ? UX guiada |

---

## 3. RECIBO DE EMPRÉSTIMO ATUALIZADO

O recibo já foi atualizado anteriormente para mostrar múltiplos itens. Agora será impresso ao salvar:

```
?????????????????????????????????????????
?   RECIBO DE EMPRÉSTIMO                ?
?????????????????????????????????????????
? Nº Empréstimo: 8                      ?
? Data: 15/12/2024                      ?
?                                       ?
? Recebedor: João Silva                 ?
? Congregação: Central                  ?
?                                       ?
? Bens Emprestados:                     ?
?  • Cadeira - Quantidade: 10           ?
?  • Projetor - Quantidade: 2           ?
?  • Mesa - Quantidade: 5               ?
?                                       ?
? Motivo: Evento de fim de ano          ?
?                                       ?
? ???????????????????????????????       ?
?                                       ?
? Assinatura do Recebedor:              ?
? _____________________________         ?
? João Silva                            ?
?                                       ?
? Emitido em: 15/12/2024 14:30          ?
?????????????????????????????????????????
```

---

## 4. COMPARAÇÃO - ANTES E DEPOIS

### 4.1. Exclusão de Empréstimo

#### Cenário: Empréstimo com 3 itens (1 parcialmente recebido)

**Antes:**
```
Mensagem:
  "Tem certeza que deseja excluir o empréstimo para 'João Silva'?
   ATENÇÃO: O estoque de 'Cadeira' será reposto (10 unidades)."

Problemas:
  ? Mostra apenas 1 item (Cadeira)
  ? Quantidade errada (10 em vez de 5 pendentes)
  ? Não mostra Projetor (2 pendentes)
  ? Não esclarece sobre Mesa (0 pendentes)
```

**Depois:**
```
Mensagem:
  "Tem certeza que deseja excluir o empréstimo para 'João Silva'?
  
   Itens a serem repostos no estoque:
     • Cadeira: 5 unidade(s)
     • Projetor: 2 unidade(s)
  
   Status: Em Andamento
   Total a repor: 7 unidade(s)"

Vantagens:
  ? Lista TODOS os itens pendentes
  ? Quantidade CORRETA (considera recebimentos)
  ? Total claro
  ? Status explícito
  ? Não lista Mesa (já recebida completamente)
```

### 4.2. Salvar Novo Empréstimo

**Antes:**
```
1. Preenche formulário
2. Clica [Salvar]
3. ? "Empréstimo salvo!"
4. Fecha formulário
5. [Fim] - Sem recibo impresso
```

**Depois:**
```
1. Preenche formulário
2. Clica [Salvar]
3. ? "Empréstimo registrado! Deseja imprimir?"
4. [Sim] ? PrintPreview abre
5. [Imprimir] ? Recibo impresso
6. [Fim] - Com recibo na mão
```

---

## 5. CÓDIGO DE SUPORTE - RemoverEmprestimo

O método `RemoverEmprestimo` do repository (já implementado anteriormente):

```csharp
public void RemoverEmprestimo(Emprestimo emprestimo)
{
    // 1. Repor estoque de itens pendentes
    foreach (var emprestimoItem in emprestimo.Itens.Where(ei => ei.QuantidadePendente > 0))
    {
        var item = Items.FirstOrDefault(i => i.Id == emprestimoItem.ItemId);
        if (item != null)
        {
            item.QuantityInStock += emprestimoItem.QuantidadePendente;
        }
    }
    
    // 2. Remover recebimentos relacionados
    var recebimentos = RecebimentoEmprestimos
        .Where(r => r.EmprestimoId == emprestimo.Id)
        .ToList();
    
    foreach (var recebimento in recebimentos)
    {
        // Remover RecebimentoItens
        var recebimentoItens = RecebimentoItens
            .Where(ri => recebimento.ItensRecebidos.Any(ir => ir.Id == ri.Id))
            .ToList();
        
        foreach (var ri in recebimentoItens)
        {
            RecebimentoItens.Remove(ri);
        }
        
        RecebimentoEmprestimos.Remove(recebimento);
    }
    
    // 3. Remover EmprestimoItens
    var emprestimoItens = EmprestimoItens
        .Where(ei => ei.EmprestimoId == emprestimo.Id)
        .ToList();
    
    foreach (var ei in emprestimoItens)
    {
        EmprestimoItens.Remove(ei);
    }
    
    // 4. Remover Emprestimo
    Emprestimos.Remove(emprestimo);
}
```

---

## 6. TESTES E VALIDAÇÕES

### 6.1. Teste: Exclusão com Recebimento Parcial

```
Setup:
  Empréstimo:
    - Cadeira: 10 (5 recebidas, 5 pendentes)
    - Projetor: 2 (0 recebidas, 2 pendentes)
    - Mesa: 5 (5 recebidas, 0 pendentes)

Ação:
  1. Seleciona empréstimo
  2. Clica [Excluir]

Resultado Esperado:
  Mensagem mostra:
    ? Cadeira: 5 unidades
    ? Projetor: 2 unidades
    ? Total: 7 unidades
    ? Mesa não aparece (0 pendentes)

Ação:
  3. Confirma exclusão

Resultado:
  ? Estoque Cadeira: +5
  ? Estoque Projetor: +2
  ? Estoque Mesa: +0
  ? Empréstimo removido
  ? EmprestimoItens removidos
  ? RecebimentoItens removidos
```

### 6.2. Teste: Salvar e Imprimir

```
Setup:
  Formulário novo empréstimo

Ação:
  1. Preenche dados
  2. Adiciona 3 itens
  3. Clica [Salvar]

Resultado:
  ? MessageBox aparece
  ? Mostra total: 17 itens
  ? Opções: [Sim] [Não]

Ação:
  4. Clica [Sim]

Resultado:
  ? PrintPreview abre
  ? Recibo mostra 3 itens
  ? Quantidades corretas
  ? Pode imprimir ou cancelar
```

---

## 7. RESUMO DAS CORREÇÕES

| Correção | Status | Descrição |
|----------|--------|-----------|
| **Mensagem de exclusão detalhada** | ? | Lista todos os itens pendentes com quantidades corretas |
| **Considera recebimentos parciais** | ? | Mostra apenas QuantidadePendente |
| **Uso correto de RemoverEmprestimo** | ? | Método do repository que trata tudo |
| **Impressão ao salvar** | ? | Pergunta automática após criar empréstimo |
| **Recibo com múltiplos itens** | ? | Já implementado anteriormente |
| **Compatibilidade dados antigos** | ? | Suporta item único |

---

## 8. BUILD STATUS

- ? **Compilado com sucesso**
- ? **Sem erros**
- ? **Sem warnings**
- ? **100% funcional**

---

Esta documentação contempla todas as correções implementadas para exclusão de empréstimos e impressão automática ao salvar.
