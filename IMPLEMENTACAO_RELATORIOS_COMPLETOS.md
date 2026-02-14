# Implementação de Relatórios de Empréstimos e Recebimentos

## Data: 2025-01-XX

## Descrição
Implementação de relatórios completos com filtros para Empréstimos e Recebimentos, com impressão em formato A4 e filtros avançados (Data, Congregação e Bem).

## Arquivos Criados

### 1. Formulários de Filtro

#### Forms\RelatorioEmprestimosFilterForm.cs
- **Funcionalidade**: Formulário modal para filtrar dados do relatório de empréstimos
- **Filtros Disponíveis**:
  - Data Inicial e Data Final (padrão: mês atual)
  - Congregação (ComboBox com opção "Todas")
  - Bem (ComboBox com opção "Todos")
- **Validações**:
  - Data inicial não pode ser maior que data final
  - Verifica se há registros antes de gerar relatório
- **Ação**: Ao clicar em "Gerar Relatório", filtra os dados e abre a impressão

#### Forms\RelatorioEmprestimosFilterForm.Designer.cs
- Layout do formulário de filtro de empréstimos
- Tamanho: 360x320 pixels
- Estilo: FixedDialog, centralizado na tela
- **Controles**:
  - Título grande e destacado
  - 2 DateTimePickers para período
  - 2 ComboBoxes para filtros
  - Botão "Gerar Relatório" (azul) e "Cancelar"

#### Forms\RelatorioRecebimentosFilterForm.cs
- **Funcionalidade**: Formulário modal para filtrar dados do relatório de recebimentos
- **Filtros Disponíveis**:
  - Data Inicial e Data Final (padrão: mês atual)
  - Congregação (ComboBox com opção "Todas")
  - Bem (ComboBox com opção "Todos")
- **Validações**:
  - Data inicial não pode ser maior que data final
  - Verifica se há registros antes de gerar relatório
- **Filtro Especial**: Filtra recebimentos por congregação através do empréstimo vinculado

#### Forms\RelatorioRecebimentosFilterForm.Designer.cs
- Layout do formulário de filtro de recebimentos
- Tamanho: 360x320 pixels
- Estilo: FixedDialog, centralizado na tela
- Mesma estrutura do formulário de empréstimos

### 2. Classes de Impressão

#### Reports\RelatorioEmprestimosPrinter.cs
- **Funcionalidade**: Geração e impressão do relatório de empréstimos em A4
- **Formato**: Retrato (Portrait), tamanho A4 (827x1169)
- **Estrutura do Relatório**:
  - **Cabeçalho**: Título "RELATÓRIO DE EMPRÉSTIMOS"
  - **Filtros Aplicados**: Período, Congregação e Bem
  - **Resumo**:
    - Total de Empréstimos
    - Total de Itens Emprestados
    - Empréstimos Em Andamento
    - Empréstimos Devolvidos
  - **Detalhamento** (Tabela):
    - Data | Recebedor | Congregação | Bens | Qtd | Status
  - **Rodapé**: Data/hora de emissão e total de registros

**Características Técnicas**:
- Ordenação por data de empréstimo
- Truncamento de textos longos com "..."
- Suporte a paginação (quando necessário)
- Fontes variadas: Title (16pt), Header (11pt), Normal (9pt), Small (8pt)

#### Reports\RelatorioRecebimentosPrinter.cs
- **Funcionalidade**: Geração e impressão do relatório de recebimentos em A4
- **Formato**: Retrato (Portrait), tamanho A4 (827x1169)
- **Estrutura do Relatório**:
  - **Cabeçalho**: Título "RELATÓRIO DE RECEBIMENTOS"
  - **Filtros Aplicados**: Período, Congregação e Bem
  - **Resumo**:
    - Total de Recebimentos
    - Total de Itens Recebidos
    - Recebimentos Parciais
    - Recebimentos Completos
  - **Detalhamento** (Tabela):
    - Data | Quem Pegou | Quem Recebeu | Bens | Qtd | Tipo
  - **Rodapé**: Data/hora de emissão e total de registros

