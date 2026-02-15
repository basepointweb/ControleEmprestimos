namespace ControleEmprestimos.Forms;

partial class RecebimentoDetailForm
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
        this.lblEmprestimo = new Label();
        this.cmbEmprestimo = new ComboBox();
        this.lblDataEmprestimo = new Label();
        this.txtDataEmprestimo = new TextBox();
        this.lblQuemPegou = new Label();
        this.txtQuemPegou = new TextBox();
        this.lblDataRecebimento = new Label();
        this.dtpDataRecebimento = new DateTimePicker();
        this.lblQuemRecebeu = new Label();
        this.txtQuemRecebeu = new TextBox();
        this.lblItensReceber = new Label();
        this.dgvItensReceber = new DataGridView();
        this.lblTotalRecebido = new Label();
        this.btnSave = new Button();
        this.btnCancel = new Button();
        this.btnImprimirRecibo = new Button();
        ((System.ComponentModel.ISupportInitialize)(this.dgvItensReceber)).BeginInit();
        this.SuspendLayout();
        // 
        // lblEmprestimo
        // 
        this.lblEmprestimo.AutoSize = true;
        this.lblEmprestimo.Location = new Point(20, 20);
        this.lblEmprestimo.Name = "lblEmprestimo";
        this.lblEmprestimo.Size = new Size(71, 15);
        this.lblEmprestimo.TabIndex = 0;
        this.lblEmprestimo.Text = "Empréstimo:";
        // 
        // cmbEmprestimo
        // 
        this.cmbEmprestimo.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbEmprestimo.FormattingEnabled = true;
        this.cmbEmprestimo.Location = new Point(20, 40);
        this.cmbEmprestimo.Name = "cmbEmprestimo";
        this.cmbEmprestimo.Size = new Size(660, 23);
        this.cmbEmprestimo.TabIndex = 1;
        this.cmbEmprestimo.SelectedIndexChanged += new EventHandler(this.CmbEmprestimo_SelectedIndexChanged);
        // 
        // lblDataEmprestimo
        // 
        this.lblDataEmprestimo.AutoSize = true;
        this.lblDataEmprestimo.Location = new Point(20, 80);
        this.lblDataEmprestimo.Name = "lblDataEmprestimo";
        this.lblDataEmprestimo.Size = new Size(119, 15);
        this.lblDataEmprestimo.TabIndex = 2;
        this.lblDataEmprestimo.Text = "Data do Empréstimo:";
        // 
        // txtDataEmprestimo
        // 
        this.txtDataEmprestimo.Location = new Point(20, 100);
        this.txtDataEmprestimo.Name = "txtDataEmprestimo";
        this.txtDataEmprestimo.ReadOnly = true;
        this.txtDataEmprestimo.Size = new Size(150, 23);
        this.txtDataEmprestimo.TabIndex = 3;
        this.txtDataEmprestimo.BackColor = SystemColors.Control;
        // 
        // lblQuemPegou
        // 
        this.lblQuemPegou.AutoSize = true;
        this.lblQuemPegou.Location = new Point(200, 80);
        this.lblQuemPegou.Name = "lblQuemPegou";
        this.lblQuemPegou.Size = new Size(130, 15);
        this.lblQuemPegou.TabIndex = 4;
        this.lblQuemPegou.Text = "Quem Pegou Emprestado:";
        // 
        // txtQuemPegou
        // 
        this.txtQuemPegou.Location = new Point(200, 100);
        this.txtQuemPegou.Name = "txtQuemPegou";
        this.txtQuemPegou.ReadOnly = true;
        this.txtQuemPegou.Size = new Size(480, 23);
        this.txtQuemPegou.TabIndex = 5;
        this.txtQuemPegou.BackColor = SystemColors.Control;
        // 
        // lblDataRecebimento
        // 
        this.lblDataRecebimento.AutoSize = true;
        this.lblDataRecebimento.Location = new Point(20, 140);
        this.lblDataRecebimento.Name = "lblDataRecebimento";
        this.lblDataRecebimento.Size = new Size(126, 15);
        this.lblDataRecebimento.TabIndex = 6;
        this.lblDataRecebimento.Text = "Data da Devolução:";
        // 
        // dtpDataRecebimento
        // 
        this.dtpDataRecebimento.Format = DateTimePickerFormat.Short;
        this.dtpDataRecebimento.Location = new Point(20, 160);
        this.dtpDataRecebimento.Name = "dtpDataRecebimento";
        this.dtpDataRecebimento.Size = new Size(150, 23);
        this.dtpDataRecebimento.TabIndex = 7;
        // 
        // lblQuemRecebeu
        // 
        this.lblQuemRecebeu.AutoSize = true;
        this.lblQuemRecebeu.Location = new Point(200, 140);
        this.lblQuemRecebeu.Name = "lblQuemRecebeu";
        this.lblQuemRecebeu.Size = new Size(115, 15);
        this.lblQuemRecebeu.TabIndex = 8;
        this.lblQuemRecebeu.Text = "Quem Recebeu de Volta:";
        // 
        // txtQuemRecebeu
        // 
        this.txtQuemRecebeu.Location = new Point(200, 160);
        this.txtQuemRecebeu.Name = "txtQuemRecebeu";
        this.txtQuemRecebeu.Size = new Size(480, 23);
        this.txtQuemRecebeu.TabIndex = 9;
        // 
        // lblItensReceber
        // 
        this.lblItensReceber.AutoSize = true;
        this.lblItensReceber.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.lblItensReceber.Location = new Point(20, 200);
        this.lblItensReceber.Name = "lblItensReceber";
        this.lblItensReceber.Size = new Size(110, 19);
        this.lblItensReceber.TabIndex = 10;
        this.lblItensReceber.Text = "Bens a Devolver:";
        // 
        // dgvItensReceber
        // 
        this.dgvItensReceber.AllowUserToAddRows = false;
        this.dgvItensReceber.AllowUserToDeleteRows = false;
        this.dgvItensReceber.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvItensReceber.Location = new Point(20, 225);
        this.dgvItensReceber.Name = "dgvItensReceber";
        this.dgvItensReceber.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.dgvItensReceber.Size = new Size(660, 200);
        this.dgvItensReceber.TabIndex = 11;
        // 
        // lblTotalRecebido
        // 
        this.lblTotalRecebido.AutoSize = true;
        this.lblTotalRecebido.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.lblTotalRecebido.Location = new Point(20, 435);
        this.lblTotalRecebido.Name = "lblTotalRecebido";
        this.lblTotalRecebido.Size = new Size(150, 15);
        this.lblTotalRecebido.TabIndex = 12;
        this.lblTotalRecebido.Text = "Total a Devolver: 0 itens";
        // 
        // btnSave
        // 
        this.btnSave.Location = new Point(20, 465);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new Size(100, 30);
        this.btnSave.TabIndex = 13;
        this.btnSave.Text = "Salvar";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new Point(126, 465);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(100, 30);
        this.btnCancel.TabIndex = 14;
        this.btnCancel.Text = "Cancelar";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);
        // 
        // btnImprimirRecibo
        // 
        this.btnImprimirRecibo.BackColor = Color.FromArgb(23, 162, 184);
        this.btnImprimirRecibo.ForeColor = Color.White;
        this.btnImprimirRecibo.Location = new Point(232, 465);
        this.btnImprimirRecibo.Name = "btnImprimirRecibo";
        this.btnImprimirRecibo.Size = new Size(120, 30);
        this.btnImprimirRecibo.TabIndex = 15;
        this.btnImprimirRecibo.Text = "Imprimir Recibo";
        this.btnImprimirRecibo.UseVisualStyleBackColor = false;
        this.btnImprimirRecibo.Visible = false;
        this.btnImprimirRecibo.Click += new EventHandler(this.BtnImprimirRecibo_Click);
        // 
        // RecebimentoDetailForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(700, 515);
        this.Controls.Add(this.btnImprimirRecibo);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.lblTotalRecebido);
        this.Controls.Add(this.dgvItensReceber);
        this.Controls.Add(this.lblItensReceber);
        this.Controls.Add(this.txtQuemRecebeu);
        this.Controls.Add(this.lblQuemRecebeu);
        this.Controls.Add(this.dtpDataRecebimento);
        this.Controls.Add(this.lblDataRecebimento);
        this.Controls.Add(this.txtQuemPegou);
        this.Controls.Add(this.lblQuemPegou);
        this.Controls.Add(this.txtDataEmprestimo);
        this.Controls.Add(this.lblDataEmprestimo);
        this.Controls.Add(this.cmbEmprestimo);
        this.Controls.Add(this.lblEmprestimo);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "RecebimentoDetailForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Devolução de Bens";
        ((System.ComponentModel.ISupportInitialize)(this.dgvItensReceber)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label lblEmprestimo;
    private ComboBox cmbEmprestimo;
    private Label lblDataEmprestimo;
    private TextBox txtDataEmprestimo;
    private Label lblQuemPegou;
    private TextBox txtQuemPegou;
    private Label lblDataRecebimento;
    private DateTimePicker dtpDataRecebimento;
    private Label lblQuemRecebeu;
    private TextBox txtQuemRecebeu;
    private Label lblItensReceber;
    private DataGridView dgvItensReceber;
    private Label lblTotalRecebido;
    private Button btnSave;
    private Button btnCancel;
    private Button btnImprimirRecibo;
}
