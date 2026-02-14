using System.Drawing.Printing;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Reports;

public class RelatorioEmprestimosPrinter
{
    private List<Emprestimo> _emprestimos;
    private DateTime _dataInicial;
    private DateTime _dataFinal;
    private string _congregacaoFiltro;
    private string _itemFiltro;
    private int _currentEmprestimoIndex = 0;

    public RelatorioEmprestimosPrinter(
        List<Emprestimo> emprestimos, 
        DateTime dataInicial, 
        DateTime dataFinal,
        string congregacaoFiltro,
        string itemFiltro)
    {
        _emprestimos = emprestimos.OrderBy(e => e.DataEmprestimo).ToList();
        _dataInicial = dataInicial;
        _dataFinal = dataFinal;
        _congregacaoFiltro = congregacaoFiltro;
        _itemFiltro = itemFiltro;
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
        if (_currentEmprestimoIndex == 0)
        {
            var titulo = "SEMIADET - RELATÓRIO DE EMPRÉSTIMOS";
            var titleSize = graphics.MeasureString(titulo, titleFont);
            
            // Desenhar título
            graphics.DrawString(titulo, titleFont, Brushes.Black, leftMargin, currentY);
            
            // Carregar e desenhar logo (da pasta do executável)
            using (var logo = LoadLogoFromFile())
            {
                if (logo != null)
                {
                    // Calcular tamanho da logo proporcional à altura do título
                    var logoHeight = (int)(titleSize.Height * 2.5);
                    var logoWidth = (int)(logo.Width * ((float)logoHeight / logo.Height));
                    
                    // Posicionar logo à direita, alinhada com o topo do título
                    var logoX = rightMargin - logoWidth;
                    var logoY = currentY; // Alinhado com o topo do título (sem ajuste negativo)
                    
                    graphics.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);
                }
            }
            
            currentY += 35;

            // Período e filtros
            graphics.DrawString($"Período: {_dataInicial:dd/MM/yyyy} a {_dataFinal:dd/MM/yyyy}", normalFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Congregação: {_congregacaoFiltro}", normalFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Bem: {_itemFiltro}", normalFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight + 10;

            // Linha separadora
            graphics.DrawLine(Pens.Black, leftMargin, currentY, rightMargin, currentY);
            currentY += 15;

            // Resumo
            var totalEmprestimos = _emprestimos.Count;
            var totalItens = _emprestimos.Sum(e => e.TotalItens);
            var totalEmAndamento = _emprestimos.Count(e => e.Status == StatusEmprestimo.EmAndamento);
            var totalDevolvidos = _emprestimos.Count(e => e.Status == StatusEmprestimo.Devolvido);

            graphics.DrawString("RESUMO", headerFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight + 5;

            graphics.DrawString($"Total de Empréstimos: {totalEmprestimos}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Total de Itens Emprestados: {totalItens}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Em Andamento: {totalEmAndamento}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Devolvidos: {totalDevolvidos}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight + 15;

            // Linha separadora
            graphics.DrawLine(Pens.Black, leftMargin, currentY, rightMargin, currentY);
            currentY += 15;

            // Cabeçalho da tabela
            graphics.DrawString("DETALHAMENTO", headerFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight + 10;
        }

        // Cabeçalho das colunas
        var colData = leftMargin;
        var colRecebedor = leftMargin + 70;
        var colCongregacao = leftMargin + 180;
        var colQtd = leftMargin + 310;
        var colStatus = leftMargin + 360;

        graphics.DrawString("Data", smallFont, Brushes.Black, colData, currentY);
        graphics.DrawString("Recebedor", smallFont, Brushes.Black, colRecebedor, currentY);
        graphics.DrawString("Congregação", smallFont, Brushes.Black, colCongregacao, currentY);
        graphics.DrawString("Qtd", smallFont, Brushes.Black, colQtd, currentY);
        graphics.DrawString("Status", smallFont, Brushes.Black, colStatus, currentY);
        currentY += 15;

        // Linha separadora
        graphics.DrawLine(Pens.Gray, leftMargin, currentY, rightMargin, currentY);
        currentY += 10;

        // Dados dos empréstimos
        for (int i = _currentEmprestimoIndex; i < _emprestimos.Count; i++)
        {
            var emprestimo = _emprestimos[i];

            // Calcular altura necessária para este empréstimo
            int numItens = 0;
            if (emprestimo.Itens != null && emprestimo.Itens.Any())
            {
                numItens = emprestimo.Itens.Count;
            }
            else
            {
                numItens = 1; // Compatibilidade com dados antigos
            }

            var alturaEmprestimo = lineHeight + (numItens * 14); // Linha principal + itens

            // Verificar se precisa de nova página
            if (currentY + alturaEmprestimo > e.PageBounds.Height - 100)
            {
                _currentEmprestimoIndex = i;
                e.HasMorePages = true;
                
                // Rodapé da página
                var rodapeY = e.PageBounds.Height - 60;
                graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, rodapeY);
                graphics.DrawString($"Página {(_currentEmprestimoIndex / 10) + 1}", smallFont, Brushes.Gray, rightMargin - 100, rodapeY);
                return;
            }

            // Linha principal do empréstimo
            graphics.DrawString(emprestimo.DataEmprestimo.ToString("dd/MM/yy"), smallFont, Brushes.Black, colData, currentY);
            
            var nomeRecebedor = emprestimo.Name.Length > 14 ? emprestimo.Name.Substring(0, 14) + "..." : emprestimo.Name;
            graphics.DrawString(nomeRecebedor, smallFont, Brushes.Black, colRecebedor, currentY);
            
            var nomeCongregacao = emprestimo.CongregacaoName.Length > 16 ? emprestimo.CongregacaoName.Substring(0, 16) + "..." : emprestimo.CongregacaoName;
            graphics.DrawString(nomeCongregacao, smallFont, Brushes.Black, colCongregacao, currentY);
            
            graphics.DrawString(emprestimo.TotalItens.ToString(), smallFont, Brushes.Black, colQtd, currentY);
            
            var statusAbrev = emprestimo.Status == StatusEmprestimo.EmAndamento ? "Andamento" : "Devolvido";
            graphics.DrawString(statusAbrev, smallFont, Brushes.Black, colStatus, currentY);
            
            currentY += lineHeight;

            // Bens em linhas separadas (indentados)
            if (emprestimo.Itens != null && emprestimo.Itens.Any())
            {
                foreach (var item in emprestimo.Itens)
                {
                    var bemTexto = $"• {item.ItemName} ({item.Quantidade} un.)";
                    if (bemTexto.Length > 50)
                    {
                        bemTexto = bemTexto.Substring(0, 50) + "...";
                    }
                    graphics.DrawString(bemTexto, tinyFont, Brushes.DarkGray, colRecebedor, currentY);
                    currentY += 14;
                }
            }
            else
            {
                // Compatibilidade com dados antigos
                var bemTexto = $"• {emprestimo.ItemName} ({emprestimo.QuantityInStock} un.)";
                if (bemTexto.Length > 50)
                {
                    bemTexto = bemTexto.Substring(0, 50) + "...";
                }
                graphics.DrawString(bemTexto, tinyFont, Brushes.DarkGray, colRecebedor, currentY);
                currentY += 14;
            }

            currentY += 5; // Espaço entre empréstimos
        }

        currentY += 10;
        // Linha separadora final
        graphics.DrawLine(Pens.Black, leftMargin, currentY, rightMargin, currentY);

        // Rodapé
        currentY = e.PageBounds.Height - 60;
        graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, currentY);
        graphics.DrawString($"Total de registros: {_emprestimos.Count}", smallFont, Brushes.Gray, rightMargin - 150, currentY);

        e.HasMorePages = false;
        _currentEmprestimoIndex = 0; // Reset para próxima impressão
    }
}
