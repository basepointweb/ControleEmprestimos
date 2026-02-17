namespace ControleEmprestimos.Forms;

partial class EtiquetasFilterForm
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
        this.titlePanel = new Panel();
        this.titleLabel = new Label();
        this.mainPanel = new Panel();
        this.groupBoxFiltro = new GroupBox();
        this.lblItensEncontrados = new Label();
        this.txtFiltroIds = new TextBox();
        this.lblInstrucoes = new Label();
        this.groupBoxItens = new GroupBox();
        this.checkedListBoxItens = new CheckedListBox();
        this.panelBotoesItens = new Panel();
        this.btnMarcarTodos = new Button();
        this.btnDesmarcarTodos = new Button();
        this.buttonPanel = new Panel();
        this.btnImprimir = new Button();
        this.btnCancelar = new Button();
        
        this.titlePanel.SuspendLayout();
        this.mainPanel.SuspendLayout();
        this.groupBoxFiltro.SuspendLayout();
        this.groupBoxItens.SuspendLayout();
        this.panelBotoesItens.SuspendLayout();
        this.buttonPanel.SuspendLayout();
        this.SuspendLayout();
        
        // 
        // titlePanel
        // 
        this.titlePanel.BackColor = Color.FromArgb(0, 120, 215);
        this.titlePanel.Controls.Add(this.titleLabel);
        this.titlePanel.Dock = DockStyle.Top;
        this.titlePanel.Location = new Point(0, 0);
        this.titlePanel.Name = "titlePanel";
        this.titlePanel.Size = new Size(750, 60);
        this.titlePanel.TabIndex = 0;
        
        // 
        // titleLabel
        // 
        this.titleLabel.Dock = DockStyle.Fill;
        this.titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        this.titleLabel.ForeColor = Color.White;
        this.titleLabel.Location = new Point(0, 0);
        this.titleLabel.Name = "titleLabel";
        this.titleLabel.Size = new Size(750, 60);
        this.titleLabel.TabIndex = 0;
        this.titleLabel.Text = "Imprimir Etiquetas de Bens";
        this.titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        
        // 
        // mainPanel
        // 
        this.mainPanel.Controls.Add(this.groupBoxItens);
        this.mainPanel.Controls.Add(this.groupBoxFiltro);
        this.mainPanel.Dock = DockStyle.Fill;
        this.mainPanel.Location = new Point(0, 60);
        this.mainPanel.Name = "mainPanel";
        this.mainPanel.Padding = new Padding(20);
        this.mainPanel.Size = new Size(750, 430);
        this.mainPanel.TabIndex = 1;
        
        // 
        // groupBoxFiltro
        // 
        this.groupBoxFiltro.Controls.Add(this.lblItensEncontrados);
        this.groupBoxFiltro.Controls.Add(this.txtFiltroIds);
        this.groupBoxFiltro.Controls.Add(this.lblInstrucoes);
        this.groupBoxFiltro.Dock = DockStyle.Top;
        this.groupBoxFiltro.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.groupBoxFiltro.Location = new Point(20, 20);
        this.groupBoxFiltro.Name = "groupBoxFiltro";
        this.groupBoxFiltro.Size = new Size(710, 120);
        this.groupBoxFiltro.TabIndex = 0;
        this.groupBoxFiltro.TabStop = false;
        this.groupBoxFiltro.Text = "Filtrar Itens";
        
        // 
        // lblInstrucoes
        // 
        this.lblInstrucoes.AutoSize = true;
        this.lblInstrucoes.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        this.lblInstrucoes.Location = new Point(15, 25);
        this.lblInstrucoes.Name = "lblInstrucoes";
        this.lblInstrucoes.Size = new Size(550, 30);
        this.lblInstrucoes.TabIndex = 0;
        this.lblInstrucoes.Text = "Digite os IDs dos itens separados por vírgula, espaço ou ponto e vírgula.\n" +
                                      "Ou digite parte do nome para filtrar por nome.";
        
        // 
        // txtFiltroIds
        // 
        this.txtFiltroIds.Font = new Font("Segoe UI", 10F);
        this.txtFiltroIds.Location = new Point(15, 60);
        this.txtFiltroIds.Name = "txtFiltroIds";
        this.txtFiltroIds.PlaceholderText = "Ex: 1, 2, 3 ou CADEIRA";
        this.txtFiltroIds.Size = new Size(680, 25);
        this.txtFiltroIds.TabIndex = 1;
        this.txtFiltroIds.TextChanged += new EventHandler(this.TxtFiltroIds_TextChanged);
        
        // 
        // lblItensEncontrados
        // 
        this.lblItensEncontrados.AutoSize = true;
        this.lblItensEncontrados.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        this.lblItensEncontrados.ForeColor = Color.Gray;
        this.lblItensEncontrados.Location = new Point(15, 90);
        this.lblItensEncontrados.Name = "lblItensEncontrados";
        this.lblItensEncontrados.Size = new Size(0, 15);
        this.lblItensEncontrados.TabIndex = 2;
        
        // 
        // groupBoxItens
        // 
        this.groupBoxItens.Controls.Add(this.checkedListBoxItens);
        this.groupBoxItens.Controls.Add(this.panelBotoesItens);
        this.groupBoxItens.Dock = DockStyle.Fill;
        this.groupBoxItens.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.groupBoxItens.Location = new Point(20, 140);
        this.groupBoxItens.Name = "groupBoxItens";
        this.groupBoxItens.Size = new Size(710, 270);
        this.groupBoxItens.TabIndex = 1;
        this.groupBoxItens.TabStop = false;
        this.groupBoxItens.Text = "Selecione os Itens";
        
        // 
        // checkedListBoxItens
        // 
        this.checkedListBoxItens.CheckOnClick = true;
        this.checkedListBoxItens.Dock = DockStyle.Fill;
        this.checkedListBoxItens.Font = new Font("Segoe UI", 9F);
        this.checkedListBoxItens.FormattingEnabled = true;
        this.checkedListBoxItens.Location = new Point(3, 23);
        this.checkedListBoxItens.Name = "checkedListBoxItens";
        this.checkedListBoxItens.Size = new Size(704, 204);
        this.checkedListBoxItens.TabIndex = 0;
        
        // 
        // panelBotoesItens
        // 
        this.panelBotoesItens.Controls.Add(this.btnDesmarcarTodos);
        this.panelBotoesItens.Controls.Add(this.btnMarcarTodos);
        this.panelBotoesItens.Dock = DockStyle.Bottom;
        this.panelBotoesItens.Location = new Point(3, 227);
        this.panelBotoesItens.Name = "panelBotoesItens";
        this.panelBotoesItens.Size = new Size(704, 40);
        this.panelBotoesItens.TabIndex = 1;
        
        // 
        // btnMarcarTodos
        // 
        this.btnMarcarTodos.Font = new Font("Segoe UI", 9F);
        this.btnMarcarTodos.Location = new Point(10, 5);
        this.btnMarcarTodos.Name = "btnMarcarTodos";
        this.btnMarcarTodos.Size = new Size(120, 30);
        this.btnMarcarTodos.TabIndex = 0;
        this.btnMarcarTodos.Text = "Marcar Todos";
        this.btnMarcarTodos.UseVisualStyleBackColor = true;
        this.btnMarcarTodos.Click += new EventHandler(this.BtnMarcarTodos_Click);
        
        // 
        // btnDesmarcarTodos
        // 
        this.btnDesmarcarTodos.Font = new Font("Segoe UI", 9F);
        this.btnDesmarcarTodos.Location = new Point(136, 5);
        this.btnDesmarcarTodos.Name = "btnDesmarcarTodos";
        this.btnDesmarcarTodos.Size = new Size(120, 30);
        this.btnDesmarcarTodos.TabIndex = 1;
        this.btnDesmarcarTodos.Text = "Desmarcar Todos";
        this.btnDesmarcarTodos.UseVisualStyleBackColor = true;
        this.btnDesmarcarTodos.Click += new EventHandler(this.BtnDesmarcarTodos_Click);
        
        // 
        // buttonPanel
        // 
        this.buttonPanel.Controls.Add(this.btnCancelar);
        this.buttonPanel.Controls.Add(this.btnImprimir);
        this.buttonPanel.Dock = DockStyle.Bottom;
        this.buttonPanel.Location = new Point(0, 490);
        this.buttonPanel.Name = "buttonPanel";
        this.buttonPanel.Size = new Size(750, 60);
        this.buttonPanel.TabIndex = 2;
        
        // 
        // btnImprimir
        // 
        this.btnImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.btnImprimir.BackColor = Color.FromArgb(0, 120, 215);
        this.btnImprimir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.btnImprimir.ForeColor = Color.White;
        this.btnImprimir.Location = new Point(420, 15);
        this.btnImprimir.Name = "btnImprimir";
        this.btnImprimir.Size = new Size(150, 35);
        this.btnImprimir.TabIndex = 0;
        this.btnImprimir.Text = "Imprimir Etiquetas";
        this.btnImprimir.UseVisualStyleBackColor = false;
        this.btnImprimir.Click += new EventHandler(this.BtnImprimir_Click);
        
        // 
        // btnCancelar
        // 
        this.btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.btnCancelar.Font = new Font("Segoe UI", 10F);
        this.btnCancelar.Location = new Point(580, 15);
        this.btnCancelar.Name = "btnCancelar";
        this.btnCancelar.Size = new Size(150, 35);
        this.btnCancelar.TabIndex = 1;
        this.btnCancelar.Text = "Cancelar";
        this.btnCancelar.UseVisualStyleBackColor = true;
        this.btnCancelar.Click += new EventHandler(this.BtnCancelar_Click);
        
        // 
        // EtiquetasFilterForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(750, 550);
        this.Controls.Add(this.mainPanel);
        this.Controls.Add(this.buttonPanel);
        this.Controls.Add(this.titlePanel);
        this.Name = "EtiquetasFilterForm";
        this.Text = "Imprimir Etiquetas de Bens";
        
        this.titlePanel.ResumeLayout(false);
        this.mainPanel.ResumeLayout(false);
        this.groupBoxFiltro.ResumeLayout(false);
        this.groupBoxFiltro.PerformLayout();
        this.groupBoxItens.ResumeLayout(false);
        this.panelBotoesItens.ResumeLayout(false);
        this.buttonPanel.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    #endregion

    private Panel titlePanel;
    private Label titleLabel;
    private Panel mainPanel;
    private GroupBox groupBoxFiltro;
    private Label lblInstrucoes;
    private TextBox txtFiltroIds;
    private Label lblItensEncontrados;
    private GroupBox groupBoxItens;
    private CheckedListBox checkedListBoxItens;
    private Panel panelBotoesItens;
    private Button btnMarcarTodos;
    private Button btnDesmarcarTodos;
    private Panel buttonPanel;
    private Button btnImprimir;
    private Button btnCancelar;
}
