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

    public List<Item> Items { get; } = new();
    public List<Emprestimo> Emprestimos { get; } = new();
    public List<RecebimentoEmprestimo> RecebimentoEmprestimos { get; } = new();
    public List<Congregacao> Congregacoes { get; } = new();
    public List<EmprestimoItem> EmprestimoItens { get; } = new();
    public List<RecebimentoItem> RecebimentoItens { get; } = new();

    private DataRepository()
    {
        // Seed inicial de congregações com setores
        var congregacoesIniciais = new (string Nome, string Setor)[]
        {
            ("Sede", ""),
            ("Bonsucesso", "SETOR E"),
            ("Sub-sede", ""),
            ("Barroso", "SETOR A"),
            ("Rosario", "SETOR A"),
            ("Corta Vento", "SETOR B"),
            ("Beira Linha", "SETOR A"),
            ("Quinta Lebrão", "SETOR C"),
            ("Fonte Santa", "SETOR C"),
            ("Fischer", "SETOR C"),
            ("Pessegueiros", "SETOR C"),
            ("Granja Florestal", "SETOR B"),
            ("Paineiras", "SETOR B"),
            ("Campanha", "SETOR E"),
            ("Vila do Pião", "SETOR C"),
            ("Vale Alpino", "SETOR E"),
            ("Brejal", "SETOR C"),
            ("Venda Nova", "SETOR D"),
            ("Caleme", "SETOR B"),
            ("Ponte do Porto", "SETOR D"),
            ("Albuquerque", "SETOR D"),
            ("Barra do Imbuí", "SETOR B"),
            ("Vargem Grande", "SETOR D"),
            ("Arrieiros", "SETOR B"),
            ("Jardim Feo", "SETOR B"),
            ("Granja Guarani", "SETOR A"),
            ("Posse", "SETOR B"),
            ("Jardim Meudom", "SETOR A"),
            ("Canoas", "SETOR D"),
            ("Rezende", "SETOR C"),
            ("Cascata do Imbuí", "SETOR B"),
            ("Coreia", "SETOR A"),
            ("Parque São Luiz", "SETOR A"),
            ("Vieira", "SETOR E"),
            ("Imbiú", "SETOR D"),
            ("Castelinho", "SETOR A"),
            ("Santa Rosa", "SETOR E"),
            ("Estrelinha", "SETOR E"),
            ("Cruzeiro", "SETOR C"),
            ("Três Córregos", "SETOR C"),
            ("Vila do Hélio", "SETOR D"),
            ("Campo Limpo", "SETOR C")
        };

        foreach (var (nome, setor) in congregacoesIniciais)
        {
            Congregacoes.Add(new Congregacao
            {
                Id = _nextCongregacaoId++,
                Name = nome,
                Setor = setor
            });
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
        return emprestimo.Id;
    }

    public void UpdateEmprestimo(Emprestimo emprestimo)
    {
        emprestimo.DataAlteracao = DateTime.Now;
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
    }
}
