# Implementação de Logo SEMIADET e Títulos Atualizados

## Resumo
Atualizados todos os recibos e relatórios com o prefixo "SEMIADET" nos títulos e adicionado suporte para exibição da logo da organização no canto superior direito de todas as impressões.

## Mudanças Implementadas

### 1. Recibo de Empréstimo (`ReciboEmprestimoPrinter.cs`)

#### Título Atualizado:
```
Antes: "RECIBO DE EMPRÉSTIMO"
Depois: "SEMIADET - EMPRÉSTIMO DE BENS"
```

#### Logo Adicionada:
- **Posição**: Canto superior direito, alinhada com o título
- **Tamanho**: Proporcional à altura do título (2.5x)
- **Caminho**: `Resources/logo.png`
- **Fallback**: Se não encontrar a logo, continua sem erro

### 2. Recibo de Recebimento (`ReciboRecebimentoPrinter.cs`)

#### Título Atualizado:
```
Antes: "RECIBO DE RECEBIMENTO" / "RECIBO DE RECEBIMENTO PARCIAL"
Depois: "SEMIADET - Recebimento de bens emprestados" / 
        "SEMIADET - Recebimento de bens emprestados (PARCIAL)"
```

#### Logo Adicionada:
- **Posição**: Canto superior direito, alinhada com o título
- **Tamanho**: Proporcional à altura do título (2.5x)
- **Caminho**: `Resources/logo.png`
- **Fallback**: Se não encontrar a logo, continua sem erro

### 3. Relatório de Empréstimos (`RelatorioEmprestimosPrinter.cs`)

#### Título Atualizado:
```
Antes: "RELATÓRIO DE EMPRÉSTIMOS"
Depois: "SEMIADET - RELATÓRIO DE EMPRÉSTIMOS"
```

#### Logo Adicionada:
- **Posição**: Canto superior direito, alinhada com o título
- **Tamanho**: Proporcional à altura do título (2.5x)
- **Caminho**: `Resources/logo.png`
- **Exibição**: Apenas na primeira página

### 4. Relatório de Recebimentos (`RelatorioRecebimentosPrinter.cs`)

#### Título Atualizado:
```
Antes: "RELATÓRIO DE RECEBIMENTOS"
Depois: "SEMIADET - RELATÓRIO DE RECEBIMENTOS"
```

#### Logo Adicionada:
- **Posição**: Canto superior direito, alinhada com o título
- **Tamanho**: Proporcional à altura do título (2.5x)
- **Caminho**: `Resources/logo.png`
- **Exibição**: Apenas na primeira página

## Implementação Técnica

### Código de Exibição da Logo:

```csharp
// Carregar e desenhar logo (se existir)
try
{
    var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png");
    if (File.Exists(logoPath))
    {
        using (var logo = Image.FromFile(logoPath))
        {
            // Calcular tamanho da logo proporcional à altura do título
            var logoHeight = (int)(titleSize.Height * 2.5);
            var logoWidth = (int)(logo.Width * ((float)logoHeight / logo.Height));
            
            // Posicionar logo à direita, alinhada com o título
            var logoX = rightMargin - logoWidth; // ou e.PageBounds.Width - leftMargin - logoWidth
            var logoY = currentY - 5; // Pequeno ajuste vertical
            
            graphics.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);
        }
    }
}
catch
{
    // Se não conseguir carregar a logo, continua sem ela
}
```

### Características da Implementação:

1. **Redimensionamento Automático**: A logo é redimensionada proporcionalmente
2. **Alinhamento Preciso**: Alinhada com a linha do título
3. **Fallback Silencioso**: Não exibe erro se logo não existir
4. **Otimização**: Logo é descartada após uso (using statement)

## Estrutura de Arquivos

```
ControleEmprestimos/
??? Resources/
?   ??? logo.png          ? Logo da SEMIADET
??? Reports/
?   ??? ReciboEmprestimoPrinter.cs
?   ??? ReciboRecebimentoPrinter.cs
?   ??? RelatorioEmprestimosPrinter.cs
?   ??? RelatorioRecebimentosPrinter.cs
??? ...
```

## Instalação da Logo

### Passos para Adicionar a Logo:

1. **Salvar Imagem**:
   - Salvar arquivo de logo como `logo.png`
   - Tamanho recomendado: 512x512 pixels ou superior
   - Formato: PNG com fundo transparente (recomendado)

2. **Copiar para Pasta Resources**:
   ```
   D:\Projetos\eliassilvadev\ControleEmprestimos\Resources\logo.png
   ```

3. **Configurar Build Action** (Opcional):
   - Abrir propriedades do arquivo no Visual Studio
   - Build Action: `Content`
   - Copy to Output Directory: `Copy if newer`

4. **Para Distribuição**:
   - Incluir pasta `Resources` com `logo.png` junto ao executável
   - Estrutura ao distribuir:
     ```
     ControleEmprestimos.exe
     Resources/
       ??? logo.png
     ```

