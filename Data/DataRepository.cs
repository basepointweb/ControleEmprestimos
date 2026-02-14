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
        var now = DateTime.Now;

        // Initialize with some sample data
        Items.Add(new Item { Id = _nextItemId++, Name = "Cadeira", QuantityInStock = 50, DataCriacao = now, DataAlteracao = now });
        Items.Add(new Item { Id = _nextItemId++, Name = "Mesa", QuantityInStock = 20, DataCriacao = now, DataAlteracao = now });
        Items.Add(new Item { Id = _nextItemId++, Name = "Projetor", QuantityInStock = 5, DataCriacao = now, DataAlteracao = now });

        Congregacoes.Add(new Congregacao { Id = _nextCongregacaoId++, Name = "Congregação Central", QuantityInStock = 0, DataCriacao = now, DataAlteracao = now });
        Congregacoes.Add(new Congregacao { Id = _nextCongregacaoId++, Name = "Congregação Norte", QuantityInStock = 0, DataCriacao = now, DataAlteracao = now });

        // Empréstimo Em Andamento
        Emprestimos.Add(new Emprestimo
        {
            Id = _nextEmprestimoId++,
            Name = "João Silva", // Recebedor
            Motivo = "Evento especial de fim de ano",
            QuantityInStock = 2,
            CongregacaoId = 1,
            CongregacaoName = "Congregação Central",
            ItemId = 3,
            ItemName = "Projetor",
            DataEmprestimo = DateTime.Now.AddDays(-5),
            Status = StatusEmprestimo.EmAndamento,
            DataCriacao = now.AddDays(-5),
            DataAlteracao = now.AddDays(-5)
        });

        // Empréstimo Devolvido
        var emprestimoDevolvido = new Emprestimo
        {
            Id = _nextEmprestimoId++,
            Name = "Maria Santos",
            Motivo = "Reunião administrativa",
            QuantityInStock = 10,
            CongregacaoId = 2,
            CongregacaoName = "Congregação Norte",
            ItemId = 1,
            ItemName = "Cadeira",
            DataEmprestimo = DateTime.Now.AddDays(-10),
            Status = StatusEmprestimo.Devolvido,
            DataCriacao = now.AddDays(-10),
            DataAlteracao = now.AddDays(-3)
        };
        Emprestimos.Add(emprestimoDevolvido);

        // Recebimento correspondente
        RecebimentoEmprestimos.Add(new RecebimentoEmprestimo
        {
            Id = _nextRecebimentoId++,
            Name = "Recebimento - Cadeira",
            NomeRecebedor = "Maria Santos",
            QuantityInStock = 10,
            EmprestimoId = emprestimoDevolvido.Id,
            DataEmprestimo = emprestimoDevolvido.DataEmprestimo,
            DataRecebimento = DateTime.Now.AddDays(-3),
            DataCriacao = now.AddDays(-3),
            DataAlteracao = now.AddDays(-3)
        });
    }

    public static DataRepository Instance => _instance ??= new DataRepository();

    public int AddItem(Item item)
    {
        item.Id = _nextItemId++;
        var now = DateTime.Now;
        item.DataCriacao = now;
        item.DataAlteracao = now;
        Items.Add(item);
        return item.Id;
    }

    public void UpdateItem(Item item)
    {
        item.DataAlteracao = DateTime.Now;
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

        // Set item name if ID is provided
        if (emprestimo.ItemId.HasValue)
        {
            var item = Items.FirstOrDefault(i => i.Id == emprestimo.ItemId.Value);
            if (item != null)
            {
                emprestimo.ItemName = item.Name;

                // Reduzir estoque ao criar empréstimo
                item.QuantityInStock -= emprestimo.QuantityInStock;
            }
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
        return emprestimo.Id;
    }

    public void UpdateEmprestimo(Emprestimo emprestimo)
    {
        emprestimo.DataAlteracao = DateTime.Now;
    }

    public void DevolverEmprestimo(Emprestimo emprestimo)
    {
        // Repor estoque ao devolver empréstimo
        if (emprestimo.ItemId.HasValue)
        {
            var item = Items.FirstOrDefault(i => i.Id == emprestimo.ItemId.Value);
            if (item != null)
            {
                item.QuantityInStock += emprestimo.QuantityInStock;
            }
        }

        emprestimo.Status = StatusEmprestimo.Devolvido;
        emprestimo.DataAlteracao = DateTime.Now;
    }

    public int AddRecebimento(RecebimentoEmprestimo recebimento)
    {
        recebimento.Id = _nextRecebimentoId++;
        var now = DateTime.Now;
        recebimento.DataCriacao = now;
        recebimento.DataAlteracao = now;

        // Ensure DataRecebimento is set
        if (recebimento.DataRecebimento == default)
        {
            recebimento.DataRecebimento = DateTime.Now;
        }

        RecebimentoEmprestimos.Add(recebimento);
        return recebimento.Id;
    }

    public int AddCongregacao(Congregacao congregacao)
    {
        congregacao.Id = _nextCongregacaoId++;
        var now = DateTime.Now;
        congregacao.DataCriacao = now;
        congregacao.DataAlteracao = now;
        Congregacoes.Add(congregacao);
        return congregacao.Id;
    }

    public void UpdateCongregacao(Congregacao congregacao)
    {
        congregacao.DataAlteracao = DateTime.Now;
    }
}