**Características Técnicas**:
- Ordenação por data de recebimento
- Truncamento de textos longos com "..."
- Suporte a paginação (quando necessário)
- Fontes variadas: Title (16pt), Header (11pt), Normal (9pt), Small (8pt)

### 3. Alterações em Formulários Existentes

#### Forms\EmprestimoListForm.Designer.cs
**Novo Botão**:
- `btnRelatorio`: Botão vermelho (RGB 220, 53, 69)
- Texto: "Relatório"
- Posição: X=688, após o botão "Receber de Volta"
- Tamanho: 100x30 pixels

#### Forms\EmprestimoListForm.cs
**Novo Método**:
```csharp
private void BtnRelatorio_Click(object sender, EventArgs e)
{
    var form = new RelatorioEmprestimosFilterForm();
    form.ShowDialog();
}
```

#### Forms\RecebimentoListForm.Designer.cs
**Novo Botão**:
- `btnRelatorio`: Botão vermelho (RGB 220, 53, 69)
- Texto: "Relatório"
- Posição: X=456, após o botão "Imprimir Recibo"
- Tamanho: 100x30 pixels

#### Forms\RecebimentoListForm.cs
**Novo Método**:
```csharp
private void BtnRelatorio_Click(object sender, EventArgs e)
{
    var form = new RelatorioRecebimentosFilterForm();
    form.ShowDialog();
}
```

## Funcionalidades Implementadas

### 1. Relatório de Empréstimos

**Fluxo de Uso**:
1. Usuário clica no botão "Relatório" (vermelho) na listagem de empréstimos
2. Abre formulário modal com filtros:
   - Data Inicial e Final (pré-preenchido com mês atual)
   - Congregação (dropdown com todas as congregações)
   - Bem (dropdown com todos os bens)
3. Usuário ajusta filtros conforme necessário
4. Clica em "Gerar Relatório"
5. Sistema valida as datas
6. Sistema filtra os empréstimos pelos critérios
7. Exibe mensagem se não houver registros
8. Abre preview de impressão do relatório

**Filtros**:
- **Por Período**: Filtra pela DataEmprestimo
- **Por Congregação**: Filtra pelo CongregacaoId do empréstimo
- **Por Bem**: Filtra empréstimos que contenham o bem específico nos itens

**Estatísticas no Relatório**:
- Total de empréstimos no período
- Total de itens emprestados
- Quantidade de empréstimos em andamento
- Quantidade de empréstimos devolvidos

### 2. Relatório de Recebimentos

**Fluxo de Uso**:
1. Usuário clica no botão "Relatório" (vermelho) na listagem de recebimentos
2. Abre formulário modal com filtros:
   - Data Inicial e Final (pré-preenchido com mês atual)
   - Congregação (dropdown com todas as congregações)
   - Bem (dropdown com todos os bens)
3. Usuário ajusta filtros conforme necessário
4. Clica em "Gerar Relatório"
5. Sistema valida as datas
6. Sistema filtra os recebimentos pelos critérios
7. Exibe mensagem se não houver registros
8. Abre preview de impressão do relatório

**Filtros**:
- **Por Período**: Filtra pela DataRecebimento
- **Por Congregação**: Filtra através do empréstimo vinculado
- **Por Bem**: Filtra recebimentos que contenham o bem específico nos itens recebidos

**Estatísticas no Relatório**:
- Total de recebimentos no período
- Total de itens recebidos
- Quantidade de recebimentos parciais
- Quantidade de recebimentos completos

## Layout dos Relatórios

### Cabeçalho
```
RELATÓRIO DE EMPRÉSTIMOS / RECEBIMENTOS
??????????????????????????????????????????
Período: 01/01/2025 a 31/01/2025
Congregação: Todas / [Nome da Congregação]
Bem: Todos / [Nome do Bem]
??????????????????????????????????????????
```

### Resumo
```
RESUMO
  Total de Empréstimos/Recebimentos: 15
  Total de Itens: 45
  Em Andamento / Parciais: 8
  Devolvidos / Completos: 7
??????????????????????????????????????????
```

