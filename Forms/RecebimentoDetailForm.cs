using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Reports;
using ControleEmprestimos.Helpers;

namespace ControleEmprestimos.Forms;

public partial class RecebimentoDetailForm : Form
{
    private RecebimentoEmprestimo? _item;
    private bool _isEditing;
    private DataRepository _repository;
    private Emprestimo? _emprestimoPreSelecionado;
    private List<ItemRecebimentoView> _itensParaReceber = new();
    
    // MaskedTextBox para data (criado programaticamente)
    private MaskedTextBox mtbDataRecebimento;

    // Classe auxiliar para o grid
    private class ItemRecebimentoView
    {
        public int EmprestimoItemId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int QuantidadeEmprestada { get; set; }
        public int QuantidadeRecebida { get; set; }
        public int QuantidadePendente { get; set; }
        public int QuantidadeAReceber { get; set; }
    }

    public RecebimentoDetailForm(RecebimentoEmprestimo? item = null)
    {
        InitializeComponent();
        
        // Criar e substituir o DateTimePicker por MaskedTextBox
        CreateDateMaskedTextBox();
        
        // Configurar controles para caixa alta
        FormControlHelper.ConfigureAllTextBoxesToUpperCase(this);
        
        _repository = DataRepository.Instance;
        _item = item;
        _isEditing = item != null;

        // Carregar empréstimos ANTES de configurar o grid e carregar dados
        LoadEmprestimos();
        ConfigureDataGridView();

        if (_isEditing && _item != null)
        {
            // Modo visualização para recebimentos já salvos
            if (_item.EmprestimoId.HasValue)
            {
                var emprestimo = _repository.Emprestimos.FirstOrDefault(e => e.Id == _item.EmprestimoId.Value);
                if (emprestimo != null)
                {
                    txtDataEmprestimo.Text = emprestimo.DataEmprestimo.ToString("dd/MM/yyyy");
                    txtQuemPegou.Text = emprestimo.Name;
                    
                    // Carregar itens recebidos
                    _itensParaReceber = _item.ItensRecebidos.Select(ir =>
                    {
                        var emprestimoItem = _repository.EmprestimoItens.FirstOrDefault(ei => ei.Id == ir.EmprestimoItemId);
                        return new ItemRecebimentoView
                        {
                            EmprestimoItemId = ir.EmprestimoItemId,
                            ItemId = ir.ItemId,
                            ItemName = ir.ItemName,
                            QuantidadeEmprestada = emprestimoItem?.Quantidade ?? 0,
                            QuantidadeRecebida = emprestimoItem?.QuantidadeRecebida ?? 0,
                            QuantidadePendente = 0,
                            QuantidadeAReceber = ir.QuantidadeRecebida
                        };
                    }).ToList();
                    
                    RefreshItensGrid();
                    
                    // Selecionar o empréstimo no combo usando reflexão para acessar tipo anônimo
                    if (cmbEmprestimo.DataSource != null)
                    {
                        var dataSource = cmbEmprestimo.DataSource as System.Collections.IEnumerable;
                        if (dataSource != null)
                        {
                            foreach (var comboItem in dataSource)
                            {
                                var emprestimoProperty = comboItem.GetType().GetProperty("Emprestimo");
                                if (emprestimoProperty != null)
                                {
                                    var emp = emprestimoProperty.GetValue(comboItem) as Emprestimo;
                                    if (emp != null && emp.Id == emprestimo.Id)
                                    {
                                        // Temporariamente remover o event handler para não disparar SelectedIndexChanged
                                        cmbEmprestimo.SelectedIndexChanged -= CmbEmprestimo_SelectedIndexChanged;
                                        cmbEmprestimo.SelectedItem = comboItem;
                                        cmbEmprestimo.SelectedIndexChanged += CmbEmprestimo_SelectedIndexChanged;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            mtbDataRecebimento.Text = _item.DataRecebimento.ToString("dd/MM/yyyy");
            txtQuemRecebeu.Text = _item.NomeQuemRecebeu;
            
            // Desabilitar edição
            cmbEmprestimo.Enabled = false;
            mtbDataRecebimento.Enabled = false;
            txtQuemRecebeu.ReadOnly = true;
            dgvItensReceber.ReadOnly = true;
            if (dgvItensReceber.Columns.Contains("colQuantidadeAReceber"))
            {
                dgvItensReceber.Columns["colQuantidadeAReceber"].ReadOnly = true;
            }
            btnSave.Visible = false;
            
            // Mostrar botão de impressão
            btnImprimirRecibo.Visible = true;
        }
        else
        {
            // Novo recebimento - data atual
            mtbDataRecebimento.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    // Construtor para recebimento com empréstimo pré-selecionado
    public RecebimentoDetailForm(Emprestimo emprestimoPreSelecionado) : this()
    {
        _emprestimoPreSelecionado = emprestimoPreSelecionado;
        
        if (_emprestimoPreSelecionado != null)
        {
            // Carregar itens disponíveis para receber ANTES de selecionar no combo
            LoadItensDoEmprestimo(_emprestimoPreSelecionado);
            
            // Preencher campos
            txtDataEmprestimo.Text = _emprestimoPreSelecionado.DataEmprestimo.ToString("dd/MM/yyyy");
            txtQuemPegou.Text = _emprestimoPreSelecionado.Name;
            mtbDataRecebimento.Text = DateTime.Now.ToString("dd/MM/yyyy");
            
            // Selecionar no ComboBox usando reflexão (igual ao modo edição)
            if (cmbEmprestimo.DataSource != null)
            {
                var dataSource = cmbEmprestimo.DataSource as System.Collections.IEnumerable;
                if (dataSource != null)
                {
                    foreach (var comboItem in dataSource)
                    {
                        var emprestimoProperty = comboItem.GetType().GetProperty("Emprestimo");
                        if (emprestimoProperty != null)
                        {
                            var emp = emprestimoProperty.GetValue(comboItem) as Emprestimo;
                            if (emp != null && emp.Id == _emprestimoPreSelecionado.Id)
                            {
                                // Temporariamente remover o event handler para não disparar SelectedIndexChanged
                                cmbEmprestimo.SelectedIndexChanged -= CmbEmprestimo_SelectedIndexChanged;
                                cmbEmprestimo.SelectedItem = comboItem;
                                cmbEmprestimo.SelectedIndexChanged += CmbEmprestimo_SelectedIndexChanged;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private void CreateDateMaskedTextBox()
    {
        // Remover DateTimePicker se existir
        var dtpToRemove = this.Controls.Find("dtpDataRecebimento", true).FirstOrDefault();
        if (dtpToRemove != null)
        {
            var location = dtpToRemove.Location;
            var size = dtpToRemove.Size;
            var tabIndex = dtpToRemove.TabIndex;
            
            this.Controls.Remove(dtpToRemove);
            dtpToRemove.Dispose();
            
            // Criar MaskedTextBox
            mtbDataRecebimento = FormControlHelper.CreateDateMaskedTextBox();
            mtbDataRecebimento.Name = "mtbDataRecebimento";
            mtbDataRecebimento.Location = location;
            mtbDataRecebimento.Size = size;
            mtbDataRecebimento.TabIndex = tabIndex;
            
            this.Controls.Add(mtbDataRecebimento);
        }
        else
        {
            // Se não encontrou o DTP, criar MaskedTextBox direto
            mtbDataRecebimento = FormControlHelper.CreateDateMaskedTextBox();
            mtbDataRecebimento.Name = "mtbDataRecebimento";
            mtbDataRecebimento.Location = new Point(20, 160);
            mtbDataRecebimento.Size = new Size(150, 23);
            mtbDataRecebimento.TabIndex = 7;
            
            this.Controls.Add(mtbDataRecebimento);
        }
    }

    private void ConfigureDataGridView()
    {
        dgvItensReceber.AutoGenerateColumns = false;
        dgvItensReceber.Columns.Clear();

        dgvItensReceber.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "ItemName",
            HeaderText = "Bem",
            Name = "colItemName",
            Width = 200,
            ReadOnly = true
        });

        dgvItensReceber.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "QuantidadeEmprestada",
            HeaderText = "Emprestada",
            Name = "colEmprestada",
            Width = 90,
            ReadOnly = true
        });

        dgvItensReceber.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "QuantidadeRecebida",
            HeaderText = "Recebida",
            Name = "colRecebida",
            Width = 80,
            ReadOnly = true
        });

        dgvItensReceber.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "QuantidadePendente",
            HeaderText = "Pendente",
            Name = "colPendente",
            Width = 80,
            ReadOnly = true
        });

        dgvItensReceber.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "QuantidadeAReceber",
            HeaderText = "A Receber",
            Name = "colQuantidadeAReceber",
            Width = 90
        });

        dgvItensReceber.CellEndEdit += DgvItensReceber_CellEndEdit;
        dgvItensReceber.CellValidating += DgvItensReceber_CellValidating;
        dgvItensReceber.DataError += DgvItensReceber_DataError;
    }

    private void DgvItensReceber_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        // Prevenir erro ao digitar valor inválido
        if (dgvItensReceber.Columns[e.ColumnIndex].Name == "colQuantidadeAReceber")
        {
            MessageBox.Show(
                "Por favor, digite um valor numérico válido.",
                "Valor Inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            e.ThrowException = false;
            e.Cancel = true;
        }
    }

    private void DgvItensReceber_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (dgvItensReceber.Columns[e.ColumnIndex].Name == "colQuantidadeAReceber")
        {
            var item = dgvItensReceber.Rows[e.RowIndex].DataBoundItem as ItemRecebimentoView;
            if (item != null)
            {
                // Tentar converter o valor
                if (int.TryParse(e.FormattedValue?.ToString(), out int quantidade))
                {
                    if (quantidade < 0)
                    {
                        MessageBox.Show(
                            "A quantidade não pode ser negativa.",
                            "Validação",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                    else if (quantidade > item.QuantidadePendente)
                    {
                        MessageBox.Show(
                            $"Quantidade não pode ser maior que a pendente ({item.QuantidadePendente}).",
                            "Validação",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(e.FormattedValue?.ToString()))
                {
                    MessageBox.Show(
                        "Por favor, digite um valor numérico válido.",
                        "Valor Inválido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }
    }

    private void DgvItensReceber_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (dgvItensReceber.Columns[e.ColumnIndex].Name == "colQuantidadeAReceber")
        {
            var item = dgvItensReceber.Rows[e.RowIndex].DataBoundItem as ItemRecebimentoView;
            if (item != null)
            {
                // Validar quantidade
                if (item.QuantidadeAReceber < 0)
                {
                    item.QuantidadeAReceber = 0;
                    dgvItensReceber.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                }
                else if (item.QuantidadeAReceber > item.QuantidadePendente)
                {
                    MessageBox.Show(
                        $"Quantidade a receber não pode ser maior que a pendente ({item.QuantidadePendente}).",
                        "Validação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    item.QuantidadeAReceber = item.QuantidadePendente;
                    dgvItensReceber.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = item.QuantidadePendente;
                }
                
                // Atualizar total sem recriar o datasource
                UpdateTotalAReceber();
            }
        }
    }

    private void UpdateTotalAReceber()
    {
        var totalAReceber = _itensParaReceber.Sum(i => i.QuantidadeAReceber);
        lblTotalRecebido.Text = $"Total a Devolver: {totalAReceber} itens";
    }

    private void LoadEmprestimos()
    {
        // Carregar apenas empréstimos com status "Em Andamento"
        var emprestimosEmAndamento = _repository.Emprestimos
            .Where(e => e.Status == StatusEmprestimo.EmAndamento)
            .Select(e => new
            {
                Emprestimo = e,
                DisplayText = $"{e.CongregacaoName} - {e.Name} - Total Itens: {e.TotalItens} - Pendente: {e.TotalPendente}"
            })
            .ToList();

        // Se estiver editando, incluir o empréstimo do recebimento mesmo que não esteja "Em Andamento"
        if (_isEditing && _item?.EmprestimoId.HasValue == true)
        {
            var emprestimoDoRecebimento = _repository.Emprestimos
                .FirstOrDefault(e => e.Id == _item.EmprestimoId.Value);
            
            if (emprestimoDoRecebimento != null)
            {
                // Verificar se já não está na lista
                var jaExiste = emprestimosEmAndamento.Any(x => 
                {
                    var emp = x.Emprestimo as Emprestimo;
                    return emp != null && emp.Id == emprestimoDoRecebimento.Id;
                });
                
                if (!jaExiste)
                {
                    // Adicionar no início da lista
                    emprestimosEmAndamento.Insert(0, new
                    {
                        Emprestimo = emprestimoDoRecebimento,
                        DisplayText = $"{emprestimoDoRecebimento.CongregacaoName} - {emprestimoDoRecebimento.Name} - {emprestimoDoRecebimento.StatusDescricao}"
                    });
                }
            }
        }

        cmbEmprestimo.DataSource = emprestimosEmAndamento;
        cmbEmprestimo.DisplayMember = "DisplayText";
        cmbEmprestimo.ValueMember = "Emprestimo";
        cmbEmprestimo.SelectedIndex = -1;
    }

    private void CmbEmprestimo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbEmprestimo.SelectedItem != null)
        {
            var selectedItem = cmbEmprestimo.SelectedItem;
            var emprestimoProperty = selectedItem.GetType().GetProperty("Emprestimo");
            if (emprestimoProperty != null)
            {
                var emprestimo = emprestimoProperty.GetValue(selectedItem) as Emprestimo;
                if (emprestimo != null)
                {
                    txtDataEmprestimo.Text = emprestimo.DataEmprestimo.ToString("dd/MM/yyyy");
                    txtQuemPegou.Text = emprestimo.Name;
                    LoadItensDoEmprestimo(emprestimo);
                    return;
                }
            }
        }
        
        txtDataEmprestimo.Clear();
        txtQuemPegou.Clear();
        _itensParaReceber.Clear();
        RefreshItensGrid();
    }

    private void LoadItensDoEmprestimo(Emprestimo emprestimo)
    {
        _itensParaReceber = _repository.EmprestimoItens
            .Where(ei => ei.EmprestimoId == emprestimo.Id && ei.QuantidadePendente > 0)
            .Select(ei => new ItemRecebimentoView
            {
                EmprestimoItemId = ei.Id,
                ItemId = ei.ItemId,
                ItemName = ei.ItemName,
                QuantidadeEmprestada = ei.Quantidade,
                QuantidadeRecebida = ei.QuantidadeRecebida,
                QuantidadePendente = ei.QuantidadePendente,
                QuantidadeAReceber = ei.QuantidadePendente // Por padrão, receber tudo pendente
            })
            .ToList();
        
        RefreshItensGrid();
    }

    private void RefreshItensGrid()
    {
        // Suspender layout para evitar flickering
        dgvItensReceber.SuspendLayout();
        
        try
        {
            dgvItensReceber.DataSource = null;
            dgvItensReceber.DataSource = _itensParaReceber;
            
            UpdateTotalAReceber();
        }
        finally
        {
            dgvItensReceber.ResumeLayout();
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (cmbEmprestimo.SelectedItem == null && _emprestimoPreSelecionado == null)
        {
            MessageBox.Show("Por favor, selecione um empréstimo.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtQuemRecebeu.Text))
        {
            MessageBox.Show("Por favor, informe quem recebeu de volta.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Validar data do recebimento
        if (!DateTime.TryParse(mtbDataRecebimento.Text, out DateTime dataRecebimento))
        {
            MessageBox.Show("Por favor, informe uma data de devolução válida.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            mtbDataRecebimento.Focus();
            return;
        }

        var itensComQuantidade = _itensParaReceber.Where(i => i.QuantidadeAReceber > 0).ToList();
        if (!itensComQuantidade.Any())
        {
            MessageBox.Show("Por favor, informe a quantidade a devolver de pelo menos um item.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // CORREÇÃO: Obter empréstimo selecionado no combo (se houver), caso contrário usar o pré-selecionado
        Emprestimo? emprestimoSelecionado = null;
        
        if (cmbEmprestimo.SelectedItem != null)
        {
            // Usuário selecionou algo no combo (pode ter trocado)
            var selectedItem = cmbEmprestimo.SelectedItem;
            var emprestimoProperty = selectedItem.GetType().GetProperty("Emprestimo");
            emprestimoSelecionado = emprestimoProperty?.GetValue(selectedItem) as Emprestimo;
        }
        else if (_emprestimoPreSelecionado != null)
        {
            // Usa o pré-selecionado apenas se não houver seleção no combo
            emprestimoSelecionado = _emprestimoPreSelecionado;
        }

        if (emprestimoSelecionado == null)
        {
            MessageBox.Show("Erro ao obter o empréstimo selecionado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Criar novo recebimento
        var itemNames = string.Join(", ", itensComQuantidade.Select(i => i.ItemName).Distinct());
        var newItem = new RecebimentoEmprestimo
        {
            Name = $"RECEBIMENTO - {itemNames}".ToUpper(),
            NomeRecebedor = emprestimoSelecionado.Name.ToUpper(),
            NomeQuemRecebeu = txtQuemRecebeu.Text.Trim().ToUpper(),
            EmprestimoId = emprestimoSelecionado.Id,
            DataEmprestimo = emprestimoSelecionado.DataEmprestimo,
            DataRecebimento = dataRecebimento,
            RecebimentoParcial = itensComQuantidade.Sum(i => i.QuantidadeAReceber) < emprestimoSelecionado.TotalPendente,
            ItensRecebidos = itensComQuantidade.Select(i => new RecebimentoItem
            {
                EmprestimoItemId = i.EmprestimoItemId,
                ItemId = i.ItemId,
                ItemName = i.ItemName.ToUpper(),
                QuantidadeRecebida = i.QuantidadeAReceber
            }).ToList()
        };
        
        _repository.AddRecebimento(newItem);

        // Atualizar status do empréstimo CORRETO (o que foi efetivamente selecionado)
        _repository.DevolverEmprestimo(emprestimoSelecionado);

        // Perguntar se deseja imprimir recibo
        var resultado = MessageBox.Show(
            $"Devolução registrada com sucesso!\n\n" +
            $"Status do empréstimo: {(emprestimoSelecionado.TodosItensRecebidos ? "Devolvido (completo)" : "Em Andamento (parcial)")}\n\n" +
            $"Deseja imprimir o recibo?",
            "Sucesso",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (resultado == DialogResult.Yes)
        {
            var printer = new ReciboRecebimentoPrinter(newItem, emprestimoSelecionado);
            printer.PrintPreview();
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void BtnImprimirRecibo_Click(object sender, EventArgs e)
    {
        if (_item == null) return;

        var emprestimo = _item.EmprestimoId.HasValue 
            ? _repository.Emprestimos.FirstOrDefault(e => e.Id == _item.EmprestimoId.Value)
            : null;

        var printer = new ReciboRecebimentoPrinter(_item, emprestimo);
        printer.PrintPreview();
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
