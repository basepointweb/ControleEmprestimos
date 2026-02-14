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
        dataGridView1 = new DataGridView();
        btnCreate = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnImprimirRecibo = new Button();
        btnRelatorio = new Button();
        panel1 = new Panel();
        titlePanel = new Panel();
        btnFiltrar = new Button();
        dtpDataFinal = new DateTimePicker();
        lblDataFinal = new Label();
        dtpDataInicial = new DateTimePicker();
        lblDataInicial = new Label();
        titleLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        panel1.SuspendLayout();
        titlePanel.SuspendLayout();
        SuspendLayout();
        // 
        // dataGridView1
        // 
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.Location = new Point(0, 67);
        dataGridView1.Margin = new Padding(3, 4, 3, 4);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.ReadOnly = true;
        dataGridView1.RowHeadersWidth = 51;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.Size = new Size(914, 466);
        dataGridView1.TabIndex = 0;
        // 
        // btnCreate
        // 
        btnCreate.Location = new Point(14, 13);
        btnCreate.Margin = new Padding(3, 4, 3, 4);
        btnCreate.Name = "btnCreate";
        btnCreate.Size = new Size(114, 40);
        btnCreate.TabIndex = 0;
        btnCreate.Text = "Criar";
        btnCreate.UseVisualStyleBackColor = true;
        btnCreate.Click += BtnCreate_Click;
        // 
        // btnEdit
        // 
        btnEdit.Location = new Point(135, 13);
        btnEdit.Margin = new Padding(3, 4, 3, 4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(114, 40);
        btnEdit.TabIndex = 1;
        btnEdit.Text = "Editar";
        btnEdit.UseVisualStyleBackColor = true;
        btnEdit.Click += BtnEdit_Click;
        // 
        // btnDelete
        // 
        btnDelete.Location = new Point(256, 13);
        btnDelete.Margin = new Padding(3, 4, 3, 4);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(114, 40);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "Excluir";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += BtnDelete_Click;
        // 
        // btnImprimirRecibo
        // 
        btnImprimirRecibo.BackColor = Color.FromArgb(23, 162, 184);
        btnImprimirRecibo.ForeColor = Color.White;
        btnImprimirRecibo.Location = new Point(377, 13);
        btnImprimirRecibo.Margin = new Padding(3, 4, 3, 4);
        btnImprimirRecibo.Name = "btnImprimirRecibo";
        btnImprimirRecibo.Size = new Size(137, 40);
        btnImprimirRecibo.TabIndex = 3;
        btnImprimirRecibo.Text = "Imprimir Recibo";
        btnImprimirRecibo.UseVisualStyleBackColor = false;
        btnImprimirRecibo.Click += BtnImprimirRecibo_Click;
        // 
        // btnRelatorio
        // 
        btnRelatorio.BackColor = Color.FromArgb(220, 53, 69);
        btnRelatorio.ForeColor = Color.White;
        btnRelatorio.Location = new Point(521, 13);
        btnRelatorio.Margin = new Padding(3, 4, 3, 4);
        btnRelatorio.Name = "btnRelatorio";
        btnRelatorio.Size = new Size(114, 40);
        btnRelatorio.TabIndex = 4;
        btnRelatorio.Text = "Relatório";
        btnRelatorio.UseVisualStyleBackColor = false;
        btnRelatorio.Click += BtnRelatorio_Click;
        // 
        // panel1
        // 
        panel1.Controls.Add(btnCreate);
        panel1.Controls.Add(btnEdit);
        panel1.Controls.Add(btnDelete);
        panel1.Controls.Add(btnImprimirRecibo);
        panel1.Controls.Add(btnRelatorio);
        panel1.Dock = DockStyle.Bottom;
        panel1.Location = new Point(0, 533);
        panel1.Margin = new Padding(3, 4, 3, 4);
        panel1.Name = "panel1";
        panel1.Size = new Size(914, 67);
        panel1.TabIndex = 1;
        // 
        // titlePanel
        // 
        titlePanel.BackColor = Color.FromArgb(0, 120, 215);
        titlePanel.Controls.Add(btnFiltrar);
        titlePanel.Controls.Add(dtpDataFinal);
        titlePanel.Controls.Add(lblDataFinal);
        titlePanel.Controls.Add(dtpDataInicial);
        titlePanel.Controls.Add(lblDataInicial);
        titlePanel.Controls.Add(titleLabel);
        titlePanel.Dock = DockStyle.Top;
        titlePanel.Location = new Point(0, 0);
        titlePanel.Margin = new Padding(3, 4, 3, 4);
        titlePanel.Name = "titlePanel";
        titlePanel.Size = new Size(914, 67);
        titlePanel.TabIndex = 2;
        // 
        // btnFiltrar
        // 
        btnFiltrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnFiltrar.BackColor = Color.FromArgb(40, 167, 69);
        btnFiltrar.ForeColor = Color.White;
        btnFiltrar.Location = new Point(754, 31);
        btnFiltrar.Margin = new Padding(3, 4, 3, 4);
        btnFiltrar.Name = "btnFiltrar";
        btnFiltrar.Size = new Size(91, 35);
        btnFiltrar.TabIndex = 5;
        btnFiltrar.Text = "Filtrar";
        btnFiltrar.UseVisualStyleBackColor = false;
        btnFiltrar.Click += BtnFiltrar_Click;
        // 
        // dtpDataFinal
        // 
        dtpDataFinal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        dtpDataFinal.Format = DateTimePickerFormat.Short;
        dtpDataFinal.Location = new Point(617, 35);
        dtpDataFinal.Margin = new Padding(3, 4, 3, 4);
        dtpDataFinal.Name = "dtpDataFinal";
        dtpDataFinal.Size = new Size(125, 27);
        dtpDataFinal.TabIndex = 4;
        // 
        // lblDataFinal
        // 
        lblDataFinal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDataFinal.AutoSize = true;
        lblDataFinal.Font = new Font("Segoe UI", 9F);
        lblDataFinal.ForeColor = Color.White;
        lblDataFinal.Location = new Point(617, 11);
        lblDataFinal.Name = "lblDataFinal";
        lblDataFinal.Size = new Size(79, 20);
        lblDataFinal.TabIndex = 3;
        lblDataFinal.Text = "Data Final:";
        // 
        // dtpDataInicial
        // 
        dtpDataInicial.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        dtpDataInicial.Format = DateTimePickerFormat.Short;
        dtpDataInicial.Location = new Point(480, 35);
        dtpDataInicial.Margin = new Padding(3, 4, 3, 4);
        dtpDataInicial.Name = "dtpDataInicial";
        dtpDataInicial.Size = new Size(125, 27);
        dtpDataInicial.TabIndex = 2;
        // 
        // lblDataInicial
        // 
        lblDataInicial.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDataInicial.AutoSize = true;
        lblDataInicial.Font = new Font("Segoe UI", 9F);
        lblDataInicial.ForeColor = Color.White;
        lblDataInicial.Location = new Point(480, 11);
        lblDataInicial.Name = "lblDataInicial";
        lblDataInicial.Size = new Size(87, 20);
        lblDataInicial.TabIndex = 1;
        lblDataInicial.Text = "Data Inicial:";
        // 
        // titleLabel
        // 
        titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        titleLabel.ForeColor = Color.White;
        titleLabel.Location = new Point(23, 0);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(405, 67);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Recebimento de Emprestimo";
        titleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // RecebimentoListForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(dataGridView1);
        Controls.Add(panel1);
        Controls.Add(titlePanel);
        Margin = new Padding(3, 4, 3, 4);
        Name = "RecebimentoListForm";
        Size = new Size(914, 600);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        panel1.ResumeLayout(false);
        titlePanel.ResumeLayout(false);
        titlePanel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private Button btnCreate;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnImprimirRecibo;
    private Button btnRelatorio;
    private Panel panel1;
    private Panel titlePanel;
    private Label titleLabel;
    private Label lblDataInicial;
    private DateTimePicker dtpDataInicial;
    private Label lblDataFinal;
    private DateTimePicker dtpDataFinal;
    private Button btnFiltrar;
}
