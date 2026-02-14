using ControleEmprestimos.Data;
using ControleEmprestimos.Models;

namespace ControleEmprestimos.Forms;

public partial class CongregacaoDetailForm : Form
{
    private Congregacao? _item;
    private bool _isEditing;
    private bool _isCloning;
    private DataRepository _repository;

    public CongregacaoDetailForm(Congregacao? item = null, bool isCloning = false)
    {
        InitializeComponent();
        _repository = DataRepository.Instance;
        _item = item;
        _isCloning = isCloning;
        _isEditing = item != null && !isCloning;

        if (_item != null)
        {
            txtName.Text = _item.Name;
            txtSetor.Text = _item.Setor;

            if (_isEditing)
            {
                // Configurar grid primeiro (sempre visível)
                ConfigureEmprestimosGrid();
                
                // Carregar empréstimos pendentes
                LoadEmprestimos();
            }
        }
    }

    private void ConfigureEmprestimosGrid()
    {
        // Configurar grid
        dgvEmprestimos.AutoGenerateColumns = false;
        dgvEmprestimos.Columns.Clear();

        dgvEmprestimos.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Recebedor",
            DataPropertyName = "Name",
            Name = "colRecebedor",
            Width = 150
        });

        // Coluna de Bens (concatenados)
        var colBens = new DataGridViewTextBoxColumn
        {
            HeaderText = "Bens",
            Name = "colBens",
            Width = 200
        };
        dgvEmprestimos.Columns.Add(colBens);

        // Coluna de Quantidade Total Pendente
        var colQtd = new DataGridViewTextBoxColumn
        {
            HeaderText = "Qtd Pendente",
            Name = "colQtd",
            Width = 90
        };
        dgvEmprestimos.Columns.Add(colQtd);

        dgvEmprestimos.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Data",
            DataPropertyName = "DataEmprestimo",
            Name = "colData",
            Width = 90,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
        });

        dgvEmprestimos.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Motivo",
            DataPropertyName = "Motivo",
            Name = "colMotivo",
            Width = 150
        });

        // Adicionar coluna de botão "Receber de Volta"
        var btnReceberColumn = new DataGridViewButtonColumn
        {
            HeaderText = "",
            Name = "colReceber",
            Text = "Receber de Volta",
            UseColumnTextForButtonValue = true,
            Width = 120
        };
        dgvEmprestimos.Columns.Add(btnReceberColumn);

        // Event handler para clique no botão
        dgvEmprestimos.CellClick += DgvEmprestimos_CellClick;
        
        // Event handler para preencher colunas calculadas após binding
        dgvEmprestimos.DataBindingComplete += DgvEmprestimos_DataBindingComplete;
        
        // Grid sempre visível
        dgvEmprestimos.Visible = true;
    }

    private void LoadEmprestimosData(List<Emprestimo> emprestimos)
    {
        // Preencher dados
        dgvEmprestimos.DataSource = null;
        dgvEmprestimos.DataSource = emprestimos;
    }

    private void DgvEmprestimos_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        // Preencher colunas calculadas após binding
        for (int i = 0; i < dgvEmprestimos.Rows.Count; i++)
        {
            if (dgvEmprestimos.Rows[i].DataBoundItem is Emprestimo emprestimo)
            {
                // Concatenar nomes dos bens
                string bens;
                if (emprestimo.Itens != null && emprestimo.Itens.Any())
                {
                    bens = string.Join(", ", emprestimo.Itens.Select(ei => ei.ItemName).Distinct());
                }
                else
                {
                    // Compatibilidade com dados antigos
                    bens = emprestimo.ItemName;
                }
                dgvEmprestimos.Rows[i].Cells["colBens"].Value = bens;
                
                // Total de itens pendentes
                dgvEmprestimos.Rows[i].Cells["colQtd"].Value = emprestimo.TotalPendente;
            }
        }
    }

    private void LoadEmprestimos()
    {
        if (_item == null) return;

        // Carregar empréstimos em andamento da congregação
        var emprestimos = _repository.Emprestimos
            .Where(e => e.CongregacaoId == _item.Id && e.Status == StatusEmprestimo.EmAndamento)
            .ToList();

        // Calcular totais (apenas itens pendentes)
        var totalEmprestimos = emprestimos.Count;
        var totalItens = emprestimos.Sum(e => e.TotalPendente);

        // Atualizar label (sempre visível)
        if (totalEmprestimos > 0)
        {
            lblEmprestimosInfo.Text = $"Empréstimos Pendentes: {totalEmprestimos} empréstimo(s) - Totalizando {totalItens} itens pendentes";
        }
        else
        {
            lblEmprestimosInfo.Text = "Nenhum empréstimo pendente para esta congregação.";
        }
        lblEmprestimosInfo.Visible = true;

        // Carregar dados no grid (mesmo que vazio)
        LoadEmprestimosData(emprestimos);
    }

    private void DgvEmprestimos_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (dgvEmprestimos.Columns[e.ColumnIndex].Name == "colReceber")
        {
            var emprestimo = dgvEmprestimos.Rows[e.RowIndex].DataBoundItem as Emprestimo;
            if (emprestimo != null)
            {
                // Abrir tela de recebimento com empréstimo pré-selecionado
                var form = new RecebimentoDetailForm(emprestimo);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Recarregar empréstimos após recebimento
                    LoadEmprestimos();
                }
            }
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Por favor, preencha o nome.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_isEditing && _item != null)
        {
            _item.Name = txtName.Text;
            _item.Setor = txtSetor.Text;
            _item.DataAlteracao = DateTime.Now;
        }
        else
        {
            var newItem = new Congregacao { Name = txtName.Text, Setor = txtSetor.Text };
            _repository.AddCongregacao(newItem);
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
