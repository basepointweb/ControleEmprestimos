using ControleEmprestimos.Models;

namespace ControleEmprestimos.Data;

public class DataRepository
{
    private static DataRepository? _instance;
    private int _nextItemId = 1;
    private int _nextEmprestimoId = 1;
    private int _nextRecebimentoId = 1;
    private int _nextCongregacaoId = 1;

    public List<Item> Items { get; } = new();
    public List<Emprestimo> Emprestimos { get; } = new();
    public List<RecebimentoEmprestimo> RecebimentoEmprestimos { get; } = new();
    public List<Congregacao> Congregacoes { get; } = new();

    private DataRepository()
    {
        // Initialize with some sample data
        Items.Add(new Item { Id = _nextItemId++, Name = "Cadeira", QuantityInStock = 50 });
        Items.Add(new Item { Id = _nextItemId++, Name = "Mesa", QuantityInStock = 20 });
        
        Emprestimos.Add(new Emprestimo { Id = _nextEmprestimoId++, Name = "Projetor", QuantityInStock = 5 });
        
        RecebimentoEmprestimos.Add(new RecebimentoEmprestimo { Id = _nextRecebimentoId++, Name = "Cadeira Emprestada", QuantityInStock = 10 });
        
        Congregacoes.Add(new Congregacao { Id = _nextCongregacaoId++, Name = "Congregação Central", QuantityInStock = 0 });
    }

    public static DataRepository Instance => _instance ??= new DataRepository();

    public int AddItem(Item item)
    {
        item.Id = _nextItemId++;
        Items.Add(item);
        return item.Id;
    }

    public int AddEmprestimo(Emprestimo emprestimo)
    {
        emprestimo.Id = _nextEmprestimoId++;
        Emprestimos.Add(emprestimo);
        return emprestimo.Id;
    }

    public int AddRecebimento(RecebimentoEmprestimo recebimento)
    {
        recebimento.Id = _nextRecebimentoId++;
        RecebimentoEmprestimos.Add(recebimento);
        return recebimento.Id;
    }

    public int AddCongregacao(Congregacao congregacao)
    {
        congregacao.Id = _nextCongregacaoId++;
        Congregacoes.Add(congregacao);
        return congregacao.Id;
    }
}
