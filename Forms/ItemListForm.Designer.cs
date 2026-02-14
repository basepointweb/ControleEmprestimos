namespace ControleEmprestimos.Forms;

partial class ItemListForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent()
    {
        this.dataGridView1 = new DataGridView();
        this.btnCreate = new Button();
        this.btnEdit = new Button();
        this.btnDelete = new Button();
        this.btnEmprestar = new Button();
        this.panel1 = new Panel();
        this.titlePanel = new Panel();
        this.titleLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        this.panel1.SuspendLayout();
        this.titlePanel.SuspendLayout();
        this.SuspendLayout();
        // 
        // titlePanel
        // 
        this.titlePanel.BackColor = Color.FromArgb(0, 120, 215);
        this.titlePanel.Controls.Add(this.titleLabel);
        this.titlePanel.Dock = DockStyle.Top;
        this.titlePanel.Location = new Point(0, 0);
        this.titlePanel.Name = "titlePanel";
        this.titlePanel.Size = new Size(800, 50);
        this.titlePanel.TabIndex = 2;
        // 
        // titleLabel
        // 
        this.titleLabel.AutoSize = false;
        this.titleLabel.Dock = DockStyle.Fill;
        this.titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        this.titleLabel.ForeColor = Color.White;
        this.titleLabel.Location = new Point(0, 0);
        this.titleLabel.Name = "titleLabel";
        this.titleLabel.Size = new Size(800, 50);
        this.titleLabel.TabIndex = 0;
        this.titleLabel.Text = "Listagem de Bens";
        this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
        this.titleLabel.Padding = new Padding(20, 0, 0, 0);
        // 
        // dataGridView1
        // 
        this.dataGridView1.AllowUserToAddRows = false;
        this.dataGridView1.AllowUserToDeleteRows = false;
        this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridView1.Dock = DockStyle.Fill;
        this.dataGridView1.Location = new Point(0, 50);
        this.dataGridView1.Name = "dataGridView1";
        this.dataGridView1.ReadOnly = true;
        this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.dataGridView1.Size = new Size(800, 350);
        this.dataGridView1.TabIndex = 0;
        // 
        // panel1
        // 
        this.panel1.Controls.Add(this.btnCreate);
        this.panel1.Controls.Add(this.btnEdit);
        this.panel1.Controls.Add(this.btnDelete);
        this.panel1.Controls.Add(this.btnEmprestar);
        this.panel1.Dock = DockStyle.Bottom;
        this.panel1.Location = new Point(0, 400);
        this.panel1.Name = "panel1";
        this.panel1.Size = new Size(800, 50);
        this.panel1.TabIndex = 1;
        // 
        // btnCreate
        // 
        this.btnCreate.Location = new Point(12, 10);
        this.btnCreate.Name = "btnCreate";
        this.btnCreate.Size = new Size(100, 30);
        this.btnCreate.TabIndex = 0;
        this.btnCreate.Text = "Criar";
        this.btnCreate.UseVisualStyleBackColor = true;
        this.btnCreate.Click += new EventHandler(this.BtnCreate_Click);
        // 
        // btnEdit
        // 
        this.btnEdit.Location = new Point(118, 10);
        this.btnEdit.Name = "btnEdit";
        this.btnEdit.Size = new Size(100, 30);
        this.btnEdit.TabIndex = 1;
        this.btnEdit.Text = "Editar";
        this.btnEdit.UseVisualStyleBackColor = true;
        this.btnEdit.Click += new EventHandler(this.BtnEdit_Click);
        // 
        // btnDelete
        // 
        this.btnDelete.Location = new Point(224, 10);
        this.btnDelete.Name = "btnDelete";
        this.btnDelete.Size = new Size(100, 30);
        this.btnDelete.TabIndex = 2;
        this.btnDelete.Text = "Excluir";
        this.btnDelete.UseVisualStyleBackColor = true;
        this.btnDelete.Click += new EventHandler(this.BtnDelete_Click);
        // 
        // btnEmprestar
        // 
        this.btnEmprestar.BackColor = Color.FromArgb(0, 120, 215);
        this.btnEmprestar.ForeColor = Color.White;
        this.btnEmprestar.Location = new Point(330, 10);
        this.btnEmprestar.Name = "btnEmprestar";
        this.btnEmprestar.Size = new Size(100, 30);
        this.btnEmprestar.TabIndex = 3;
        this.btnEmprestar.Text = "Emprestar";
        this.btnEmprestar.UseVisualStyleBackColor = false;
        this.btnEmprestar.Click += new EventHandler(this.BtnEmprestar_Click);
        // 
        // ItemListForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.Controls.Add(this.dataGridView1);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.titlePanel);
        this.Name = "ItemListForm";
        this.Size = new Size(800, 450);
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        this.panel1.ResumeLayout(false);
        this.titlePanel.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private Button btnCreate;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnEmprestar;
    private Panel panel1;
    private Panel titlePanel;
    private Label titleLabel;
}
