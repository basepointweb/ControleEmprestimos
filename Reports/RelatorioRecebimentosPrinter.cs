using System.Drawing.Printing;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Reports;

public class RelatorioRecebimentosPrinter
{
    private List<RecebimentoEmprestimo> _recebimentos;
    private DateTime _dataInicial;
    private DateTime _dataFinal;
    private string _congregacaoFiltro;
    private string _itemFiltro;
    private int _currentRecebimentoIndex = 0;

    public RelatorioRecebimentosPrinter(
        List<RecebimentoEmprestimo> recebimentos, 
        DateTime dataInicial, 
        DateTime dataFinal,
        string congregacaoFiltro,
        string itemFiltro)
    {
        _recebimentos = recebimentos.OrderBy(r => r.DataRecebimento).ToList();
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
        if (_currentRecebimentoIndex == 0)
        {
            var titulo = "SEMIADET - RELATÓRIO DE RECEBIMENTOS";
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
            var totalRecebimentos = _recebimentos.Count;
            var totalItens = _recebimentos.Sum(r => r.TotalItensRecebidos);
            var recebimentosParciais = _recebimentos.Count(r => r.RecebimentoParcial);
            var recebimentosCompletos = _recebimentos.Count(r => !r.RecebimentoParcial);

            graphics.DrawString("RESUMO", headerFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight + 5;

            graphics.DrawString($"Total de Recebimentos: {totalRecebimentos}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Total de Itens Recebidos: {totalItens}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Recebimentos Parciais: {recebimentosParciais}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
            graphics.DrawString($"Recebimentos Completos: {recebimentosCompletos}", normalFont, Brushes.Black, leftMargin + 10, currentY);
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
        var colQuemPegou = leftMargin + 70;
        var colQuemRecebeu = leftMargin + 180;
        var colQtd = leftMargin + 290;
        var colTipo = leftMargin + 340;

        graphics.DrawString("Data", smallFont, Brushes.Black, colData, currentY);
        graphics.DrawString("Quem Pegou", smallFont, Brushes.Black, colQuemPegou, currentY);
        graphics.DrawString("Quem Recebeu", smallFont, Brushes.Black, colQuemRecebeu, currentY);
        graphics.DrawString("Qtd", smallFont, Brushes.Black, colQtd, currentY);
        graphics.DrawString("Tipo", smallFont, Brushes.Black, colTipo, currentY);
        currentY += 15;

        // Linha separadora
        graphics.DrawLine(Pens.Gray, leftMargin, currentY, rightMargin, currentY);
        currentY += 10;

        // Dados dos recebimentos
        for (int i = _currentRecebimentoIndex; i < _recebimentos.Count; i++)
        {
            var recebimento = _recebimentos[i];

            // Calcular altura necessária para este recebimento
            int numItens = 0;
            if (recebimento.ItensRecebidos != null && recebimento.ItensRecebidos.Any())
            {
                numItens = recebimento.ItensRecebidos.Count;
            }
            else
            {
                numItens = 1; // Mínimo
            }

            var alturaRecebimento = lineHeight + (numItens * 14); // Linha principal + itens

            // Verificar se precisa de nova página
            if (currentY + alturaRecebimento > e.PageBounds.Height - 100)
            {
                _currentRecebimentoIndex = i;
                e.HasMorePages = true;
                
                // Rodapé da página
                var rodapeY = e.PageBounds.Height - 60;
                graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, rodapeY);
                graphics.DrawString($"Página {(_currentRecebimentoIndex / 10) + 1}", smallFont, Brushes.Gray, rightMargin - 100, rodapeY);
                return;
            }

            // Linha principal do recebimento
            graphics.DrawString(recebimento.DataRecebimento.ToString("dd/MM/yy"), smallFont, Brushes.Black, colData, currentY);
            
            var nomeQuemPegou = recebimento.NomeRecebedor.Length > 14 ? recebimento.NomeRecebedor.Substring(0, 14) + "..." : recebimento.NomeRecebedor;
            graphics.DrawString(nomeQuemPegou, smallFont, Brushes.Black, colQuemPegou, currentY);
            
            var nomeQuemRecebeu = recebimento.NomeQuemRecebeu.Length > 14 ? recebimento.NomeQuemRecebeu.Substring(0, 14) + "..." : recebimento.NomeQuemRecebeu;
            graphics.DrawString(nomeQuemRecebeu, smallFont, Brushes.Black, colQuemRecebeu, currentY);
            
            graphics.DrawString(recebimento.TotalItensRecebidos.ToString(), smallFont, Brushes.Black, colQtd, currentY);
            
            var tipo = recebimento.RecebimentoParcial ? "Parcial" : "Completo";
            graphics.DrawString(tipo, smallFont, Brushes.Black, colTipo, currentY);
            
            currentY += lineHeight;

            // Bens recebidos em linhas separadas (indentados)
            if (recebimento.ItensRecebidos != null && recebimento.ItensRecebidos.Any())
            {
                foreach (var item in recebimento.ItensRecebidos)
                {
                    var bemTexto = $"• {item.ItemName} ({item.QuantidadeRecebida} un.)";
                    if (bemTexto.Length > 50)
                    {
                        bemTexto = bemTexto.Substring(0, 50) + "...";
                    }
                    graphics.DrawString(bemTexto, tinyFont, Brushes.DarkGray, colQuemPegou, currentY);
                    currentY += 14;
                }
            }
            else
            {
                // Sem itens
                graphics.DrawString("• N/A", tinyFont, Brushes.DarkGray, colQuemPegou, currentY);
                currentY += 14;
            }

            currentY += 5; // Espaço entre recebimentos
        }

        currentY += 10;
        // Linha separadora final
        graphics.DrawLine(Pens.Black, leftMargin, currentY, rightMargin, currentY);

        // Rodapé
        currentY = e.PageBounds.Height - 60;
        graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, currentY);
        graphics.DrawString($"Total de registros: {_recebimentos.Count}", smallFont, Brushes.Gray, rightMargin - 150, currentY);

        e.HasMorePages = false;
        _currentRecebimentoIndex = 0; // Reset para próxima impressão
    }
}
