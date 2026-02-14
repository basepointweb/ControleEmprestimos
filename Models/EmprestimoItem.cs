namespace ControleEmprestimos.Models;

public class EmprestimoItem
{
    public int Id { get; set; }
    public int EmprestimoId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public int QuantidadeRecebida { get; set; } = 0;
    
    // Propriedade calculada
    public int QuantidadePendente => Quantidade - QuantidadeRecebida;
    public bool TotalmenteRecebido => QuantidadeRecebida >= Quantidade;
    
    // Auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;
}
