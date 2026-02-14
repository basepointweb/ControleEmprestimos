using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class RecebimentoDetailForm : Form
{
    private RecebimentoEmprestimo? _item;
    private bool _isEditing;
    private DataRepository _repository;
    private Emprestimo? _emprestimoPreSelecionado;

    public RecebimentoDetailForm(RecebimentoEmprestimo? item = null)
    {
        InitializeComponent();
        _repository = DataRepository.Instance;
        _item = item;
        _isEditing = item != null;

        LoadEmprestimos();

        if (_isEditing && _item != null)
        {
            // Modo visualização para recebimentos já salvos
            if (_item.EmprestimoId.HasValue)
            {
                var emprestimo = _repository.Emprestimos.FirstOrDefault(e => e.Id == _item.EmprestimoId.Value);
                if (emprestimo != null)
                {
                    // Adicionar o empréstimo à lista para visualização (mesmo que não esteja Em Andamento)
                    var emprestimoDisplay = new
                    {
                        Emprestimo = emprestimo,
                        DisplayText = $"{emprestimo.CongregacaoName} - {emprestimo.ItemName} - {emprestimo.Name}"
                    };
                    
                    var currentList = cmbEmprestimo.DataSource as List<dynamic>;
                    if (currentList != null && !currentList.Any(x => x.Emprestimo.Id == emprestimo.Id))
                    {
                        currentList.Add(emprestimoDisplay);
                        cmbEmprestimo.DataSource = null;
                        cmbEmprestimo.DataSource = currentList;
                        cmbEmprestimo.DisplayMember = "DisplayText";
                        cmbEmprestimo.ValueMember = "Emprestimo";
                    }
                    
                    cmbEmprestimo.SelectedItem = emprestimoDisplay;
                }
            }
            
            if (_item.DataEmprestimo.HasValue)
            {
                txtDataEmprestimo.Text = _item.DataEmprestimo.Value.ToString("dd/MM/yyyy");
            }
            
            txtRecebedor.Text = _item.NomeRecebedor;
            numQuantity.Value = _item.QuantityInStock;
            dtpDataRecebimento.Value = _item.DataRecebimento;
            
            // Desabilitar edição
            cmbEmprestimo.Enabled = false;
            dtpDataRecebimento.Enabled = false;
            btnSave.Visible = false;
        }
        else
        {
            // Novo recebimento - data atual
            dtpDataRecebimento.Value = DateTime.Now;
        }
    }

    // Construtor para recebimento com empréstimo pré-selecionado
    public RecebimentoDetailForm(Emprestimo emprestimoPreSelecionado) : this()
    {
        _emprestimoPreSelecionado = emprestimoPreSelecionado;
        
        if (_emprestimoPreSelecionado != null)
        {
            // Encontrar o item no ComboBox e selecionar
            var dataSource = cmbEmprestimo.DataSource as List<dynamic>;
            if (dataSource != null)
            {
                var item = dataSource.FirstOrDefault(x => x.Emprestimo.Id == _emprestimoPreSelecionado.Id);
                if (item != null)
                {
                    cmbEmprestimo.SelectedItem = item;
                }
            }
            
            dtpDataRecebimento.Value = DateTime.Now;
        }
    }

    private void LoadEmprestimos()
    {
        // Carregar apenas empréstimos com status "Em Andamento"
        var emprestimosEmAndamento = _repository.Emprestimos
            .Where(e => e.Status == StatusEmprestimo.EmAndamento)
            .Select(e => new
            {
                Emprestimo = e,
                DisplayText = $"{e.CongregacaoName} - {e.ItemName} - {e.Name}"
            })
            .ToList<dynamic>();

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
                    txtRecebedor.Text = emprestimo.Name;
                    numQuantity.Value = emprestimo.QuantityInStock;
                    return;
                }
            }
        }
        
        txtDataEmprestimo.Clear();
        txtRecebedor.Clear();
        numQuantity.Value = 0;
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (cmbEmprestimo.SelectedItem == null)
        {
            MessageBox.Show("Por favor, selecione um empréstimo.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedItem = cmbEmprestimo.SelectedItem;
        var emprestimoProperty = selectedItem.GetType().GetProperty("Emprestimo");
        var emprestimoSelecionado = emprestimoProperty?.GetValue(selectedItem) as Emprestimo;

        if (emprestimoSelecionado == null)
        {
            MessageBox.Show("Erro ao obter o empréstimo selecionado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (_isEditing && _item != null)
        {
            // Não deve acontecer porque desabilitamos a edição
            MessageBox.Show("Não é possível editar um recebimento já registrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        else
        {
            // Criar novo recebimento
            var newItem = new RecebimentoEmprestimo
            {
                Name = $"Recebimento - {emprestimoSelecionado.ItemName}",
                NomeRecebedor = emprestimoSelecionado.Name,
                QuantityInStock = emprestimoSelecionado.QuantityInStock,
                EmprestimoId = emprestimoSelecionado.Id,
                DataEmprestimo = emprestimoSelecionado.DataEmprestimo,
                DataRecebimento = dtpDataRecebimento.Value
            };
            _repository.AddRecebimento(newItem);

            // Devolver empréstimo (repõe estoque e atualiza status)
            _repository.DevolverEmprestimo(emprestimoSelecionado);
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
