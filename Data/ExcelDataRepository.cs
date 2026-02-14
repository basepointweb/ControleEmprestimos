using ControleEmprestimos.Models;
using OfficeOpenXml;

namespace ControleEmprestimos.Data;

public class ExcelDataRepository
{
    private readonly string _filePath;
    private const string ITEMS_SHEET = "Itens";
    private const string CONGREGACOES_SHEET = "Congregacoes";
    private const string EMPRESTIMOS_SHEET = "Emprestimos";
    private const string EMPRESTIMO_ITENS_SHEET = "EmprestimoItens";
    private const string RECEBIMENTOS_SHEET = "Recebimentos";
    private const string RECEBIMENTO_ITENS_SHEET = "RecebimentoItens";

    public ExcelDataRepository(string filePath)
    {
        _filePath = filePath;
        
        // Configurar licença do EPPlus (uso não comercial)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        // Criar arquivo se não existir
        if (!File.Exists(_filePath))
        {
            CreateEmptyExcelFile();
        }
    }

    private void CreateEmptyExcelFile()
    {
        using var package = new ExcelPackage();
        
        // Criar abas
        CreateItemsSheet(package);
        CreateCongregacoesSheet(package);
        CreateEmprestimosSheet(package);
        CreateEmprestimoItensSheet(package);
        CreateRecebimentosSheet(package);
        CreateRecebimentoItensSheet(package);
        
        // Salvar arquivo
        package.SaveAs(new FileInfo(_filePath));
    }

