using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Reports;

namespace ControleEmprestimos.Forms;

public partial class EmprestimoListForm : UserControl
{
    private DataRepository _repository;
    private List<Emprestimo> _allEmprestimos = new();
    private Dictionary<string, List<string>> _columnFilters = new();

    public EmprestimoListForm()
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
            HeaderText = "Recebedor",
            Name = "colRecebedor",
            Width = 130
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "ItemName",
            HeaderText = "Bem",
            Name = "colItem",
            Width = 110
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "QuantityInStock",
            HeaderText = "Qtd",
            Name = "colQuantity",
            Width = 50
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "CongregacaoName",
            HeaderText = "Congregação",
            Name = "colCongregacao",
            Width = 120
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "DataEmprestimo",
            HeaderText = "Data",
            Name = "colData",
            Width = 90,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "StatusDescricao",
            HeaderText = "Status",
            Name = "colStatus",
            Width = 100
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Motivo",
            HeaderText = "Motivo",
            Name = "colMotivo",
            Width = 150,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
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
        var distinctValues = _allEmprestimos
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

            // Aplica filtros
            ApplyFilters();
        }
    }

    private string GetPropertyValue(Emprestimo emprestimo, string propertyName)
    {
        var property = typeof(Emprestimo).GetProperty(propertyName);
        if (property == null) return string.Empty;

        var value = property.GetValue(emprestimo);
        
        // Tratamento especial para DateTime
        if (value is DateTime dateValue)
        {
            return dateValue.ToString("dd/MM/yyyy");
        }
        
        return value?.ToString() ?? string.Empty;
    }

    private void ApplyFilters()
    {
        var filteredEmprestimos = _allEmprestimos.AsEnumerable();

        foreach (var filter in _columnFilters)
        {
            var columnName = filter.Key;
            var selectedValues = filter.Value;
            
            // Encontrar o DataPropertyName da coluna
            var column = dataGridView1.Columns[columnName];
            if (column == null) continue;

            var propertyName = column.DataPropertyName;
            
            filteredEmprestimos = filteredEmprestimos.Where(emprestimo =>
            {
                var value = GetPropertyValue(emprestimo, propertyName);
                return selectedValues.Contains(value);
            });
        }

        dataGridView1.DataSource = null;
        dataGridView1.DataSource = filteredEmprestimos.ToList();
    }

    private void LoadData()
    {
        _allEmprestimos = _repository.Emprestimos.ToList();
        _columnFilters.Clear();
        ApplyFilters();
    }

    private void BtnCreate_Click(object sender, EventArgs e)
    {
        var form = new EmprestimoDetailForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadData();
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Emprestimo item)
        {
            var form = new EmprestimoDetailForm(item);
            if (form.ShowDialog() == DialogResult.OK)
            {
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
        if (dataGridView1.CurrentRow?.DataBoundItem is Emprestimo item)
        {
            var result = MessageBox.Show(
                $"Tem certeza que deseja excluir o empréstimo para '{item.Name}'?\n\n" +
                $"ATENÇÃO: O estoque de '{item.ItemName}' será reposto ({item.QuantityInStock} unidades).",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Repor estoque antes de excluir (apenas se status Em Andamento)
                if (item.Status == StatusEmprestimo.EmAndamento && item.ItemId.HasValue)
                {
                    var itemEstoque = _repository.Items.FirstOrDefault(i => i.Id == item.ItemId.Value);
                    if (itemEstoque != null)
                    {
                        itemEstoque.QuantityInStock += item.QuantityInStock;
                    }
                }

                _repository.Emprestimos.Remove(item);
                LoadData();
                
                MessageBox.Show(
                    "Empréstimo excluído com sucesso!" +
                    (item.Status == StatusEmprestimo.EmAndamento ? "\nEstoque reposto." : ""),
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
        if (dataGridView1.CurrentRow?.DataBoundItem is Emprestimo itemOriginal)
        {
            // Criar novo empréstimo com dados clonados
            var novoEmprestimo = new Emprestimo
            {
                Name = itemOriginal.Name,
                Motivo = itemOriginal.Motivo,
                QuantityInStock = itemOriginal.QuantityInStock,
                ItemId = itemOriginal.ItemId,
                ItemName = itemOriginal.ItemName,
                CongregacaoId = itemOriginal.CongregacaoId,
                CongregacaoName = itemOriginal.CongregacaoName,
                DataEmprestimo = DateTime.Now,
                Status = StatusEmprestimo.EmAndamento
            };

            // Abrir formulário em modo criação com dados clonados
            var form = new EmprestimoDetailForm(novoEmprestimo, isCloning: true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um empréstimo para clonar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnImprimirRecibo_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Emprestimo emprestimo)
        {
            var printer = new ReciboEmprestimoPrinter(emprestimo);
            printer.PrintPreview();
        }
        else
        {
            MessageBox.Show("Por favor, selecione um empréstimo para imprimir o recibo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnReceberDeVolta_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Emprestimo emprestimo)
        {
            // Validar se o empréstimo está Em Andamento
            if (emprestimo.Status != StatusEmprestimo.EmAndamento)
            {
                MessageBox.Show(
                    $"Este empréstimo está com status '{emprestimo.StatusDescricao}' e não pode ser recebido.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Abrir tela de recebimento com empréstimo pré-selecionado
            var form = new RecebimentoDetailForm(emprestimo);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um empréstimo para receber de volta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
