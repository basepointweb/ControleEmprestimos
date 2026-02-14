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
        graphics.DrawString("RELATÓRIO DE RECEBIMENTOS", titleFont, Brushes.Black, leftMargin, currentY);
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

        var colData = leftMargin;
        var colQuemPegou = leftMargin + 80;
        var colQuemRecebeu = leftMargin + 200;
        var colItens = leftMargin + 330;
        var colQtd = leftMargin + 530;
        var colTipo = leftMargin + 580;

        graphics.DrawString("Data", smallFont, Brushes.Black, colData, currentY);
        graphics.DrawString("Quem Pegou", smallFont, Brushes.Black, colQuemPegou, currentY);
        graphics.DrawString("Quem Recebeu", smallFont, Brushes.Black, colQuemRecebeu, currentY);
        graphics.DrawString("Bens", smallFont, Brushes.Black, colItens, currentY);
        graphics.DrawString("Qtd", smallFont, Brushes.Black, colQtd, currentY);
        graphics.DrawString("Tipo", smallFont, Brushes.Black, colTipo, currentY);
        currentY += 15;

        // Linha separadora
        graphics.DrawLine(Pens.Gray, leftMargin, currentY, rightMargin, currentY);
        currentY += 10;

        // Dados dos recebimentos
        foreach (var recebimento in _recebimentos)
        {
            // Verificar se precisa de nova página
            if (currentY > e.PageBounds.Height - 100)
            {
                e.HasMorePages = true;
                return;
            }

            graphics.DrawString(recebimento.DataRecebimento.ToString("dd/MM/yy"), smallFont, Brushes.Black, colData, currentY);
            
            var nomeQuemPegou = recebimento.NomeRecebedor.Length > 15 ? recebimento.NomeRecebedor.Substring(0, 15) + "..." : recebimento.NomeRecebedor;
            graphics.DrawString(nomeQuemPegou, smallFont, Brushes.Black, colQuemPegou, currentY);
            
            var nomeQuemRecebeu = recebimento.NomeQuemRecebeu.Length > 18 ? recebimento.NomeQuemRecebeu.Substring(0, 18) + "..." : recebimento.NomeQuemRecebeu;
            graphics.DrawString(nomeQuemRecebeu, smallFont, Brushes.Black, colQuemRecebeu, currentY);
            
            // Bens
            string bens;
            if (recebimento.ItensRecebidos != null && recebimento.ItensRecebidos.Any())
            {
                var nomes = string.Join(", ", recebimento.ItensRecebidos.Select(i => i.ItemName).Distinct());
                bens = nomes.Length > 30 ? nomes.Substring(0, 30) + "..." : nomes;
            }
            else
            {
                bens = "N/A";
            }
            graphics.DrawString(bens, smallFont, Brushes.Black, colItens, currentY);
            
            graphics.DrawString(recebimento.TotalItensRecebidos.ToString(), smallFont, Brushes.Black, colQtd, currentY);
            
            var tipo = recebimento.RecebimentoParcial ? "Parcial" : "Completo";
            graphics.DrawString(tipo, smallFont, Brushes.Black, colTipo, currentY);
            
            currentY += lineHeight;
        }

        currentY += 10;
        // Linha separadora final
        graphics.DrawLine(Pens.Black, leftMargin, currentY, rightMargin, currentY);

        // Rodapé
        currentY = e.PageBounds.Height - 60;
        graphics.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont, Brushes.Gray, leftMargin, currentY);
        graphics.DrawString($"Total de registros: {totalRecebimentos}", smallFont, Brushes.Gray, rightMargin - 150, currentY);

        e.HasMorePages = false;
    }
}