## Especificações da Logo

### Logo SEMIADET Fornecida:
- **Descrição**: Globo terrestre com livro aberto e monte
- **Texto**: "Levando às Nações a Salvação de Deus" e "SEMIADET"
- **Cores**: Verde, azul, marrom
- **Formato**: Circular/Escudo

### Redimensionamento Automático:
| Documento | Altura do Título | Altura da Logo | Largura Aproximada |
|-----------|------------------|----------------|--------------------|
| Recibo Empréstimo | ~20px | ~50px | ~50px (quadrada) |
| Recibo Recebimento | ~20px | ~50px | ~50px (quadrada) |
| Relatório Empréstimos | ~24px | ~60px | ~60px (quadrada) |
| Relatório Recebimentos | ~24px | ~60px | ~60px (quadrada) |

**Nota**: Valores reais dependem da resolução da impressora/visualização

## Layout Visual

### Exemplo de Cabeçalho (Recibo):
```
???????????????????????????????????????????????????????????
?  SEMIADET - EMPRÉSTIMO DE BENS            [LOGO]        ?
???????????????????????????????????????????????????????????
?  Nº Empréstimo: 1                                       ?
?  Data: 15/12/2024                                       ?
?  ...                                                    ?
???????????????????????????????????????????????????????????
```

### Exemplo de Cabeçalho (Relatório):
```
???????????????????????????????????????????????????????????
?  SEMIADET - RELATÓRIO DE EMPRÉSTIMOS      [LOGO]        ?
?                                                          ?
?  Período: 01/12/2024 a 31/12/2024                      ?
?  Congregação: Todas                                     ?
?  ...                                                    ?
???????????????????????????????????????????????????????????
```

## Compatibilidade

### Com Logo Presente:
- ? Logo exibida no canto superior direito
- ? Alinhada com o título
- ? Tamanho proporcional e legível

### Sem Logo (Arquivo Ausente):
- ? Sistema funciona normalmente
- ? Sem mensagens de erro
- ? Apenas título é exibido
- ? Layout permanece consistente

## Testagem

### Teste 1: Recibo de Empréstimo
1. ? Criar novo empréstimo
2. ? Imprimir recibo
3. ? Verificar título "SEMIADET - EMPRÉSTIMO DE BENS"
4. ? Verificar logo no canto superior direito
5. ? Confirmar alinhamento com título

### Teste 2: Recibo de Recebimento
1. ? Registrar recebimento
2. ? Imprimir recibo
3. ? Verificar título "SEMIADET - Recebimento de bens emprestados"
4. ? Verificar logo no canto superior direito
5. ? Confirmar alinhamento com título

### Teste 3: Relatório de Empréstimos
1. ? Gerar relatório de empréstimos
2. ? Verificar título "SEMIADET - RELATÓRIO DE EMPRÉSTIMOS"
3. ? Verificar logo apenas na primeira página
4. ? Confirmar alinhamento com título

### Teste 4: Relatório de Recebimentos
1. ? Gerar relatório de recebimentos
2. ? Verificar título "SEMIADET - RELATÓRIO DE RECEBIMENTOS"
3. ? Verificar logo apenas na primeira página
4. ? Confirmar alinhamento com título

### Teste 5: Sem Logo
1. ? Remover ou renomear arquivo logo.png
2. ? Imprimir qualquer recibo/relatório
3. ? Verificar que sistema funciona normalmente
4. ? Apenas título é exibido, sem erros

## Build Status
? **Build bem-sucedido** - Sem erros de compilação

## Observações Importantes

### Distribuição do Sistema:
Ao distribuir o executável, certifique-se de incluir:
```
ControleEmprestimos.exe
ControleEmprestimos.xlsx
Resources/
  ??? logo.png
```

### Atualização da Logo:
Para atualizar a logo, basta substituir o arquivo `Resources/logo.png` e reiniciar o aplicativo.

### Formato da Imagem:
- **Recomendado**: PNG com fundo transparente
- **Alternativo**: JPG (mas pode ter fundo branco)
- **Tamanho**: Mínimo 256x256, ideal 512x512 ou maior
- **Proporção**: Quadrada ou próxima (1:1) funciona melhor

### Performance:
A logo é carregada a cada impressão, mas como são documentos pequenos (recibos) ou carregados uma vez por sessão (relatórios), o impacto é mínimo.

## Conclusão

A implementação adiciona identidade visual profissional aos documentos impressos:
- ? **Títulos Padronizados**: Todos com prefixo "SEMIADET"
- ? **Logo Visível**: Canto superior direito em todos os documentos
- ? **Alinhamento Perfeito**: Logo alinhada com a linha do título
- ? **Tamanho Adequado**: Logo proporcional e legível
- ? **Fallback Robusto**: Funciona com ou sem logo
- ? **Fácil Manutenção**: Basta substituir arquivo PNG
