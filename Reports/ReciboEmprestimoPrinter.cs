using System.Drawing.Printing;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Reports;

public class ReciboEmprestimoPrinter
{
    private Emprestimo _emprestimo;

    public ReciboEmprestimoPrinter(Emprestimo emprestimo)
    {
        _emprestimo = emprestimo;
    }

    public void Print()
    {
        var printDocument = new PrintDocument();
        printDocument.PrintPage += PrintDocument_PrintPage;
        
        // Configurar para meia página A4
        printDocument.DefaultPageSettings.PaperSize = new PaperSize("Half A4", 827, 583); // ~210mm x 148mm
        
        var printDialog = new PrintDialog
        {
            Document = printDocument
        };

        if (printDialog.ShowDialog() == DialogResult.OK)
        {
            printDocument.Print();
        }
    }

    public void PrintPreview()
    {
        var printDocument = new PrintDocument();
        printDocument.PrintPage += PrintDocument_PrintPage;
        
        // Configurar para meia página A4
        printDocument.DefaultPageSettings.PaperSize = new PaperSize("Half A4", 827, 583);
        
        var printPreviewDialog = new PrintPreviewDialog
        {
            Document = printDocument,
            Width = 800,
            Height = 600,
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
        var leftMargin = 40;
        var topMargin = 40;
        var currentY = topMargin;
        var lineHeight = 20;

        // Fontes
        var titleFont = new Font("Arial", 14, FontStyle.Bold);
        var headerFont = new Font("Arial", 10, FontStyle.Bold);
        var normalFont = new Font("Arial", 9, FontStyle.Regular);
        var smallFont = new Font("Arial", 8, FontStyle.Regular);

        // Título e Logo
        var titulo = "SEMIADET - EMPRÉSTIMO DE BENS";
        var titleSize = graphics.MeasureString(titulo, titleFont);
        
        // Desenhar título
        graphics.DrawString(titulo, titleFont, Brushes.Black, leftMargin, currentY);
        
        // Carregar e desenhar logo (da pasta do executável)
        var logoHeight = 0;
        using (var logo = LoadLogoFromFile())
        {
            if (logo != null)
            {
                // Calcular tamanho da logo proporcional à altura do título
                logoHeight = (int)(titleSize.Height * 2.5); // Logo um pouco maior que o título
                var logoWidth = (int)(logo.Width * ((float)logoHeight / logo.Height));
                
                // Posicionar logo à direita, alinhada com o topo do título
                var logoX = e.PageBounds.Width - leftMargin - logoWidth;
                var logoY = currentY; // Alinhado com o topo do título (sem ajuste negativo)
                
                graphics.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);
            }
        }
        
        // Avançar para baixo da logo (usar altura da logo ou altura do título)
        currentY += Math.Max(logoHeight, (int)titleSize.Height) + 10; // +10 para espaçamento extra

        // Linha separadora (agora abaixo da logo)
        graphics.DrawLine(Pens.Black, leftMargin, currentY, e.PageBounds.Width - leftMargin, currentY);
        currentY += 15;

        // Dados do empréstimo
        graphics.DrawString($"Nº Empréstimo: {_emprestimo.Id}", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight + 5;

        graphics.DrawString($"Data: {_emprestimo.DataEmprestimo:dd/MM/yyyy}", normalFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight + 10;

        // Recebedor
        graphics.DrawString("Recebedor:", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight;
        graphics.DrawString(_emprestimo.Name, normalFont, Brushes.Black, leftMargin + 10, currentY);
        currentY += lineHeight + 10;

        // Congregação
        graphics.DrawString("Congregação:", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight;
        graphics.DrawString(_emprestimo.CongregacaoName, normalFont, Brushes.Black, leftMargin + 10, currentY);
        currentY += lineHeight + 10;

        // Bens Emprestados (múltiplos itens)
        graphics.DrawString("Bens Emprestados:", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight;
        
        if (_emprestimo.Itens != null && _emprestimo.Itens.Any())
        {
            foreach (var item in _emprestimo.Itens)
            {
                graphics.DrawString($"• {item.ItemName} - Quantidade: {item.Quantidade}", normalFont, Brushes.Black, leftMargin + 10, currentY);
                currentY += lineHeight;
            }
        }
        else
        {
            // Compatibilidade com dados antigos
            graphics.DrawString($"• {_emprestimo.ItemName} - Quantidade: {_emprestimo.QuantityInStock}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
        }
        
        currentY += 5;

        // Motivo
        graphics.DrawString("Motivo:", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight;
        
        // Quebrar linha do motivo se for muito longo
        var motivoLines = WrapText(_emprestimo.Motivo, normalFont, e.PageBounds.Width - leftMargin - 50, graphics);
        foreach (var line in motivoLines)
        {
            graphics.DrawString(line, normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
        }
        currentY += 10;

        // Linha separadora
        graphics.DrawLine(Pens.Black, leftMargin, currentY, e.PageBounds.Width - leftMargin, currentY);
        currentY += 20;

        // Assinaturas
        graphics.DrawString("Assinatura do Recebedor:", headerFont, Brushes.Black, leftMargin, currentY);
        graphics.DrawString("Quem Liberou:", headerFont, Brushes.Black, e.PageBounds.Width / 2 + 20, currentY);
        currentY += 30;
        
        // Linha para assinatura do recebedor
        graphics.DrawLine(Pens.Black, leftMargin, currentY, e.PageBounds.Width / 2 - 20, currentY);
        graphics.DrawString(_emprestimo.Name, smallFont, Brushes.Gray, leftMargin, currentY + 5);

        // Linha para assinatura de quem liberou
        graphics.DrawLine(Pens.Black, e.PageBounds.Width / 2 + 20, currentY, e.PageBounds.Width - leftMargin, currentY);
        if (!string.IsNullOrEmpty(_emprestimo.QuemLiberou))
        {
            graphics.DrawString(_emprestimo.QuemLiberou, smallFont, Brushes.Gray, e.PageBounds.Width / 2 + 20, currentY + 5);
        }

        // Rodapé
        currentY = e.PageBounds.Height - 60;
        graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, currentY);
        currentY += 15;
        graphics.DrawString("Este recibo comprova o recebimento dos itens acima descritos.", smallFont, Brushes.Gray, leftMargin, currentY);
    }

    private List<string> WrapText(string text, Font font, int maxWidth, Graphics graphics)
    {
        var lines = new List<string>();
        var words = text.Split(' ');
        var currentLine = "";

        foreach (var word in words)
        {
            var testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
            var size = graphics.MeasureString(testLine, font);

            if (size.Width > maxWidth && !string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
                currentLine = word;
            }
            else
            {
                currentLine = testLine;
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine);
        }

        return lines;
    }
}
