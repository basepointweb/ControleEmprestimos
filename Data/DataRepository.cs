using ControleEmprestimos.Models;

namespace ControleEmprestimos.Data;

public class DataRepository
{
    private static DataRepository? _instance;
    private int _nextItemId = 1;
    private int _nextEmprestimoId = 1;
    private int _nextRecebimentoId = 1;
    private int _nextCongregacaoId = 1;
    private int _nextEmprestimoItemId = 1;
    private int _nextRecebimentoItemId = 1;
    
    private readonly ExcelDataRepository _excelRepository;
    private readonly string _dataFilePath;

    public List<Item> Items { get; } = new();
    public List<Emprestimo> Emprestimos { get; } = new();
    public List<RecebimentoEmprestimo> RecebimentoEmprestimos { get; } = new();
    public List<Congregacao> Congregacoes { get; } = new();
    public List<EmprestimoItem> EmprestimoItens { get; } = new();
    public List<RecebimentoItem> RecebimentoItens { get; } = new();

    private DataRepository()
    {
        // Caminho do arquivo Excel na pasta do executável
        var exePath = AppDomain.CurrentDomain.BaseDirectory;
        _dataFilePath = Path.Combine(exePath, "ControleEmprestimos.xlsx");
        
        _excelRepository = new ExcelDataRepository(_dataFilePath);
        
        // Carregar dados do Excel
        LoadFromExcel();
    }

    private void LoadFromExcel()
    {
        try
        {
            var data = _excelRepository.LoadData();
            
            Items.Clear();
            Items.AddRange(data.Items);
            
            Congregacoes.Clear();
            Congregacoes.AddRange(data.Congregacoes);
            
            Emprestimos.Clear();
            Emprestimos.AddRange(data.Emprestimos);
            
            EmprestimoItens.Clear();
            EmprestimoItens.AddRange(data.EmprestimoItens);
            
            RecebimentoEmprestimos.Clear();
            RecebimentoEmprestimos.AddRange(data.Recebimentos);
            
            RecebimentoItens.Clear();
            RecebimentoItens.AddRange(data.RecebimentoItens);
            
            _nextItemId = data.NextItemId;
            _nextCongregacaoId = data.NextCongregacaoId;
            _nextEmprestimoId = data.NextEmprestimoId;
            _nextEmprestimoItemId = data.NextEmprestimoItemId;
            _nextRecebimentoId = data.NextRecebimentoId;
            _nextRecebimentoItemId = data.NextRecebimentoItemId;
        }
        catch (Exception ex)
        {
            // Log do erro (em produção, usar um logger adequado)
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar dados do Excel: {ex.Message}");
        }
    }

    /// <summary>
    /// Recarrega todos os dados do arquivo Excel
    /// </summary>
    public void ReloadFromExcel()
    {
        LoadFromExcel();
    }

    private void SaveToExcel()
    {
        try
        {
            _excelRepository.SaveData(Items, Congregacoes, Emprestimos, EmprestimoItens, RecebimentoEmprestimos, RecebimentoItens);
        }
        catch (Exception ex)
        {
            // Log do erro (em produção, usar um logger adequado)
            System.Diagnostics.Debug.WriteLine($"Erro ao salvar dados no Excel: {ex.Message}");
            throw; // Re-lançar para que o usuário saiba que houve um erro ao salvar
        }
    }

    public static DataRepository Instance => _instance ??= new DataRepository();

    public int AddItem(Item item)
    {
        item.Id = _nextItemId++;
        var now = DateTime.Now;
        item.DataCriacao = now;
        item.DataAlteracao = now;
        Items.Add(item);
        SaveToExcel();
        return item.Id;
    }

    public void UpdateItem(Item item)
    {
        item.DataAlteracao = DateTime.Now;
        SaveToExcel();
    }

    public int AddEmprestimo(Emprestimo emprestimo)
    {
        emprestimo.Id = _nextEmprestimoId++;
        var now = DateTime.Now;
        emprestimo.DataCriacao = now;
        emprestimo.DataAlteracao = now;

        // Set congregation name if ID is provided
        if (emprestimo.CongregacaoId.HasValue)
        {
            var congregacao = Congregacoes.FirstOrDefault(c => c.Id == emprestimo.CongregacaoId.Value);
            if (congregacao != null)
            {
                emprestimo.CongregacaoName = congregacao.Name;
            }
        }

        // Processar itens do empréstimo
        foreach (var emprestimoItem in emprestimo.Itens)
        {
            emprestimoItem.Id = _nextEmprestimoItemId++;
            emprestimoItem.EmprestimoId = emprestimo.Id;
            emprestimoItem.DataCriacao = now;
            emprestimoItem.DataAlteracao = now;

            // Obter nome do item
            var item = Items.FirstOrDefault(i => i.Id == emprestimoItem.ItemId);
            if (item != null)
            {
                emprestimoItem.ItemName = item.Name;

                // Reduzir estoque
                item.QuantityInStock -= emprestimoItem.Quantidade;
            }

            EmprestimoItens.Add(emprestimoItem);
        }

        // Ensure DataEmprestimo is set
        if (emprestimo.DataEmprestimo == default)
        {
            emprestimo.DataEmprestimo = DateTime.Now;
        }

        // Ensure Status is set
        if (emprestimo.Status == default)
        {
            emprestimo.Status = StatusEmprestimo.EmAndamento;
        }

        Emprestimos.Add(emprestimo);
        SaveToExcel();
        return emprestimo.Id;
    }

