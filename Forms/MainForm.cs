namespace ControleEmprestimos.Forms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void MenuListagemBens_Click(object sender, EventArgs e)
    {
        var form = new ItemListForm();
        form.ShowDialog();
    }

    private void MenuEmprestimo_Click(object sender, EventArgs e)
    {
        var form = new EmprestimoListForm();
        form.ShowDialog();
    }

    private void MenuRecebimento_Click(object sender, EventArgs e)
    {
        var form = new RecebimentoListForm();
        form.ShowDialog();
    }

    private void MenuCongregacoes_Click(object sender, EventArgs e)
    {
        var form = new CongregacaoListForm();
        form.ShowDialog();
    }
}