### Detalhamento (Empréstimos)
```
DETALHAMENTO

Data    | Recebedor | Congregação | Bens          | Qtd | Status
???????????????????????????????????????????????????????????????
01/01   | João...   | Sede       | Cadeiras...   | 10  | Andamento
02/01   | Maria...  | Bonsuce... | Mesas...      | 5   | Devolvido
...
```

### Detalhamento (Recebimentos)
```
DETALHAMENTO

Data    | Quem Pegou | Quem Recebeu | Bens       | Qtd | Tipo
?????????????????????????????????????????????????????????????
01/01   | João...    | Pedro...     | Cadeiras.. | 10  | Completo
02/01   | Maria...   | Ana...       | Mesas...   | 3   | Parcial
...
```

### Rodapé
```
??????????????????????????????????????????
Emitido em: 15/01/2025 14:30        Total de registros: 15
```

## Validações e Tratamentos

### Validação de Datas
- Data inicial não pode ser maior que data final
- Mensagem de erro clara ao usuário
- Impede geração do relatório se inválido

### Validação de Dados
- Verifica se há registros com os filtros selecionados
- Exibe mensagem informativa se não houver dados
- Não abre preview vazio

### Tratamento de Textos Longos
- Nomes longos são truncados com "..."
- Mantém legibilidade do relatório
- Exemplos:
  - "João da Silva e Souza" ? "João da Silva..."
  - "Bonsucesso - SETOR E" ? "Bonsuce..."

### Compatibilidade
- Suporta empréstimos antigos (formato legado)
- Suporta empréstimos novos (com múltiplos itens)
- Exibe "N/A" quando dados não disponíveis

## Cores e Estilos

### Botão Relatório
- **Cor de Fundo**: RGB(220, 53, 69) - Vermelho Bootstrap (danger)
- **Texto**: Branco
- **Destaque**: Cor diferente para chamar atenção
- **Posicionamento**: Última posição no painel de botões

### Botão Gerar Relatório (Filtro)
- **Cor de Fundo**: RGB(0, 120, 215) - Azul padrão
- **Texto**: Branco
- **Estilo**: Botão primário destacado

## Benefícios

### 1. Análise de Dados
- Visão consolidada de empréstimos/recebimentos
- Estatísticas automáticas
- Filtros flexíveis
- Exportação para impressão

### 2. Gestão
- Acompanhamento de períodos específicos
- Identificação de congregações mais ativas
- Controle de bens mais emprestados
- Análise de recebimentos parciais

### 3. Documentação
- Relatórios impressos para arquivamento
- Formato profissional A4
- Informações completas e organizadas
- Data de emissão registrada

### 4. Usabilidade
- Interface intuitiva
- Filtros pré-preenchidos com mês atual
- Preview antes de imprimir
- Validações claras

## Exemplos de Uso

### Exemplo 1: Relatório Mensal de Empréstimos
**Cenário**: Gerar relatório de todos os empréstimos de janeiro/2025

**Passos**:
1. Ir para "Empréstimos"
2. Clicar em "Relatório" (vermelho)
3. Verificar datas: 01/01/2025 a 31/01/2025
4. Deixar "Todas" nas congregações
5. Deixar "Todos" nos bens
6. Clicar em "Gerar Relatório"

**Resultado**: Relatório completo com todos os empréstimos do mês

### Exemplo 2: Relatório de Congregação Específica
**Cenário**: Verificar todos os recebimentos da congregação "Sede"

**Passos**:
1. Ir para "Recebimento de Emprestimo"
2. Clicar em "Relatório" (vermelho)
3. Ajustar período conforme necessário
4. Selecionar "Sede" no dropdown Congregação
5. Deixar "Todos" nos bens
6. Clicar em "Gerar Relatório"

**Resultado**: Relatório apenas com recebimentos da Sede

### Exemplo 3: Relatório de Bem Específico
**Cenário**: Analisar empréstimos de "Cadeiras" no ano

**Passos**:
1. Ir para "Empréstimos"
2. Clicar em "Relatório" (vermelho)
3. Ajustar para 01/01/2025 a 31/12/2025
4. Deixar "Todas" nas congregações
5. Selecionar "Cadeiras" no dropdown Bem
6. Clicar em "Gerar Relatório"

