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
        this.btnSave = new Button();
        this.btnCancel = new Button();
        this.lblEmprestimosInfo = new Label();
        this.dgvEmprestimos = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)(this.dgvEmprestimos)).BeginInit();
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
        this.txtName.Size = new Size(560, 23);
        this.txtName.TabIndex = 1;
        // 
        // lblEmprestimosInfo
        // 
        this.lblEmprestimosInfo.AutoSize = true;
        this.lblEmprestimosInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.lblEmprestimosInfo.Location = new Point(20, 80);
        this.lblEmprestimosInfo.Name = "lblEmprestimosInfo";
        this.lblEmprestimosInfo.Size = new Size(300, 15);
        this.lblEmprestimosInfo.TabIndex = 2;
        this.lblEmprestimosInfo.Text = "Empréstimos Pendentes: 0 empréstimo(s) - Totalizando 0 itens";
        this.lblEmprestimosInfo.Visible = false;
        // 
        // dgvEmprestimos
        // 
        this.dgvEmprestimos.AllowUserToAddRows = false;
        this.dgvEmprestimos.AllowUserToDeleteRows = false;
        this.dgvEmprestimos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvEmprestimos.Location = new Point(20, 105);
        this.dgvEmprestimos.Name = "dgvEmprestimos";
        this.dgvEmprestimos.ReadOnly = true;
        this.dgvEmprestimos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.dgvEmprestimos.Size = new Size(760, 250);
        this.dgvEmprestimos.TabIndex = 3;
        this.dgvEmprestimos.Visible = false;
        // 
        // btnSave
        // 
        this.btnSave.Location = new Point(20, 375);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new Size(100, 30);
        this.btnSave.TabIndex = 4;
        this.btnSave.Text = "Salvar";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new Point(126, 375);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(100, 30);
        this.btnCancel.TabIndex = 5;
        this.btnCancel.Text = "Cancelar";
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
        this.Controls.Add(this.dgvEmprestimos);
        this.Controls.Add(this.lblEmprestimosInfo);
        this.Controls.Add(this.txtName);
        this.Controls.Add(this.lblName);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "CongregacaoDetailForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Detalhes da Congregação";
        ((System.ComponentModel.ISupportInitialize)(this.dgvEmprestimos)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label lblName;
    private TextBox txtName;
    private Label lblEmprestimosInfo;
    private DataGridView dgvEmprestimos;
    private Button btnSave;
    private Button btnCancel;
}
