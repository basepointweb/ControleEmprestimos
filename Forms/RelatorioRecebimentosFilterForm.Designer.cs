namespace ControleEmprestimos.Forms;

partial class RelatorioRecebimentosFilterForm
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
        this.lblTitulo = new Label();
        this.lblDataInicial = new Label();
        this.dtpDataInicial = new DateTimePicker();
        this.lblDataFinal = new Label();
        this.dtpDataFinal = new DateTimePicker();
        this.lblCongregacao = new Label();
        this.cmbCongregacao = new ComboBox();
        this.lblItem = new Label();
        this.cmbItem = new ComboBox();
        this.btnGerar = new Button();
        this.btnCancelar = new Button();
        this.SuspendLayout();
        // 
        // lblTitulo
        // 
        this.lblTitulo.AutoSize = true;
        this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        this.lblTitulo.Location = new Point(20, 20);
        this.lblTitulo.Name = "lblTitulo";
        this.lblTitulo.Size = new Size(280, 25);
        this.lblTitulo.TabIndex = 0;
        this.lblTitulo.Text = "Relatório de Recebimentos";
        // 
        // lblDataInicial
        // 
        this.lblDataInicial.AutoSize = true;
        this.lblDataInicial.Location = new Point(20, 70);
        this.lblDataInicial.Name = "lblDataInicial";
        this.lblDataInicial.Size = new Size(70, 15);
        this.lblDataInicial.TabIndex = 1;
        this.lblDataInicial.Text = "Data Inicial:";
        // 
        // dtpDataInicial
        // 
        this.dtpDataInicial.Format = DateTimePickerFormat.Short;
        this.dtpDataInicial.Location = new Point(20, 90);
        this.dtpDataInicial.Name = "dtpDataInicial";
        this.dtpDataInicial.Size = new Size(150, 23);
        this.dtpDataInicial.TabIndex = 2;
        // 
        // lblDataFinal
        // 
        this.lblDataFinal.AutoSize = true;
        this.lblDataFinal.Location = new Point(190, 70);
        this.lblDataFinal.Name = "lblDataFinal";
        this.lblDataFinal.Size = new Size(63, 15);
        this.lblDataFinal.TabIndex = 3;
        this.lblDataFinal.Text = "Data Final:";
        // 
        // dtpDataFinal
        // 
        this.dtpDataFinal.Format = DateTimePickerFormat.Short;
        this.dtpDataFinal.Location = new Point(190, 90);
        this.dtpDataFinal.Name = "dtpDataFinal";
        this.dtpDataFinal.Size = new Size(150, 23);
        this.dtpDataFinal.TabIndex = 4;
        // 
        // lblCongregacao
        // 
        this.lblCongregacao.AutoSize = true;
        this.lblCongregacao.Location = new Point(20, 130);
        this.lblCongregacao.Name = "lblCongregacao";
        this.lblCongregacao.Size = new Size(80, 15);
        this.lblCongregacao.TabIndex = 5;
        this.lblCongregacao.Text = "Congregação:";
        // 
        // cmbCongregacao
        // 
        this.cmbCongregacao.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbCongregacao.FormattingEnabled = true;
        this.cmbCongregacao.Location = new Point(20, 150);
        this.cmbCongregacao.Name = "cmbCongregacao";
        this.cmbCongregacao.Size = new Size(320, 23);
        this.cmbCongregacao.TabIndex = 6;
        // 
        // lblItem
        // 
        this.lblItem.AutoSize = true;
        this.lblItem.Location = new Point(20, 190);
        this.lblItem.Name = "lblItem";
        this.lblItem.Size = new Size(34, 15);
        this.lblItem.TabIndex = 7;
        this.lblItem.Text = "Bem:";
        // 
        // cmbItem
        // 
        this.cmbItem.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbItem.FormattingEnabled = true;
        this.cmbItem.Location = new Point(20, 210);
        this.cmbItem.Name = "cmbItem";
        this.cmbItem.Size = new Size(320, 23);
        this.cmbItem.TabIndex = 8;
        // 
        // btnGerar
        // 
        this.btnGerar.BackColor = Color.FromArgb(0, 120, 215);
        this.btnGerar.ForeColor = Color.White;
        this.btnGerar.Location = new Point(20, 260);
        this.btnGerar.Name = "btnGerar";
        this.btnGerar.Size = new Size(150, 35);
        this.btnGerar.TabIndex = 9;
        this.btnGerar.Text = "Gerar Relatório";
        this.btnGerar.UseVisualStyleBackColor = false;
        this.btnGerar.Click += new EventHandler(this.BtnGerar_Click);
        // 
        // btnCancelar
        // 
        this.btnCancelar.Location = new Point(190, 260);
        this.btnCancelar.Name = "btnCancelar";
        this.btnCancelar.Size = new Size(150, 35);
        this.btnCancelar.TabIndex = 10;
        this.btnCancelar.Text = "Cancelar";
        this.btnCancelar.UseVisualStyleBackColor = true;
        this.btnCancelar.Click += new EventHandler(this.BtnCancelar_Click);
        // 
        // RelatorioRecebimentosFilterForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(360, 320);
        this.Controls.Add(this.btnCancelar);
        this.Controls.Add(this.btnGerar);
        this.Controls.Add(this.cmbItem);
        this.Controls.Add(this.lblItem);
        this.Controls.Add(this.cmbCongregacao);
        this.Controls.Add(this.lblCongregacao);
        this.Controls.Add(this.dtpDataFinal);
        this.Controls.Add(this.lblDataFinal);
        this.Controls.Add(this.dtpDataInicial);
        this.Controls.Add(this.lblDataInicial);
        this.Controls.Add(this.lblTitulo);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "RelatorioRecebimentosFilterForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Filtros - Relatório de Recebimentos";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label lblTitulo;
    private Label lblDataInicial;
    private DateTimePicker dtpDataInicial;
    private Label lblDataFinal;
    private DateTimePicker dtpDataFinal;
    private Label lblCongregacao;
    private ComboBox cmbCongregacao;
    private Label lblItem;
    private ComboBox cmbItem;
    private Button btnGerar;
    private Button btnCancelar;
}
