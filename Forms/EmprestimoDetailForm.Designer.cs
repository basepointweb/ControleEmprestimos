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
        this.lblName = new Label();
        this.txtName = new TextBox();
        this.lblQuantity = new Label();
        this.numQuantity = new NumericUpDown();
        this.btnSave = new Button();
        this.btnCancel = new Button();
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
        this.txtName.Name = "txtName";
        this.txtName.Size = new Size(360, 23);
        this.txtName.TabIndex = 1;
        // 
        // lblQuantity
        // 
        this.lblQuantity.AutoSize = true;
        this.lblQuantity.Location = new Point(20, 80);
        this.lblQuantity.Name = "lblQuantity";
        this.lblQuantity.Size = new Size(121, 15);
        this.lblQuantity.TabIndex = 2;
        this.lblQuantity.Text = "Quantidade em Estoque:";
        // 
        // numQuantity
        // 
        this.numQuantity.Location = new Point(20, 100);
        this.numQuantity.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        this.numQuantity.Name = "numQuantity";
        this.numQuantity.Size = new Size(120, 23);
        this.numQuantity.TabIndex = 3;
        // 
        // btnSave
        // 
        this.btnSave.Location = new Point(20, 150);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new Size(100, 30);
        this.btnSave.TabIndex = 4;
        this.btnSave.Text = "Salvar";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new Point(126, 150);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(100, 30);
        this.btnCancel.TabIndex = 5;
        this.btnCancel.Text = "Cancelar";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);
        // 
        // EmprestimoDetailForm
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(400, 200);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.numQuantity);
        this.Controls.Add(this.lblQuantity);
        this.Controls.Add(this.txtName);
        this.Controls.Add(this.lblName);
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

    private Label lblName;
    private TextBox txtName;
    private Label lblQuantity;
    private NumericUpDown numQuantity;
    private Button btnSave;
    private Button btnCancel;
}
