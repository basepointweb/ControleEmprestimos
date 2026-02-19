using ControleEmprestimos.Data;
using ControleEmprestimos.Models;
using ControleEmprestimos.Helpers;

namespace ControleEmprestimos.Forms;

public partial class ItemDetailForm : Form
{
    private Item? _item;
    private bool _isEditing;
    private bool _isCloning;

    public ItemDetailForm(Item? item = null, bool isCloning = false)
    {
        InitializeComponent();
        
        // Configurar controles para caixa alta
        FormControlHelper.ConfigureAllTextBoxesToUpperCase(this);
        
        _item = item;
        _isEditing = item != null && !isCloning;
        _isCloning = isCloning;

        if (_item != null)
        {
            txtName.Text = _item.Name;
            numQuantity.Value = _item.QuantityInStock;

            if (_isCloning)
            {
                this.Text = "Clonar Bem";
            }
            else if (_isEditing)
            {
                this.Text = "Editar Bem";
                // Em modo edição, desabilitar múltiplas linhas
                txtName.Multiline = false;
                txtName.ScrollBars = ScrollBars.None;
                lblInstrucoes.Visible = false;
            }
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Por favor, informe o nome do item.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_isEditing && _item != null)
        {
            // Modo edição - item único
            _item.Name = txtName.Text.Trim().ToUpper();
            _item.QuantityInStock = (int)numQuantity.Value;
            DataRepository.Instance.UpdateItem(_item);
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else
        {
            // Modo criação (novo ou clonado) - pode ser múltiplos itens
            var repository = DataRepository.Instance;
            var linhas = txtName.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Verificar se é múltiplas linhas ou item único
            if (linhas.Length > 1)
            {
                // Múltiplos itens - processar linha por linha
                var itensParaSalvar = new List<(string Nome, int Quantidade)>();
                var erros = new List<string>();
                
                foreach (var linha in linhas)
                {
                    var linhaTrim = linha.Trim();
                    if (string.IsNullOrWhiteSpace(linhaTrim))
                        continue;
                    
                    string nome;
                    int quantidade;
                    
                    // Verificar se tem ponto e vírgula (formato: NOME;QUANTIDADE)
                    if (linhaTrim.Contains(';'))
                    {
                        var partes = linhaTrim.Split(';');
                        if (partes.Length != 2)
                        {
                            erros.Add($"Formato inválido: {linhaTrim}");
                            continue;
                        }
                        
                        nome = partes[0].Trim().ToUpper();
                        
                        if (!int.TryParse(partes[1].Trim(), out quantidade) || quantidade < 0)
                        {
                            erros.Add($"Quantidade inválida em: {linhaTrim}");
                            continue;
                        }
                    }
                    else
                    {
                        // Sem ponto e vírgula - quantidade padrão = 1
                        nome = linhaTrim.ToUpper();
                        quantidade = 1;
                    }
                    
                    if (string.IsNullOrWhiteSpace(nome))
                    {
                        erros.Add($"Nome vazio em: {linhaTrim}");
                        continue;
                    }
                    
                    itensParaSalvar.Add((nome, quantidade));
                }
                
                // Verificar duplicados
                var duplicados = itensParaSalvar
                    .GroupBy(i => i.Nome)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();
                
                if (duplicados.Any())
                {
                    erros.Add($"Itens duplicados:\n{string.Join("\n", duplicados)}");
                }
                
                // Se há erros, exibir e não salvar
                if (erros.Any())
                {
                    MessageBox.Show(
                        $"Erros encontrados:\n\n{string.Join("\n", erros)}",
                        "Validação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                
                // Se não há itens para salvar
                if (itensParaSalvar.Count == 0)
                {
                    MessageBox.Show(
                        "Nenhum item válido para salvar.",
                        "Validação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                
                // Salvar todos os itens
                int salvos = 0;
                foreach (var (nome, quantidade) in itensParaSalvar)
                {
                    var newItem = new Item
                    {
                        Name = nome,
                        QuantityInStock = quantidade
                    };
                    repository.AddItem(newItem);
                    salvos++;
                }
                
                MessageBox.Show(
                    $"{salvos} {(salvos == 1 ? "item foi salvo" : "itens foram salvos")} com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Item único - usar campo de quantidade
                var nome = txtName.Text.Trim().ToUpper();
                
                // Verificar se tem ponto e vírgula no item único
                if (nome.Contains(';'))
                {
                    var partes = nome.Split(';');
                    if (partes.Length == 2 && int.TryParse(partes[1].Trim(), out int qtd) && qtd > 0)
                    {
                        // Usar quantidade do texto
                        nome = partes[0].Trim().ToUpper();
                        var newItem = new Item
                        {
                            Name = nome,
                            QuantityInStock = qtd
                        };
                        repository.AddItem(newItem);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Formato inválido. Use: NOME;QUANTIDADE",
                            "Validação",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    // Usar campo de quantidade normal
                    var newItem = new Item
                    {
                        Name = nome,
                        QuantityInStock = (int)numQuantity.Value
                    };
                    repository.AddItem(newItem);
                }
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
