using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Reports;
using ControleEmprestimos.Helpers;
using System.ComponentModel;

namespace ControleEmprestimos.Forms;

public partial class EmprestimoDetailForm : Form
{
    private Emprestimo? _item;
    private bool _isEditing;
    private bool _isCloning;
    private DataRepository _repository;
    private Item? _itemPreSelecionado;
    private List<EmprestimoItem> _itensEmprestimo = new();
    private bool _validationFailed = false; // Flag para controlar se a validação falhou

    public EmprestimoDetailForm(Emprestimo? item = null, bool isCloning = false)
    {
        InitializeComponent();
        
        // Substituir todos DateTimePickers por MaskedTextBox
        FormControlHelper.ReplaceAllDateTimePickersWithMaskedTextBox(this);
        
        // Configurar controles para caixa alta
        FormControlHelper.ConfigureAllTextBoxesToUpperCase(this);
        
        // Configurar evento KeyPress do txtItemId
        ConfigureItemIdTextBox();
        
        _repository = DataRepository.Instance;
        _item = item;
        _isEditing = item != null && !isCloning;
        _isCloning = isCloning;

        LoadItems();
        LoadCongregacoes();
        ConfigureDataGridView();

        if (_item != null)
        {
            txtRecebedor.Text = _item.Name;
            txtMotivo.Text = _item.Motivo;
            
            // Buscar MaskedTextBox de data
            var mtbDataEmprestimo = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataEmprestimo");
            if (mtbDataEmprestimo != null)
            {
                mtbDataEmprestimo.Text = _item.DataEmprestimo.ToString("dd/MM/yyyy");
            }
            
            txtQuemLiberou.Text = _item.QuemLiberou;
            txtStatus.Text = _item.StatusDescricao;
            
            if (_item.CongregacaoId.HasValue)
            {
                var congregacao = _repository.Congregacoes.FirstOrDefault(c => c.Id == _item.CongregacaoId.Value);
                if (congregacao != null)
                {
                    cmbCongregacao.SelectedItem = congregacao;
                }
            }

            // Carregar itens do empréstimo
            if (_item.Itens != null && _item.Itens.Any())
            {
                _itensEmprestimo = _item.Itens.Select(ei => new EmprestimoItem
                {
                    ItemId = ei.ItemId,
                    ItemName = ei.ItemName,
                    Quantidade = ei.Quantidade,
                    QuantidadeRecebida = _isCloning ? 0 : ei.QuantidadeRecebida
                }).ToList();
            }
            else if (_item.ItemId.HasValue && _item.QuantityInStock > 0)
            {
                // Compatibilidade com dados antigos
                _itensEmprestimo.Add(new EmprestimoItem
                {
                    ItemId = _item.ItemId.Value,
                    ItemName = _item.ItemName,
                    Quantidade = _item.QuantityInStock,
                    QuantidadeRecebida = _isCloning ? 0 : 0
                });
            }

            RefreshItensGrid();

            if (_isEditing)
            {
                // Modo visualização - SEMPRE SOMENTE LEITURA
                lblStatus.Visible = true;
                txtStatus.Visible = true;
                
                // Tornar todos os campos somente leitura
                txtRecebedor.ReadOnly = true;
                txtMotivo.ReadOnly = true;
                txtQuemLiberou.ReadOnly = true;
                cmbCongregacao.Enabled = false;
                if (mtbDataEmprestimo != null) mtbDataEmprestimo.Enabled = false;
                
                // Ocultar completamente os controles de adição de itens
                lblItem.Visible = false;
                txtItemId.Visible = false;
                lblBem.Visible = false;
                cmbItem.Visible = false;
                lblQuantity.Visible = false;
                numQuantity.Visible = false;
                btnAdicionarItem.Visible = false;
                
                // Tornar coluna de quantidade ReadOnly em modo edição
                if (dgvItens.Columns["colQuantidade"] != null)
                {
                    dgvItens.Columns["colQuantidade"].ReadOnly = true;
                }
                
                // Ocultar botão remover do grid
                if (dgvItens.Columns["colRemover"] != null)
                {
                    dgvItens.Columns["colRemover"].Visible = false;
                }
                
                // Ocultar botão salvar - somente visualização
                btnSave.Visible = false;
                
                // Alterar texto do botão fechar para "Voltar"
                btnFechar.Text = "Voltar";
                
                // Alterar título do formulário
                this.Text = "Visualizar Empréstimo";
            }
            else if (_isCloning)
            {
                // Modo clonagem - atualizar data e status
                if (mtbDataEmprestimo != null)
                {
                    mtbDataEmprestimo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                txtStatus.Text = "Em Andamento";
                this.Text = "Clonar Empréstimo";
            }
        }
        else
        {
            // Para novo empréstimo, definir a data atual
            var mtbDataEmprestimo = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataEmprestimo");
            if (mtbDataEmprestimo != null)
            {
                mtbDataEmprestimo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            txtStatus.Text = "Em Andamento";
        }
    }

    // Construtor para empréstimo com item pré-selecionado
    public EmprestimoDetailForm(Item itemPreSelecionado) : this()
    {
        _itemPreSelecionado = itemPreSelecionado;
        
        if (_itemPreSelecionado != null)
        {
            // Adicionar item à lista com quantidade 1
            _itensEmprestimo.Add(new EmprestimoItem
            {
                ItemId = _itemPreSelecionado.Id,
                ItemName = _itemPreSelecionado.Name,
                Quantidade = 1,
                QuantidadeRecebida = 0
            });
            
            RefreshItensGrid();
            
            var mtbDataEmprestimo = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataEmprestimo");
            if (mtbDataEmprestimo != null)
            {
                mtbDataEmprestimo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            txtStatus.Text = "Em Andamento";
        }
    }

    // Construtor para empréstimo com múltiplos itens pré-selecionados
    public EmprestimoDetailForm(List<Item> itensPreSelecionados) : this()
    {
        if (itensPreSelecionados != null && itensPreSelecionados.Any())
        {
            // Adicionar todos os itens à lista com quantidade 1
            foreach (var item in itensPreSelecionados)
            {
                _itensEmprestimo.Add(new EmprestimoItem
                {
                    ItemId = item.Id,
                    ItemName = item.Name,
                    Quantidade = 1,
                    QuantidadeRecebida = 0
                });
            }
            
            RefreshItensGrid();
            
            var mtbDataEmprestimo = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataEmprestimo");
            if (mtbDataEmprestimo != null)
            {
                mtbDataEmprestimo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            txtStatus.Text = "Em Andamento";
        }
    }

    private void ConfigureDataGridView()
    {
        dgvItens.AutoGenerateColumns = false;
        dgvItens.Columns.Clear();
        
        // Configurar para permitir edição
        dgvItens.EditMode = DataGridViewEditMode.EditOnEnter;
        dgvItens.AllowUserToAddRows = false;
        dgvItens.AllowUserToDeleteRows = false;
        
        // Configurar tecla Enter para confirmar edição e ir para próxima linha
        dgvItens.KeyDown += DgvItens_KeyDown;

        dgvItens.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "ItemName",
            HeaderText = "Bem",
            Name = "colItem",
            Width = 300,
            ReadOnly = true
        });

        dgvItens.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Quantidade",
            HeaderText = "Quantidade",
            Name = "colQuantidade",
            Width = 100,
            ReadOnly = false // Permitir edição
        });

        if (_isEditing)
        {
            dgvItens.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "QuantidadeRecebida",
                HeaderText = "Recebida",
                Name = "colRecebida",
                Width = 80,
                ReadOnly = true
            });

