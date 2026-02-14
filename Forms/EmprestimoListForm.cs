using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class EmprestimoListForm : UserControl
{
    private DataRepository _repository;

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
    }

    private void LoadData()
    {
        dataGridView1.DataSource = null;
        dataGridView1.DataSource = _repository.Emprestimos;
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
