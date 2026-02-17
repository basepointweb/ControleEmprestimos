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
        this.btnClonar = new Button();
        this.btnEmprestar = new Button();
        this.btnRelatorio = new Button();
        this.btnEtiquetas = new Button();
        this.btnListar = new Button();
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
        this.panel1.Controls.Add(this.btnClonar);
        this.panel1.Controls.Add(this.btnEmprestar);
        this.panel1.Controls.Add(this.btnRelatorio);
        this.panel1.Controls.Add(this.btnEtiquetas);
        this.panel1.Controls.Add(this.btnListar);
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
        this.btnCreate.Size = new Size(75, 30);
        this.btnCreate.TabIndex = 0;
        this.btnCreate.Text = "Criar";
        this.btnCreate.UseVisualStyleBackColor = true;
        this.btnCreate.Click += new EventHandler(this.BtnCreate_Click);
        // 
        // btnEdit
        // 
        this.btnEdit.Location = new Point(93, 10);
        this.btnEdit.Name = "btnEdit";
        this.btnEdit.Size = new Size(75, 30);
        this.btnEdit.TabIndex = 1;
        this.btnEdit.Text = "Editar";
        this.btnEdit.UseVisualStyleBackColor = true;
        this.btnEdit.Click += new EventHandler(this.BtnEdit_Click);
        // 
        // btnDelete
        // 
        this.btnDelete.Location = new Point(174, 10);
        this.btnDelete.Name = "btnDelete";
        this.btnDelete.Size = new Size(75, 30);
        this.btnDelete.TabIndex = 2;
        this.btnDelete.Text = "Excluir";
        this.btnDelete.UseVisualStyleBackColor = true;
        this.btnDelete.Click += new EventHandler(this.BtnDelete_Click);
        // 
        // btnClonar
        // 
        this.btnClonar.BackColor = Color.FromArgb(108, 117, 125);
        this.btnClonar.ForeColor = Color.White;
        this.btnClonar.Location = new Point(255, 10);
        this.btnClonar.Name = "btnClonar";
        this.btnClonar.Size = new Size(75, 30);
        this.btnClonar.TabIndex = 3;
        this.btnClonar.Text = "Clonar";
        this.btnClonar.UseVisualStyleBackColor = false;
        this.btnClonar.Click += new EventHandler(this.BtnClonar_Click);
        // 
        // btnEmprestar
        // 
        this.btnEmprestar.BackColor = Color.FromArgb(0, 120, 215);
        this.btnEmprestar.ForeColor = Color.White;
        this.btnEmprestar.Location = new Point(336, 10);
        this.btnEmprestar.Name = "btnEmprestar";
        this.btnEmprestar.Size = new Size(90, 30);
        this.btnEmprestar.TabIndex = 4;
        this.btnEmprestar.Text = "Emprestar";
        this.btnEmprestar.UseVisualStyleBackColor = false;
        this.btnEmprestar.Click += new EventHandler(this.BtnEmprestar_Click);
        // 
        // btnRelatorio
        // 
        this.btnRelatorio.BackColor = Color.FromArgb(220, 53, 69);
        this.btnRelatorio.ForeColor = Color.White;
        this.btnRelatorio.Location = new Point(432, 10);
        this.btnRelatorio.Name = "btnRelatorio";
        this.btnRelatorio.Size = new Size(90, 30);
        this.btnRelatorio.TabIndex = 5;
        this.btnRelatorio.Text = "Relatório";
        this.btnRelatorio.UseVisualStyleBackColor = false;
        this.btnRelatorio.Click += new EventHandler(this.BtnRelatorio_Click);
        // 
        // btnEtiquetas
        // 
        this.btnEtiquetas.BackColor = Color.FromArgb(255, 193, 7);
        this.btnEtiquetas.ForeColor = Color.Black;
        this.btnEtiquetas.Location = new Point(528, 10);
        this.btnEtiquetas.Name = "btnEtiquetas";
        this.btnEtiquetas.Size = new Size(90, 30);
        this.btnEtiquetas.TabIndex = 6;
        this.btnEtiquetas.Text = "Etiquetas";
        this.btnEtiquetas.UseVisualStyleBackColor = false;
        this.btnEtiquetas.Click += new EventHandler(this.BtnEtiquetas_Click);
        // 
        // btnListar
        // 
        this.btnListar.BackColor = Color.FromArgb(40, 167, 69);
        this.btnListar.ForeColor = Color.White;
        this.btnListar.Location = new Point(624, 10);
        this.btnListar.Name = "btnListar";
        this.btnListar.Size = new Size(75, 30);
        this.btnListar.TabIndex = 7;
        this.btnListar.Text = "Listar";
        this.btnListar.UseVisualStyleBackColor = false;
        this.btnListar.Click += new EventHandler(this.BtnListar_Click);
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
    private Button btnClonar;
    private Button btnEmprestar;
    private Button btnRelatorio;
    private Button btnEtiquetas;
    private Button btnListar;
    private Panel panel1;
    private Panel titlePanel;
    private Label titleLabel;
}