    public void UpdateEmprestimo(Emprestimo emprestimo)
    {
        emprestimo.DataAlteracao = DateTime.Now;
        SaveToExcel();
    }

    public void DevolverEmprestimo(Emprestimo emprestimo)
    {
        // Verificar se todos os itens foram recebidos
        if (emprestimo.TodosItensRecebidos)
        {
            emprestimo.Status = StatusEmprestimo.Devolvido;
        }
        else
        {
            emprestimo.Status = StatusEmprestimo.EmAndamento;
        }

        emprestimo.DataAlteracao = DateTime.Now;
        SaveToExcel();
    }

    public int AddRecebimento(RecebimentoEmprestimo recebimento)
    {
        recebimento.Id = _nextRecebimentoId++;
        var now = DateTime.Now;
        recebimento.DataCriacao = now;
        recebimento.DataAlteracao = now;

        // Processar itens recebidos
        foreach (var recebimentoItem in recebimento.ItensRecebidos)
        {
            recebimentoItem.Id = _nextRecebimentoItemId++;
            recebimentoItem.RecebimentoEmprestimoId = recebimento.Id;
            recebimentoItem.DataCriacao = now;
            recebimentoItem.DataAlteracao = now;

            // Atualizar quantidade recebida no EmprestimoItem
            var emprestimoItem = EmprestimoItens.FirstOrDefault(ei => ei.Id == recebimentoItem.EmprestimoItemId);
            if (emprestimoItem != null)
            {
                emprestimoItem.QuantidadeRecebida += recebimentoItem.QuantidadeRecebida;
                emprestimoItem.DataAlteracao = now;

                // Obter nome do item
                var item = Items.FirstOrDefault(i => i.Id == emprestimoItem.ItemId);
                if (item != null)
                {
                    recebimentoItem.ItemName = item.Name;

                    // Repor estoque
                    item.QuantityInStock += recebimentoItem.QuantidadeRecebida;
                }
            }

            RecebimentoItens.Add(recebimentoItem);
        }

        // Ensure DataRecebimento is set
        if (recebimento.DataRecebimento == default)
        {
            recebimento.DataRecebimento = DateTime.Now;
        }

        RecebimentoEmprestimos.Add(recebimento);
        SaveToExcel();
        return recebimento.Id;
    }

    public int AddCongregacao(Congregacao congregacao)
    {
        congregacao.Id = _nextCongregacaoId++;
        var now = DateTime.Now;
        congregacao.DataCriacao = now;
        congregacao.DataAlteracao = now;
        Congregacoes.Add(congregacao);
        SaveToExcel();
        return congregacao.Id;
    }

    public void UpdateCongregacao(Congregacao congregacao)
    {
        congregacao.DataAlteracao = DateTime.Now;
        SaveToExcel();
    }

    public void RemoverEmprestimo(Emprestimo emprestimo)
    {
        // Repor estoque de todos os itens não recebidos
        foreach (var emprestimoItem in emprestimo.Itens)
        {
            var quantidadePendente = emprestimoItem.QuantidadePendente;
            if (quantidadePendente > 0)
            {
                var item = Items.FirstOrDefault(i => i.Id == emprestimoItem.ItemId);
                if (item != null)
                {
                    item.QuantityInStock += quantidadePendente;
                }
            }

            // Remover EmprestimoItem
            EmprestimoItens.Remove(emprestimoItem);
        }

        // Remover empréstimo
        Emprestimos.Remove(emprestimo);
        SaveToExcel();
    }

    public void RemoverRecebimento(RecebimentoEmprestimo recebimento)
    {
        if (recebimento.EmprestimoId.HasValue)
        {
            var emprestimo = Emprestimos.FirstOrDefault(e => e.Id == recebimento.EmprestimoId.Value);
            if (emprestimo != null)
            {
                // Reverter quantidades recebidas e estoque
                foreach (var recebimentoItem in recebimento.ItensRecebidos)
                {
                    var emprestimoItem = EmprestimoItens.FirstOrDefault(ei => ei.Id == recebimentoItem.EmprestimoItemId);
                    if (emprestimoItem != null)
                    {
                        // Reverter quantidade recebida
                        emprestimoItem.QuantidadeRecebida -= recebimentoItem.QuantidadeRecebida;
                        emprestimoItem.DataAlteracao = DateTime.Now;
                    }

                    // Reduzir estoque novamente
                    var item = Items.FirstOrDefault(i => i.Id == recebimentoItem.ItemId);
                    if (item != null)
                    {
                        item.QuantityInStock -= recebimentoItem.QuantidadeRecebida;
                    }

                    // Remover RecebimentoItem
                    RecebimentoItens.Remove(recebimentoItem);
                }

                // Atualizar status do empréstimo
                emprestimo.Status = emprestimo.TodosItensRecebidos ? StatusEmprestimo.Devolvido : StatusEmprestimo.EmAndamento;
            }
        }

        // Remover recebimento
        RecebimentoEmprestimos.Remove(recebimento);
        SaveToExcel();
    }
}
