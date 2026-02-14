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
        this.lblQuantity = new Label();
        this.numQuantity = new NumericUpDown();
        this.lblDataRecebimento = new Label();
        this.dtpDataRecebimento = new DateTimePicker();
        this.lblQuemRecebeu = new Label();
        this.txtQuemRecebeu = new TextBox();
        this.btnSave = new Button();
        this.btnCancel = new Button();
        this.btnImprimirRecibo = new Button();
        ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
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
        this.cmbEmprestimo.Size = new Size(560, 23);
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
        this.lblQuemPegou.Location = new Point(20, 140);
        this.lblQuemPegou.Name = "lblQuemPegou";
        this.lblQuemPegou.Size = new Size(130, 15);
        this.lblQuemPegou.TabIndex = 4;
        this.lblQuemPegou.Text = "Quem Pegou Emprestado:";
        // 
        // txtQuemPegou
        // 
        this.txtQuemPegou.Location = new Point(20, 160);
        this.txtQuemPegou.Name = "txtQuemPegou";
        this.txtQuemPegou.ReadOnly = true;
        this.txtQuemPegou.Size = new Size(560, 23);
        this.txtQuemPegou.TabIndex = 5;
        this.txtQuemPegou.BackColor = SystemColors.Control;
        // 
        // lblQuantity
        // 
        this.lblQuantity.AutoSize = true;
        this.lblQuantity.Location = new Point(20, 200);
        this.lblQuantity.Name = "lblQuantity";
        this.lblQuantity.Size = new Size(72, 15);
        this.lblQuantity.TabIndex = 6;
        this.lblQuantity.Text = "Quantidade:";
        // 
        // numQuantity
        // 
        this.numQuantity.Location = new Point(20, 220);
        this.numQuantity.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        this.numQuantity.Name = "numQuantity";
        this.numQuantity.ReadOnly = true;
        this.numQuantity.Size = new Size(120, 23);
        this.numQuantity.TabIndex = 7;
        this.numQuantity.Enabled = false;
        // 
        // lblDataRecebimento
        // 
        this.lblDataRecebimento.AutoSize = true;
        this.lblDataRecebimento.Location = new Point(20, 260);
        this.lblDataRecebimento.Name = "lblDataRecebimento";
        this.lblDataRecebimento.Size = new Size(126, 15);
        this.lblDataRecebimento.TabIndex = 8;
        this.lblDataRecebimento.Text = "Data do Recebimento:";
        // 
        // dtpDataRecebimento
        // 
        this.dtpDataRecebimento.Format = DateTimePickerFormat.Short;
        this.dtpDataRecebimento.Location = new Point(20, 280);
        this.dtpDataRecebimento.Name = "dtpDataRecebimento";
        this.dtpDataRecebimento.Size = new Size(150, 23);
        this.dtpDataRecebimento.TabIndex = 9;
        // 
        // lblQuemRecebeu
        // 
        this.lblQuemRecebeu.AutoSize = true;
        this.lblQuemRecebeu.Location = new Point(20, 320);
        this.lblQuemRecebeu.Name = "lblQuemRecebeu";
        this.lblQuemRecebeu.Size = new Size(115, 15);
        this.lblQuemRecebeu.TabIndex = 10;
        this.lblQuemRecebeu.Text = "Quem Recebeu Volta:";
        // 
        // txtQuemRecebeu
        // 
        this.txtQuemRecebeu.Location = new Point(20, 340);
        this.txtQuemRecebeu.Name = "txtQuemRecebeu";
        this.txtQuemRecebeu.Size = new Size(560, 23);
        this.txtQuemRecebeu.TabIndex = 11;
        // 
        // btnSave
        // 
        this.btnSave.Location = new Point(20, 380);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new Size(100, 30);
        this.btnSave.TabIndex = 12;
        this.btnSave.Text = "Salvar";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new Point(126, 380);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(100, 30);
        this.btnCancel.TabIndex = 13;
        this.btnCancel.Text = "Cancelar";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);
        // 
        // btnImprimirRecibo
        // 
        this.btnImprimirRecibo.BackColor = Color.FromArgb(23, 162, 184);
        this.btnImprimirRecibo.ForeColor = Color.White;
        this.btnImprimirRecibo.Location = new Point(232, 380);
        this.btnImprimirRecibo.Name = "btnImprimirRecibo";
        this.btnImprimirRecibo.Size = new Size(120, 30);
        this.btnImprimirRecibo.TabIndex = 14;
        this.btnImprimirRecibo.Text = "Imprimir Recibo";
        this.btnImprimirRecibo.UseVisualStyleBackColor = false;
        this.btnImprimirRecibo.Visible = false;
        this.btnImprimirRecibo.Click += new EventHandler(this.BtnImprimirRecibo_Click);
        // 
        // RecebimentoDetailForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(600, 430);
        this.Controls.Add(this.btnImprimirRecibo);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.txtQuemRecebeu);
        this.Controls.Add(this.lblQuemRecebeu);
        this.Controls.Add(this.dtpDataRecebimento);
        this.Controls.Add(this.lblDataRecebimento);
        this.Controls.Add(this.numQuantity);
        this.Controls.Add(this.lblQuantity);
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
        this.Text = "Detalhes do Recebimento";
        ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
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
    private Label lblQuantity;
    private NumericUpDown numQuantity;
    private Label lblDataRecebimento;
    private DateTimePicker dtpDataRecebimento;
    private Label lblQuemRecebeu;
    private TextBox txtQuemRecebeu;
    private Button btnSave;
    private Button btnCancel;
    private Button btnImprimirRecibo;
}
