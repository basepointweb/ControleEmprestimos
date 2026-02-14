using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class CongregacaoListForm : UserControl
{
    private DataRepository _repository;
    private List<Congregacao> _allCongregacoes = new();
    private Dictionary<string, List<string>> _columnFilters = new();

    public CongregacaoListForm()
    {
        InitializeComponent();
        _repository = DataRepository.Instance;
        ConfigureDataGridView();
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        if (Visible)
        {
            LoadData();
        }
    }

    private void ConfigureDataGridView()
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Id",
            HeaderText = "ID",
            Name = "colId",
            Width = 50
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Name",
            HeaderText = "Nome",
            Name = "colName",
            Width = 250
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Setor",
            HeaderText = "Setor",
            Name = "colSetor",
            Width = 100
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "TotalItensEmprestados",
            HeaderText = "Total de Itens Emprestados",
            Name = "colTotalEmprestados",
            Width = 180
        });

        // Event handler para clique no header
        dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;
    }

    private void DataGridView1_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            var column = dataGridView1.Columns[e.ColumnIndex];
            ShowColumnFilter(column);
        }
    }

    private void ShowColumnFilter(DataGridViewColumn column)
    {
        // Obter valores distintos da coluna
        var distinctValues = _allCongregacoes
            .Select(item => GetPropertyValue(item, column.DataPropertyName))
            .Where(v => !string.IsNullOrEmpty(v))
            .Distinct()
            .OrderBy(v => v)
            .ToList();

        if (distinctValues.Count == 0)
            return;

        // Obter filtros atuais para esta coluna
        var currentFilters = _columnFilters.ContainsKey(column.Name) 
            ? _columnFilters[column.Name] 
            : new List<string>();

        // Mostrar dialog de filtro
        using var filterDialog = new ColumnFilterDialog(column.HeaderText, distinctValues, currentFilters);
        if (filterDialog.ShowDialog() == DialogResult.OK)
        {
            if (filterDialog.SelectedValues.Count == 0 || filterDialog.SelectedValues.Count == filterDialog.AllValues.Count)
            {
                // Remove filtro se todos estão selecionados ou nenhum
                _columnFilters.Remove(column.Name);
            }
            else
            {
                // Salva filtros selecionados
                _columnFilters[column.Name] = filterDialog.SelectedValues;
            }

            // Aplica filtros (mas NÃO recarrega do Excel)
            ApplyFilters();
        }
    }

    private string GetPropertyValue(Congregacao congregacao, string propertyName)
    {
        var property = typeof(Congregacao).GetProperty(propertyName);
        if (property == null) return string.Empty;

        var value = property.GetValue(congregacao);
        return value?.ToString() ?? string.Empty;
    }

    private void ApplyFilters()
    {
        var filteredCongregacoes = _allCongregacoes.AsEnumerable();

        foreach (var filter in _columnFilters)
        {
            var columnName = filter.Key;
            var selectedValues = filter.Value;
            
            // Encontrar o DataPropertyName da coluna
            var column = dataGridView1.Columns[columnName];
            if (column == null) continue;

            var propertyName = column.DataPropertyName;
            
            filteredCongregacoes = filteredCongregacoes.Where(congregacao =>
            {
                var value = GetPropertyValue(congregacao, propertyName);
                return selectedValues.Contains(value);
            });
        }

        dataGridView1.DataSource = null;
        dataGridView1.DataSource = filteredCongregacoes.ToList();
    }

    private void LoadData()
    {
        // Recarregar dados do Excel
        _repository.ReloadFromExcel();
        
        // Calcular total de itens emprestados para cada congregação
        // Considerar apenas itens pendentes (não recebidos)
        foreach (var congregacao in _repository.Congregacoes)
        {
            congregacao.TotalItensEmprestados = _repository.Emprestimos
                .Where(e => e.CongregacaoId == congregacao.Id && e.Status == StatusEmprestimo.EmAndamento)
                .Sum(e => e.TotalPendente); // ✅ Usar TotalPendente em vez de TotalItens
        }

        _allCongregacoes = _repository.Congregacoes.ToList();
        _columnFilters.Clear();
        ApplyFilters();
    }

    private void BtnListar_Click(object sender, EventArgs e)
    {
        // Recarregar dados do Excel
        LoadData();
        MessageBox.Show(
            "Dados recarregados com sucesso!",
            "Listar",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void BtnCreate_Click(object sender, EventArgs e)
    {
        var form = new CongregacaoDetailForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadData();
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Congregacao item)
        {
            var form = new CongregacaoDetailForm(item);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _repository.UpdateCongregacao(item);
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um item para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Congregacao item)
        {
            // Verificar se há empréstimos para esta congregação (independente do status)
            var emprestimos = _repository.Emprestimos
                .Where(e => e.CongregacaoId == item.Id)
                .ToList();

            if (emprestimos.Any())
            {
                // Agrupar por status para exibir informações detalhadas
                var emprestimosInfo = emprestimos
                    .GroupBy(e => e.Status)
                    .Select(g => $"{g.Count()} empréstimo(s) {g.Key switch 
                    {
                        StatusEmprestimo.EmAndamento => "Em Andamento",
                        StatusEmprestimo.Devolvido => "Devolvido(s)",
                        _ => g.Key.ToString()
                    }}")
                    .ToList();

                MessageBox.Show(
                    $"Não é possível excluir a congregação '{item.Name}' porque ela possui empréstimos registrados:\n\n" +
                    string.Join("\n", emprestimosInfo) + "\n\n" +
                    "Para excluir esta congregação, primeiro exclua todos os empréstimos relacionados a ela.",
                    "Exclusão Não Permitida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Tem certeza que deseja excluir '{item.Name}'?", 
                "Confirmar Exclusão", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                _repository.Congregacoes.Remove(item);
                LoadData();
                
                MessageBox.Show(
                    "Congregação excluída com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um item para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnClonar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Congregacao itemOriginal)
        {
            // Criar nova congregação com dados clonados
            var novaCongregacao = new Congregacao
            {
                Name = itemOriginal.Name
            };

            // Abrir formulário em modo criação com dados clonados
            var form = new CongregacaoDetailForm(novaCongregacao, isCloning: true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione uma congregação para clonar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
