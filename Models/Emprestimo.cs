namespace ControleEmprestimos.Models;

public class Emprestimo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Recebedor
    public string Motivo { get; set; } = string.Empty;
    public int QuantityInStock { get; set; }
    public int? CongregacaoId { get; set; }
    public string CongregacaoName { get; set; } = string.Empty;
    public int? ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public DateTime DataEmprestimo { get; set; } = DateTime.Now;
    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.EmAndamento;
    
    // Propriedade para exibição do status
    public string StatusDescricao => Status switch
    {
        StatusEmprestimo.EmAndamento => "Em Andamento",
        StatusEmprestimo.Devolvido => "Devolvido",
        StatusEmprestimo.Cancelado => "Cancelado",
        _ => "Desconhecido"
    };
}
