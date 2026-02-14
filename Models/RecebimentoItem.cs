namespace ControleEmprestimos.Models;

public class RecebimentoItem
{
    public int Id { get; set; }
    public int RecebimentoEmprestimoId { get; set; }
    public int EmprestimoItemId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int QuantidadeRecebida { get; set; }
    
    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;
}
