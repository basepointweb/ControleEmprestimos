# Mudanças - Recibos de Empréstimo e Recebimento com Impressão

## Resumo
Implementação completa de recibos de empréstimo e recebimento com funcionalidade de impressão/visualização, otimizados para meia folha A4, incluindo campos para assinatura.

---

## 1. NOVO CAMPO NO MODELO

### 1.1. RecebimentoEmprestimo - NomeQuemRecebeu

```csharp
public class RecebimentoEmprestimo
{
    public string NomeRecebedor { get; set; }      // Quem pegou emprestado
    public string NomeQuemRecebeu { get; set; }    // Quem recebeu de volta (NOVO)
}
```

**Diferenciação:**
- **NomeRecebedor**: Pessoa que pegou o bem emprestado
- **NomeQuemRecebeu**: Pessoa que recebeu o bem de volta (pode ser diferente)

**Motivo:**
- ? Pessoa que empresta pode não ser a mesma que recebe
- ? Rastreabilidade completa
- ? Responsabilização adequada

---

## 2. RECIBO DE EMPRÉSTIMO

### 2.1. Classe ReciboEmprestimoPrinter

**Localização:** `Reports\ReciboEmprestimoPrinter.cs`

```csharp
public class ReciboEmprestimoPrinter
{
    private Emprestimo _emprestimo;

    public void Print()              // Impressão direta
    public void PrintPreview()        // Visualização antes de imprimir
}
```

### 2.2. Conteúdo do Recibo

```
?????????????????????????????????????????????????????????
?         RECIBO DE EMPRÉSTIMO                          ?
?????????????????????????????????????????????????????????
?                                                       ?
? Nº Empréstimo: 5                                      ?
? Data: 15/12/2024                                      ?
?                                                       ?
? Recebedor:                                            ?
?   João Silva                                          ?
?                                                       ?
? Congregação:                                          ?
?   Congregação Central                                 ?
?                                                       ?
? Bem Emprestado:                                       ?
?   Projetor - Quantidade: 2                            ?
?                                                       ?
? Motivo:                                               ?
?   Evento especial de fim de ano para                  ?
?   apresentação multimídia                             ?
?                                                       ?
?????????????????????????????????????????????????????????
?                                                       ?
? Assinatura do Recebedor:                              ?
?                                                       ?
? _____________________________________                 ?
? João Silva                                            ?
?                                                       ?
?                                                       ?
? Emitido em: 15/12/2024 14:30                          ?
? Este recibo comprova o recebimento dos itens         ?
? acima descritos.                                      ?
?????????????????????????????????????????????????????????
```

### 2.3. Características

**Dimensões:**
- ? Meia folha A4 (210mm x 148mm)
- ? ~583 x 827 pixels (72 DPI)
- ? Cabe perfeitamente em meia folha

**Fontes:**
- **Título**: Arial 14pt Bold
- **Headers**: Arial 10pt Bold
- **Conteúdo**: Arial 9pt Regular
- **Rodapé**: Arial 8pt Regular

**Elementos:**
1. ? Título destacado
2. ? Número do empréstimo
3. ? Data do empréstimo
4. ? Nome do recebedor
5. ? Congregação
6. ? Bem e quantidade
7. ? Motivo (com quebra de linha automática)
8. ? Linha para assinatura
9. ? Nome impresso sob a linha
10. ? Rodapé com data de emissão

### 2.4. Funcionalidades Especiais

#### Quebra de Linha Automática
```csharp
private List<string> WrapText(string text, Font font, int maxWidth, Graphics graphics)
{
    // Quebra texto longo em múltiplas linhas
    // Evita que motivo ultrapasse margem
}
```

**Exemplo:**
```
Motivo:
  Evento especial de fim de ano para
  apresentação multimídia durante o culto
  de adoração com participação especial
```

---

## 3. RECIBO DE RECEBIMENTO

### 3.1. Classe ReciboRecebimentoPrinter

