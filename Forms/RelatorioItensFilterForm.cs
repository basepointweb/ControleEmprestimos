using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Reports;
using ControleEmprestimos.Helpers;

namespace ControleEmprestimos.Forms;

public partial class RelatorioItensFilterForm : Form
{
    private DataRepository _repository;

    public bool ApenasBensEmprestados { get; private set; }

    public RelatorioItensFilterForm()
    {
        InitializeComponent();
        
        _repository = DataRepository.Instance;
    }

    private void BtnGerar_Click(object sender, EventArgs e)
    {
        // Verificar filtro selecionado
        ApenasBensEmprestados = rbApenasBensEmprestados.Checked;

        // Calcular total emprestado para cada item
        foreach (var item in _repository.Items)
        {
            item.TotalEmprestado = _repository.EmprestimoItens
                .Where(ei => ei.ItemId == item.Id)
                .Join(_repository.Emprestimos,
                    ei => ei.EmprestimoId,
                    e => e.Id,
                    (ei, e) => new { EmprestimoItem = ei, Emprestimo = e })
                .Where(x => x.Emprestimo.Status == StatusEmprestimo.EmAndamento)
                .Sum(x => x.EmprestimoItem.QuantidadePendente);
        }

        // Filtrar itens
        var itens = _repository.Items.ToList();

        if (ApenasBensEmprestados)
        {
            itens = itens.Where(i => i.TotalEmprestado > 0).ToList();
        }

        if (!itens.Any())
        {
            MessageBox.Show(
                "Nenhum bem encontrado com os filtros selecionados.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        // Obter informações de empréstimos por item
        var itensComEmprestimos = new List<(Item Item, List<(Emprestimo Emprestimo, int Quantidade)> Emprestimos)>();

        foreach (var item in itens)
        {
            var emprestimosItem = _repository.EmprestimoItens
                .Where(ei => ei.ItemId == item.Id)
                .Join(_repository.Emprestimos,
                    ei => ei.EmprestimoId,
                    e => e.Id,
                    (ei, e) => new { EmprestimoItem = ei, Emprestimo = e })
                .Where(x => x.Emprestimo.Status == StatusEmprestimo.EmAndamento)
                .Where(x => x.EmprestimoItem.QuantidadePendente > 0)
                .Select(x => (x.Emprestimo, x.EmprestimoItem.QuantidadePendente))
                .ToList();

            itensComEmprestimos.Add((item, emprestimosItem));
        }

        // Gerar relatório
        var printer = new RelatorioItensPrinter(
            itensComEmprestimos,
            ApenasBensEmprestados);
        
        printer.PrintPreview();
        
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void BtnCancelar_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
