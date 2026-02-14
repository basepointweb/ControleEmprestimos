using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class RecebimentoListForm : UserControl
{
    private DataRepository _repository;

    public RecebimentoListForm()
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
            Width = 180
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "NomeRecebedor",
            HeaderText = "Recebedor",
            Name = "colRecebedor",
            Width = 150
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "QuantityInStock",
            HeaderText = "Quantidade",
            Name = "colQuantity",
            Width = 90
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "DataEmprestimo",
            HeaderText = "Data Empréstimo",
            Name = "colDataEmprestimo",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "DataRecebimento",
            HeaderText = "Data Recebimento",
            Name = "colDataRecebimento",
            Width = 130,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
        });
    }

    private void LoadData()
    {
        dataGridView1.DataSource = null;
        dataGridView1.DataSource = _repository.RecebimentoEmprestimos;
    }

    private void BtnCreate_Click(object sender, EventArgs e)
    {
        var form = new RecebimentoDetailForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadData();
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is RecebimentoEmprestimo item)
        {
            var form = new RecebimentoDetailForm(item);
            form.ShowDialog();
        }
        else
        {
            MessageBox.Show("Por favor, selecione um item para visualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is RecebimentoEmprestimo item)
        {
            var result = MessageBox.Show(
                $"Tem certeza que deseja excluir '{item.Name}'?\n\n" +
                $"ATENÇÃO: O empréstimo voltará ao status 'Em Andamento' e o estoque será reduzido novamente.",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Reverter empréstimo e estoque
                if (item.EmprestimoId.HasValue)
                {
                    var emprestimo = _repository.Emprestimos.FirstOrDefault(e => e.Id == item.EmprestimoId.Value);
                    if (emprestimo != null)
                    {
                        // Voltar status do empréstimo para Em Andamento
                        emprestimo.Status = StatusEmprestimo.EmAndamento;

                        // Reduzir estoque novamente
                        if (emprestimo.ItemId.HasValue)
                        {
                            var itemEstoque = _repository.Items.FirstOrDefault(i => i.Id == emprestimo.ItemId.Value);
                            if (itemEstoque != null)
                            {
                                itemEstoque.QuantityInStock -= emprestimo.QuantityInStock;
                            }
                        }
                    }
                }

                _repository.RecebimentoEmprestimos.Remove(item);
                LoadData();

                MessageBox.Show(
                    "Recebimento excluído com sucesso!\n" +
                    "O empréstimo voltou ao status 'Em Andamento' e o estoque foi reduzido.",
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
}
