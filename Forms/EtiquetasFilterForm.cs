using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Reports;

namespace ControleEmprestimos.Forms;

public partial class EtiquetasFilterForm : Form
{
    private DataRepository _repository;
    private List<Item> _todosItens;
    private List<Item> _itensSelecionados = new();

    public EtiquetasFilterForm()
    {
        InitializeComponent();
        _repository = DataRepository.Instance;
        _todosItens = _repository.Items.OrderBy(i => i.Name).ToList();
        
        ConfigureForm();
        CarregarItens();
    }

    private void ConfigureForm()
    {
        this.Text = "Imprimir Etiquetas de Bens";
        this.Size = new Size(750, 600);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
    }

    private void CarregarItens()
    {
        // Carregar todos os itens no checkedListBox
        checkedListBoxItens.Items.Clear();
        foreach (var item in _todosItens)
        {
            checkedListBoxItens.Items.Add(item.Name, false);
        }
    }

    private void TxtFiltroIds_TextChanged(object? sender, EventArgs e)
    {
        AplicarFiltro();
    }

    private void AplicarFiltro()
    {
        var filtroTexto = txtFiltroIds.Text.Trim();
        
        if (string.IsNullOrWhiteSpace(filtroTexto))
        {
            // Mostrar todos os itens
            checkedListBoxItens.Items.Clear();
            foreach (var item in _todosItens)
            {
                checkedListBoxItens.Items.Add(item.Name, false);
            }
            lblItensEncontrados.Text = "";
            return;
        }

        // Filtrar por IDs (separados por vírgula, espaço, ou ponto e vírgula)
        var separadores = new[] { ',', ' ', ';' };
        var ids = filtroTexto.Split(separadores, StringSplitOptions.RemoveEmptyEntries)
            .Where(s => int.TryParse(s, out _))
            .Select(int.Parse)
            .Distinct()
            .ToList();

        if (ids.Any())
        {
            // Filtrar itens pelos IDs
            var itensFiltrados = _todosItens.Where(i => ids.Contains(i.Id)).ToList();
            
            checkedListBoxItens.Items.Clear();
            foreach (var item in itensFiltrados)
            {
                checkedListBoxItens.Items.Add(item.Name, true); // Marca automaticamente
            }
            
            lblItensEncontrados.Text = $"{itensFiltrados.Count} item(ns) encontrado(s)";
        }
        else
        {
            // Filtrar por nome
            var itensFiltrados = _todosItens
                .Where(i => i.Name.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            checkedListBoxItens.Items.Clear();
            foreach (var item in itensFiltrados)
            {
                checkedListBoxItens.Items.Add(item.Name, false);
            }
            
            lblItensEncontrados.Text = $"{itensFiltrados.Count} item(ns) encontrado(s)";
        }
    }

    private void BtnMarcarTodos_Click(object? sender, EventArgs e)
    {
        for (int i = 0; i < checkedListBoxItens.Items.Count; i++)
        {
            checkedListBoxItens.SetItemChecked(i, true);
        }
    }

    private void BtnDesmarcarTodos_Click(object? sender, EventArgs e)
    {
        for (int i = 0; i < checkedListBoxItens.Items.Count; i++)
        {
            checkedListBoxItens.SetItemChecked(i, false);
        }
    }

    private void BtnImprimir_Click(object? sender, EventArgs e)
    {
        // Obter itens selecionados
        _itensSelecionados.Clear();
        
        for (int i = 0; i < checkedListBoxItens.CheckedItems.Count; i++)
        {
            var nomeItem = checkedListBoxItens.CheckedItems[i].ToString();
            var item = _todosItens.FirstOrDefault(it => it.Name == nomeItem);
            if (item != null)
            {
                _itensSelecionados.Add(item);
            }
        }

        if (!_itensSelecionados.Any())
        {
            MessageBox.Show(
                "Por favor, selecione pelo menos um item para imprimir as etiquetas.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        // Abrir preview de impressão
        var printer = new EtiquetasPrinter(_itensSelecionados);
        printer.PrintPreview();
        
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
