using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class EmprestimoDetailForm : Form
{
    private Emprestimo? _item;
    private bool _isEditing;
    private DataRepository _repository;
    private Item? _itemPreSelecionado;

    public EmprestimoDetailForm(Emprestimo? item = null)
    {
        InitializeComponent();
        _repository = DataRepository.Instance;
        _item = item;
        _isEditing = item != null;

        LoadItems();
        LoadCongregacoes();

        if (_isEditing && _item != null)
        {
            txtRecebedor.Text = _item.Name;
            txtMotivo.Text = _item.Motivo;
            numQuantity.Value = _item.QuantityInStock;
            dtpDataEmprestimo.Value = _item.DataEmprestimo;
            txtStatus.Text = _item.StatusDescricao;
            
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
                cmbItem.Enabled = false;
                numQuantity.Enabled = false;
                cmbCongregacao.Enabled = false;
                dtpDataEmprestimo.Enabled = false;
                btnSave.Visible = false;
                btnCancelar.Visible = false;
            }
            
            if (_item.ItemId.HasValue)
            {
                var itemObj = _repository.Items.FirstOrDefault(i => i.Id == _item.ItemId.Value);
                if (itemObj != null)
                {
                    cmbItem.SelectedItem = itemObj;
                }
            }
            
            if (_item.CongregacaoId.HasValue)
            {
                var congregacao = _repository.Congregacoes.FirstOrDefault(c => c.Id == _item.CongregacaoId.Value);
                if (congregacao != null)
                {
                    cmbCongregacao.SelectedItem = congregacao;
                }
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

    // Novo construtor para empréstimo com item pré-selecionado
    public EmprestimoDetailForm(Item itemPreSelecionado) : this()
    {
        _itemPreSelecionado = itemPreSelecionado;
        
        // Pré-selecionar o item
        if (_itemPreSelecionado != null)
        {
            cmbItem.SelectedItem = _itemPreSelecionado;
            numQuantity.Value = 1;
            dtpDataEmprestimo.Value = DateTime.Now;
            txtStatus.Text = "Em Andamento";
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

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtRecebedor.Text))
        {
            MessageBox.Show("Por favor, informe o nome do recebedor.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (cmbItem.SelectedItem == null)
        {
            MessageBox.Show("Por favor, selecione um bem.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (cmbCongregacao.SelectedItem == null)
        {
            MessageBox.Show("Por favor, selecione uma congregação.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedItem = (Item)cmbItem.SelectedItem;
        var selectedCongregacao = (Congregacao)cmbCongregacao.SelectedItem;
        
        // Validar estoque disponível
        var quantidadeEmprestimo = (int)numQuantity.Value;
        if (selectedItem.QuantityInStock < quantidadeEmprestimo)
        {
            MessageBox.Show($"Estoque insuficiente. Disponível: {selectedItem.QuantityInStock}", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_isEditing && _item != null)
        {
            _item.Name = txtRecebedor.Text;
            _item.Motivo = txtMotivo.Text;
            _item.QuantityInStock = quantidadeEmprestimo;
            _item.ItemId = selectedItem.Id;
            _item.ItemName = selectedItem.Name;
            _item.CongregacaoId = selectedCongregacao.Id;
            _item.CongregacaoName = selectedCongregacao.Name;
            _item.DataEmprestimo = dtpDataEmprestimo.Value;
        }
        else
        {
            var newItem = new Emprestimo
            {
                Name = txtRecebedor.Text,
                Motivo = txtMotivo.Text,
                QuantityInStock = quantidadeEmprestimo,
                ItemId = selectedItem.Id,
                ItemName = selectedItem.Name,
                CongregacaoId = selectedCongregacao.Id,
                CongregacaoName = selectedCongregacao.Name,
                DataEmprestimo = dtpDataEmprestimo.Value,
                Status = StatusEmprestimo.EmAndamento
            };
            _repository.AddEmprestimo(newItem);
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
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
