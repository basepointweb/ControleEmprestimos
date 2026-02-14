using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class CongregacaoDetailForm : Form
{
    private Congregacao? _item;
    private bool _isEditing;
    private DataRepository _repository;

    public CongregacaoDetailForm(Congregacao? item = null)
    {
        InitializeComponent();
        _repository = DataRepository.Instance;
        _item = item;
        _isEditing = item != null;

        ConfigureDataGridView();

        if (_isEditing && _item != null)
        {
            txtName.Text = _item.Name;
            LoadEmprestimos();
        }
        else
        {
            // Ao criar nova congregação, ocultar grid e botão receber
            lblEmprestimos.Visible = false;
            dataGridView1.Visible = false;
            btnReceber.Visible = false;
            
            // Ajustar tamanho do formulário
            this.ClientSize = new Size(800, 200);
            btnSave.Location = new Point(560, 150);
            btnCancel.Location = new Point(666, 150);
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
            Width = 150
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "ItemName",
            HeaderText = "Bem",
            Name = "colItem",
            Width = 120
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "QuantityInStock",
            HeaderText = "Qtd",
            Name = "colQuantity",
            Width = 60
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "DataEmprestimo",
            HeaderText = "Data Empréstimo",
            Name = "colData",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Motivo",
            HeaderText = "Motivo",
            Name = "colMotivo",
            Width = 230,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
    }

    private void LoadEmprestimos()
    {
        if (_item == null) return;

        // Carregar apenas empréstimos Em Andamento para esta congregação
        var emprestimos = _repository.Emprestimos
            .Where(e => e.CongregacaoId == _item.Id && e.Status == StatusEmprestimo.EmAndamento)
            .ToList();

        dataGridView1.DataSource = null;
        dataGridView1.DataSource = emprestimos;

        // Atualizar label com contador
        lblEmprestimos.Text = $"Itens Emprestados (Em Andamento): {emprestimos.Count}";
    }

    private void BtnReceber_Click(object sender, EventArgs e)
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
                LoadEmprestimos(); // Atualiza grid
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um empréstimo para receber de volta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Por favor, informe o nome da congregação.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_isEditing && _item != null)
        {
            _item.Name = txtName.Text;
        }
        else
        {
            var newItem = new Congregacao
            {
                Name = txtName.Text,
                QuantityInStock = 0
            };
            _repository.AddCongregacao(newItem);
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