**Localização:** `Reports\ReciboRecebimentoPrinter.cs`

```csharp
public class ReciboRecebimentoPrinter
{
    private RecebimentoEmprestimo _recebimento;
    private Emprestimo? _emprestimo;

    public void Print()
    public void PrintPreview()
}
```

### 3.2. Conteúdo do Recibo

```
?????????????????????????????????????????????????????????
?         RECIBO DE RECEBIMENTO                         ?
?????????????????????????????????????????????????????????
?                                                       ?
? Nº Empréstimo: 5                                      ?
? Data de Empréstimo: 10/12/2024                        ?
? Data de Recebimento: 15/12/2024 14:30                 ?
?                                                       ?
? Quem Pegou Emprestado:                                ?
?   João Silva                                          ?
?                                                       ?
? Bem Devolvido:                                        ?
?   Projetor - Quantidade: 2                            ?
?                                                       ?
? Congregação:                                          ?
?   Congregação Central                                 ?
?                                                       ?
?????????????????????????????????????????????????????????
?                                                       ?
? Recebido por:                                         ?
?   Maria Santos                                        ?
?                                                       ?
? Assinatura de Quem Recebeu:                           ?
?                                                       ?
? _____________________________________                 ?
? Maria Santos                                          ?
?                                                       ?
?                                                       ?
? Emitido em: 15/12/2024 14:32                          ?
? Este recibo comprova a devolução dos itens            ?
? acima descritos.                                      ?
?????????????????????????????????????????????????????????
```

### 3.3. Características

**Dimensões:**
- ? Meia folha A4 (210mm x 148mm)
- ? Mesmo tamanho do recibo de empréstimo
- ? Otimizado para impressão econômica

**Diferenças do Recibo de Empréstimo:**
1. ? Título: "RECIBO DE RECEBIMENTO"
2. ? Mostra data de empréstimo E recebimento
3. ? "Quem Pegou Emprestado" em vez de "Recebedor"
4. ? Campo adicional: "Recebido por"
5. ? Assinatura de quem recebeu (não de quem emprestou)

---

## 4. FORMULÁRIO DE RECEBIMENTO ATUALIZADO

### 4.1. Mudanças no RecebimentoDetailForm

#### Labels Atualizadas:
```csharp
// ANTES
lblRecebedor.Text = "Recebedor:"

// DEPOIS
lblQuemPegou.Text = "Quem Pegou Emprestado:"
lblQuemRecebeu.Text = "Quem Recebeu de Volta:"
```

#### Novo Campo:
```csharp
private TextBox txtQuemRecebeu;  // Campo de texto livre para digitação
```

**Características:**
- ? Campo editável (não read-only)
- ? Obrigatório para salvar
- ? 560px de largura
- ? Localização: Abaixo da data de recebimento

#### Botão de Impressão:
```csharp
private Button btnImprimirRecibo;
```

**Características:**
- ? Cor azul-turquesa (RGB: 23, 162, 184)
- ? Texto branco
- ? Visível apenas em modo visualização
- ? Abre visualização de impressão

### 4.2. Validação no Salvamento

```csharp
if (string.IsNullOrWhiteSpace(txtQuemRecebeu.Text))
{
    MessageBox.Show("Por favor, informe quem recebeu de volta.");
    return;
}
```

**Nova validação:**
- ? Campo NomeQuemRecebeu obrigatório

### 4.3. Impressão Automática Após Salvar

```csharp
var resultado = MessageBox.Show(
    "Recebimento registrado com sucesso!\n\nDeseja imprimir o recibo?",
    "Sucesso",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);

if (resultado == DialogResult.Yes)
{
    var printer = new ReciboRecebimentoPrinter(newItem, emprestimoSelecionado);
    printer.PrintPreview();
}
```

**Fluxo:**
1. ? Salva recebimento
2. ? Pergunta se deseja imprimir
3. ? Se sim, abre visualização
4. ? Se não, fecha formulário

