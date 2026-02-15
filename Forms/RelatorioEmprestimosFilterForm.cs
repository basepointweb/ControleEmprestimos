using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Reports;
using ControleEmprestimos.Helpers;

namespace ControleEmprestimos.Forms;

public partial class RelatorioEmprestimosFilterForm : Form
{
    private DataRepository _repository;

    public DateTime DataInicial { get; private set; }
    public DateTime DataFinal { get; private set; }
    public int? CongregacaoId { get; private set; }
    public int? ItemId { get; private set; }

    public RelatorioEmprestimosFilterForm()
    {
        InitializeComponent();
        
        // Substituir DateTimePickers por MaskedTextBox
        FormControlHelper.ReplaceAllDateTimePickersWithMaskedTextBox(this);
        
        _repository = DataRepository.Instance;

        // Definir mês atual como padrão
        var hoje = DateTime.Now;
        var dataInicial = new DateTime(hoje.Year, hoje.Month, 1);
        var dataFinal = new DateTime(hoje.Year, hoje.Month, DateTime.DaysInMonth(hoje.Year, hoje.Month));
        
        var mtbDataInicial = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataInicial");
        var mtbDataFinal = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataFinal");
        
        if (mtbDataInicial != null)
        {
            mtbDataInicial.Text = dataInicial.ToString("dd/MM/yyyy");
        }
        
        if (mtbDataFinal != null)
        {
            mtbDataFinal.Text = dataFinal.ToString("dd/MM/yyyy");
        }

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
        // Obter datas dos MaskedTextBox
        var mtbDataInicial = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataInicial");
        var mtbDataFinal = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataFinal");

        if (mtbDataInicial == null || !DateTime.TryParse(mtbDataInicial.Text, out DateTime dataInicial))
        {
            MessageBox.Show(
                "Por favor, informe uma data inicial válida.",
                "Validação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            mtbDataInicial?.Focus();
            return;
        }

        if (mtbDataFinal == null || !DateTime.TryParse(mtbDataFinal.Text, out DateTime dataFinal))
        {
            MessageBox.Show(
                "Por favor, informe uma data final válida.",
                "Validação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            mtbDataFinal?.Focus();
            return;
        }

        if (dataInicial.Date > dataFinal.Date)
        {
            MessageBox.Show(
                "A data inicial não pode ser maior que a data final.",
                "Validação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        DataInicial = dataInicial.Date;
        DataFinal = dataFinal.Date;

        var selectedCongregacao = cmbCongregacao.SelectedItem as dynamic;
        CongregacaoId = selectedCongregacao?.Id;

        var selectedItem = cmbItem.SelectedItem as dynamic;
        ItemId = selectedItem?.Id;

        // Filtrar empréstimos
        var emprestimos = _repository.Emprestimos
            .Where(e => e.DataEmprestimo.Date >= DataInicial && e.DataEmprestimo.Date <= DataFinal)
            .ToList();

        if (CongregacaoId.HasValue)
        {
            emprestimos = emprestimos.Where(e => e.CongregacaoId == CongregacaoId.Value).ToList();
        }

        if (ItemId.HasValue)
        {
            emprestimos = emprestimos
                .Where(e => e.Itens != null && e.Itens.Any(i => i.ItemId == ItemId.Value))
                .ToList();
        }

        if (!emprestimos.Any())
        {
            MessageBox.Show(
                "Nenhum empréstimo encontrado com os filtros selecionados.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        // Gerar relatório
        var printer = new RelatorioEmprestimosPrinter(
            emprestimos, 
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