**Resultado**: Todos os empréstimos de cadeiras no ano

### Exemplo 4: Relatório Filtro Combinado
**Cenário**: Recebimentos de "Mesas" da "Bonsucesso" em fevereiro

**Passos**:
1. Ir para "Recebimento de Emprestimo"
2. Clicar em "Relatório" (vermelho)
3. Ajustar para 01/02/2025 a 29/02/2025
4. Selecionar "Bonsucesso" na congregação
5. Selecionar "Mesas" no bem
6. Clicar em "Gerar Relatório"

**Resultado**: Recebimentos específicos filtrados

## Observações Técnicas

### Impressão
- Usa `PrintDocument` do .NET
- Preview com `PrintPreviewDialog`
- Tamanho A4: 827x1169 (100 dpi)
- Orientação: Retrato (Portrait)
- Margens: 50 pixels (esquerda/direita/topo)

### Paginação
- Suporte a múltiplas páginas (se necessário)
- Quebra automática quando atinge limite
- Rodapé repetido em cada página
- `e.HasMorePages` gerencia continuação

### Performance
- Filtros aplicados em memória (LINQ)
- Ordenação antes da impressão
- Cálculos estatísticos otimizados
- Sem consultas repetidas ao repositório

### Fontes
- **Arial** usada em todos os tamanhos
- Tamanhos: 16pt (título), 11pt (header), 9pt (normal), 8pt (small)
- Estilos: Bold para títulos e headers
- Regular para conteúdo

## Melhorias Futuras Sugeridas

1. **Exportação para PDF**: Salvar relatório como arquivo PDF
2. **Exportação para Excel**: Dados em planilha para análise
3. **Gráficos**: Visualizações gráficas de estatísticas
4. **Filtro por Status**: Adicionar filtro de status de empréstimo
5. **Filtro por Setor**: Filtrar por setor da congregação
6. **Agrupamento**: Agrupar por congregação/bem no relatório
7. **Totalizadores**: Subtotais por grupo
8. **Filtro de Data de Empréstimo**: Para recebimentos, filtrar também pela data do empréstimo original
9. **Preview Customizado**: Permitir zoom e navegação entre páginas
10. **Salvamento de Filtros**: Salvar combinações de filtros favoritas

## Testes Sugeridos

### Relatório de Empréstimos
1. ? Gerar relatório do mês atual (padrão)
2. ? Filtrar por congregação específica
3. ? Filtrar por bem específico
4. ? Filtrar por período personalizado
5. ? Combinar múltiplos filtros
6. ? Tentar gerar com data inválida (inicial > final)
7. ? Tentar gerar sem registros (deve mostrar mensagem)
8. ? Verificar estatísticas no resumo
9. ? Verificar detalhamento na tabela
10. ? Imprimir relatório

### Relatório de Recebimentos
1. ? Gerar relatório do mês atual (padrão)
2. ? Filtrar por congregação através do empréstimo
3. ? Filtrar por bem recebido
4. ? Filtrar por período personalizado
5. ? Combinar múltiplos filtros
6. ? Verificar recebimentos parciais vs completos
7. ? Verificar estatísticas corretas
8. ? Imprimir relatório com muitos registros (testar paginação)

### Interface
1. ? Botão "Relatório" visível e destacado
2. ? Formulário de filtro abre corretamente
3. ? ComboBoxes carregam dados corretos
4. ? Datas pré-preenchidas com mês atual
5. ? Validações funcionam corretamente
6. ? Preview abre em tamanho adequado
7. ? Cancelar fecha sem gerar relatório

## Conclusão

Os relatórios implementados oferecem uma solução completa para análise e documentação de empréstimos e recebimentos, com:
- **Interface intuitiva** e fácil de usar
- **Filtros flexíveis** para diferentes necessidades
- **Formato profissional** A4 para impressão
- **Estatísticas automáticas** para análise rápida
- **Validações robustas** para evitar erros
- **Compatibilidade** com dados antigos e novos

A funcionalidade está pronta para uso em produção e pode ser expandida conforme as necessidades futuras do sistema.
