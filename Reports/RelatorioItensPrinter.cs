using System.Drawing.Printing;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Reports;

public class RelatorioItensPrinter
{
    private List<(Item Item, List<(Emprestimo Emprestimo, int Quantidade)> Emprestimos)> _itensComEmprestimos;
    private bool _apenasBensEmprestados;
    private int _currentItemIndex = 0;

    public RelatorioItensPrinter(
        List<(Item Item, List<(Emprestimo Emprestimo, int Quantidade)> Emprestimos)> itensComEmprestimos,
        bool apenasBensEmprestados)
    {
        _itensComEmprestimos = itensComEmprestimos.OrderBy(x => x.Item.Name).ToList();
        _apenasBensEmprestados = apenasBensEmprestados;
    }

    public void PrintPreview()
    {
        var printDocument = new PrintDocument();
        printDocument.PrintPage += PrintDocument_PrintPage;
        
        // Configurar para A4
        printDocument.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
        printDocument.DefaultPageSettings.Landscape = false;
        
        var printPreviewDialog = new PrintPreviewDialog
        {
            Document = printDocument,
            Width = 1000,
            Height = 700,
            StartPosition = FormStartPosition.CenterScreen
        };

        printPreviewDialog.ShowDialog();
    }

    private Image? LoadLogoFromFile()
    {
        try
        {
            // Buscar logo na mesma pasta do executável
            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png");
            
            if (File.Exists(logoPath))
            {
                return Image.FromFile(logoPath);
            }
        }
        catch
        {
            // Se não conseguir carregar, retorna null
        }
        
        return null;
    }

    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        if (e.Graphics == null) return;

        var graphics = e.Graphics;
        var leftMargin = 50;
        var rightMargin = e.PageBounds.Width - 50;
        var topMargin = 50;
        var currentY = topMargin;
        var lineHeight = 18;

        // Fontes
        var titleFont = new Font("Arial", 16, FontStyle.Bold);
        var headerFont = new Font("Arial", 11, FontStyle.Bold);
        var normalFont = new Font("Arial", 9, FontStyle.Regular);
        var smallFont = new Font("Arial", 8, FontStyle.Regular);
        var tinyFont = new Font("Arial", 7, FontStyle.Regular);

        // Título (apenas na primeira página)
        if (_currentItemIndex == 0)
        {
            var titulo = "SEMIADET - RELATÓRIO DE BENS";
            var titleSize = graphics.MeasureString(titulo, titleFont);
            
            // Desenhar título
            graphics.DrawString(titulo, titleFont, Brushes.Black, leftMargin, currentY);
            
            // Variável para armazenar a altura da logo
            int logoHeight = 0;
            
            // Carregar e desenhar logo (da pasta do executável)
            using (var logo = LoadLogoFromFile())
            {
                if (logo != null)
                {
                    // Calcular tamanho da logo proporcional à altura do título
                    logoHeight = (int)(titleSize.Height * 2.5);
                    var logoWidth = (int)(logo.Width * ((float)logoHeight / logo.Height));
                    
                    // Posicionar logo à direita, alinhada com o topo do título
                    var logoX = rightMargin - logoWidth;
                    var logoY = currentY;
                    
                    graphics.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);
                }
            }
            
            // Avançar currentY considerando o maior entre título e logo
            if (logoHeight > titleSize.Height)
            {
                currentY += logoHeight + 10; // Espaço após a logo
            }
            else
            {
                currentY += (int)titleSize.Height + 10; // Espaço após o título
            }

