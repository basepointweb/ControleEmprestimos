using System.Drawing.Printing;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Reports;

public class ReciboRecebimentoPrinter
{
    private RecebimentoEmprestimo _recebimento;
    private Emprestimo? _emprestimo;
    private string _itemName = "";

    public ReciboRecebimentoPrinter(RecebimentoEmprestimo recebimento, Emprestimo? emprestimo = null)
    {
        _recebimento = recebimento;
        _emprestimo = emprestimo;
        
        // Extrair nome do item do Name se não tiver empréstimo
        if (emprestimo == null && !string.IsNullOrEmpty(recebimento.Name))
        {
            _itemName = recebimento.Name.Replace("Recebimento - ", "");
        }
        else if (emprestimo != null)
        {
            _itemName = emprestimo.ItemName;
        }
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

        // Título
        graphics.DrawString("RECIBO DE RECEBIMENTO", titleFont, Brushes.Black, leftMargin, currentY);
        currentY += 30;

        // Linha separadora
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

        // Bem
        graphics.DrawString("Bem Devolvido:", headerFont, Brushes.Black, leftMargin, currentY);
        currentY += lineHeight;
        graphics.DrawString($"{_itemName} - Quantidade: {_recebimento.QuantityInStock}", normalFont, Brushes.Black, leftMargin + 10, currentY);
        currentY += lineHeight + 10;

        // Congregação (se houver empréstimo)
        if (_emprestimo != null)
        {
            graphics.DrawString("Congregação:", headerFont, Brushes.Black, leftMargin, currentY);
            currentY += lineHeight;
            graphics.DrawString(_emprestimo.CongregacaoName, normalFont, Brushes.Black, leftMargin + 10, currentY);
            currentY += lineHeight + 10;
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