            dgvItens.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "QuantidadePendente",
                HeaderText = "Pendente",
                Name = "colPendente",
                Width = 80,
                ReadOnly = true
            });
        }

        var btnColumn = new DataGridViewButtonColumn
        {
            HeaderText = "",
            Name = "colRemover",
            Text = "Remover",
            UseColumnTextForButtonValue = true,
            Width = 80
        };
        dgvItens.Columns.Add(btnColumn);

        dgvItens.CellClick += DgvItens_CellClick;
        dgvItens.CellValidating += DgvItens_CellValidating;
        dgvItens.CellValidated += DgvItens_CellValidated;
        dgvItens.CellEndEdit += DgvItens_CellEndEdit;
    }

    private void DgvItens_KeyDown(object? sender, KeyEventArgs e)
    {
        // Permitir que Enter confirme a edição e mova para próxima linha
        if (e.KeyCode == Keys.Enter && dgvItens.IsCurrentCellInEditMode)
        {
            // Resetar flag antes de tentar validar
            _validationFailed = false;
            
            // Evitar o comportamento padrão do Enter
            e.Handled = true;
            e.SuppressKeyPress = true;
            
            // Tentar finalizar a edição (isso dispara CellValidating)
            bool editSuccessful = dgvItens.EndEdit();
            
            // Se EndEdit() retornou false, a validação falhou
            if (!editSuccessful)
            {
                _validationFailed = true;
            }
            
            // Usar BeginInvoke para garantir que todas as validações terminaram
            BeginInvoke(new Action(() =>
            {
                // Apenas mover para próxima linha se a validação passou
                if (!_validationFailed && dgvItens.CurrentCell != null)
                {
                    int currentRow = dgvItens.CurrentCell.RowIndex;
                    int currentCol = dgvItens.CurrentCell.ColumnIndex;
                    
                    // Ir para a mesma coluna na próxima linha
                    if (currentRow + 1 < dgvItens.Rows.Count)
                    {
                        dgvItens.CurrentCell = dgvItens.Rows[currentRow + 1].Cells[currentCol];
                        
                        // Se a próxima célula for editável, entrar em modo de edição
                        if (!dgvItens.Rows[currentRow + 1].Cells[currentCol].ReadOnly)
                        {
                            dgvItens.BeginEdit(true);
                        }
                    }
                }
                else if (_validationFailed && dgvItens.CurrentCell != null)
                {
                    // Se a validação falhou, voltar para modo de edição na mesma célula
                    if (!dgvItens.CurrentCell.ReadOnly)
                    {
                        dgvItens.BeginEdit(true);
                    }
                }
            }));
        }
    }

    private void DgvItens_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        // Validar apenas a coluna de quantidade
        if (dgvItens.Columns[e.ColumnIndex].Name == "colQuantidade")
        {
            var newValue = e.FormattedValue.ToString();
            
            // Validar se é um número válido
            if (!int.TryParse(newValue, out int quantidade) || quantidade < 0)
            {
                e.Cancel = true;
                _validationFailed = true;
                MessageBox.Show(
                    "Por favor, informe uma quantidade válida (número inteiro maior ou igual a zero).",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            
            // Se chegou aqui, o valor é válido numericamente
            // Agora validar regras de negócio
            var item = dgvItens.Rows[e.RowIndex].DataBoundItem as EmprestimoItem;
            if (item == null)
            {
                e.Cancel = true;
                _validationFailed = true;
                return;
            }
            
            int novaQuantidade = quantidade;
            
            // Se a quantidade não mudou, permitir
            if (novaQuantidade == item.Quantidade)
            {
                return;
            }
            
            // Se quantidade for 0, permitir (será removido no CellValidated)
            if (novaQuantidade == 0)
            {
                _validationFailed = false;
                return;
            }
            
            // Verificar se item já tem recebimentos (modo edição)
            if (_isEditing && item.QuantidadeRecebida > 0)
            {
                if (novaQuantidade < item.QuantidadeRecebida)
                {
                    e.Cancel = true;
                    _validationFailed = true;
                    MessageBox.Show(
                        $"Não é possível reduzir a quantidade para {novaQuantidade} porque já foram recebidas {item.QuantidadeRecebida} unidades.",
                        "Validação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }
            
            // Validar estoque disponível
            var itemEstoque = _repository.Items.FirstOrDefault(i => i.Id == item.ItemId);
            if (itemEstoque != null)
            {
                // Calcular quanto será adicionado/removido
                int diferenca = novaQuantidade - item.Quantidade;
                
                // Se está aumentando a quantidade, verificar estoque
                if (diferenca > 0)
                {
                    // Estoque disponível = estoque atual + quantidade original do item (que já foi descontada)
                    int estoqueDisponivel = itemEstoque.QuantityInStock + item.Quantidade;
                    
                    if (novaQuantidade > estoqueDisponivel)
                    {
                        e.Cancel = true;
                        _validationFailed = true;
                        MessageBox.Show(
                            $"Estoque insuficiente para '{item.ItemName}'.\n" +
                            $"Quantidade solicitada: {novaQuantidade}\n" +
                            $"Disponível em estoque: {estoqueDisponivel}",
                            "Estoque Insuficiente",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            
            // Se chegou aqui, todas as validações passaram
            _validationFailed = false;
        }
    }

    private void DgvItens_CellValidated(object? sender, DataGridViewCellEventArgs e)
    {
        // Atualizar o total após validação bem-sucedida
        if (dgvItens.Columns[e.ColumnIndex].Name == "colQuantidade")
        {
            var item = dgvItens.Rows[e.RowIndex].DataBoundItem as EmprestimoItem;
            if (item == null) return;
            
            var cell = dgvItens.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (int.TryParse(cell.Value?.ToString(), out int novaQuantidade))
            {
                // Se quantidade for 0, remover o item automaticamente
                if (novaQuantidade == 0)
                {
                    _itensEmprestimo.Remove(item);
                    RefreshItensGrid();
                    return;
                }
                
                // Atualizar a quantidade do item
                if (novaQuantidade != item.Quantidade && novaQuantidade > 0)
                {
                    item.Quantidade = novaQuantidade;
                }
                
                // Atualizar o total imediatamente
                lblTotalItens.Text = $"Total de Itens: {_itensEmprestimo.Sum(i => i.Quantidade)}";
            }
        }
    }

    private void DgvItens_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (dgvItens.Columns[e.ColumnIndex].Name == "colQuantidade")
        {
            // Apenas garantir que o total está correto
            // A quantidade já foi atualizada ou o item já foi removido no CellValidated
            lblTotalItens.Text = $"Total de Itens: {_itensEmprestimo.Sum(i => i.Quantidade)}";
        }
    }

    private void DgvItens_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (dgvItens.Columns[e.ColumnIndex].Name == "colRemover")
        {
            var item = dgvItens.Rows[e.RowIndex].DataBoundItem as EmprestimoItem;
            if (item != null)
            {
                // Verificar se item já tem recebimentos
                if (_isEditing && item.QuantidadeRecebida > 0)
                {
                    MessageBox.Show(
                        $"Não é possível remover '{item.ItemName}' pois já possui {item.QuantidadeRecebida} unidades recebidas.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"Remover '{item.ItemName}' da lista?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _itensEmprestimo.Remove(item);
                    RefreshItensGrid();
                }
            }
        }
    }

    private void LoadItems()
    {
        cmbItem.DataSource = _repository.Items.ToList();
        cmbItem.DisplayMember = "Name";
        cmbItem.ValueMember = "Id";
        cmbItem.SelectedIndex = -1;
    }

    private void LoadCongregacoes()
    {
        cmbCongregacao.DataSource = _repository.Congregacoes.ToList();
        cmbCongregacao.DisplayMember = "Name";
        cmbCongregacao.ValueMember = "Id";
        cmbCongregacao.SelectedIndex = -1;
    }

    private void BtnAdicionarItem_Click(object sender, EventArgs e)
    {
        if (cmbItem.SelectedItem == null)
        {
            MessageBox.Show("Por favor, selecione um bem.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedItem = (Item)cmbItem.SelectedItem;
        var quantidade = (int)numQuantity.Value;

        // Validar estoque disponível
        if (selectedItem.QuantityInStock < quantidade)
        {
            MessageBox.Show($"Estoque insuficiente. Disponível: {selectedItem.QuantityInStock}", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Verificar se item já foi adicionado
        var itemExistente = _itensEmprestimo.FirstOrDefault(i => i.ItemId == selectedItem.Id);
        if (itemExistente != null)
        {
            itemExistente.Quantidade += quantidade;
            
            // Validar estoque total
            if (selectedItem.QuantityInStock < itemExistente.Quantidade)
            {
                itemExistente.Quantidade -= quantidade;
                MessageBox.Show($"Estoque insuficiente. Disponível: {selectedItem.QuantityInStock}", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        else
        {
            _itensEmprestimo.Add(new EmprestimoItem
            {
                ItemId = selectedItem.Id,
                ItemName = selectedItem.Name,
                Quantidade = quantidade,
                QuantidadeRecebida = 0
            });
        }

        RefreshItensGrid();
        cmbItem.SelectedIndex = -1;
        numQuantity.Value = 1;
    }

    private void RefreshItensGrid()
    {
        // Evitar refresh se estiver em modo de edição
        if (dgvItens.IsCurrentCellInEditMode)
        {
            dgvItens.EndEdit();
        }
        
        // Suspender layout para evitar flickering
        dgvItens.SuspendLayout();
        
        try
        {
            var bindingList = new BindingList<EmprestimoItem>(_itensEmprestimo);
            dgvItens.DataSource = bindingList;
            lblTotalItens.Text = $"Total de Itens: {_itensEmprestimo.Sum(i => i.Quantidade)}";
        }
        finally
        {
            dgvItens.ResumeLayout();
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        // Aplicar trim em todos os campos de texto
        txtRecebedor.Text = txtRecebedor.Text.Trim();
        txtMotivo.Text = txtMotivo.Text.Trim();
        txtQuemLiberou.Text = txtQuemLiberou.Text.Trim();
        
        if (string.IsNullOrWhiteSpace(txtRecebedor.Text))
        {
            MessageBox.Show("Por favor, informe o nome do recebedor.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtQuemLiberou.Text))
        {
            MessageBox.Show("Por favor, informe quem liberou o bem.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (cmbCongregacao.SelectedItem == null)
        {
            MessageBox.Show("Por favor, selecione uma congregação.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!_itensEmprestimo.Any())
        {
            MessageBox.Show("Por favor, adicione pelo menos um item ao empréstimo.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Obter data do MaskedTextBox
        var mtbDataEmprestimo = FormControlHelper.FindDateMaskedTextBox(this, "dtpDataEmprestimo");
        if (mtbDataEmprestimo == null || !DateTime.TryParse(mtbDataEmprestimo.Text, out DateTime dataEmprestimo))
        {
            MessageBox.Show("Por favor, informe uma data de empréstimo válida.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            mtbDataEmprestimo?.Focus();
            return;
        }

        var selectedCongregacao = (Congregacao)cmbCongregacao.SelectedItem;

        if (_isEditing && _item != null)
        {
            // Modo edição (apenas dados gerais, não permite alterar itens)
            _item.Name = txtRecebedor.Text.ToUpper();
            _item.Motivo = txtMotivo.Text.ToUpper();
            _item.QuemLiberou = txtQuemLiberou.Text.ToUpper();
            _item.CongregacaoId = selectedCongregacao.Id;
            _item.CongregacaoName = selectedCongregacao.Name;
            _item.DataEmprestimo = dataEmprestimo;
            _repository.UpdateEmprestimo(_item);
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else
        {
            // Modo criação (novo ou clonado)
            var newItem = new Emprestimo
            {
                Name = txtRecebedor.Text.ToUpper(),
                Motivo = txtMotivo.Text.ToUpper(),
                QuemLiberou = txtQuemLiberou.Text.ToUpper(),
                CongregacaoId = selectedCongregacao.Id,
                CongregacaoName = selectedCongregacao.Name,
                DataEmprestimo = dataEmprestimo,
                Status = StatusEmprestimo.EmAndamento,
                Itens = _itensEmprestimo
            };
            _repository.AddEmprestimo(newItem);

            // Perguntar se deseja imprimir recibo de empréstimo
            var resultado = MessageBox.Show(
                $"Empréstimo registrado com sucesso!\n\n" +
                $"Total de itens: {newItem.TotalItens}\n\n" +
                $"Deseja imprimir o recibo de empréstimo?",
                "Sucesso",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                var printer = new ReciboEmprestimoPrinter(newItem);
                printer.PrintPreview();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    private void BtnFechar_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }

    private void ConfigureItemIdTextBox()
    {
        // Configurar para aceitar apenas números
        txtItemId.KeyPress += TxtItemId_KeyPress;
    }

    private void TxtItemId_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // Permitir apenas números, backspace e Enter
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Enter)
        {
            e.Handled = true;
            return;
        }

        // Se pressionou Enter, adicionar item
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            AdicionarItemPorId();
        }
    }

    private void AdicionarItemPorId()
    {
        if (string.IsNullOrWhiteSpace(txtItemId.Text))
        {
            return;
        }

        if (!int.TryParse(txtItemId.Text, out int itemId))
        {
            MessageBox.Show("Por favor, digite um ID válido.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtItemId.Clear();
            txtItemId.Focus();
            return;
        }

        var selectedItem = _repository.Items.FirstOrDefault(i => i.Id == itemId);
        
        if (selectedItem == null)
        {
            MessageBox.Show($"Bem com ID {itemId} não encontrado.", "Não Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtItemId.Clear();
            txtItemId.Focus();
            return;
        }

        // Validar estoque disponível
        if (selectedItem.QuantityInStock < 1)
        {
            MessageBox.Show($"Bem '{selectedItem.Name}' sem estoque disponível.", "Estoque Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtItemId.Clear();
            txtItemId.Focus();
            return;
        }

        // Verificar se item já foi adicionado
        var itemExistente = _itensEmprestimo.FirstOrDefault(i => i.ItemId == selectedItem.Id);
        if (itemExistente != null)
        {
            itemExistente.Quantidade += 1;
            
            // Validar estoque total
            if (selectedItem.QuantityInStock < itemExistente.Quantidade)
            {
                itemExistente.Quantidade -= 1;
                MessageBox.Show($"Estoque insuficiente. Disponível: {selectedItem.QuantityInStock}", "Estoque Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtItemId.Clear();
                txtItemId.Focus();
                return;
            }
        }
        else
        {
            _itensEmprestimo.Add(new EmprestimoItem
            {
                ItemId = selectedItem.Id,
                ItemName = selectedItem.Name,
                Quantidade = 1,
                QuantidadeRecebida = 0
            });
        }

        RefreshItensGrid();
        txtItemId.Clear();
        txtItemId.Focus();
    }
}