---

## 5. LISTAGEM DE EMPRÉSTIMOS ATUALIZADA

### 5.1. Novo Botão "Imprimir Recibo"

```csharp
private Button btnImprimirRecibo;
```

**Características:**
- ? Cor azul-turquesa (RGB: 23, 162, 184)
- ? Texto branco
- ? Largura: 120px
- ? Posicionado entre "Clonar" e "Receber de Volta"

**Posicionamento dos Botões:**
```
[Criar] [Editar] [Excluir] [Clonar] [Imprimir Recibo] [Receber de Volta]
  12px    118px    224px    330px       436px             562px
```

### 5.2. Funcionalidade

```csharp
private void BtnImprimirRecibo_Click(object sender, EventArgs e)
{
    if (dataGridView1.CurrentRow?.DataBoundItem is Emprestimo emprestimo)
    {
        var printer = new ReciboEmprestimoPrinter(emprestimo);
        printer.PrintPreview();
    }
}
```

**Comportamento:**
1. ? Seleciona empréstimo no grid
2. ? Clica "Imprimir Recibo"
3. ? Abre visualização de impressão
4. ? Pode imprimir ou salvar como PDF

---

## 6. FUNCIONALIDADES DE IMPRESSÃO

### 6.1. PrintPreview()

**Características:**
- ? Abre janela de visualização
- ? Tamanho: 800x600
- ? Centralizada na tela
- ? Permite imprimir, salvar PDF, ou cancelar

**Botões da Visualização:**
- ??? Imprimir
- ?? Salvar como PDF
- ?? Zoom
- ? Fechar

### 6.2. Print()

**Características:**
- ? Abre diálogo de impressão
- ? Permite escolher impressora
- ? Configurações de impressão
- ? Imprime direto

### 6.3. Configuração de Página

```csharp
printDocument.DefaultPageSettings.PaperSize = new PaperSize("Half A4", 827, 583);
```

**Meia Folha A4:**
- ? Largura: 210mm (~827 pixels)
- ? Altura: 148mm (~583 pixels)
- ? Orientação: Paisagem
- ? Econômico (2 recibos por folha)

---

## 7. FLUXOS COMPLETOS

### 7.1. Fluxo: Emprestar e Imprimir Recibo

```
[Listagem de Bens]
    ? Seleciona "Projetor"
    ? Clica "Emprestar"
[Formulário de Empréstimo]
    ? Preenche dados
    ? Salva
[Listagem de Empréstimos]
    ? Novo empréstimo aparece no grid
    ? Seleciona empréstimo
    ? Clica "Imprimir Recibo"
[Visualização de Impressão]
    ? Mostra recibo formatado
    ? Pronto para assinar
    ? Opções:
      - Imprimir
      - Salvar PDF
      - Fechar
```

### 7.2. Fluxo: Receber de Volta e Imprimir Recibo

```
[Listagem de Empréstimos]
    ? Seleciona empréstimo "Em Andamento"
    ? Clica "Receber de Volta"
[Formulário de Recebimento]
    ? Empréstimo pré-selecionado
    ? "Quem Pegou Emprestado": João Silva (automático)
    ? Digita "Quem Recebeu de Volta": Maria Santos
    ? Salva
[Pergunta de Impressão]
    "Deseja imprimir o recibo?"
    ? Clica [Sim]
[Visualização de Impressão]
    ? Recibo de recebimento formatado
    ? Mostra ambos os nomes
    ? Linha para assinatura de Maria Santos
    ? Imprime ou salva PDF
```

### 7.3. Fluxo: Reimprimir Recibo de Recebimento

```
[Listagem de Recebimentos]
    ? Seleciona recebimento
    ? Clica "Editar" (modo visualização)
[Formulário de Recebimento]
    ? Modo visualização (sem edição)
    ? Botão "Imprimir Recibo" visível
    ? Clica "Imprimir Recibo"
[Visualização de Impressão]
    ? Recibo completo
    ? Pronto para reimprimir
```

