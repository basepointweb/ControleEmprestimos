using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Reports;
using ControleEmprestimos.Helpers;

namespace ControleEmprestimos.Forms;

public partial class EmprestimoListForm : UserControl
{
    private DataRepository _repository;
    private List<Emprestimo> _allEmprestimos = new();
    private Dictionary<string, List<string>> _columnFilters = new();
    private DateTime _dataInicialFiltro;
    private DateTime _dataFinalFiltro;

    public EmprestimoListForm()
    {
        InitializeComponent();
        
        // Substituir DateTimePickers por MaskedTextBox
        FormControlHelper.ReplaceAllDateTimePickersWithMaskedTextBox(this);
        
        _repository = DataRepository.Instance;
        
        // Inicializar filtro de data com o mês atual
        var hoje = DateTime.Now;
        _dataInicialFiltro = new DateTime(hoje.Year, hoje.Month, 1); // Primeiro dia do mês
        _dataFinalFiltro = new DateTime(hoje.Year, hoje.Month, DateTime.DaysInMonth(hoje.Year, hoje.Month)); // Último dia do mês
        
        var mtbDataInicial = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataInicial");
        var mtbDataFinal = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataFinal");
        
        if (mtbDataInicial != null)
        {
            mtbDataInicial.Text = _dataInicialFiltro.ToString("dd/MM/yyyy");
        }
        
        if (mtbDataFinal != null)
        {
            mtbDataFinal.Text = _dataFinalFiltro.ToString("dd/MM/yyyy");
        }
        
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

        // Coluna de Bens (concatenados) - não tem DataPropertyName pois será preenchida manualmente
        var colBem = new DataGridViewTextBoxColumn
        {
            HeaderText = "Bem",
            Name = "colItem",
            Width = 200
        };
        dataGridView1.Columns.Add(colBem);

        // Coluna de Quantidade (total de itens) - não tem DataPropertyName pois será preenchida manualmente
        var colQtd = new DataGridViewTextBoxColumn
        {
            HeaderText = "Qtd",
            Name = "colQuantity",
            Width = 50
        };
        dataGridView1.Columns.Add(colQtd);

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
        
        // Event handler para preencher colunas calculadas
        dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;
        
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

    private void DataGridView1_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        // Preencher colunas calculadas após binding
        for (int i = 0; i < dataGridView1.Rows.Count; i++)
        {
            if (dataGridView1.Rows[i].DataBoundItem is Emprestimo emprestimo)
            {
                // Concatenar nomes dos bens
                string bens;
                if (emprestimo.Itens != null && emprestimo.Itens.Any())
                {
                    bens = string.Join(", ", emprestimo.Itens.Select(ei => ei.ItemName).Distinct());
                }
                else
                {
                    // Compatibilidade com dados antigos
                    bens = emprestimo.ItemName;
                }
                dataGridView1.Rows[i].Cells["colItem"].Value = bens;
                
                // Total de itens
                int totalItens;
                if (emprestimo.Itens != null && emprestimo.Itens.Any())
                {
                    totalItens = emprestimo.TotalItens;
                }
                else
                {
                    // Compatibilidade com dados antigos
                    totalItens = emprestimo.QuantityInStock;
                }
                dataGridView1.Rows[i].Cells["colQuantity"].Value = totalItens;
            }
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

        // Aplicar filtro de data (por DataEmprestimo, ignorando hora)
        filteredEmprestimos = filteredEmprestimos.Where(e => 
            e.DataEmprestimo.Date >= _dataInicialFiltro.Date && 
            e.DataEmprestimo.Date <= _dataFinalFiltro.Date);

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
        // Recarregar dados do Excel
        _repository.ReloadFromExcel();
        
        _allEmprestimos = _repository.Emprestimos.ToList();
        _columnFilters.Clear();
        ApplyFilters();
    }

    private void BtnFiltrar_Click(object sender, EventArgs e)
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

        // Validar datas
        if (dataInicial.Date > dataFinal.Date)
        {
            MessageBox.Show(
                "A data inicial não pode ser maior que a data final.",
                "Validação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        // Atualizar filtros
        _dataInicialFiltro = dataInicial.Date;
        _dataFinalFiltro = dataFinal.Date;

        // Recarregar dados do Excel antes de aplicar filtros
        LoadData();
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
        if (dataGridView1.CurrentRow?.DataBoundItem is Emprestimo emprestimo)
        {
            // Verificar se há recebimentos para este empréstimo
            var recebimentos = _repository.RecebimentoEmprestimos
                .Where(r => r.EmprestimoId == emprestimo.Id)
                .ToList();

            if (recebimentos.Any())
            {
                MessageBox.Show(
                    $"Não é possível excluir este empréstimo porque já possui {recebimentos.Count} recebimento(s) registrado(s).\n\n" +
                    $"Para excluir este empréstimo, primeiro exclua todos os recebimentos relacionados.",
                    "Exclusão Não Permitida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Construir mensagem detalhada com itens a repor
            string mensagemItens;
            int totalARepor = 0;
            
            if (emprestimo.Itens != null && emprestimo.Itens.Any())
            {
                // Múltiplos itens - calcular quantidades pendentes
                var itensComPendencia = emprestimo.Itens
                    .Where(ei => ei.QuantidadePendente > 0)
                    .ToList();
                
                if (itensComPendencia.Any())
                {
                    var listaItens = itensComPendencia
                        .Select(ei => $"  • {ei.ItemName}: {ei.QuantidadePendente} unidade(s)")
                        .ToList();
                    
                    mensagemItens = "Itens a serem repostos no estoque:\n" + 
                                   string.Join("\n", listaItens);
                    totalARepor = itensComPendencia.Sum(ei => ei.QuantidadePendente);
                }
                else
                {
                    mensagemItens = "Todos os itens já foram recebidos de volta.";
                }
            }
            else
            {
                // Compatibilidade com dados antigos (item único)
                if (emprestimo.Status == StatusEmprestimo.EmAndamento)
                {
                    mensagemItens = $"Item a ser reposto no estoque:\n  • {emprestimo.ItemName}: {emprestimo.QuantityInStock} unidade(s)";
                    totalARepor = emprestimo.QuantityInStock;
                }
                else
                {
                    mensagemItens = "Item já foi devolvido.";
                }
            }

            var statusInfo = emprestimo.Status == StatusEmprestimo.EmAndamento
                ? $"\n\nStatus: Em Andamento\nTotal a repor: {totalARepor} unidade(s)"
                : $"\n\nStatus: {emprestimo.StatusDescricao}";

            var result = MessageBox.Show(
                $"Tem certeza que deseja excluir o empréstimo para '{emprestimo.Name}'?\n\n" +
                mensagemItens +
                statusInfo,
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Usar método correto do repository que já trata tudo
                _repository.RemoverEmprestimo(emprestimo);
                
                LoadData();
                
                MessageBox.Show(
                    "Empréstimo excluído com sucesso!" +
                    (totalARepor > 0 ? $"\nEstoque reposto: {totalARepor} unidade(s)" : ""),
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
            // Carregar os itens do empréstimo original
            var itensOriginais = _repository.EmprestimoItens
                .Where(ei => ei.EmprestimoId == itemOriginal.Id)
                .ToList();

            // Criar cópia dos itens para o novo empréstimo
            var itensClonados = itensOriginais.Select(ei => new EmprestimoItem
            {
                ItemId = ei.ItemId,
                ItemName = ei.ItemName,
                Quantidade = ei.Quantidade,
                QuantidadeRecebida = 0 // Resetar recebimentos
            }).ToList();

            // Criar novo empréstimo com dados clonados
            var novoEmprestimo = new Emprestimo
            {
                Name = itemOriginal.Name,
                Motivo = itemOriginal.Motivo,
                CongregacaoId = itemOriginal.CongregacaoId,
                CongregacaoName = itemOriginal.CongregacaoName,
                DataEmprestimo = DateTime.Now,
                Status = StatusEmprestimo.EmAndamento,
                Itens = itensClonados // Atribuir itens clonados
            };

            // Abrir formulário em modo clonagem com dados clonados
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

    private void BtnRelatorio_Click(object sender, EventArgs e)
    {
        var form = new RelatorioEmprestimosFilterForm();
        form.ShowDialog();
    }
}
