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

        // Título
        graphics.DrawString("RELATÓRIO DE EMPRÉSTIMOS", titleFont, Brushes.Black, leftMargin, currentY);
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

        var colData = leftMargin;
        var colRecebedor = leftMargin + 80;
        var colCongregacao = leftMargin + 200;
        var colItens = leftMargin + 330;
        var colQtd = leftMargin + 530;
        var colStatus = leftMargin + 580;

        graphics.DrawString("Data", smallFont, Brushes.Black, colData, currentY);
        graphics.DrawString("Recebedor", smallFont, Brushes.Black, colRecebedor, currentY);
        graphics.DrawString("Congregação", smallFont, Brushes.Black, colCongregacao, currentY);
        graphics.DrawString("Bens", smallFont, Brushes.Black, colItens, currentY);
        graphics.DrawString("Qtd", smallFont, Brushes.Black, colQtd, currentY);
        graphics.DrawString("Status", smallFont, Brushes.Black, colStatus, currentY);
        currentY += 15;

        // Linha separadora
        graphics.DrawLine(Pens.Gray, leftMargin, currentY, rightMargin, currentY);
        currentY += 10;

        // Dados dos empréstimos
        foreach (var emprestimo in _emprestimos)
        {
            // Verificar se precisa de nova página
            if (currentY > e.PageBounds.Height - 100)
            {
                e.HasMorePages = true;
                return;
            }

            graphics.DrawString(emprestimo.DataEmprestimo.ToString("dd/MM/yy"), smallFont, Brushes.Black, colData, currentY);
            
            var nomeRecebedor = emprestimo.Name.Length > 15 ? emprestimo.Name.Substring(0, 15) + "..." : emprestimo.Name;
            graphics.DrawString(nomeRecebedor, smallFont, Brushes.Black, colRecebedor, currentY);
            
            var nomeCongregacao = emprestimo.CongregacaoName.Length > 18 ? emprestimo.CongregacaoName.Substring(0, 18) + "..." : emprestimo.CongregacaoName;
            graphics.DrawString(nomeCongregacao, smallFont, Brushes.Black, colCongregacao, currentY);
            
            // Bens
            string bens;
            if (emprestimo.Itens != null && emprestimo.Itens.Any())
            {
                var nomes = string.Join(", ", emprestimo.Itens.Select(i => i.ItemName).Distinct());
                bens = nomes.Length > 30 ? nomes.Substring(0, 30) + "..." : nomes;
            }
            else
            {
                bens = emprestimo.ItemName.Length > 30 ? emprestimo.ItemName.Substring(0, 30) + "..." : emprestimo.ItemName;
            }
            graphics.DrawString(bens, smallFont, Brushes.Black, colItens, currentY);
            
            graphics.DrawString(emprestimo.TotalItens.ToString(), smallFont, Brushes.Black, colQtd, currentY);
            
            var statusAbrev = emprestimo.Status == StatusEmprestimo.EmAndamento ? "Andamento" : "Devolvido";
            graphics.DrawString(statusAbrev, smallFont, Brushes.Black, colStatus, currentY);
            
            currentY += lineHeight;
        }

        currentY += 10;
        // Linha separadora final
        graphics.DrawLine(Pens.Black, leftMargin, currentY, rightMargin, currentY);

        // Rodapé
        currentY = e.PageBounds.Height - 60;
        graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, currentY);
        graphics.DrawString($"Total de registros: {totalEmprestimos}", smallFont, Brushes.Gray, rightMargin - 150, currentY);

        e.HasMorePages = false;
    }
}
