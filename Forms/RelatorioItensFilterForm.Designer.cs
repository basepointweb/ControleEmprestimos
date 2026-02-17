namespace ControleEmprestimos.Forms;

partial class RelatorioItensFilterForm
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
        this.gbFiltros = new GroupBox();
        this.rbTodosBens = new RadioButton();
        this.rbApenasBensEmprestados = new RadioButton();
        this.btnGerar = new Button();
        this.btnCancelar = new Button();
        this.gbFiltros.SuspendLayout();
        this.SuspendLayout();
        // 
        // lblTitulo
        // 
        this.lblTitulo.AutoSize = true;
        this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        this.lblTitulo.Location = new Point(12, 15);
        this.lblTitulo.Name = "lblTitulo";
        this.lblTitulo.Size = new Size(230, 25);
        this.lblTitulo.TabIndex = 0;
        this.lblTitulo.Text = "Relatório de Bens";
        // 
        // gbFiltros
        // 
        this.gbFiltros.Controls.Add(this.rbTodosBens);
        this.gbFiltros.Controls.Add(this.rbApenasBensEmprestados);
        this.gbFiltros.Location = new Point(12, 50);
        this.gbFiltros.Name = "gbFiltros";
        this.gbFiltros.Size = new Size(336, 90);
        this.gbFiltros.TabIndex = 1;
        this.gbFiltros.TabStop = false;
        this.gbFiltros.Text = "Filtros";
        // 
        // rbTodosBens
        // 
        this.rbTodosBens.AutoSize = true;
        this.rbTodosBens.Checked = true;
        this.rbTodosBens.Location = new Point(15, 30);
        this.rbTodosBens.Name = "rbTodosBens";
        this.rbTodosBens.Size = new Size(110, 19);
        this.rbTodosBens.TabIndex = 0;
        this.rbTodosBens.TabStop = true;
        this.rbTodosBens.Text = "Todos os Bens";
        this.rbTodosBens.UseVisualStyleBackColor = true;
        // 
        // rbApenasBensEmprestados
        // 
        this.rbApenasBensEmprestados.AutoSize = true;
        this.rbApenasBensEmprestados.Location = new Point(15, 55);
        this.rbApenasBensEmprestados.Name = "rbApenasBensEmprestados";
        this.rbApenasBensEmprestados.Size = new Size(280, 19);
        this.rbApenasBensEmprestados.TabIndex = 1;
        this.rbApenasBensEmprestados.Text = "Apenas Bens com Itens Emprestados";
        this.rbApenasBensEmprestados.UseVisualStyleBackColor = true;
        // 
        // btnGerar
        // 
        this.btnGerar.BackColor = Color.FromArgb(0, 120, 215);
        this.btnGerar.ForeColor = Color.White;
        this.btnGerar.Location = new Point(148, 160);
        this.btnGerar.Name = "btnGerar";
        this.btnGerar.Size = new Size(100, 35);
        this.btnGerar.TabIndex = 2;
        this.btnGerar.Text = "Gerar Relatório";
        this.btnGerar.UseVisualStyleBackColor = false;
        this.btnGerar.Click += new EventHandler(this.BtnGerar_Click);
        // 
        // btnCancelar
        // 
        this.btnCancelar.Location = new Point(254, 160);
        this.btnCancelar.Name = "btnCancelar";
        this.btnCancelar.Size = new Size(94, 35);
        this.btnCancelar.TabIndex = 3;
        this.btnCancelar.Text = "Cancelar";
        this.btnCancelar.UseVisualStyleBackColor = true;
        this.btnCancelar.Click += new EventHandler(this.BtnCancelar_Click);
        // 
        // RelatorioItensFilterForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(360, 210);
        this.Controls.Add(this.btnCancelar);
        this.Controls.Add(this.btnGerar);
        this.Controls.Add(this.gbFiltros);
        this.Controls.Add(this.lblTitulo);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "RelatorioItensFilterForm";
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Filtrar Relatório de Bens";
        this.gbFiltros.ResumeLayout(false);
        this.gbFiltros.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label lblTitulo;
    private GroupBox gbFiltros;
    private RadioButton rbTodosBens;
    private RadioButton rbApenasBensEmprestados;
    private Button btnGerar;
    private Button btnCancelar;
}
