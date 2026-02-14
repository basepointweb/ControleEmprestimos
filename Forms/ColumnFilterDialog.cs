namespace ControleEmprestimos.Forms;

public partial class ColumnFilterDialog : Form
{
    private CheckedListBox checkedListBox;
    private Button btnOk;
    private Button btnCancel;
    private Button btnSelectAll;
    private Button btnDeselectAll;
    private Panel buttonPanel;

    public List<string> SelectedValues { get; private set; } = new();
    public List<string> AllValues { get; private set; } = new();

    public ColumnFilterDialog(string columnName, List<string> distinctValues, List<string> currentlySelected)
    {
        InitializeComponent();
        this.Text = $"Filtrar: {columnName}";
        AllValues = distinctValues;
        LoadValues(distinctValues, currentlySelected);
    }

    private void InitializeComponent()
    {
        this.checkedListBox = new CheckedListBox();
        this.btnOk = new Button();
        this.btnCancel = new Button();
        this.btnSelectAll = new Button();
        this.btnDeselectAll = new Button();
        this.buttonPanel = new Panel();
        
        this.SuspendLayout();
        
        // checkedListBox
        this.checkedListBox.CheckOnClick = true;
        this.checkedListBox.Dock = DockStyle.Fill;
        this.checkedListBox.FormattingEnabled = true;
        this.checkedListBox.Location = new Point(0, 0);
        this.checkedListBox.Name = "checkedListBox";
        this.checkedListBox.Size = new Size(350, 400);
        this.checkedListBox.TabIndex = 0;
        
        // buttonPanel
        this.buttonPanel.Controls.Add(this.btnSelectAll);
        this.buttonPanel.Controls.Add(this.btnDeselectAll);
        this.buttonPanel.Controls.Add(this.btnOk);
        this.buttonPanel.Controls.Add(this.btnCancel);
        this.buttonPanel.Dock = DockStyle.Bottom;
        this.buttonPanel.Location = new Point(0, 400);
        this.buttonPanel.Name = "buttonPanel";
        this.buttonPanel.Size = new Size(350, 80);
        this.buttonPanel.TabIndex = 1;
        
        // btnSelectAll
        this.btnSelectAll.Location = new Point(12, 10);
        this.btnSelectAll.Name = "btnSelectAll";
        this.btnSelectAll.Size = new Size(160, 25);
        this.btnSelectAll.TabIndex = 0;
        this.btnSelectAll.Text = "Selecionar Todos";
        this.btnSelectAll.UseVisualStyleBackColor = true;
        this.btnSelectAll.Click += BtnSelectAll_Click;
        
        // btnDeselectAll
        this.btnDeselectAll.Location = new Point(178, 10);
        this.btnDeselectAll.Name = "btnDeselectAll";
        this.btnDeselectAll.Size = new Size(160, 25);
        this.btnDeselectAll.TabIndex = 1;
        this.btnDeselectAll.Text = "Desmarcar Todos";
        this.btnDeselectAll.UseVisualStyleBackColor = true;
        this.btnDeselectAll.Click += BtnDeselectAll_Click;
        
        // btnOk
        this.btnOk.Location = new Point(12, 45);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new Size(160, 25);
        this.btnOk.TabIndex = 2;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        this.btnOk.Click += BtnOk_Click;
        
        // btnCancel
        this.btnCancel.Location = new Point(178, 45);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(160, 25);
        this.btnCancel.TabIndex = 3;
        this.btnCancel.Text = "Cancelar";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += BtnCancel_Click;
        
        // ColumnFilterDialog
        this.ClientSize = new Size(350, 480);
        this.Controls.Add(this.checkedListBox);
        this.Controls.Add(this.buttonPanel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ColumnFilterDialog";
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Filtrar Coluna";
        
        this.ResumeLayout(false);
    }

    private void LoadValues(List<string> values, List<string> currentlySelected)
    {
        checkedListBox.Items.Clear();
        
        foreach (var value in values.OrderBy(v => v))
        {
            bool isChecked = currentlySelected == null || currentlySelected.Count == 0 || currentlySelected.Contains(value);
            checkedListBox.Items.Add(value, isChecked);
        }
    }

    private void BtnSelectAll_Click(object? sender, EventArgs e)
    {
        for (int i = 0; i < checkedListBox.Items.Count; i++)
        {
            checkedListBox.SetItemChecked(i, true);
        }
    }

    private void BtnDeselectAll_Click(object? sender, EventArgs e)
    {
        for (int i = 0; i < checkedListBox.Items.Count; i++)
        {
            checkedListBox.SetItemChecked(i, false);
        }
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        SelectedValues.Clear();
        foreach (var item in checkedListBox.CheckedItems)
        {
            SelectedValues.Add(item.ToString() ?? string.Empty);
        }
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
