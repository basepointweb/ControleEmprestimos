namespace ControleEmprestimos.Forms;

partial class RecebimentoListForm
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
        this.btnImprimirRecibo = new Button();
        this.panel1 = new Panel();
        this.titlePanel = new Panel();
        this.titleLabel = new Label();
        this.lblDataInicial = new Label();
        this.dtpDataInicial = new DateTimePicker();
        this.lblDataFinal = new Label();
        this.dtpDataFinal = new DateTimePicker();
        this.btnFiltrar = new Button();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        this.panel1.SuspendLayout();
        this.titlePanel.SuspendLayout();
        this.SuspendLayout();
        // 
        // titlePanel
        // 
        this.titlePanel.BackColor = Color.FromArgb(0, 120, 215);
        this.titlePanel.Controls.Add(this.btnFiltrar);
        this.titlePanel.Controls.Add(this.dtpDataFinal);
        this.titlePanel.Controls.Add(this.lblDataFinal);
        this.titlePanel.Controls.Add(this.dtpDataInicial);
        this.titlePanel.Controls.Add(this.lblDataInicial);
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
        this.titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        this.titleLabel.ForeColor = Color.White;
        this.titleLabel.Location = new Point(20, 0);
        this.titleLabel.Name = "titleLabel";
        this.titleLabel.Size = new Size(320, 50);
        this.titleLabel.TabIndex = 0;
        this.titleLabel.Text = "Recebimento de Emprestimo";
        this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDataInicial
        // 
        this.lblDataInicial.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
        this.lblDataInicial.AutoSize = true;
        this.lblDataInicial.Font = new Font("Segoe UI", 9F);
        this.lblDataInicial.ForeColor = Color.White;
        this.lblDataInicial.Location = new Point(420, 8);
        this.lblDataInicial.Name = "lblDataInicial";
        this.lblDataInicial.Size = new Size(70, 15);
        this.lblDataInicial.TabIndex = 1;
        this.lblDataInicial.Text = "Data Inicial:";
        // 
        // dtpDataInicial
        // 
        this.dtpDataInicial.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
        this.dtpDataInicial.Format = DateTimePickerFormat.Short;
        this.dtpDataInicial.Location = new Point(420, 26);
        this.dtpDataInicial.Name = "dtpDataInicial";
        this.dtpDataInicial.Size = new Size(110, 23);
        this.dtpDataInicial.TabIndex = 2;
        // 
        // lblDataFinal
        // 
        this.lblDataFinal.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
        this.lblDataFinal.AutoSize = true;
        this.lblDataFinal.Font = new Font("Segoe UI", 9F);
        this.lblDataFinal.ForeColor = Color.White;
        this.lblDataFinal.Location = new Point(540, 8);
        this.lblDataFinal.Name = "lblDataFinal";
        this.lblDataFinal.Size = new Size(63, 15);
        this.lblDataFinal.TabIndex = 3;
        this.lblDataFinal.Text = "Data Final:";
        // 
        // dtpDataFinal
        // 
        this.dtpDataFinal.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
        this.dtpDataFinal.Format = DateTimePickerFormat.Short;
        this.dtpDataFinal.Location = new Point(540, 26);
        this.dtpDataFinal.Name = "dtpDataFinal";
        this.dtpDataFinal.Size = new Size(110, 23);
        this.dtpDataFinal.TabIndex = 4;
        // 
        // btnFiltrar
        // 
        this.btnFiltrar.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
        this.btnFiltrar.BackColor = Color.FromArgb(40, 167, 69);
        this.btnFiltrar.ForeColor = Color.White;
        this.btnFiltrar.Location = new Point(660, 23);
        this.btnFiltrar.Name = "btnFiltrar";
        this.btnFiltrar.Size = new Size(80, 26);
        this.btnFiltrar.TabIndex = 5;
        this.btnFiltrar.Text = "Filtrar";
        this.btnFiltrar.UseVisualStyleBackColor = false;
        this.btnFiltrar.Click += new EventHandler(this.BtnFiltrar_Click);
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
        this.panel1.Controls.Add(this.btnImprimirRecibo);
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
        // btnImprimirRecibo
        // 
        this.btnImprimirRecibo.BackColor = Color.FromArgb(23, 162, 184);
        this.btnImprimirRecibo.ForeColor = Color.White;
        this.btnImprimirRecibo.Location = new Point(330, 10);
        this.btnImprimirRecibo.Name = "btnImprimirRecibo";
        this.btnImprimirRecibo.Size = new Size(120, 30);
        this.btnImprimirRecibo.TabIndex = 3;
        this.btnImprimirRecibo.Text = "Imprimir Recibo";
        this.btnImprimirRecibo.UseVisualStyleBackColor = false;
        this.btnImprimirRecibo.Click += new EventHandler(this.BtnImprimirRecibo_Click);
        // 
        // RecebimentoListForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.Controls.Add(this.dataGridView1);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.titlePanel);
        this.Name = "RecebimentoListForm";
        this.Size = new Size(800, 450);
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        this.panel1.ResumeLayout(false);
        this.titlePanel.ResumeLayout(false);
        this.titlePanel.PerformLayout();
        this.ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private Button btnCreate;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnImprimirRecibo;
    private Panel panel1;
    private Panel titlePanel;
    private Label titleLabel;
    private Label lblDataInicial;
    private DateTimePicker dtpDataInicial;
    private Label lblDataFinal;
    private DateTimePicker dtpDataFinal;
    private Button btnFiltrar;
}
