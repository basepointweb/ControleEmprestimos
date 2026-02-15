using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class ItemListForm : UserControl
{
    private DataRepository _repository;
    private List<Item> _allItems = new();
    private Dictionary<string, List<string>> _columnFilters = new();

    public ItemListForm()
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
            Width = 200
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "QuantityInStock",
            HeaderText = "Total em Estoque",
            Name = "colQuantity",
            Width = 120
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "TotalEmprestado",
            HeaderText = "Total Emprestado",
            Name = "colEmprestado",
            Width = 120
        });

        // Event handler para clique no header
        dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;
        
        // Event handler para duplo clique (editar)
        dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        
        // Event handler para tecla Delete (excluir)
        dataGridView1.KeyDown += DataGridView1_KeyDown;
    }

    private void DataGridView1_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        // Ignorar clique no header
        if (e.RowIndex >= 0)
        {
            BtnEdit_Click(sender, EventArgs.Empty);
        }
    }

    private void DataGridView1_KeyDown(object? sender, KeyEventArgs e)
    {
        // Tecla Delete para excluir
        if (e.KeyCode == Keys.Delete)
        {
            BtnDelete_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
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
        var distinctValues = _allItems
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

    private string GetPropertyValue(Item item, string propertyName)
    {
        var property = typeof(Item).GetProperty(propertyName);
        if (property == null) return string.Empty;

        var value = property.GetValue(item);
        return value?.ToString() ?? string.Empty;
    }

    private void ApplyFilters()
    {
        var filteredItems = _allItems.AsEnumerable();

        foreach (var filter in _columnFilters)
        {
            var columnName = filter.Key;
            var selectedValues = filter.Value;
            
            // Encontrar o DataPropertyName da coluna
            var column = dataGridView1.Columns[columnName];
            if (column == null) continue;

            var propertyName = column.DataPropertyName;
            
            filteredItems = filteredItems.Where(item =>
            {
                var value = GetPropertyValue(item, propertyName);
                return selectedValues.Contains(value);
            });
        }

        dataGridView1.DataSource = null;
        dataGridView1.DataSource = filteredItems.ToList();
    }

    private void LoadData()
    {
        // Recarregar dados do Excel
        _repository.ReloadFromExcel();
        
        // Calcular total emprestado para cada item (apenas empréstimos Em Andamento)
        // Usar EmprestimoItens para calcular corretamente com múltiplos itens
        foreach (var item in _repository.Items)
        {
            item.TotalEmprestado = _repository.EmprestimoItens
                .Where(ei => ei.ItemId == item.Id)
                .Join(_repository.Emprestimos,
                    ei => ei.EmprestimoId,
                    e => e.Id,
                    (ei, e) => new { EmprestimoItem = ei, Emprestimo = e })
                .Where(x => x.Emprestimo.Status == StatusEmprestimo.EmAndamento)
                .Sum(x => x.EmprestimoItem.QuantidadePendente); // Soma apenas o que ainda está emprestado
        }

        _allItems = _repository.Items.ToList();
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
        var form = new ItemDetailForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadData();
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Item item)
        {
            var form = new ItemDetailForm(item);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _repository.UpdateItem(item);
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
        if (dataGridView1.CurrentRow?.DataBoundItem is Item item)
        {
            // Verificar se o bem possui empréstimos (independente do status)
            var emprestimosComItem = _repository.EmprestimoItens
                .Where(ei => ei.ItemId == item.Id)
                .ToList();

            if (emprestimosComItem.Any())
            {
                // Buscar informações dos empréstimos para exibição
                var emprestimosInfo = emprestimosComItem
                    .Select(ei => _repository.Emprestimos.FirstOrDefault(e => e.Id == ei.EmprestimoId))
                    .Where(e => e != null)
                    .GroupBy(e => e!.Status)
                    .Select(g => $"{g.Count()} empréstimo(s) {g.Key switch 
                    {
                        StatusEmprestimo.EmAndamento => "Em Andamento",
                        StatusEmprestimo.Devolvido => "Devolvido(s)",
                        _ => g.Key.ToString()
                    }}")
                    .ToList();

                MessageBox.Show(
                    $"Não é possível excluir o bem '{item.Name}' porque ele possui empréstimos registrados:\n\n" +
                    string.Join("\n", emprestimosInfo) + "\n\n" +
                    "Para excluir este bem, primeiro exclua todos os empréstimos relacionados a ele.",
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
                _repository.Items.Remove(item);
                LoadData();
                
                MessageBox.Show(
                    "Bem excluído com sucesso!",
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
        if (dataGridView1.CurrentRow?.DataBoundItem is Item itemOriginal)
        {
            // Criar novo item com dados clonados
            var novoItem = new Item
            {
                Name = itemOriginal.Name,
                QuantityInStock = itemOriginal.QuantityInStock
                // DataCriacao e DataAlteracao serão definidas automaticamente pelo repositório
            };

            // Abrir formulário em modo criação com dados clonados
            var form = new ItemDetailForm(novoItem, isCloning: true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um item para clonar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnEmprestar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Item item)
        {
            // Verificar se há estoque disponível
            if (item.QuantityInStock <= 0)
            {
                MessageBox.Show($"Não há estoque disponível de '{item.Name}'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Criar um novo empréstimo com o item selecionado
            var form = new EmprestimoDetailForm(item);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um item para emprestar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
