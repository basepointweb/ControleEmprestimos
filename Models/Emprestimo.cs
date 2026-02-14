namespace ControleEmprestimos.Models;

public class Emprestimo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Recebedor
    public string Motivo { get; set; } = string.Empty;
    public int? CongregacaoId { get; set; }
    public string CongregacaoName { get; set; } = string.Empty;
    public DateTime DataEmprestimo { get; set; } = DateTime.Now;
    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.EmAndamento;
    
    // Lista de itens do empréstimo (NOVO)
    public List<EmprestimoItem> Itens { get; set; } = new();
    
    // Propriedade para exibição do status
    public string StatusDescricao => Status switch
    {
        StatusEmprestimo.EmAndamento => "Em Andamento",
        StatusEmprestimo.Devolvido => "Devolvido",
        StatusEmprestimo.Cancelado => "Cancelado",
        _ => "Desconhecido"
    };
    
    // Propriedades calculadas
    public int TotalItens => Itens.Sum(i => i.Quantidade);
    public int TotalRecebido => Itens.Sum(i => i.QuantidadeRecebida);
    public int TotalPendente => TotalItens - TotalRecebido;
    public bool TodosItensRecebidos => Itens.All(i => i.TotalmenteRecebido);
    
    // Compatibilidade com código antigo (deprecated)
    [Obsolete("Use Itens ao invés de ItemId")]
    public int? ItemId { get; set; }
    
    [Obsolete("Use Itens ao invés de ItemName")]
    public string ItemName { get; set; } = string.Empty;
    
    [Obsolete("Use TotalItens ao invés de QuantityInStock")]
    public int QuantityInStock { get; set; }
    
    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;
}
