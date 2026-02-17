# Implementação: Impressão de Etiquetas de Bens

## Resumo
Implementação de um sistema de impressão de etiquetas para itens/bens cadastrados no sistema. As etiquetas têm dimensões exatas de 5cm x 3cm e são organizadas automaticamente para caber múltiplas etiquetas por página.

## Arquivos Criados

### 1. `Forms/EtiquetasFilterForm.cs` e `Forms/EtiquetasFilterForm.Designer.cs`
Formulário de filtro e seleção de itens para impressão de etiquetas.

**Funcionalidades:**
- **Filtro por ID**: Digite IDs separados por vírgula, espaço ou ponto e vírgula (ex: "1, 2, 3")
- **Filtro por Nome**: Digite parte do nome do item para filtrar
- **Seleção múltipla**: CheckedListBox permite marcar/desmarcar itens individualmente
- **Marcar/Desmarcar Todos**: Botões para facilitar seleção em massa
- **Auto-seleção**: Ao filtrar por IDs, os itens são automaticamente marcados

**Interface:**
- Painel de título azul com o nome "Imprimir Etiquetas de Bens"
- Área de filtro com instruções claras
- Lista de itens com checkboxes
- Contador de itens encontrados
- Botões de ação: "Imprimir Etiquetas" e "Cancelar" (com Anchor para responsividade)
- Tamanho do formulário: 750x600 pixels

### 2. `Reports/EtiquetasPrinter.cs`
Classe responsável pela impressão das etiquetas.

**Características das Etiquetas:**
- **Dimensões EXATAS**: 5cm largura × 3cm altura (50mm x 30mm)
- **Borda**: Linha tracejada (dash style) em cinza
- **Margem interna**: 2mm para espaçamento do conteúdo

**Conteúdo da Etiqueta (Simplificado):**
1. **ID do item** (canto superior direito, fonte pequena, cinza, 6pt)
2. **Nome do bem** (centralizado vertical e horizontalmente, negrito, 8pt)

**Layout Inteligente:**
- Calcula automaticamente quantas etiquetas cabem por linha
- Calcula automaticamente quantas linhas de etiquetas cabem por página
- Distribui etiquetas em múltiplas linhas conforme necessário
- Cria novas páginas automaticamente quando o espaço acaba
- Adaptável a diferentes configurações de impressora (DPI)

**Exemplo de Layout em uma Folha A4:**
```
?????????????????????????
? Et1 ? Et2 ? Et3 ? Et4 ?
?????????????????????????
? Et5 ? Et6 ? Et7 ? Et8 ?
?????????????????????????
? Et9 ? Et10? Et11? Et12?
?????????????????????????
```

### 3. Modificações em `Forms/ItemListForm.cs` e `Forms/ItemListForm.Designer.cs`
Adicionado novo botão "Etiquetas" na listagem de bens.

**Características do Botão:**
- **Cor**: Amarelo (RGB: 255, 193, 7) com texto preto
- **Posição**: Entre o botão "Relatório" e "Listar"
- **Tamanho**: 90px de largura × 30px de altura
- **Evento**: Abre o formulário `EtiquetasFilterForm`

## Fluxo de Uso

### Passo 1: Acessar a Funcionalidade
1. Abrir a listagem de bens (menu principal)
2. Clicar no botão **"Etiquetas"** (amarelo)

### Passo 2: Filtrar e Selecionar Itens
Opção A - Filtrar por IDs:
1. Digite os IDs no campo de filtro (ex: "1, 5, 10")
2. Os itens serão automaticamente marcados
3. Clique em "Imprimir Etiquetas"

Opção B - Filtrar por Nome:
1. Digite parte do nome no campo de filtro (ex: "CADEIRA")
2. Marque os itens desejados manualmente
3. Clique em "Imprimir Etiquetas"

Opção C - Selecionar Manualmente:
1. Use "Marcar Todos" ou marque individualmente
2. Clique em "Imprimir Etiquetas"

### Passo 3: Visualizar e Imprimir
1. A janela de preview de impressão será aberta
2. Visualize as etiquetas organizadas em páginas
3. Use o botão de impressão para enviar à impressora
4. Ou use "Exportar para PDF" se disponível

## Detalhes Técnicos

### Dimensões Exatas
- **Etiqueta**: 50mm x 30mm (5cm x 3cm)
- **Margem interna**: 2mm
- **Área útil**: 46mm x 26mm

### Conversão de Unidades
- **Dimensões da etiqueta**: Definidas em milímetros (mm)
- **Conversão para pixels**: Usa o DPI da impressora
  ```csharp
  float pixels = (mm / 25.4f) * DPI
  ```
- **Tamanho do papel**: A4 (827 × 1169 centésimos de polegada)

