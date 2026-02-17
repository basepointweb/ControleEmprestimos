using System.Drawing.Printing;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Reports;

public class EtiquetasPrinter
{
    private List<Item> _itens;
    private int _currentItemIndex = 0;

    public EtiquetasPrinter(List<Item> itens)
    {
        _itens = itens.OrderBy(i => i.Name).ToList();
    }

    public void PrintPreview()
    {
        var printDocument = new PrintDocument();
        printDocument.PrintPage += PrintDocument_PrintPage;
        
        // Configurar para A4
        printDocument.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
        printDocument.DefaultPageSettings.Landscape = false;
        printDocument.DefaultPageSettings.Margins = new Margins(20, 20, 20, 20);
        
        var printPreviewDialog = new PrintPreviewDialog
        {
            Document = printDocument,
            Width = 1000,
            Height = 700,
            StartPosition = FormStartPosition.CenterScreen
        };

        printPreviewDialog.ShowDialog();
    }

    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        if (e.Graphics == null) return;

        var graphics = e.Graphics;
        
        // Obter DPI (geralmente 96 ou 100 para tela)
        float dpiX = graphics.DpiX;
        float dpiY = graphics.DpiY;
        
        // DIMENSÕES FIXAS DAS ETIQUETAS EM PIXELS (baseado em 96 DPI)
        // 4.75cm = 1.87 polegadas * 96 DPI = 179 pixels
        // 3cm = 1.18 polegadas * 96 DPI = 113 pixels
        float etiquetaLarguraPx = 179f;
        float etiquetaAlturaPx = 113f;
        
        // Espaçamento entre etiquetas
        float espacamentoHPx = 10f;
        float espacamentoVPx = 8f;
        
        // Margem interna
        float margemInternaPx = 4f;
        
        // Margens da página
        float margemEsquerda = e.MarginBounds.Left;
        float margemTopo = e.MarginBounds.Top;
        float alturaDisponivel = e.MarginBounds.Height;
        
        // FORÇAR 4 etiquetas por linha
        int etiquetasPorLinha = 4;
        
        // Calcular quantas linhas cabem
        float alturaLinha = etiquetaAlturaPx + espacamentoVPx;
        int etiquetasPorColuna = (int)(alturaDisponivel / alturaLinha);
        
        if (etiquetasPorColuna < 1) etiquetasPorColuna = 1;
        
        // Fontes - ID maior e mesma cor do nome
        var nomeFont = new Font("Arial", 6, FontStyle.Bold);
        var idFont = new Font("Arial", 6, FontStyle.Regular);  // Aumentado de 5 para 6
        
        // Pen para bordas tracejadas (bem fina)
        var dashedPen = new Pen(Color.Gray, 0.5f);
        dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        
        // Desenhar etiquetas
        for (int linha = 0; linha < etiquetasPorColuna && _currentItemIndex < _itens.Count; linha++)
        {
            for (int coluna = 0; coluna < etiquetasPorLinha && _currentItemIndex < _itens.Count; coluna++)
            {
                var item = _itens[_currentItemIndex];
                
                // Calcular posição da etiqueta com espaçamento
                float x = margemEsquerda + (coluna * (etiquetaLarguraPx + espacamentoHPx));
                float y = margemTopo + (linha * (etiquetaAlturaPx + espacamentoVPx));
                
                // Desenhar borda tracejada da etiqueta
                graphics.DrawRectangle(dashedPen, x, y, etiquetaLarguraPx, etiquetaAlturaPx);
                
                // Área interna (com margem)
                float xInterno = x + margemInternaPx;
                float yInterno = y + margemInternaPx;
                float larguraInterna = etiquetaLarguraPx - (2 * margemInternaPx);
                float alturaInterna = etiquetaAlturaPx - (2 * margemInternaPx);
                
                // Desenhar conteúdo da etiqueta
                float yAtual = yInterno;
                
                // ID do item (canto superior direito, PRETO e maior)
                var idTexto = $"ID: {item.Id}";
                var idSize = graphics.MeasureString(idTexto, idFont);
                graphics.DrawString(idTexto, idFont, Brushes.Black,  // Mudado de Gray para Black
                    xInterno + larguraInterna - idSize.Width, yAtual);
                yAtual += idSize.Height + 1;
                
                // Nome do item (centralizado)
                var nomeTexto = item.Name;
                
                // Calcular posição para centralizar verticalmente
                float espacoVerticalDisponivel = alturaInterna - (yAtual - yInterno);
                
                // Área para o nome
                var nomeRect = new RectangleF(xInterno, yAtual, larguraInterna, espacoVerticalDisponivel);
                var stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisWord
                };
                
                graphics.DrawString(nomeTexto, nomeFont, Brushes.Black, nomeRect, stringFormat);
                
                _currentItemIndex++;
            }
        }
        
        // Verificar se há mais páginas
        if (_currentItemIndex < _itens.Count)
        {
            e.HasMorePages = true;
        }
        else
        {
            e.HasMorePages = false;
            _currentItemIndex = 0; // Reset para próxima impressão
        }
        
        // Limpar recursos
        dashedPen.Dispose();
    }
}
