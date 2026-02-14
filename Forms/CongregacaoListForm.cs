using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class CongregacaoListForm : UserControl
{
    private DataRepository _repository;

    public CongregacaoListForm()
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
            Width = 300
        });

        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "TotalItensEmprestados",
            HeaderText = "Total de Itens Emprestados",
            Name = "colTotalEmprestados",
            Width = 180
        });
    }

    private void LoadData()
    {
        // Calcular total de itens emprestados para cada congregação
        foreach (var congregacao in _repository.Congregacoes)
        {
            congregacao.TotalItensEmprestados = _repository.Emprestimos
                .Where(e => e.CongregacaoId == congregacao.Id)
                .Sum(e => e.QuantityInStock);
        }

        dataGridView1.DataSource = null;
        dataGridView1.DataSource = _repository.Congregacoes;
    }

    private void BtnCreate_Click(object sender, EventArgs e)
    {
        var form = new CongregacaoDetailForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadData();
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Congregacao item)
        {
            var form = new CongregacaoDetailForm(item);
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
        if (dataGridView1.CurrentRow?.DataBoundItem is Congregacao item)
        {
            var result = MessageBox.Show($"Tem certeza que deseja excluir '{item.Name}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _repository.Congregacoes.Remove(item);
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Por favor, selecione um item para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
