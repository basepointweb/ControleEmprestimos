namespace ControleEmprestimos.Models;

public class RecebimentoEmprestimo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NomeRecebedor { get; set; } = string.Empty; // Quem pegou emprestado
    public string NomeQuemRecebeu { get; set; } = string.Empty; // Quem recebeu de volta (novo campo)
    public int QuantityInStock { get; set; }
    public int? EmprestimoId { get; set; }
    public DateTime? DataEmprestimo { get; set; }
    public DateTime DataRecebimento { get; set; } = DateTime.Now;
    
    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;
}
