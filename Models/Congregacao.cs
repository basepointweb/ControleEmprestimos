namespace ControleEmprestimos.Models;

public class Congregacao
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Setor { get; set; } = string.Empty;
    public int QuantityInStock { get; set; }
    
    // Propriedade calculada - será preenchida pela listagem
    public int TotalItensEmprestados { get; set; }
    
    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;
}
