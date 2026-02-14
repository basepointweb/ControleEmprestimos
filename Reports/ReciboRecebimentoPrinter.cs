using System.Drawing.Printing;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Reports;

public class ReciboRecebimentoPrinter
{
    private RecebimentoEmprestimo _recebimento;
    private Emprestimo? _emprestimo;

    public ReciboRecebimentoPrinter(RecebimentoEmprestimo recebimento, Emprestimo? emprestimo = null)
    {
        _recebimento = recebimento;
        _emprestimo = emprestimo;
    }

    public void Print()
    {
        var printDocument = new PrintDocument();
        printDocument.PrintPage += PrintDocument_PrintPage;
        
        // Configurar para meia página A4
        printDocument.DefaultPageSettings.PaperSize = new PaperSize("Half A4", 827, 583);
        
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
        var warningFont = new Font("Arial", 9, FontStyle.Bold | FontStyle.Italic);

        // Título e Logo
        var tituloBase = "SEMIADET - Recebimento de bens emprestados";
        var titulo = _recebimento.RecebimentoParcial ? tituloBase + " (PARCIAL)" : tituloBase;
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

        // Dados do recebimento
        graphics.DrawString($"Nº Empréstimo: {_recebimento.EmprestimoId ?? 0}", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight + 5;

        graphics.DrawString($"Data de Empréstimo: {_recebimento.DataEmprestimo?.ToString("dd/MM/yyyy") ?? "N/A"}", normalFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight;
        
        graphics.DrawString($"Data de Recebimento: {_recebimento.DataRecebimento:dd/MM/yyyy HH:mm}", normalFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight + 10;

        // Quem pegou emprestado
        graphics.DrawString("Quem Pegou Emprestado:", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight;
        graphics.DrawString(_recebimento.NomeRecebedor, normalFont, Brushes.Black, leftMargin + 10, currentY);
        currentY += lineHeight + 10;

        // Bens Devolvidos (itens recebidos neste recebimento)
        graphics.DrawString("Bens Devolvidos:", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight;
        
        if (_recebimento.ItensRecebidos != null && _recebimento.ItensRecebidos.Any())
        {
            foreach (var item in _recebimento.ItensRecebidos)
            {
                graphics.DrawString($"• {item.ItemName} - Quantidade: {item.QuantidadeRecebida}", normalFont, Brushes.Black, leftMargin + 10, currentY);
                currentY += lineHeight;
            }
        }
        else
        {
            // Compatibilidade com dados antigos
            var itemName = _recebimento.Name.Replace("Recebimento - ", "");
            graphics.DrawString($"• {itemName} - Quantidade: {_recebimento.QuantityInStock}", normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight;
        }
        
        currentY += 5;

        // Congregação (se houver empréstimo)
        if (_emprestimo != null)
        {
            graphics.DrawString("Congregação:", headerFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight;
            graphics.DrawString(_emprestimo.CongregacaoName, normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight + 10;
        }

        // Indicador de recebimento parcial com lista de itens pendentes
        if (_recebimento.RecebimentoParcial && _emprestimo != null)
        {
            graphics.DrawString("? RECEBIMENTO PARCIAL - Ainda há itens pendentes de devolução", 
                warningFont, 
                Brushes.DarkOrange, 
                leftMargin, 
                currentY);
            currentY += lineHeight + 5;

            // Listar itens pendentes
            var itensPendentes = _emprestimo.Itens
                .Where(ei => ei.QuantidadePendente > 0)
                .ToList();

            if (itensPendentes.Any())
            {
                graphics.DrawString("Itens ainda pendentes:", headerFont, Brushes.DarkOrange, leftMargin, currentY);
                currentY += lineHeight;

                foreach (var itemPendente in itensPendentes)
                {
                    graphics.DrawString(
                        $"• {itemPendente.ItemName} - Pendente: {itemPendente.QuantidadePendente} unidade(s)",
                        normalFont,
                        Brushes.DarkOrange,
                        leftMargin + 10,
                        currentY);
                    currentY += lineHeight;
                }
                currentY += 5;
            }
        }

        // Linha separadora
        graphics.DrawLine(Pens.Black, leftMargin, currentY, e.PageBounds.Width - leftMargin, currentY);
        currentY += 20;

        // Quem recebeu de volta
        graphics.DrawString("Recebido por:", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight;
        graphics.DrawString(_recebimento.NomeQuemRecebeu, normalFont, Brushes.Black, leftMargin + 10, currentY);
        currentY += lineHeight + 15;

        // Assinatura
        graphics.DrawString("Assinatura de Quem Recebeu:", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += 30;
        
        // Linha para assinatura
        graphics.DrawLine(Pens.Black, leftMargin, currentY, e.PageBounds.Width - leftMargin - 200, currentY);
        currentY += 5;
        graphics.DrawString(_recebimento.NomeQuemRecebeu, smallFont, Brushes.Gray, leftMargin, currentY);

        // Rodapé
        currentY = e.PageBounds.Height - 60;
        graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, currentY);
        currentY += 15;
        graphics.DrawString("Este recibo comprova a devolução dos itens acima descritos.", smallFont, Brushes.Gray, leftMargin, currentY);
    }
}
