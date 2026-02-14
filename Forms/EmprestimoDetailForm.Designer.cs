namespace ControleEmprestimos.Forms;

partial class EmprestimoDetailForm
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
        this.lblRecebedor = new Label();
        this.txtRecebedor = new TextBox();
        this.lblMotivo = new Label();
        this.txtMotivo = new TextBox();
        this.lblItem = new Label();
        this.cmbItem = new ComboBox();
        this.lblQuantity = new Label();
        this.numQuantity = new NumericUpDown();
        this.lblCongregacao = new Label();
        this.cmbCongregacao = new ComboBox();
        this.lblDataEmprestimo = new Label();
        this.dtpDataEmprestimo = new DateTimePicker();
        this.lblStatus = new Label();
        this.txtStatus = new TextBox();
        this.btnSave = new Button();
        this.btnCancelar = new Button();
        this.btnFechar = new Button();
        ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
        this.SuspendLayout();
        // 
        // lblRecebedor
        // 
        this.lblRecebedor.AutoSize = true;
        this.lblRecebedor.Location = new Point(20, 20);
        this.lblRecebedor.Name = "lblRecebedor";
        this.lblRecebedor.Size = new Size(64, 15);
        this.lblRecebedor.TabIndex = 0;
        this.lblRecebedor.Text = "Recebedor:";
        // 
        // txtRecebedor
        // 
        this.txtRecebedor.Location = new Point(20, 40);
        this.txtRecebedor.Name = "txtRecebedor";
        this.txtRecebedor.Size = new Size(360, 23);
        this.txtRecebedor.TabIndex = 1;
        // 
        // lblMotivo
        // 
        this.lblMotivo.AutoSize = true;
        this.lblMotivo.Location = new Point(20, 75);
        this.lblMotivo.Name = "lblMotivo";
        this.lblMotivo.Size = new Size(48, 15);
        this.lblMotivo.TabIndex = 2;
        this.lblMotivo.Text = "Motivo:";
        // 
        // txtMotivo
        // 
        this.txtMotivo.Location = new Point(20, 95);
        this.txtMotivo.Multiline = true;
        this.txtMotivo.Name = "txtMotivo";
        this.txtMotivo.Size = new Size(360, 50);
        this.txtMotivo.TabIndex = 3;
        // 
        // lblItem
        // 
        this.lblItem.AutoSize = true;
        this.lblItem.Location = new Point(20, 160);
        this.lblItem.Name = "lblItem";
        this.lblItem.Size = new Size(33, 15);
        this.lblItem.TabIndex = 4;
        this.lblItem.Text = "Bem:";
        // 
        // cmbItem
        // 
        this.cmbItem.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbItem.FormattingEnabled = true;
        this.cmbItem.Location = new Point(20, 180);
        this.cmbItem.Name = "cmbItem";
        this.cmbItem.Size = new Size(360, 23);
        this.cmbItem.TabIndex = 5;
        // 
        // lblQuantity
        // 
        this.lblQuantity.AutoSize = true;
        this.lblQuantity.Location = new Point(20, 220);
        this.lblQuantity.Name = "lblQuantity";
        this.lblQuantity.Size = new Size(72, 15);
        this.lblQuantity.TabIndex = 6;
        this.lblQuantity.Text = "Quantidade:";
        // 
        // numQuantity
        // 
        this.numQuantity.Location = new Point(20, 240);
        this.numQuantity.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        this.numQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numQuantity.Name = "numQuantity";
        this.numQuantity.Size = new Size(120, 23);
        this.numQuantity.TabIndex = 7;
        this.numQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblCongregacao
        // 
        this.lblCongregacao.AutoSize = true;
        this.lblCongregacao.Location = new Point(20, 280);
        this.lblCongregacao.Name = "lblCongregacao";
        this.lblCongregacao.Size = new Size(79, 15);
        this.lblCongregacao.TabIndex = 8;
        this.lblCongregacao.Text = "Congregação:";
        // 
        // cmbCongregacao
        // 
        this.cmbCongregacao.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbCongregacao.FormattingEnabled = true;
        this.cmbCongregacao.Location = new Point(20, 300);
        this.cmbCongregacao.Name = "cmbCongregacao";
        this.cmbCongregacao.Size = new Size(360, 23);
        this.cmbCongregacao.TabIndex = 9;
        // 
        // lblDataEmprestimo
        // 
        this.lblDataEmprestimo.AutoSize = true;
        this.lblDataEmprestimo.Location = new Point(20, 340);
        this.lblDataEmprestimo.Name = "lblDataEmprestimo";
        this.lblDataEmprestimo.Size = new Size(119, 15);
        this.lblDataEmprestimo.TabIndex = 10;
        this.lblDataEmprestimo.Text = "Data do Empréstimo:";
        // 
        // dtpDataEmprestimo
        // 
        this.dtpDataEmprestimo.Format = DateTimePickerFormat.Short;
        this.dtpDataEmprestimo.Location = new Point(20, 360);
        this.dtpDataEmprestimo.Name = "dtpDataEmprestimo";
        this.dtpDataEmprestimo.Size = new Size(150, 23);
        this.dtpDataEmprestimo.TabIndex = 11;
        // 
        // lblStatus
        // 
        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new Point(20, 400);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new Size(42, 15);
        this.lblStatus.TabIndex = 12;
        this.lblStatus.Text = "Status:";
        // 
        // txtStatus
        // 
        this.txtStatus.Location = new Point(20, 420);
        this.txtStatus.Name = "txtStatus";
        this.txtStatus.ReadOnly = true;
        this.txtStatus.Size = new Size(150, 23);
        this.txtStatus.TabIndex = 13;
        this.txtStatus.BackColor = SystemColors.Control;
        // 
        // btnSave
        // 
        this.btnSave.Location = new Point(20, 470);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new Size(100, 30);
        this.btnSave.TabIndex = 14;
        this.btnSave.Text = "Salvar";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        // 
        // btnCancelar
        // 
        this.btnCancelar.Location = new Point(126, 470);
        this.btnCancelar.Name = "btnCancelar";
        this.btnCancelar.Size = new Size(120, 30);
        this.btnCancelar.TabIndex = 15;
        this.btnCancelar.Text = "Cancelar Empréstimo";
        this.btnCancelar.UseVisualStyleBackColor = true;
        this.btnCancelar.Visible = false;
        this.btnCancelar.Click += new EventHandler(this.BtnCancelarEmprestimo_Click);
        // 
        // btnFechar
        // 
        this.btnFechar.Location = new Point(252, 470);
        this.btnFechar.Name = "btnFechar";
        this.btnFechar.Size = new Size(100, 30);
        this.btnFechar.TabIndex = 16;
        this.btnFechar.Text = "Fechar";
        this.btnFechar.UseVisualStyleBackColor = true;
        this.btnFechar.Click += new EventHandler(this.BtnFechar_Click);
        // 
        // EmprestimoDetailForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(400, 520);
        this.Controls.Add(this.btnFechar);
        this.Controls.Add(this.btnCancelar);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.txtStatus);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.dtpDataEmprestimo);
        this.Controls.Add(this.lblDataEmprestimo);
        this.Controls.Add(this.cmbCongregacao);
        this.Controls.Add(this.lblCongregacao);
        this.Controls.Add(this.numQuantity);
        this.Controls.Add(this.lblQuantity);
        this.Controls.Add(this.cmbItem);
        this.Controls.Add(this.lblItem);
        this.Controls.Add(this.txtMotivo);
        this.Controls.Add(this.lblMotivo);
        this.Controls.Add(this.txtRecebedor);
        this.Controls.Add(this.lblRecebedor);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "EmprestimoDetailForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Detalhes do Emprestimo";
        ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label lblRecebedor;
    private TextBox txtRecebedor;
    private Label lblMotivo;
    private TextBox txtMotivo;
    private Label lblItem;
    private ComboBox cmbItem;
    private Label lblQuantity;
    private NumericUpDown numQuantity;
    private Label lblCongregacao;
    private ComboBox cmbCongregacao;
    private Label lblDataEmprestimo;
    private DateTimePicker dtpDataEmprestimo;
    private Label lblStatus;
    private TextBox txtStatus;
    private Button btnSave;
    private Button btnCancelar;
    private Button btnFechar;
}
