namespace ControleEmprestimos.Forms;

partial class CongregacaoDetailForm
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
        this.lblName = new Label();
        this.txtName = new TextBox();
        this.lblEmprestimos = new Label();
        this.dataGridView1 = new DataGridView();
        this.btnReceber = new Button();
        this.btnSave = new Button();
        this.btnCancel = new Button();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        this.SuspendLayout();
        // 
        // lblName
        // 
        this.lblName.AutoSize = true;
        this.lblName.Location = new Point(20, 20);
        this.lblName.Name = "lblName";
        this.lblName.Size = new Size(40, 15);
        this.lblName.TabIndex = 0;
        this.lblName.Text = "Nome:";
        // 
        // txtName
        // 
        this.txtName.Location = new Point(20, 40);
        this.txtName.Name = "txtName";
        this.txtName.Size = new Size(760, 23);
        this.txtName.TabIndex = 1;
        // 
        // lblEmprestimos
        // 
        this.lblEmprestimos.AutoSize = true;
        this.lblEmprestimos.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.lblEmprestimos.Location = new Point(20, 80);
        this.lblEmprestimos.Name = "lblEmprestimos";
        this.lblEmprestimos.Size = new Size(199, 19);
        this.lblEmprestimos.TabIndex = 2;
        this.lblEmprestimos.Text = "Itens Emprestados (Em Andamento):";
        // 
        // dataGridView1
        // 
        this.dataGridView1.AllowUserToAddRows = false;
        this.dataGridView1.AllowUserToDeleteRows = false;
        this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridView1.Location = new Point(20, 110);
        this.dataGridView1.Name = "dataGridView1";
        this.dataGridView1.ReadOnly = true;
        this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.dataGridView1.Size = new Size(760, 250);
        this.dataGridView1.TabIndex = 3;
        // 
        // btnReceber
        // 
        this.btnReceber.BackColor = Color.FromArgb(40, 167, 69);
        this.btnReceber.ForeColor = Color.White;
        this.btnReceber.Location = new Point(20, 370);
        this.btnReceber.Name = "btnReceber";
        this.btnReceber.Size = new Size(120, 30);
        this.btnReceber.TabIndex = 4;
        this.btnReceber.Text = "Receber de Volta";
        this.btnReceber.UseVisualStyleBackColor = false;
        this.btnReceber.Click += new EventHandler(this.BtnReceber_Click);
        // 
        // btnSave
        // 
        this.btnSave.Location = new Point(560, 370);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new Size(100, 30);
        this.btnSave.TabIndex = 5;
        this.btnSave.Text = "Salvar";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new Point(666, 370);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(100, 30);
        this.btnCancel.TabIndex = 6;
        this.btnCancel.Text = "Fechar";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);
        // 
        // CongregacaoDetailForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(800, 420);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.btnReceber);
        this.Controls.Add(this.dataGridView1);
        this.Controls.Add(this.lblEmprestimos);
        this.Controls.Add(this.txtName);
        this.Controls.Add(this.lblName);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "CongregacaoDetailForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Detalhes da Congregacao";
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label lblName;
    private TextBox txtName;
    private Label lblEmprestimos;
    private DataGridView dataGridView1;
    private Button btnReceber;
    private Button btnSave;
    private Button btnCancel;
}