### Cálculo de Layout
```csharp
etiquetasPorLinha = larguraDisponivel / larguraEtiqueta
etiquetasPorColuna = alturaDisponivel / alturaEtiqueta
etiquetasPorPagina = etiquetasPorLinha × etiquetasPorColuna
```

### Renderização de Texto
- **Nome do bem**: Usa `StringFormat` com quebra de linha automática
- **Centralização**: Vertical e horizontal
- **Truncamento**: Texto muito longo é truncado com "..." (ellipsis)

### Paginação
- Verifica se há espaço suficiente antes de desenhar cada etiqueta
- Define `e.HasMorePages = true` quando há mais itens
- Reseta o índice após completar todas as páginas

## Fontes Utilizadas
- **ID do item**: Arial, 6pt, Regular
- **Nome do bem**: Arial, 8pt, Bold

## Cores
- **Borda**: Cinza (Gray)
- **ID**: Cinza (Gray)
- **Nome**: Preto (Black)

## Exemplo de Etiqueta
```
???????????????????????????????
?                    ID: 1    ? (cinza, 6pt)
?                             ?
?       CADEIRA PLÁSTICA      ? (negrito, 8pt, centralizado)
?                             ?
?                             ?
???????????????????????????????
       5cm x 3cm
```

## Benefícios

1. **Organização**: Facilita a identificação visual dos bens
2. **Flexibilidade**: Permite selecionar exatamente quais itens imprimir
3. **Eficiência**: Múltiplas etiquetas por página reduz desperdício
4. **Simplicidade**: Layout minimalista com apenas ID e Nome
5. **Profissional**: Layout organizado com bordas tracejadas
6. **Adaptável**: Ajusta-se automaticamente ao tamanho da página
7. **Dimensões exatas**: 5cm x 3cm conforme solicitado

## Melhorias Implementadas (v1.1)

### Correções de Layout
1. **Formulário de filtro**: Aumentado para 750x600 pixels
2. **Botões**: Usam Anchor para posicionamento correto (Bottom, Right)
3. **Botão Cancelar**: Agora visível completamente na tela

### Simplificação das Etiquetas
1. **Removidas informações**: Estoque, Emprestado e Total
2. **Conteúdo minimalista**: Apenas ID e Nome do bem
3. **Tamanho reduzido**: Fontes ajustadas para caber em 5x3cm
4. **Centralização**: Nome centralizado vertical e horizontalmente

## Possíveis Melhorias Futuras

1. **Customização de tamanho**: Permitir ao usuário escolher o tamanho das etiquetas
2. **Templates**: Diferentes modelos de etiqueta (com/sem bordas, com/sem ID)
3. **QR Code**: Adicionar QR code com ID do item
4. **Logo**: Incluir logo da instituição na etiqueta
5. **Código de barras**: Adicionar código de barras para leitura automática
6. **Quantidade por item**: Permitir imprimir múltiplas cópias da mesma etiqueta
7. **Exportação**: Salvar etiquetas como PDF ou imagem
8. **Configuração de margens**: Ajustar margens da página
9. **Preview individual**: Visualizar cada etiqueta antes de imprimir
10. **Impressão em adesivos**: Suporte para folhas de etiquetas adesivas pré-cortadas

## Notas de Uso

- **Papel recomendado**: A4 (210mm × 297mm)
- **Orientação**: Retrato (vertical)
- **Margens**: 20mm em todos os lados
- **Dimensões das etiquetas**: EXATAMENTE 5cm x 3cm (50mm x 30mm)
- **Impressão**: Funciona com qualquer impressora (jato de tinta, laser)
- **Corte**: Use as linhas tracejadas como guia para corte manual
- **Adesivos**: Pode-se imprimir em papel adesivo para colar nos bens

## Cálculo Aproximado de Etiquetas por Página

Com etiquetas de 5cm x 3cm em papel A4 (21cm x 29.7cm):
- **Largura útil** (21cm - 4cm margens): 17cm ? 3 etiquetas por linha
- **Altura útil** (29.7cm - 4cm margens): 25.7cm ? 8 etiquetas por coluna
- **Total aproximado**: ~24 etiquetas por página

## Teste e Validação

Para testar a funcionalidade:
1. Cadastre alguns bens com nomes variados
2. Acesse a listagem de bens
3. Clique em "Etiquetas"
4. Teste diferentes filtros (por ID e por nome)
5. Selecione múltiplos itens
6. Visualize o preview
7. Verifique se as etiquetas têm exatamente 5x3cm
8. Verifique se apenas ID e Nome aparecem
9. Teste a paginação com muitos itens (ex: 50+ itens)
10. Verifique se o botão "Cancelar" está visível

---
**Data da implementação**: Janeiro de 2025  
**Versão**: 1.1 (Dimensões corrigidas e layout simplificado)  
**Status**: ? Implementado, testado e corrigido
