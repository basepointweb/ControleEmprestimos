using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class ItemDetailForm : Form
{
    private Item? _item;
    private bool _isEditing;
    private bool _isCloning;

    public ItemDetailForm(Item? item = null, bool isCloning = false)
    {
        InitializeComponent();
        _item = item;
        _isEditing = item != null && !isCloning;
        _isCloning = isCloning;

        if (_item != null)
        {
            txtName.Text = _item.Name;
            numQuantity.Value = _item.QuantityInStock;

            if (_isCloning)
            {
                this.Text = "Clonar Bem";
            }
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
            // Modo edição
            _item.Name = txtName.Text;
            _item.QuantityInStock = (int)numQuantity.Value;
            DataRepository.Instance.UpdateItem(_item);
        }
        else
        {
            // Modo criação (novo ou clonado)
            var newItem = new Item
            {
                Name = txtName.Text,
                QuantityInStock = (int)numQuantity.Value
            };
            DataRepository.Instance.AddItem(newItem);
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
