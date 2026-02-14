using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class ItemListForm : UserControl
{
    private DataRepository _repository;

    public ItemListForm()
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
            Width = 200
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "QuantityInStock",
            HeaderText = "Total em Estoque",
            Name = "colQuantity",
            Width = 120
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "TotalEmprestado",
            HeaderText = "Total Emprestado",
            Name = "colEmprestado",
            Width = 120
        });
    }

    private void LoadData()
    {
        // Calcular total emprestado para cada item (apenas empréstimos Em Andamento)
        foreach (var item in _repository.Items)
        {
            item.TotalEmprestado = _repository.Emprestimos
                .Where(e => e.ItemId == item.Id && e.Status == StatusEmprestimo.EmAndamento)
                .Sum(e => e.QuantityInStock);
        }

        dataGridView1.DataSource = null;
        dataGridView1.DataSource = _repository.Items;
    }

    private void BtnCreate_Click(object sender, EventArgs e)
    {
        var form = new ItemDetailForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadData();
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Item item)
        {
            var form = new ItemDetailForm(item);
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
        if (dataGridView1.CurrentRow?.DataBoundItem is Item item)
        {
            var result = MessageBox.Show($"Tem certeza que deseja excluir '{item.Name}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _repository.Items.Remove(item);
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um item para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnEmprestar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Item item)
        {
            // Verificar se há estoque disponível
            if (item.QuantityInStock <= 0)
            {
                MessageBox.Show($"Não há estoque disponível de '{item.Name}'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Criar um novo empréstimo com o item selecionado
            var form = new EmprestimoDetailForm(item);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um item para emprestar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
