using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Reports;

namespace ControleEmprestimos.Forms;

public partial class RelatorioRecebimentosFilterForm : Form
{
    private DataRepository _repository;

    public DateTime DataInicial { get; private set; }
    public DateTime DataFinal { get; private set; }
    public int? CongregacaoId { get; private set; }
    public int? ItemId { get; private set; }

    public RelatorioRecebimentosFilterForm()
    {
        InitializeComponent();
        _repository = DataRepository.Instance;

        // Definir mês atual como padrão
        var hoje = DateTime.Now;
        dtpDataInicial.Value = new DateTime(hoje.Year, hoje.Month, 1);
        dtpDataFinal.Value = new DateTime(hoje.Year, hoje.Month, DateTime.DaysInMonth(hoje.Year, hoje.Month));

        LoadCongregacoes();
        LoadItens();
    }

    private void LoadCongregacoes()
    {
        var congregacoes = _repository.Congregacoes
            .OrderBy(c => c.Name)
            .ToList();

        cmbCongregacao.Items.Add(new { Id = (int?)null, Name = "Todas" });
        foreach (var congregacao in congregacoes)
        {
            cmbCongregacao.Items.Add(new { Id = (int?)congregacao.Id, Name = congregacao.Name });
        }

        cmbCongregacao.DisplayMember = "Name";
        cmbCongregacao.ValueMember = "Id";
        cmbCongregacao.SelectedIndex = 0;
    }

    private void LoadItens()
    {
        var itens = _repository.Items
            .OrderBy(i => i.Name)
            .ToList();

        cmbItem.Items.Add(new { Id = (int?)null, Name = "Todos" });
        foreach (var item in itens)
        {
            cmbItem.Items.Add(new { Id = (int?)item.Id, Name = item.Name });
        }

        cmbItem.DisplayMember = "Name";
        cmbItem.ValueMember = "Id";
        cmbItem.SelectedIndex = 0;
    }

    private void BtnGerar_Click(object sender, EventArgs e)
    {
        if (dtpDataInicial.Value.Date > dtpDataFinal.Value.Date)
        {
            MessageBox.Show(
                "A data inicial não pode ser maior que a data final.",
                "Validação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        DataInicial = dtpDataInicial.Value.Date;
        DataFinal = dtpDataFinal.Value.Date;

        var selectedCongregacao = cmbCongregacao.SelectedItem as dynamic;
        CongregacaoId = selectedCongregacao?.Id;

        var selectedItem = cmbItem.SelectedItem as dynamic;
        ItemId = selectedItem?.Id;

        // Filtrar recebimentos
        var recebimentos = _repository.RecebimentoEmprestimos
            .Where(r => r.DataRecebimento.Date >= DataInicial && r.DataRecebimento.Date <= DataFinal)
            .ToList();

        // Filtrar por congregação através do empréstimo
        if (CongregacaoId.HasValue)
        {
            recebimentos = recebimentos
                .Where(r => r.EmprestimoId.HasValue)
                .Where(r => 
                {
                    var emprestimo = _repository.Emprestimos.FirstOrDefault(e => e.Id == r.EmprestimoId.Value);
                    return emprestimo != null && emprestimo.CongregacaoId == CongregacaoId.Value;
                })
                .ToList();
        }

        // Filtrar por item
        if (ItemId.HasValue)
        {
            recebimentos = recebimentos
                .Where(r => r.ItensRecebidos != null && r.ItensRecebidos.Any(i => i.ItemId == ItemId.Value))
                .ToList();
        }

        if (!recebimentos.Any())
        {
            MessageBox.Show(
                "Nenhum recebimento encontrado com os filtros selecionados.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        // Gerar relatório
        var printer = new RelatorioRecebimentosPrinter(
            recebimentos, 
            DataInicial, 
            DataFinal,
            CongregacaoId.HasValue ? selectedCongregacao.Name : "Todas",
            ItemId.HasValue ? selectedItem.Name : "Todos");
        
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