            // Filtro aplicado
            var filtroTexto = _apenasBensEmprestados ? "Apenas Bens com Itens Emprestados" : "Todos os Bens";
            graphics.DrawString($"Filtro: {filtroTexto}", normalFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight + 10;

            // Linha separadora (agora abaixo da logo)
            graphics.DrawLine(Pens.Black, leftMargin, currentY, rightMargin, currentY);
            currentY += 15;

            // Resumo
            var totalBens = _itensComEmprestimos.Count;
            var totalEstoque = _itensComEmprestimos.Sum(x => x.Item.QuantityInStock);
            var totalEmprestado = _itensComEmprestimos.Sum(x => x.Item.TotalEmprestado);
            var totalGeral = _itensComEmprestimos.Sum(x => x.Item.QuantidadeTotal);
            var bensComEmprestimo = _itensComEmprestimos.Count(x => x.Item.TotalEmprestado > 0);

            graphics.DrawString("RESUMO", headerFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight + 5;

            graphics.DrawString($"Total de Bens: {totalBens}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Quantidade em Estoque: {totalEstoque}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Quantidade Emprestada: {totalEmprestado}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Quantidade Total: {totalGeral}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Bens com Itens Emprestados: {bensComEmprestimo}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight + 15;

            // Linha separadora
            graphics.DrawLine(Pens.Black, leftMargin, currentY, rightMargin, currentY);
            currentY += 15;

            // Cabeçalho da tabela
            graphics.DrawString("DETALHAMENTO", headerFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight + 10;
        }

        // Cabeçalho das colunas
        var colNome = leftMargin;
        var colEstoque = leftMargin + 200;
        var colEmprestado = leftMargin + 300;
        var colTotal = leftMargin + 410;

        graphics.DrawString("Nome do Bem", smallFont, Brushes.Black, colNome, currentY);
        graphics.DrawString("Estoque", smallFont, Brushes.Black, colEstoque, currentY);
        graphics.DrawString("Emprestado", smallFont, Brushes.Black, colEmprestado, currentY);
        graphics.DrawString("Total", smallFont, Brushes.Black, colTotal, currentY);
        currentY += 15;

        // Linha separadora
        graphics.DrawLine(Pens.Gray, leftMargin, currentY, rightMargin, currentY);
        currentY += 10;

        // Dados dos itens
        for (int i = _currentItemIndex; i < _itensComEmprestimos.Count; i++)
        {
            var itemData = _itensComEmprestimos[i];
            var item = itemData.Item;
            var emprestimos = itemData.Emprestimos;

            // Calcular altura necessária para este item
            int numEmprestimos = emprestimos.Count;
            var alturaItem = lineHeight + (numEmprestimos > 0 ? numEmprestimos * 14 + 5 : 0);

            // Verificar se precisa de nova página
            if (currentY + alturaItem > e.PageBounds.Height - 100)
            {
                _currentItemIndex = i;
                e.HasMorePages = true;
                
                // Rodapé da página
                var rodapeY = e.PageBounds.Height - 60;
                graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, rodapeY);
                graphics.DrawString($"Página {(_currentItemIndex / 15) + 1}", smallFont, Brushes.Gray, rightMargin - 100, rodapeY);
                return;
            }

            // Linha principal do item
            var nomeBem = item.Name.Length > 30 ? item.Name.Substring(0, 30) + "..." : item.Name;
            graphics.DrawString(nomeBem, smallFont, Brushes.Black, colNome, currentY);
            graphics.DrawString(item.QuantityInStock.ToString(), smallFont, Brushes.Black, colEstoque, currentY);
            graphics.DrawString(item.TotalEmprestado.ToString(), smallFont, Brushes.Black, colEmprestado, currentY);
            graphics.DrawString(item.QuantidadeTotal.ToString(), smallFont, Brushes.Black, colTotal, currentY);
            
            currentY += lineHeight;

            // Empréstimos em linhas separadas (indentados)
            if (emprestimos.Any())
            {
                foreach (var emp in emprestimos)
                {
                    var congregacaoNome = emp.Emprestimo.CongregacaoName.Length > 25 
                        ? emp.Emprestimo.CongregacaoName.Substring(0, 25) + "..." 
                        : emp.Emprestimo.CongregacaoName;
                    
                    var emprestimoTexto = $"  • {congregacaoNome} - {emp.Quantidade} un.";
                    graphics.DrawString(emprestimoTexto, tinyFont, Brushes.DarkGray, colNome, currentY);
                    currentY += 14;
                }
                currentY += 5;
            }

            currentY += 5; // Espaço entre itens
        }

        currentY += 10;
        // Linha separadora final
        graphics.DrawLine(Pens.Black, leftMargin, currentY, rightMargin, currentY);

        // Rodapé
        currentY = e.PageBounds.Height - 60;
        graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, currentY);
        graphics.DrawString($"Total de registros: {_itensComEmprestimos.Count}", smallFont, Brushes.Gray, rightMargin - 150, currentY);

        e.HasMorePages = false;
        _currentItemIndex = 0; // Reset para próxima impressão
    }
}
