using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class EmprestimoDetailForm : Form
{
    private Emprestimo? _item;
    private bool _isEditing;

    public EmprestimoDetailForm(Emprestimo? item = null)
    {
        InitializeComponent();
        _item = item;
        _isEditing = item != null;

        if (_isEditing && _item != null)
        {
            txtName.Text = _item.Name;
            numQuantity.Value = _item.QuantityInStock;
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Por favor, informe o nome do item.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_isEditing && _item != null)
        {
            _item.Name = txtName.Text;
            _item.QuantityInStock = (int)numQuantity.Value;
        }
        else
        {
            var newItem = new Emprestimo
            {
                Name = txtName.Text,
                QuantityInStock = (int)numQuantity.Value
            };
            DataRepository.Instance.AddEmprestimo(newItem);
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
