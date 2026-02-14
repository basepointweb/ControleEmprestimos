namespace ControleEmprestimos.Forms;

public partial class MainForm : Form
{
    private ItemListForm _itemListForm;
    private EmprestimoListForm _emprestimoListForm;
    private RecebimentoListForm _recebimentoListForm;
    private CongregacaoListForm _congregacaoListForm;

    public MainForm()
    {
        InitializeComponent();
        InitializeUserControls();
        ShowItemList();
    }

    private void InitializeUserControls()
    {
        // Create all user controls
        _itemListForm = new ItemListForm { Dock = DockStyle.Fill, Visible = false };
        _emprestimoListForm = new EmprestimoListForm { Dock = DockStyle.Fill, Visible = false };
        _recebimentoListForm = new RecebimentoListForm { Dock = DockStyle.Fill, Visible = false };
        _congregacaoListForm = new CongregacaoListForm { Dock = DockStyle.Fill, Visible = false };

        // Add all to the content panel
        contentPanel.Controls.Add(_itemListForm);
        contentPanel.Controls.Add(_emprestimoListForm);
        contentPanel.Controls.Add(_recebimentoListForm);
        contentPanel.Controls.Add(_congregacaoListForm);
    }

    private void HideAllUserControls()
    {
        _itemListForm.Visible = false;
        _emprestimoListForm.Visible = false;
        _recebimentoListForm.Visible = false;
        _congregacaoListForm.Visible = false;
    }

    private void ShowItemList()
    {
        HideAllUserControls();
        _itemListForm.Visible = true;
    }

    private void ShowEmprestimoList()
    {
        HideAllUserControls();
        _emprestimoListForm.Visible = true;
    }

    private void ShowRecebimentoList()
    {
        HideAllUserControls();
        _recebimentoListForm.Visible = true;
    }

    private void ShowCongregacaoList()
    {
        HideAllUserControls();
        _congregacaoListForm.Visible = true;
    }

    private void MenuListagemBens_Click(object sender, EventArgs e)
    {
        ShowItemList();
    }

    private void MenuEmprestimo_Click(object sender, EventArgs e)
    {
        ShowEmprestimoList();
    }

    private void MenuRecebimento_Click(object sender, EventArgs e)
    {
        ShowRecebimentoList();
    }

    private void MenuCongregacoes_Click(object sender, EventArgs e)
    {
        ShowCongregacaoList();
    }
}
