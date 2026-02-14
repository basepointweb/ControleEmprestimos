namespace ControleEmprestimos.Models;

public class RecebimentoEmprestimo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NomeRecebedor { get; set; } = string.Empty; // Quem pegou emprestado
    public string NomeQuemRecebeu { get; set; } = string.Empty; // Quem recebeu de volta
    public int? EmprestimoId { get; set; }
    public DateTime? DataEmprestimo { get; set; }
    public DateTime DataRecebimento { get; set; } = DateTime.Now;
    
    // Lista de itens recebidos (NOVO)
    public List<RecebimentoItem> ItensRecebidos { get; set; } = new();
    
    // Propriedades calculadas
    public int TotalItensRecebidos => ItensRecebidos.Sum(i => i.QuantidadeRecebida);
    public bool RecebimentoParcial { get; set; } = false;
    
    // Compatibilidade com código antigo (deprecated)
    [Obsolete("Use ItensRecebidos ao invés de QuantityInStock")]
    public int QuantityInStock { get; set; }
    
    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;
}