---

## 8. EXEMPLOS PRÁTICOS

### 8.1. Caso de Uso: Empréstimo para Evento

**Situação:**
- Projetor emprestado para evento
- Precisa de recibo assinado

**Passos:**
1. Criar empréstimo
2. Imprimir recibo de empréstimo
3. Levar recibo impresso para o evento
4. Recebedor assina no local
5. Guardar recibo assinado

**Resultado:**
- ? Comprovante físico de empréstimo
- ? Assinatura do responsável
- ? Rastreabilidade completa

### 8.2. Caso de Uso: Devolução por Pessoa Diferente

**Situação:**
- João pegou emprestado
- Maria devolveu (representando congregação)

**Passos:**
1. Receber de volta
2. Campo "Quem Pegou": João Silva (automático)
3. Campo "Quem Recebeu": Maria Santos (digitar)
4. Salvar
5. Imprimir recibo
6. Maria assina recibo

**Resultado:**
- ? Histórico correto (João pegou)
- ? Responsável pela devolução (Maria)
- ? Assinatura de quem efetivamente devolveu

---

## 9. BENEFÍCIOS IMPLEMENTADOS

### 9.1. Documentação
- ? **Comprovante físico** de empréstimo
- ? **Comprovante físico** de devolução
- ? **Assinaturas** para responsabilização
- ? **Arquivo PDF** para backup digital

### 9.2. Rastreabilidade
- ? **Registro de quem pegou** emprestado
- ? **Registro de quem recebeu** de volta
- ? **Datas precisas** (empréstimo e recebimento)
- ? **Números de identificação** (ID do empréstimo)

### 9.3. Profissionalismo
- ? **Recibos formatados** profissionalmente
- ? **Layout limpo** e organizado
- ? **Fácil leitura** (fontes adequadas)
- ? **Espaço para assinatura** destacado

### 9.4. Praticidade
- ? **Meia folha A4** (econômico)
- ? **Visualização antes** de imprimir
- ? **Salvar como PDF** sem impressora
- ? **Reimprimir** quando necessário

### 9.5. Flexibilidade
- ? **Pessoa diferente** pode devolver
- ? **Impressão opcional** após salvar
- ? **Reimprimir** a qualquer momento
- ? **Múltiplos recibos** de um empréstimo

---

## 10. ESPECIFICAÇÕES TÉCNICAS

### 10.1. Tamanhos de Papel

| Formato | Largura | Altura | Pixels (72 DPI) |
|---------|---------|--------|-----------------|
| A4 Inteira | 210mm | 297mm | 827 x 1169 |
| Meia A4 | 210mm | 148mm | 827 x 583 |

**Recibos usam:** Meia A4 (827 x 583)

### 10.2. Fontes

| Elemento | Fonte | Tamanho | Estilo |
|----------|-------|---------|--------|
| Título | Arial | 14pt | Bold |
| Headers | Arial | 10pt | Bold |
| Conteúdo | Arial | 9pt | Regular |
| Rodapé | Arial | 8pt | Regular |

### 10.3. Margens

- **Esquerda**: 40px
- **Superior**: 40px
- **Direita**: 40px
- **Inferior**: 40px

### 10.4. Espaçamento

- **Altura de linha**: 20px
- **Espaço entre seções**: 10px
- **Linha de assinatura**: 30px de altura

---

## 11. COMPARAÇÃO ANTES E DEPOIS

### 11.1. Empréstimo

#### Antes:
```
? Sem comprovante físico
? Sem assinatura
? Difícil comprovar quem pegou
? Sem documento formal
```

#### Depois:
```
? Recibo profissional impresso
? Espaço para assinatura
? Nome do recebedor destacado
? Documento formal completo
? Visualização antes de imprimir
? Opção de salvar PDF
```

### 11.2. Recebimento

