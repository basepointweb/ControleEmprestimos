using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Reports;

namespace ControleEmprestimos.Forms;

public partial class EmprestimoDetailForm : Form
{
    private Emprestimo? _item;
    private bool _isEditing;
    private bool _isCloning;
    private DataRepository _repository;
    private Item? _itemPreSelecionado;
    private List<EmprestimoItem> _itensEmprestimo = new();

    public EmprestimoDetailForm(Emprestimo? item = null, bool isCloning = false)
    {
        InitializeComponent();
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
            dtpDataEmprestimo.Value = _item.DataEmprestimo;
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
                // Modo visualização
                lblStatus.Visible = true;
                txtStatus.Visible = true;
                
                // Mostrar botão Cancelar apenas se estiver Em Andamento
                if (_item.Status == StatusEmprestimo.EmAndamento)
                {
                    btnCancelar.Visible = true;
                }
                
                // Se já foi devolvido ou cancelado, desabilitar edição
                if (_item.Status != StatusEmprestimo.EmAndamento)
                {
                    txtRecebedor.ReadOnly = true;
                    txtMotivo.ReadOnly = true;
                    txtQuemLiberou.ReadOnly = true;
                    cmbCongregacao.Enabled = false;
                    dtpDataEmprestimo.Enabled = false;
                    cmbItem.Enabled = false;
                    numQuantity.Enabled = false;
                    btnAdicionarItem.Visible = false;
                    dgvItens.Columns["colRemover"].Visible = false;
                    btnSave.Visible = false;
                    btnCancelar.Visible = false;
                }
                else
                {
                    // Em andamento mas não permite adicionar/remover itens já com recebimentos
                    btnAdicionarItem.Visible = false;
                    dgvItens.Columns["colRemover"].Visible = false;
                    lblItem.Visible = false;
                    cmbItem.Visible = false;
                    lblQuantity.Visible = false;
                    numQuantity.Visible = false;
                }
            }
            else if (_isCloning)
            {
                // Modo clonagem - atualizar data e status
                dtpDataEmprestimo.Value = DateTime.Now;
                txtStatus.Text = "Em Andamento";
                btnCancelar.Visible = false;
                this.Text = "Clonar Empréstimo";
            }
        }
        else
        {
            // Para novo empréstimo, definir a data atual
            dtpDataEmprestimo.Value = DateTime.Now;
            txtStatus.Text = "Em Andamento";
            btnCancelar.Visible = false;
        }
    }

    // Construtor para empréstimo com item pré-selecionado
    public EmprestimoDetailForm(Item itemPreSelecionado) : this()
    {
        _itemPreSelecionado = itemPreSelecionado;
        
        if (_itemPreSelecionado != null)
        {
            cmbItem.SelectedItem = _itemPreSelecionado;
            numQuantity.Value = 1;
            dtpDataEmprestimo.Value = DateTime.Now;
            txtStatus.Text = "Em Andamento";
        }
    }

    private void ConfigureDataGridView()
    {
        dgvItens.AutoGenerateColumns = false;
        dgvItens.Columns.Clear();

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
            ReadOnly = true
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
        dgvItens.DataSource = null;
        dgvItens.DataSource = _itensEmprestimo;
        lblTotalItens.Text = $"Total de Itens: {_itensEmprestimo.Sum(i => i.Quantidade)}";
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
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

        var selectedCongregacao = (Congregacao)cmbCongregacao.SelectedItem;

        if (_isEditing && _item != null)
        {
            // Modo edição (apenas dados gerais, não permite alterar itens)
            _item.Name = txtRecebedor.Text;
            _item.Motivo = txtMotivo.Text;
            _item.QuemLiberou = txtQuemLiberou.Text;
            _item.CongregacaoId = selectedCongregacao.Id;
            _item.CongregacaoName = selectedCongregacao.Name;
            _item.DataEmprestimo = dtpDataEmprestimo.Value;
            _repository.UpdateEmprestimo(_item);
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else
        {
            // Modo criação (novo ou clonado)
            var newItem = new Emprestimo
            {
                Name = txtRecebedor.Text,
                Motivo = txtMotivo.Text,
                QuemLiberou = txtQuemLiberou.Text,
                CongregacaoId = selectedCongregacao.Id,
                CongregacaoName = selectedCongregacao.Name,
                DataEmprestimo = dtpDataEmprestimo.Value,
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

    private void BtnCancelarEmprestimo_Click(object sender, EventArgs e)
    {
        if (_item == null) return;

        var result = MessageBox.Show(
            "Tem certeza que deseja cancelar este empréstimo?",
            "Confirmar Cancelamento",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            _item.Status = StatusEmprestimo.Cancelado;
            _repository.UpdateEmprestimo(_item);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    private void BtnFechar_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
