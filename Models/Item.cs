namespace ControleEmprestimos.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int QuantityInStock { get; set; }
    
    // Propriedade calculada - será preenchida pela listagem
    public int TotalEmprestado { get; set; }
}
