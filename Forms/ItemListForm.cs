using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class ItemListForm : Form
{
    private DataRepository _repository;

    public ItemListForm()
    {
        InitializeComponent();
        _repository = DataRepository.Instance;
        LoadData();
    }

    private void LoadData()
    {
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
}
