namespace ControleEmprestimos.Forms;

partial class EmprestimoListForm
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

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.dataGridView1 = new DataGridView();
        this.btnCreate = new Button();
        this.btnEdit = new Button();
        this.btnDelete = new Button();
        this.panel1 = new Panel();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        this.panel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // dataGridView1
        // 
        this.dataGridView1.AllowUserToAddRows = false;
        this.dataGridView1.AllowUserToDeleteRows = false;
        this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridView1.Dock = DockStyle.Fill;
        this.dataGridView1.Location = new Point(0, 0);
        this.dataGridView1.Name = "dataGridView1";
        this.dataGridView1.ReadOnly = true;
        this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.dataGridView1.Size = new Size(800, 400);
        this.dataGridView1.TabIndex = 0;
        // 
        // panel1
        // 
        this.panel1.Controls.Add(this.btnCreate);
        this.panel1.Controls.Add(this.btnEdit);
        this.panel1.Controls.Add(this.btnDelete);
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
        // EmprestimoListForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(800, 450);
        this.Controls.Add(this.dataGridView1);
        this.Controls.Add(this.panel1);
        this.Name = "EmprestimoListForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Emprestimo";
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        this.panel1.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private Button btnCreate;
    private Button btnEdit;
    private Button btnDelete;
    private Panel panel1;
}