    private void CreateItemsSheet(ExcelPackage package)
    {
        var worksheet = package.Workbook.Worksheets.Add(ITEMS_SHEET);
        
        // Cabeçalhos
        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Nome";
        worksheet.Cells[1, 3].Value = "QuantidadeEstoque";
        worksheet.Cells[1, 4].Value = "DataCriacao";
        worksheet.Cells[1, 5].Value = "DataAlteracao";
        
        // Formatar cabeçalhos
        using (var range = worksheet.Cells[1, 1, 1, 5])
        {
            range.Style.Font.Bold = true;
            range.AutoFilter = true;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void CreateCongregacoesSheet(ExcelPackage package)
    {
        var worksheet = package.Workbook.Worksheets.Add(CONGREGACOES_SHEET);
        
        // Cabeçalhos
        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Nome";
        worksheet.Cells[1, 3].Value = "Setor";
        worksheet.Cells[1, 4].Value = "DataCriacao";
        worksheet.Cells[1, 5].Value = "DataAlteracao";
        
        // Formatar cabeçalhos
        using (var range = worksheet.Cells[1, 1, 1, 5])
        {
            range.Style.Font.Bold = true;
            range.AutoFilter = true;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void CreateEmprestimosSheet(ExcelPackage package)
    {
        var worksheet = package.Workbook.Worksheets.Add(EMPRESTIMOS_SHEET);
        
        // Cabeçalhos
        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Nome";
        worksheet.Cells[1, 3].Value = "Motivo";
        worksheet.Cells[1, 4].Value = "CongregacaoId";
        worksheet.Cells[1, 5].Value = "CongregacaoNome";
        worksheet.Cells[1, 6].Value = "DataEmprestimo";
        worksheet.Cells[1, 7].Value = "Status";
        worksheet.Cells[1, 8].Value = "DataCriacao";
        worksheet.Cells[1, 9].Value = "DataAlteracao";
        
        // Formatar cabeçalhos
        using (var range = worksheet.Cells[1, 1, 1, 9])
        {
            range.Style.Font.Bold = true;
            range.AutoFilter = true;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void CreateEmprestimoItensSheet(ExcelPackage package)
    {
        var worksheet = package.Workbook.Worksheets.Add(EMPRESTIMO_ITENS_SHEET);
        
        // Cabeçalhos
        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "EmprestimoId";
        worksheet.Cells[1, 3].Value = "ItemId";
        worksheet.Cells[1, 4].Value = "ItemNome";
        worksheet.Cells[1, 5].Value = "Quantidade";
        worksheet.Cells[1, 6].Value = "QuantidadeRecebida";
        worksheet.Cells[1, 7].Value = "DataCriacao";
        worksheet.Cells[1, 8].Value = "DataAlteracao";
        
        // Formatar cabeçalhos
        using (var range = worksheet.Cells[1, 1, 1, 8])
        {
            range.Style.Font.Bold = true;
            range.AutoFilter = true;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void CreateRecebimentosSheet(ExcelPackage package)
    {
        var worksheet = package.Workbook.Worksheets.Add(RECEBIMENTOS_SHEET);
        
        // Cabeçalhos
        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Nome";
        worksheet.Cells[1, 3].Value = "NomeRecebedor";
        worksheet.Cells[1, 4].Value = "NomeQuemRecebeu";
        worksheet.Cells[1, 5].Value = "EmprestimoId";
        worksheet.Cells[1, 6].Value = "DataEmprestimo";
        worksheet.Cells[1, 7].Value = "DataRecebimento";
        worksheet.Cells[1, 8].Value = "RecebimentoParcial";
        worksheet.Cells[1, 9].Value = "DataCriacao";
        worksheet.Cells[1, 10].Value = "DataAlteracao";
        
        // Formatar cabeçalhos
        using (var range = worksheet.Cells[1, 1, 1, 10])
        {
            range.Style.Font.Bold = true;
            range.AutoFilter = true;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void CreateRecebimentoItensSheet(ExcelPackage package)
    {
        var worksheet = package.Workbook.Worksheets.Add(RECEBIMENTO_ITENS_SHEET);
        
        // Cabeçalhos
        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "RecebimentoEmprestimoId";
        worksheet.Cells[1, 3].Value = "EmprestimoItemId";
        worksheet.Cells[1, 4].Value = "ItemId";
        worksheet.Cells[1, 5].Value = "ItemNome";
        worksheet.Cells[1, 6].Value = "QuantidadeRecebida";
        worksheet.Cells[1, 7].Value = "DataCriacao";
        worksheet.Cells[1, 8].Value = "DataAlteracao";
        
        // Formatar cabeçalhos
        using (var range = worksheet.Cells[1, 1, 1, 8])
        {
            range.Style.Font.Bold = true;
            range.AutoFilter = true;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    public void SaveData(
        List<Item> items,
        List<Congregacao> congregacoes,
        List<Emprestimo> emprestimos,
        List<EmprestimoItem> emprestimoItens,
        List<RecebimentoEmprestimo> recebimentos,
        List<RecebimentoItem> recebimentoItens)
    {
        using var package = new ExcelPackage(new FileInfo(_filePath));
        
        // Salvar cada entidade em sua aba
        SaveItems(package, items);
        SaveCongregacoes(package, congregacoes);
        SaveEmprestimos(package, emprestimos);
        SaveEmprestimoItens(package, emprestimoItens);
        SaveRecebimentos(package, recebimentos);
        SaveRecebimentoItens(package, recebimentoItens);
        
        // Salvar arquivo
        package.Save();
    }

    private void SaveItems(ExcelPackage package, List<Item> items)
    {
        var worksheet = package.Workbook.Worksheets[ITEMS_SHEET];
        
        // Limpar dados existentes (manter cabeçalho)
        if (worksheet.Dimension != null && worksheet.Dimension.End.Row > 1)
        {
            worksheet.DeleteRow(2, worksheet.Dimension.End.Row - 1);
        }
        
        // Adicionar dados
        int row = 2;
        foreach (var item in items)
        {
            worksheet.Cells[row, 1].Value = item.Id;
            worksheet.Cells[row, 2].Value = item.Name;
            worksheet.Cells[row, 3].Value = item.QuantityInStock;
            worksheet.Cells[row, 4].Value = item.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 5].Value = item.DataAlteracao.ToString("yyyy-MM-dd HH:mm:ss");
            row++;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void SaveCongregacoes(ExcelPackage package, List<Congregacao> congregacoes)
    {
        var worksheet = package.Workbook.Worksheets[CONGREGACOES_SHEET];
        
        // Limpar dados existentes (manter cabeçalho)
        if (worksheet.Dimension != null && worksheet.Dimension.End.Row > 1)
        {
            worksheet.DeleteRow(2, worksheet.Dimension.End.Row - 1);
        }
        
        // Adicionar dados
        int row = 2;
        foreach (var congregacao in congregacoes)
        {
            worksheet.Cells[row, 1].Value = congregacao.Id;
            worksheet.Cells[row, 2].Value = congregacao.Name;
            worksheet.Cells[row, 3].Value = congregacao.Setor;
            worksheet.Cells[row, 4].Value = congregacao.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 5].Value = congregacao.DataAlteracao.ToString("yyyy-MM-dd HH:mm:ss");
            row++;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void SaveEmprestimos(ExcelPackage package, List<Emprestimo> emprestimos)
    {
        var worksheet = package.Workbook.Worksheets[EMPRESTIMOS_SHEET];
        
        // Limpar dados existentes (manter cabeçalho)
        if (worksheet.Dimension != null && worksheet.Dimension.End.Row > 1)
        {
            worksheet.DeleteRow(2, worksheet.Dimension.End.Row - 1);
        }
        
        // Adicionar dados
        int row = 2;
        foreach (var emprestimo in emprestimos)
        {
            worksheet.Cells[row, 1].Value = emprestimo.Id;
            worksheet.Cells[row, 2].Value = emprestimo.Name;
            worksheet.Cells[row, 3].Value = emprestimo.Motivo;
            worksheet.Cells[row, 4].Value = emprestimo.CongregacaoId;
            worksheet.Cells[row, 5].Value = emprestimo.CongregacaoName;
            worksheet.Cells[row, 6].Value = emprestimo.DataEmprestimo.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 7].Value = (int)emprestimo.Status;
            worksheet.Cells[row, 8].Value = emprestimo.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 9].Value = emprestimo.DataAlteracao.ToString("yyyy-MM-dd HH:mm:ss");
            row++;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void SaveEmprestimoItens(ExcelPackage package, List<EmprestimoItem> emprestimoItens)
    {
        var worksheet = package.Workbook.Worksheets[EMPRESTIMO_ITENS_SHEET];
        
        // Limpar dados existentes (manter cabeçalho)
        if (worksheet.Dimension != null && worksheet.Dimension.End.Row > 1)
        {
            worksheet.DeleteRow(2, worksheet.Dimension.End.Row - 1);
        }
        
        // Adicionar dados
        int row = 2;
        foreach (var emprestimoItem in emprestimoItens)
        {
            worksheet.Cells[row, 1].Value = emprestimoItem.Id;
            worksheet.Cells[row, 2].Value = emprestimoItem.EmprestimoId;
            worksheet.Cells[row, 3].Value = emprestimoItem.ItemId;
            worksheet.Cells[row, 4].Value = emprestimoItem.ItemName;
            worksheet.Cells[row, 5].Value = emprestimoItem.Quantidade;
            worksheet.Cells[row, 6].Value = emprestimoItem.QuantidadeRecebida;
            worksheet.Cells[row, 7].Value = emprestimoItem.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 8].Value = emprestimoItem.DataAlteracao.ToString("yyyy-MM-dd HH:mm:ss");
            row++;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void SaveRecebimentos(ExcelPackage package, List<RecebimentoEmprestimo> recebimentos)
    {
        var worksheet = package.Workbook.Worksheets[RECEBIMENTOS_SHEET];
        
        // Limpar dados existentes (manter cabeçalho)
        if (worksheet.Dimension != null && worksheet.Dimension.End.Row > 1)
        {
            worksheet.DeleteRow(2, worksheet.Dimension.End.Row - 1);
        }
        
        // Adicionar dados
        int row = 2;
        foreach (var recebimento in recebimentos)
        {
            worksheet.Cells[row, 1].Value = recebimento.Id;
            worksheet.Cells[row, 2].Value = recebimento.Name;
            worksheet.Cells[row, 3].Value = recebimento.NomeRecebedor;
            worksheet.Cells[row, 4].Value = recebimento.NomeQuemRecebeu;
            worksheet.Cells[row, 5].Value = recebimento.EmprestimoId;
            worksheet.Cells[row, 6].Value = recebimento.DataEmprestimo?.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 7].Value = recebimento.DataRecebimento.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 8].Value = recebimento.RecebimentoParcial;
            worksheet.Cells[row, 9].Value = recebimento.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 10].Value = recebimento.DataAlteracao.ToString("yyyy-MM-dd HH:mm:ss");
            row++;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    private void SaveRecebimentoItens(ExcelPackage package, List<RecebimentoItem> recebimentoItens)
    {
        var worksheet = package.Workbook.Worksheets[RECEBIMENTO_ITENS_SHEET];
        
        // Limpar dados existentes (manter cabeçalho)
        if (worksheet.Dimension != null && worksheet.Dimension.End.Row > 1)
        {
            worksheet.DeleteRow(2, worksheet.Dimension.End.Row - 1);
        }
        
        // Adicionar dados
        int row = 2;
        foreach (var recebimentoItem in recebimentoItens)
        {
            worksheet.Cells[row, 1].Value = recebimentoItem.Id;
            worksheet.Cells[row, 2].Value = recebimentoItem.RecebimentoEmprestimoId;
            worksheet.Cells[row, 3].Value = recebimentoItem.EmprestimoItemId;
            worksheet.Cells[row, 4].Value = recebimentoItem.ItemId;
            worksheet.Cells[row, 5].Value = recebimentoItem.ItemName;
            worksheet.Cells[row, 6].Value = recebimentoItem.QuantidadeRecebida;
            worksheet.Cells[row, 7].Value = recebimentoItem.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 8].Value = recebimentoItem.DataAlteracao.ToString("yyyy-MM-dd HH:mm:ss");
            row++;
        }
        
        worksheet.Cells.AutoFitColumns();
    }

    public (List<Item> Items,
            List<Congregacao> Congregacoes,
            List<Emprestimo> Emprestimos,
            List<EmprestimoItem> EmprestimoItens,
            List<RecebimentoEmprestimo> Recebimentos,
            List<RecebimentoItem> RecebimentoItens,
            int NextItemId,
            int NextCongregacaoId,
            int NextEmprestimoId,
            int NextEmprestimoItemId,
            int NextRecebimentoId,
            int NextRecebimentoItemId) LoadData()
    {
        var items = new List<Item>();
        var congregacoes = new List<Congregacao>();
        var emprestimos = new List<Emprestimo>();
        var emprestimoItens = new List<EmprestimoItem>();
        var recebimentos = new List<RecebimentoEmprestimo>();
        var recebimentoItens = new List<RecebimentoItem>();

        int nextItemId = 1;
        int nextCongregacaoId = 1;
        int nextEmprestimoId = 1;
        int nextEmprestimoItemId = 1;
        int nextRecebimentoId = 1;
        int nextRecebimentoItemId = 1;

        using (var package = new ExcelPackage(new FileInfo(_filePath)))
        {
            // Carregar Itens
            var itemsSheet = package.Workbook.Worksheets[ITEMS_SHEET];
            if (itemsSheet?.Dimension != null)
            {
                for (int row = 2; row <= itemsSheet.Dimension.End.Row; row++)
                {
                    var id = int.Parse(itemsSheet.Cells[row, 1].Value?.ToString() ?? "0");
                    items.Add(new Item
                    {
                        Id = id,
                        Name = itemsSheet.Cells[row, 2].Value?.ToString() ?? string.Empty,
                        QuantityInStock = int.Parse(itemsSheet.Cells[row, 3].Value?.ToString() ?? "0"),
                        DataCriacao = DateTime.Parse(itemsSheet.Cells[row, 4].Value?.ToString() ?? DateTime.Now.ToString()),
                        DataAlteracao = DateTime.Parse(itemsSheet.Cells[row, 5].Value?.ToString() ?? DateTime.Now.ToString())
                    });
                    
                    if (id >= nextItemId)
                        nextItemId = id + 1;
                }
            }

            // Carregar Congregações
            var congregacoesSheet = package.Workbook.Worksheets[CONGREGACOES_SHEET];
            if (congregacoesSheet?.Dimension != null)
            {
                for (int row = 2; row <= congregacoesSheet.Dimension.End.Row; row++)
                {
                    var id = int.Parse(congregacoesSheet.Cells[row, 1].Value?.ToString() ?? "0");
                    congregacoes.Add(new Congregacao
                    {
                        Id = id,
                        Name = congregacoesSheet.Cells[row, 2].Value?.ToString() ?? string.Empty,
                        Setor = congregacoesSheet.Cells[row, 3].Value?.ToString() ?? string.Empty,
                        DataCriacao = DateTime.Parse(congregacoesSheet.Cells[row, 4].Value?.ToString() ?? DateTime.Now.ToString()),
                        DataAlteracao = DateTime.Parse(congregacoesSheet.Cells[row, 5].Value?.ToString() ?? DateTime.Now.ToString())
                    });
                    
                    if (id >= nextCongregacaoId)
                        nextCongregacaoId = id + 1;
                }
            }

            // Carregar Empréstimos
            var emprestimosSheet = package.Workbook.Worksheets[EMPRESTIMOS_SHEET];
            if (emprestimosSheet?.Dimension != null)
            {
                for (int row = 2; row <= emprestimosSheet.Dimension.End.Row; row++)
                {
                    var id = int.Parse(emprestimosSheet.Cells[row, 1].Value?.ToString() ?? "0");
                    var congregacaoIdStr = emprestimosSheet.Cells[row, 4].Value?.ToString();
                    
                    emprestimos.Add(new Emprestimo
                    {
                        Id = id,
                        Name = emprestimosSheet.Cells[row, 2].Value?.ToString() ?? string.Empty,
                        Motivo = emprestimosSheet.Cells[row, 3].Value?.ToString() ?? string.Empty,
                        CongregacaoId = string.IsNullOrEmpty(congregacaoIdStr) ? null : int.Parse(congregacaoIdStr),
                        CongregacaoName = emprestimosSheet.Cells[row, 5].Value?.ToString() ?? string.Empty,
                        DataEmprestimo = DateTime.Parse(emprestimosSheet.Cells[row, 6].Value?.ToString() ?? DateTime.Now.ToString()),
                        Status = (StatusEmprestimo)int.Parse(emprestimosSheet.Cells[row, 7].Value?.ToString() ?? "1"),
                        DataCriacao = DateTime.Parse(emprestimosSheet.Cells[row, 8].Value?.ToString() ?? DateTime.Now.ToString()),
                        DataAlteracao = DateTime.Parse(emprestimosSheet.Cells[row, 9].Value?.ToString() ?? DateTime.Now.ToString())
                    });
                    
                    if (id >= nextEmprestimoId)
                        nextEmprestimoId = id + 1;
                }
            }

            // Carregar EmprestimoItens
            var emprestimoItensSheet = package.Workbook.Worksheets[EMPRESTIMO_ITENS_SHEET];
            if (emprestimoItensSheet?.Dimension != null)
            {
                for (int row = 2; row <= emprestimoItensSheet.Dimension.End.Row; row++)
                {
                    var id = int.Parse(emprestimoItensSheet.Cells[row, 1].Value?.ToString() ?? "0");
                    emprestimoItens.Add(new EmprestimoItem
                    {
                        Id = id,
                        EmprestimoId = int.Parse(emprestimoItensSheet.Cells[row, 2].Value?.ToString() ?? "0"),
                        ItemId = int.Parse(emprestimoItensSheet.Cells[row, 3].Value?.ToString() ?? "0"),
                        ItemName = emprestimoItensSheet.Cells[row, 4].Value?.ToString() ?? string.Empty,
                        Quantidade = int.Parse(emprestimoItensSheet.Cells[row, 5].Value?.ToString() ?? "0"),
                        QuantidadeRecebida = int.Parse(emprestimoItensSheet.Cells[row, 6].Value?.ToString() ?? "0"),
                        DataCriacao = DateTime.Parse(emprestimoItensSheet.Cells[row, 7].Value?.ToString() ?? DateTime.Now.ToString()),
                        DataAlteracao = DateTime.Parse(emprestimoItensSheet.Cells[row, 8].Value?.ToString() ?? DateTime.Now.ToString())
                    });
                    
                    if (id >= nextEmprestimoItemId)
                        nextEmprestimoItemId = id + 1;
                }
            }

            // Carregar Recebimentos
            var recebimentosSheet = package.Workbook.Worksheets[RECEBIMENTOS_SHEET];
            if (recebimentosSheet?.Dimension != null)
            {
                for (int row = 2; row <= recebimentosSheet.Dimension.End.Row; row++)
                {
                    var id = int.Parse(recebimentosSheet.Cells[row, 1].Value?.ToString() ?? "0");
                    var emprestimoIdStr = recebimentosSheet.Cells[row, 5].Value?.ToString();
                    var dataEmprestimoStr = recebimentosSheet.Cells[row, 6].Value?.ToString();
                    
                    recebimentos.Add(new RecebimentoEmprestimo
                    {
                        Id = id,
                        Name = recebimentosSheet.Cells[row, 2].Value?.ToString() ?? string.Empty,
                        NomeRecebedor = recebimentosSheet.Cells[row, 3].Value?.ToString() ?? string.Empty,
                        NomeQuemRecebeu = recebimentosSheet.Cells[row, 4].Value?.ToString() ?? string.Empty,
                        EmprestimoId = string.IsNullOrEmpty(emprestimoIdStr) ? null : int.Parse(emprestimoIdStr),
                        DataEmprestimo = string.IsNullOrEmpty(dataEmprestimoStr) ? null : DateTime.Parse(dataEmprestimoStr),
                        DataRecebimento = DateTime.Parse(recebimentosSheet.Cells[row, 7].Value?.ToString() ?? DateTime.Now.ToString()),
                        RecebimentoParcial = bool.Parse(recebimentosSheet.Cells[row, 8].Value?.ToString() ?? "False"),
                        DataCriacao = DateTime.Parse(recebimentosSheet.Cells[row, 9].Value?.ToString() ?? DateTime.Now.ToString()),
                        DataAlteracao = DateTime.Parse(recebimentosSheet.Cells[row, 10].Value?.ToString() ?? DateTime.Now.ToString())
                    });
                    
                    if (id >= nextRecebimentoId)
                        nextRecebimentoId = id + 1;
                }
            }

            // Carregar RecebimentoItens
            var recebimentoItensSheet = package.Workbook.Worksheets[RECEBIMENTO_ITENS_SHEET];
            if (recebimentoItensSheet?.Dimension != null)
            {
                for (int row = 2; row <= recebimentoItensSheet.Dimension.End.Row; row++)
                {
                    var id = int.Parse(recebimentoItensSheet.Cells[row, 1].Value?.ToString() ?? "0");
                    recebimentoItens.Add(new RecebimentoItem
                    {
                        Id = id,
                        RecebimentoEmprestimoId = int.Parse(recebimentoItensSheet.Cells[row, 2].Value?.ToString() ?? "0"),
                        EmprestimoItemId = int.Parse(recebimentoItensSheet.Cells[row, 3].Value?.ToString() ?? "0"),
                        ItemId = int.Parse(recebimentoItensSheet.Cells[row, 4].Value?.ToString() ?? "0"),
                        ItemName = recebimentoItensSheet.Cells[row, 5].Value?.ToString() ?? string.Empty,
                        QuantidadeRecebida = int.Parse(recebimentoItensSheet.Cells[row, 6].Value?.ToString() ?? "0"),
                        DataCriacao = DateTime.Parse(recebimentoItensSheet.Cells[row, 7].Value?.ToString() ?? DateTime.Now.ToString()),
                        DataAlteracao = DateTime.Parse(recebimentoItensSheet.Cells[row, 8].Value?.ToString() ?? DateTime.Now.ToString())
                    });
                    
                    if (id >= nextRecebimentoItemId)
                        nextRecebimentoItemId = id + 1;
                }
            }
        }

        // Relacionar EmprestimoItens com Emprestimos
        foreach (var emprestimo in emprestimos)
        {
            emprestimo.Itens = emprestimoItens.Where(ei => ei.EmprestimoId == emprestimo.Id).ToList();
        }

        // Relacionar RecebimentoItens com Recebimentos
        foreach (var recebimento in recebimentos)
        {
            recebimento.ItensRecebidos = recebimentoItens.Where(ri => ri.RecebimentoEmprestimoId == recebimento.Id).ToList();
        }

        return (items, congregacoes, emprestimos, emprestimoItens, recebimentos, recebimentoItens,
                nextItemId, nextCongregacaoId, nextEmprestimoId, nextEmprestimoItemId, nextRecebimentoId, nextRecebimentoItemId);
    }
}
