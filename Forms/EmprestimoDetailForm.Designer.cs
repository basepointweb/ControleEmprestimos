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
        this.lblCongregacao = new Label();
        this.cmbCongregacao = new ComboBox();
        this.lblDataEmprestimo = new Label();
        this.dtpDataEmprestimo = new DateTimePicker();
        this.lblQuemLiberou = new Label();
        this.txtQuemLiberou = new TextBox();
        this.lblStatus = new Label();
        this.txtStatus = new TextBox();
        this.lblItens = new Label();
        this.dgvItens = new DataGridView();
        this.lblItem = new Label();
        this.txtItemId = new TextBox();
        this.lblBem = new Label();
        this.cmbItem = new ComboBox();
        this.lblQuantity = new Label();
        this.numQuantity = new NumericUpDown();
        this.btnAdicionarItem = new Button();
        this.lblTotalItens = new Label();
        this.btnSave = new Button();
        this.btnFechar = new Button();
        ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
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
        this.txtRecebedor.Size = new Size(560, 23);
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
        this.txtMotivo.Size = new Size(560, 50);
        this.txtMotivo.TabIndex = 3;
        // 
        // lblCongregacao
        // 
        this.lblCongregacao.AutoSize = true;
        this.lblCongregacao.Location = new Point(20, 160);
        this.lblCongregacao.Name = "lblCongregacao";
        this.lblCongregacao.Size = new Size(79, 15);
        this.lblCongregacao.TabIndex = 4;
        this.lblCongregacao.Text = "Congregação:";
        // 
        // cmbCongregacao
        // 
        this.cmbCongregacao.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbCongregacao.FormattingEnabled = true;
        this.cmbCongregacao.Location = new Point(20, 180);
        this.cmbCongregacao.Name = "cmbCongregacao";
        this.cmbCongregacao.Size = new Size(360, 23);
        this.cmbCongregacao.TabIndex = 5;
        // 
        // lblDataEmprestimo
        // 
        this.lblDataEmprestimo.AutoSize = true;
        this.lblDataEmprestimo.Location = new Point(400, 160);
        this.lblDataEmprestimo.Name = "lblDataEmprestimo";
        this.lblDataEmprestimo.Size = new Size(119, 15);
        this.lblDataEmprestimo.TabIndex = 6;
        this.lblDataEmprestimo.Text = "Data do Empréstimo:";
        // 
        // dtpDataEmprestimo
        // 
        this.dtpDataEmprestimo.Format = DateTimePickerFormat.Short;
        this.dtpDataEmprestimo.Location = new Point(400, 180);
        this.dtpDataEmprestimo.Name = "dtpDataEmprestimo";
        this.dtpDataEmprestimo.Size = new Size(180, 23);
        this.dtpDataEmprestimo.TabIndex = 7;
        // 
        // lblQuemLiberou
        // 
        this.lblQuemLiberou.AutoSize = true;
        this.lblQuemLiberou.Location = new Point(20, 220);
        this.lblQuemLiberou.Name = "lblQuemLiberou";
        this.lblQuemLiberou.Size = new Size(86, 15);
        this.lblQuemLiberou.TabIndex = 8;
        this.lblQuemLiberou.Text = "Quem Liberou:";
        // 
        // txtQuemLiberou
        // 
        this.txtQuemLiberou.Location = new Point(20, 240);
        this.txtQuemLiberou.Name = "txtQuemLiberou";
        this.txtQuemLiberou.Size = new Size(360, 23);
        this.txtQuemLiberou.TabIndex = 9;
        // 
        // lblStatus
        // 
        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new Point(400, 220);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new Size(42, 15);
        this.lblStatus.TabIndex = 10;
        this.lblStatus.Text = "Status:";
        this.lblStatus.Visible = false;
        // 
        // txtStatus
        // 
        this.txtStatus.Location = new Point(400, 240);
        this.txtStatus.Name = "txtStatus";
        this.txtStatus.ReadOnly = true;
        this.txtStatus.Size = new Size(180, 23);
        this.txtStatus.TabIndex = 11;
        this.txtStatus.BackColor = SystemColors.Control;
        this.txtStatus.Visible = false;
        // 
        // lblItens
        // 
        this.lblItens.AutoSize = true;
        this.lblItens.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.lblItens.Location = new Point(20, 280);
        this.lblItens.Name = "lblItens";
        this.lblItens.Size = new Size(135, 19);
        this.lblItens.TabIndex = 12;
        this.lblItens.Text = "Itens do Empréstimo:";
        // 
        // dgvItens
        // 
        this.dgvItens.AllowUserToAddRows = false;
        this.dgvItens.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvItens.Location = new Point(20, 305);
        this.dgvItens.Name = "dgvItens";
        this.dgvItens.ReadOnly = true;
        this.dgvItens.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.dgvItens.Size = new Size(560, 150);
        this.dgvItens.TabIndex = 13;
        // 
        // lblItem
        // 
        this.lblItem.AutoSize = true;
        this.lblItem.Location = new Point(20, 470);
        this.lblItem.Name = "lblItem";
        this.lblItem.Size = new Size(21, 15);
        this.lblItem.TabIndex = 14;
        this.lblItem.Text = "ID:";
        // 
        // txtItemId
        // 
        this.txtItemId.Location = new Point(20, 490);
        this.txtItemId.Name = "txtItemId";
        this.txtItemId.Size = new Size(60, 23);
        this.txtItemId.TabIndex = 15;
        this.txtItemId.KeyPress += new KeyPressEventHandler(this.TxtItemId_KeyPress);
        // 
        // lblBem
        // 
        this.lblBem.AutoSize = true;
        this.lblBem.Location = new Point(90, 470);
        this.lblBem.Name = "lblBem";
        this.lblBem.Size = new Size(33, 15);
        this.lblBem.TabIndex = 16;
        this.lblBem.Text = "Bem:";
        // 
        // cmbItem
        // 
        this.cmbItem.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbItem.FormattingEnabled = true;
        this.cmbItem.Location = new Point(90, 490);
        this.cmbItem.Name = "cmbItem";
        this.cmbItem.Size = new Size(230, 23);
        this.cmbItem.TabIndex = 17;
        // 
        // lblQuantity
        // 
        this.lblQuantity.AutoSize = true;
        this.lblQuantity.Location = new Point(330, 470);
        this.lblQuantity.Name = "lblQuantity";
        this.lblQuantity.Size = new Size(72, 15);
        this.lblQuantity.TabIndex = 18;
        this.lblQuantity.Text = "Quantidade:";
        // 
        // numQuantity
        // 
        this.numQuantity.Location = new Point(330, 490);
        this.numQuantity.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        this.numQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numQuantity.Name = "numQuantity";
        this.numQuantity.Size = new Size(120, 23);
        this.numQuantity.TabIndex = 19;
        this.numQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // btnAdicionarItem
        // 
        this.btnAdicionarItem.BackColor = Color.FromArgb(0, 120, 215);
        this.btnAdicionarItem.ForeColor = Color.White;
        this.btnAdicionarItem.Location = new Point(460, 488);
        this.btnAdicionarItem.Name = "btnAdicionarItem";
        this.btnAdicionarItem.Size = new Size(120, 27);
        this.btnAdicionarItem.TabIndex = 20;
        this.btnAdicionarItem.Text = "Adicionar Item";
        this.btnAdicionarItem.UseVisualStyleBackColor = false;
        this.btnAdicionarItem.Click += new EventHandler(this.BtnAdicionarItem_Click);
        // 
        // lblTotalItens
        // 
        this.lblTotalItens.AutoSize = true;
        this.lblTotalItens.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.lblTotalItens.Location = new Point(20, 525);
        this.lblTotalItens.Name = "lblTotalItens";
        this.lblTotalItens.Size = new Size(100, 15);
        this.lblTotalItens.TabIndex = 21;
        this.lblTotalItens.Text = "Total de Itens: 0";
        // 
        // btnSave
        // 
        this.btnSave.Location = new Point(20, 560);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new Size(100, 30);
        this.btnSave.TabIndex = 22;
        this.btnSave.Text = "Salvar";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        // 
        // btnFechar
        // 
        this.btnFechar.Location = new Point(126, 560);
        this.btnFechar.Name = "btnFechar";
        this.btnFechar.Size = new Size(100, 30);
        this.btnFechar.TabIndex = 24;
        this.btnFechar.Text = "Fechar";
        this.btnFechar.UseVisualStyleBackColor = true;
        this.btnFechar.Click += new EventHandler(this.BtnFechar_Click);
        // 
        // EmprestimoDetailForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(600, 610);
        this.Controls.Add(this.btnFechar);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.lblTotalItens);
        this.Controls.Add(this.btnAdicionarItem);
        this.Controls.Add(this.numQuantity);
        this.Controls.Add(this.lblQuantity);
        this.Controls.Add(this.cmbItem);
        this.Controls.Add(this.lblBem);
        this.Controls.Add(this.txtItemId);
        this.Controls.Add(this.lblItem);
        this.Controls.Add(this.dgvItens);
        this.Controls.Add(this.lblItens);
        this.Controls.Add(this.txtStatus);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.txtQuemLiberou);
        this.Controls.Add(this.lblQuemLiberou);
        this.Controls.Add(this.dtpDataEmprestimo);
        this.Controls.Add(this.lblDataEmprestimo);
        this.Controls.Add(this.cmbCongregacao);
        this.Controls.Add(this.lblCongregacao);
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
        ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label lblRecebedor;
    private TextBox txtRecebedor;
    private Label lblMotivo;
    private TextBox txtMotivo;
    private Label lblCongregacao;
    private ComboBox cmbCongregacao;
    private Label lblDataEmprestimo;
    private DateTimePicker dtpDataEmprestimo;
    private Label lblQuemLiberou;
    private TextBox txtQuemLiberou;
    private Label lblStatus;
    private TextBox txtStatus;
    private Label lblItens;
    private DataGridView dgvItens;
    private Label lblItem;
    private TextBox txtItemId;
    private Label lblBem;
    private ComboBox cmbItem;
    private Label lblQuantity;
    private NumericUpDown numQuantity;
    private Button btnAdicionarItem;
    private Label lblTotalItens;
    private Button btnSave;
    private Button btnFechar;
}
