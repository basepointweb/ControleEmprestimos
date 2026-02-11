namespace ControleEmprestimos.Forms;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.menuStrip1 = new MenuStrip();
        this.menuListagemBens = new ToolStripMenuItem();
        this.menuEmprestimo = new ToolStripMenuItem();
        this.menuRecebimento = new ToolStripMenuItem();
        this.menuCongregacoes = new ToolStripMenuItem();
        this.menuStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new ToolStripItem[] {
            this.menuListagemBens,
            this.menuEmprestimo,
            this.menuRecebimento,
            this.menuCongregacoes});
        this.menuStrip1.Location = new Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new Size(800, 24);
        this.menuStrip1.TabIndex = 0;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // menuListagemBens
        // 
        this.menuListagemBens.Name = "menuListagemBens";
        this.menuListagemBens.Size = new Size(116, 20);
        this.menuListagemBens.Text = "Listagem de Bens";
        this.menuListagemBens.Click += new EventHandler(this.MenuListagemBens_Click);
        // 
        // menuEmprestimo
        // 
        this.menuEmprestimo.Name = "menuEmprestimo";
        this.menuEmprestimo.Size = new Size(82, 20);
        this.menuEmprestimo.Text = "Emprestimo";
        this.menuEmprestimo.Click += new EventHandler(this.MenuEmprestimo_Click);
        // 
        // menuRecebimento
        // 
        this.menuRecebimento.Name = "menuRecebimento";
        this.menuRecebimento.Size = new Size(169, 20);
        this.menuRecebimento.Text = "Recebimento de Emprestimo";
        this.menuRecebimento.Click += new EventHandler(this.MenuRecebimento_Click);
        // 
        // menuCongregacoes
        // 
        this.menuCongregacoes.Name = "menuCongregacoes";
        this.menuCongregacoes.Size = new Size(95, 20);
        this.menuCongregacoes.Text = "Congregacoes";
        this.menuCongregacoes.Click += new EventHandler(this.MenuCongregacoes_Click);
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(800, 450);
        this.Controls.Add(this.menuStrip1);
        this.MainMenuStrip = this.menuStrip1;
        this.Name = "MainForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Controle de Emprestimos";
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip1;
    private ToolStripMenuItem menuListagemBens;
    private ToolStripMenuItem menuEmprestimo;
    private ToolStripMenuItem menuRecebimento;
    private ToolStripMenuItem menuCongregacoes;
}