#### Antes:
```
? Sem registro de quem devolveu
? Sem comprovante de devolução
? Mesmo campo para quem pegou e quem recebeu
```

#### Depois:
```
? Campo separado para quem recebeu
? Recibo de devolução impresso
? Assinatura de quem recebeu
? Rastreabilidade completa
? Histórico preciso
```

---

## 12. ARQUIVOS CRIADOS/MODIFICADOS

### 12.1. Novos Arquivos

1. **Reports\ReciboEmprestimoPrinter.cs** (NOVO)
   - Classe de impressão de recibo de empréstimo
   - Métodos Print() e PrintPreview()
   - Formatação otimizada para meia A4

2. **Reports\ReciboRecebimentoPrinter.cs** (NOVO)
   - Classe de impressão de recibo de recebimento
   - Métodos Print() e PrintPreview()
   - Diferenciação de quem pegou vs quem recebeu

### 12.2. Arquivos Modificados

3. **Models\RecebimentoEmprestimo.cs**
   - Novo campo: NomeQuemRecebeu

4. **Forms\RecebimentoDetailForm.Designer.cs**
   - Labels atualizadas (QuemPegou, QuemRecebeu)
   - Novo TextBox txtQuemRecebeu
   - Novo Button btnImprimirRecibo

5. **Forms\RecebimentoDetailForm.cs**
   - Lógica de validação de NomeQuemRecebeu
   - Pergunta de impressão após salvar
   - Método BtnImprimirRecibo_Click()

6. **Forms\EmprestimoListForm.Designer.cs**
   - Novo Button btnImprimirRecibo

7. **Forms\EmprestimoListForm.cs**
   - Método BtnImprimirRecibo_Click()
   - Import do namespace Reports

---

## 13. COMPATIBILIDADE

- ? **Build compilado com sucesso**
- ? **Todas as funcionalidades anteriores mantidas**
- ? **Sem quebra de código existente**
- ? **System.Drawing.Printing** (nativo .NET)
- ? **.NET 8 / C# 12**

---

## 14. PRÓXIMAS MELHORIAS SUGERIDAS

### 14.1. Recibos
- ?? Logo da organização no cabeçalho
- ?? QR Code com link para validação online
- ?? Código de barras com ID do empréstimo
- ?? Carimbo de "SEGUNDA VIA"

### 14.2. Funcionalidades
- ?? Salvar PDF automaticamente em pasta
- ?? Enviar recibo por e-mail
- ?? Gerar recibo em imagem para WhatsApp
- ?? Histórico de impressões

### 14.3. Personalização
- ?? Template customizável
- ?? Escolher cores e fontes
- ?? Adicionar campos personalizados
- ?? Múltiplos formatos (A4, Carta, etc)

### 14.4. Relatórios
- ?? Relatório de recibos emitidos
- ?? Estatísticas de impressão
- ?? Auditoria de assinaturas

---

## 15. RESUMO TÉCNICO

### Funcionalidades Implementadas:

#### Recibo de Empréstimo:
1. ? Classe ReciboEmprestimoPrinter
2. ? Formatação para meia A4
3. ? Espaço para assinatura
4. ? Quebra de linha automática
5. ? Botão no grid de empréstimos

#### Recibo de Recebimento:
1. ? Classe ReciboRecebimentoPrinter
2. ? Campo NomeQuemRecebeu (separado)
3. ? Label "Quem Pegou Emprestado"
4. ? Validação obrigatória
5. ? Pergunta de impressão após salvar
6. ? Botão em modo visualização

### Características Comuns:
1. ? PrintPreview() com visualização
2. ? Print() para impressão direta
3. ? Meia folha A4 (econômico)
4. ? Fontes profissionais
5. ? Layout limpo e organizado
6. ? Rodapé com data de emissão

---

Esta documentação contempla todas as alterações relacionadas aos recibos de empréstimo e recebimento com funcionalidade de impressão.
