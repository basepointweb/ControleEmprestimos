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
        _isEditing = item != null && !isCloning;
        _isCloning = isCloning;

        if (_item != null)
        {
            txtName.Text = _item.Name;

            if (_isEditing)
            {
                // Modo edição - mostrar informações de empréstimos
                lblEmprestimosInfo.Visible = true;
                dgvEmprestimos.Visible = true;
                
                // Carregar empréstimos da congregação
                LoadEmprestimos();
            }
            else if (_isCloning)
            {
                // Modo clonagem
                this.Text = "Clonar Congregação";
                lblEmprestimosInfo.Visible = false;
                dgvEmprestimos.Visible = false;
            }
        }
        else
        {
            // Novo registro
            lblEmprestimosInfo.Visible = false;
            dgvEmprestimos.Visible = false;
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

        // Atualizar label
        lblEmprestimosInfo.Text = $"Empréstimos Pendentes: {totalEmprestimos} empréstimo(s) - Totalizando {totalItens} itens";

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

        // Preencher dados
        dgvEmprestimos.DataSource = emprestimos;

        // Preencher colunas calculadas
        for (int i = 0; i < dgvEmprestimos.Rows.Count; i++)
        {
            var emprestimo = emprestimos[i];
            
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

        // Event handler para clique no botão
        dgvEmprestimos.CellClick += DgvEmprestimos_CellClick;
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
            MessageBox.Show("Por favor, informe o nome da congregação.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_isEditing && _item != null)
        {
            _item.Name = txtName.Text;
            _repository.UpdateCongregacao(_item);
        }
        else
        {
            var newItem = new Congregacao
            {
                Name = txtName.Text
            };
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
