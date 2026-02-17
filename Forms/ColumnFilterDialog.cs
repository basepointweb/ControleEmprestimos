namespace ControleEmprestimos.Forms;

public partial class ColumnFilterDialog : Form
{
    private TextBox txtFilter;
    private CheckedListBox checkedListBox;
    private Button btnOk;
    private Button btnCancel;
    private Button btnSelectAll;
    private Button btnDeselectAll;
    private Panel buttonPanel;
    private Panel filterPanel;

    public List<string> SelectedValues { get; private set; } = new();
    public List<string> AllValues { get; private set; } = new();
    
    // Armazenar todos os valores e seus estados de seleção
    private Dictionary<string, bool> _allValuesWithState = new();

    public ColumnFilterDialog(string columnName, List<string> distinctValues, List<string> currentlySelected)
    {
        InitializeComponent();
        this.Text = $"Filtrar: {columnName}";
        AllValues = distinctValues;
        
        // Inicializar o dicionário com todos os valores e seus estados
        foreach (var value in distinctValues)
        {
            bool isChecked = currentlySelected == null || currentlySelected.Count == 0 || currentlySelected.Contains(value);
            _allValuesWithState[value] = isChecked;
        }
        
        LoadValues(distinctValues, currentlySelected);
    }

    private void InitializeComponent()
    {
        this.txtFilter = new TextBox();
        this.checkedListBox = new CheckedListBox();
        this.btnOk = new Button();
        this.btnCancel = new Button();
        this.btnSelectAll = new Button();
        this.btnDeselectAll = new Button();
        this.buttonPanel = new Panel();
        this.filterPanel = new Panel();
        
        this.SuspendLayout();
        
        // filterPanel
        this.filterPanel.Controls.Add(this.txtFilter);
        this.filterPanel.Dock = DockStyle.Top;
        this.filterPanel.Location = new Point(0, 0);
        this.filterPanel.Name = "filterPanel";
        this.filterPanel.Size = new Size(350, 40);
        this.filterPanel.TabIndex = 0;
        this.filterPanel.Padding = new Padding(10, 8, 10, 8);
        
        // txtFilter
        this.txtFilter.Dock = DockStyle.Fill;
        this.txtFilter.Name = "txtFilter";
        this.txtFilter.PlaceholderText = "Digite para filtrar...";
        this.txtFilter.TabIndex = 0;
        this.txtFilter.TextChanged += TxtFilter_TextChanged;
        
        // checkedListBox
        this.checkedListBox.CheckOnClick = true;
        this.checkedListBox.Dock = DockStyle.Fill;
        this.checkedListBox.FormattingEnabled = true;
        this.checkedListBox.Location = new Point(0, 40);
        this.checkedListBox.Name = "checkedListBox";
        this.checkedListBox.Size = new Size(350, 360);
        this.checkedListBox.TabIndex = 1;
        this.checkedListBox.ItemCheck += CheckedListBox_ItemCheck;
        
        // buttonPanel
        this.buttonPanel.Controls.Add(this.btnSelectAll);
        this.buttonPanel.Controls.Add(this.btnDeselectAll);
        this.buttonPanel.Controls.Add(this.btnOk);
        this.buttonPanel.Controls.Add(this.btnCancel);
        this.buttonPanel.Dock = DockStyle.Bottom;
        this.buttonPanel.Location = new Point(0, 400);
        this.buttonPanel.Name = "buttonPanel";
        this.buttonPanel.Size = new Size(350, 80);
        this.buttonPanel.TabIndex = 2;
        
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
        this.Controls.Add(this.filterPanel);
        this.Controls.Add(this.buttonPanel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ColumnFilterDialog";
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Filtrar Coluna";
        
        this.ResumeLayout(false);
    }

    private void CheckedListBox_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        // Atualizar o estado no dicionário quando o usuário marcar/desmarcar
        if (e.Index >= 0 && e.Index < checkedListBox.Items.Count)
        {
            var value = checkedListBox.Items[e.Index].ToString() ?? string.Empty;
            _allValuesWithState[value] = e.NewValue == CheckState.Checked;
        }
    }

    private void TxtFilter_TextChanged(object? sender, EventArgs e)
    {
        var filterText = txtFilter.Text.Trim().ToUpper();
        
        // Filtrar valores baseado no texto digitado
        var filteredValues = string.IsNullOrWhiteSpace(filterText)
            ? AllValues
            : AllValues.Where(v => v.ToUpper().Contains(filterText)).ToList();
        
        // Recarregar a lista mantendo os estados de seleção
        LoadFilteredValues(filteredValues);
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

    private void LoadFilteredValues(List<string> filteredValues)
    {
        // Suspender atualização visual
        checkedListBox.BeginUpdate();
        checkedListBox.Items.Clear();
        
        // Adicionar apenas os valores filtrados, mantendo o estado de seleção do dicionário
        foreach (var value in filteredValues.OrderBy(v => v))
        {
            bool isChecked = _allValuesWithState.ContainsKey(value) && _allValuesWithState[value];
            checkedListBox.Items.Add(value, isChecked);
        }
        
        // Retomar atualização visual
        checkedListBox.EndUpdate();
    }

    private void BtnSelectAll_Click(object? sender, EventArgs e)
    {
        // Marcar apenas os itens VISÍVEIS (filtrados)
        for (int i = 0; i < checkedListBox.Items.Count; i++)
        {
            var value = checkedListBox.Items[i].ToString() ?? string.Empty;
            checkedListBox.SetItemChecked(i, true);
            _allValuesWithState[value] = true;
        }
    }

    private void BtnDeselectAll_Click(object? sender, EventArgs e)
    {
        // Desmarcar apenas os itens VISÍVEIS (filtrados)
        for (int i = 0; i < checkedListBox.Items.Count; i++)
        {
            var value = checkedListBox.Items[i].ToString() ?? string.Empty;
            checkedListBox.SetItemChecked(i, false);
            _allValuesWithState[value] = false;
        }
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        SelectedValues.Clear();
        
        // Retornar TODOS os valores selecionados (não apenas os visíveis)
        foreach (var kvp in _allValuesWithState.Where(kvp => kvp.Value))
        {
            SelectedValues.Add(kvp.Key);
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
