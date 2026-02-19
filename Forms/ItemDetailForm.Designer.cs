namespace ControleEmprestimos.Forms;

partial class ItemDetailForm
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
        this.lblName = new Label();
        this.txtName = new TextBox();
        this.lblQuantity = new Label();
        this.numQuantity = new NumericUpDown();
        this.btnSave = new Button();
        this.btnCancel = new Button();
        this.lblInstrucoes = new Label();
        ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
        this.SuspendLayout();
        // 
        // lblName
        // 
        this.lblName.AutoSize = true;
        this.lblName.Location = new Point(20, 20);
        this.lblName.Name = "lblName";
        this.lblName.Size = new Size(40, 15);
        this.lblName.TabIndex = 0;
        this.lblName.Text = "Nome:";
        // 
        // txtName
        // 
        this.txtName.Location = new Point(20, 40);
        this.txtName.Multiline = true;
        this.txtName.Name = "txtName";
        this.txtName.ScrollBars = ScrollBars.Vertical;
        this.txtName.Size = new Size(460, 120);
        this.txtName.TabIndex = 1;
        // 
        // lblInstrucoes
        // 
        this.lblInstrucoes.AutoSize = false;
        this.lblInstrucoes.Location = new Point(20, 165);
        this.lblInstrucoes.Name = "lblInstrucoes";
        this.lblInstrucoes.Size = new Size(460, 60);
        this.lblInstrucoes.TabIndex = 2;
        this.lblInstrucoes.Text = "Instruções:\n" +
            "• Uma linha = um item (quantidade padrão: 1)\n" +
            "• Para quantidade diferente: NOME;QUANTIDADE (ex: CADEIRA;10)\n" +
            "• Para item único: use o campo Nome e Quantidade abaixo";
        this.lblInstrucoes.ForeColor = Color.FromArgb(64, 64, 64);
        // 
        // lblQuantity
        // 
        this.lblQuantity.AutoSize = true;
        this.lblQuantity.Location = new Point(20, 230);
        this.lblQuantity.Name = "lblQuantity";
        this.lblQuantity.Size = new Size(180, 15);
        this.lblQuantity.TabIndex = 3;
        this.lblQuantity.Text = "Quantidade (para item único):";
        // 
        // numQuantity
        // 
        this.numQuantity.Location = new Point(20, 250);
        this.numQuantity.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        this.numQuantity.Name = "numQuantity";
        this.numQuantity.Size = new Size(120, 23);
        this.numQuantity.TabIndex = 4;
        this.numQuantity.Value = 1;
        // 
        // btnSave
        // 
        this.btnSave.Location = new Point(20, 300);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new Size(100, 30);
        this.btnSave.TabIndex = 5;
        this.btnSave.Text = "Salvar";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new Point(126, 300);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(100, 30);
        this.btnCancel.TabIndex = 6;
        this.btnCancel.Text = "Cancelar";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);
        // 
        // ItemDetailForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(500, 350);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.numQuantity);
        this.Controls.Add(this.lblQuantity);
        this.Controls.Add(this.lblInstrucoes);
        this.Controls.Add(this.txtName);
        this.Controls.Add(this.lblName);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ItemDetailForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Criar Item(s)";
        ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label lblName;
    private TextBox txtName;
    private Label lblQuantity;
    private NumericUpDown numQuantity;
    private Button btnSave;
    private Button btnCancel;
    private Label lblInstrucoes;
}
