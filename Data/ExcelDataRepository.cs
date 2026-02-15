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
    private const string INSTRUCOES_SHEET = "Instrucoes";

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
        
        // Criar aba de instruções PRIMEIRO (para aparecer como primeira aba)
        CreateInstrucoesSheet(package);
        
        // Criar abas de dados
        CreateItemsSheet(package);
        CreateCongregacoesSheet(package);
        CreateEmprestimosSheet(package);
        CreateEmprestimoItensSheet(package);
        CreateRecebimentosSheet(package);
        CreateRecebimentoItensSheet(package);
        
        // Salvar arquivo
        package.SaveAs(new FileInfo(_filePath));
    }

    private void CreateInstrucoesSheet(ExcelPackage package)
    {
        var worksheet = package.Workbook.Worksheets.Add(INSTRUCOES_SHEET);
        
        int row = 1;
        
        // Título
        worksheet.Cells[row, 1].Value = "INSTRUÇÕES - CONTROLE DE EMPRÉSTIMOS DE BENS";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 16;
        worksheet.Cells[row, 1, row, 4].Merge = true;
        row += 2;
        
        // Seção 1: Visão Geral
        worksheet.Cells[row, 1].Value = "1. VISÃO GERAL DO SISTEMA";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.LightBlue);
        row++;
        
        worksheet.Cells[row, 1].Value = "Este sistema controla empréstimos de bens patrimoniais para congregações.";
        row++;
        worksheet.Cells[row, 1].Value = "Os dados são armazenados em 6 abas principais:";
        row++;
        worksheet.Cells[row, 1].Value = "  • Itens: Cadastro dos bens disponíveis";
        row++;
        worksheet.Cells[row, 1].Value = "  • Congregacoes: Cadastro das congregações";
        row++;
        worksheet.Cells[row, 1].Value = "  • Emprestimos: Registro dos empréstimos realizados";
        row++;
        worksheet.Cells[row, 1].Value = "  • EmprestimoItens: Detalhamento dos bens emprestados";
        row++;
        worksheet.Cells[row, 1].Value = "  • Recebimentos: Registro das devoluções";
        row++;
        worksheet.Cells[row, 1].Value = "  • RecebimentoItens: Detalhamento dos bens devolvidos";
        row += 2;
        
        // Seção 2: Aba Itens
        worksheet.Cells[row, 1].Value = "2. ABA: ITENS (Bens Patrimoniais)";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.LightGreen);
        row++;
        
        worksheet.Cells[row, 1].Value = "Campo";
        worksheet.Cells[row, 2].Value = "Tipo";
        worksheet.Cells[row, 3].Value = "Descrição";
        worksheet.Cells[row, 4].Value = "Exemplo";
        worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
        row++;
        
        worksheet.Cells[row, 1].Value = "Id";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Identificador único do bem (sequencial)";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "Nome";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Nome do bem";
        worksheet.Cells[row, 4].Value = "CADEIRA";
        row++;
        
        worksheet.Cells[row, 1].Value = "QuantidadeEstoque";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Quantidade disponível em estoque";
        worksheet.Cells[row, 4].Value = "50";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataCriacao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data de cadastro do bem";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataAlteracao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data da última modificação";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row += 2;
        
        // Seção 3: Aba Congregacoes
        worksheet.Cells[row, 1].Value = "3. ABA: CONGREGACOES";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.LightGreen);
        row++;
        
        worksheet.Cells[row, 1].Value = "Campo";
        worksheet.Cells[row, 2].Value = "Tipo";
        worksheet.Cells[row, 3].Value = "Descrição";
        worksheet.Cells[row, 4].Value = "Exemplo";
        worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
        row++;
        
        worksheet.Cells[row, 1].Value = "Id";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Identificador único da congregação";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "Nome";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Nome da congregação";
        worksheet.Cells[row, 4].Value = "CONGREGAÇÃO CENTRAL";
        row++;
        
        worksheet.Cells[row, 1].Value = "Setor";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Setor ou região da congregação";
        worksheet.Cells[row, 4].Value = "ZONA SUL";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataCriacao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data de cadastro";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataAlteracao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data da última modificação";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row += 2;
        
        // Seção 4: Aba Emprestimos
        worksheet.Cells[row, 1].Value = "4. ABA: EMPRESTIMOS";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.LightGreen);
        row++;
        
        worksheet.Cells[row, 1].Value = "Campo";
        worksheet.Cells[row, 2].Value = "Tipo";
        worksheet.Cells[row, 3].Value = "Descrição";
        worksheet.Cells[row, 4].Value = "Exemplo";
        worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
        row++;
        
        worksheet.Cells[row, 1].Value = "Id";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Identificador único do empréstimo";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "Nome";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Nome da pessoa que recebeu o empréstimo";
        worksheet.Cells[row, 4].Value = "JOÃO SILVA";
        row++;
        
        worksheet.Cells[row, 1].Value = "Motivo";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Motivo do empréstimo";
        worksheet.Cells[row, 4].Value = "EVENTO ESPECIAL";
        row++;
        
        worksheet.Cells[row, 1].Value = "CongregacaoId";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "ID da congregação (referência à aba Congregacoes)";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "CongregacaoNome";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Nome da congregação (cópia para facilitar)";
        worksheet.Cells[row, 4].Value = "CONGREGAÇÃO CENTRAL";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataEmprestimo";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data em que o empréstimo foi realizado";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row++;
        
        worksheet.Cells[row, 1].Value = "Status";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Status: 1=Em Andamento, 2=Devolvido, 3=Cancelado";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "QuemLiberou";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Nome de quem autorizou o empréstimo";
        worksheet.Cells[row, 4].Value = "MARIA SANTOS";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataCriacao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data de registro do empréstimo";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataAlteracao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data da última modificação";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row += 2;
        
        // Seção 5: Aba EmprestimoItens
        worksheet.Cells[row, 1].Value = "5. ABA: EMPRESTIMOITENS (Detalhamento dos Bens Emprestados)";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.LightGreen);
        row++;
        
        worksheet.Cells[row, 1].Value = "Campo";
        worksheet.Cells[row, 2].Value = "Tipo";
        worksheet.Cells[row, 3].Value = "Descrição";
        worksheet.Cells[row, 4].Value = "Exemplo";
        worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
        row++;
        
        worksheet.Cells[row, 1].Value = "Id";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Identificador único do item emprestado";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "EmprestimoId";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "ID do empréstimo (referência à aba Emprestimos)";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "ItemId";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "ID do bem (referência à aba Itens)";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "ItemNome";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Nome do bem (cópia para facilitar)";
        worksheet.Cells[row, 4].Value = "CADEIRA";
        row++;
        
        worksheet.Cells[row, 1].Value = "Quantidade";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Quantidade emprestada deste bem";
        worksheet.Cells[row, 4].Value = "10";
        row++;
        
        worksheet.Cells[row, 1].Value = "QuantidadeRecebida";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Quantidade já devolvida deste bem";
        worksheet.Cells[row, 4].Value = "5";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataCriacao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data de registro";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataAlteracao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data da última modificação";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row += 2;
        
        // Seção 6: Aba Recebimentos
        worksheet.Cells[row, 1].Value = "6. ABA: RECEBIMENTOS (Devoluções)";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.LightGreen);
        row++;
        
        worksheet.Cells[row, 1].Value = "Campo";
        worksheet.Cells[row, 2].Value = "Tipo";
        worksheet.Cells[row, 3].Value = "Descrição";
        worksheet.Cells[row, 4].Value = "Exemplo";
        worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
        row++;
        
        worksheet.Cells[row, 1].Value = "Id";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Identificador único da devolução";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "Nome";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Descrição da devolução";
        worksheet.Cells[row, 4].Value = "RECEBIMENTO - CADEIRA";
        row++;
        
        worksheet.Cells[row, 1].Value = "NomeRecebedor";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Nome de quem pegou emprestado";
        worksheet.Cells[row, 4].Value = "JOÃO SILVA";
        row++;
        
        worksheet.Cells[row, 1].Value = "NomeQuemRecebeu";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Nome de quem recebeu a devolução";
        worksheet.Cells[row, 4].Value = "MARIA SANTOS";
        row++;
        
        worksheet.Cells[row, 1].Value = "EmprestimoId";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "ID do empréstimo relacionado";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataEmprestimo";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data do empréstimo original";
        worksheet.Cells[row, 4].Value = "2025-01-15 10:30:00";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataRecebimento";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data em que a devolução foi realizada";
        worksheet.Cells[row, 4].Value = "2025-01-20 14:00:00";
        row++;
        
        worksheet.Cells[row, 1].Value = "RecebimentoParcial";
        worksheet.Cells[row, 2].Value = "Sim/Não";
        worksheet.Cells[row, 3].Value = "Indica se foi devolução parcial (TRUE/FALSE)";
        worksheet.Cells[row, 4].Value = "FALSE";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataCriacao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data de registro";
        worksheet.Cells[row, 4].Value = "2025-01-20 14:00:00";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataAlteracao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data da última modificação";
        worksheet.Cells[row, 4].Value = "2025-01-20 14:00:00";
        row += 2;
        
        // Seção 7: Aba RecebimentoItens
        worksheet.Cells[row, 1].Value = "7. ABA: RECEBIMENTOITENS (Detalhamento das Devoluções)";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.LightGreen);
        row++;
        
        worksheet.Cells[row, 1].Value = "Campo";
        worksheet.Cells[row, 2].Value = "Tipo";
        worksheet.Cells[row, 3].Value = "Descrição";
        worksheet.Cells[row, 4].Value = "Exemplo";
        worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
        row++;
        
        worksheet.Cells[row, 1].Value = "Id";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Identificador único do item devolvido";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "RecebimentoEmprestimoId";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "ID da devolução (referência à aba Recebimentos)";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "EmprestimoItemId";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "ID do item emprestado original";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "ItemId";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "ID do bem (referência à aba Itens)";
        worksheet.Cells[row, 4].Value = "1";
        row++;
        
        worksheet.Cells[row, 1].Value = "ItemNome";
        worksheet.Cells[row, 2].Value = "Texto";
        worksheet.Cells[row, 3].Value = "Nome do bem (cópia para facilitar)";
        worksheet.Cells[row, 4].Value = "CADEIRA";
        row++;
        
        worksheet.Cells[row, 1].Value = "QuantidadeRecebida";
        worksheet.Cells[row, 2].Value = "Número";
        worksheet.Cells[row, 3].Value = "Quantidade devolvida nesta operação";
        worksheet.Cells[row, 4].Value = "10";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataCriacao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data de registro";
        worksheet.Cells[row, 4].Value = "2025-01-20 14:00:00";
        row++;
        
        worksheet.Cells[row, 1].Value = "DataAlteracao";
        worksheet.Cells[row, 2].Value = "Data/Hora";
        worksheet.Cells[row, 3].Value = "Data da última modificação";
        worksheet.Cells[row, 4].Value = "2025-01-20 14:00:00";
        row += 2;
        
        // Seção 8: Como fazer empréstimo manual
        worksheet.Cells[row, 1].Value = "8. COMO FAZER UM EMPRÉSTIMO MANUALMENTE";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
        row++;
        
        worksheet.Cells[row, 1].Value = "PASSO 1: Verifique o estoque disponível na aba 'Itens'";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Confirme que há quantidade suficiente em 'QuantidadeEstoque'";
        row += 2;
        
        worksheet.Cells[row, 1].Value = "PASSO 2: Adicione um registro na aba 'Emprestimos'";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Id: Próximo número sequencial (ex: se o último é 5, use 6)";
        row++;
        worksheet.Cells[row, 1].Value = "  - Nome: JOÃO SILVA (quem está recebendo)";
        row++;
        worksheet.Cells[row, 1].Value = "  - Motivo: EVENTO ESPECIAL";
        row++;
        worksheet.Cells[row, 1].Value = "  - CongregacaoId: 1 (deve existir na aba Congregacoes)";
        row++;
        worksheet.Cells[row, 1].Value = "  - CongregacaoNome: CONGREGAÇÃO CENTRAL (copie da aba)";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataEmprestimo: 2025-01-15 10:30:00";
        row++;
        worksheet.Cells[row, 1].Value = "  - Status: 1 (Em Andamento)";
        row++;
        worksheet.Cells[row, 1].Value = "  - QuemLiberou: MARIA SANTOS";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataCriacao: 2025-01-15 10:30:00";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataAlteracao: 2025-01-15 10:30:00";
        row += 2;
        
        worksheet.Cells[row, 1].Value = "PASSO 3: Adicione registros na aba 'EmprestimoItens' (um por bem)";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Id: Próximo número sequencial";
        row++;
        worksheet.Cells[row, 1].Value = "  - EmprestimoId: 6 (o Id do empréstimo criado no passo 2)";
        row++;
        worksheet.Cells[row, 1].Value = "  - ItemId: 1 (deve existir na aba Itens)";
        row++;
        worksheet.Cells[row, 1].Value = "  - ItemNome: CADEIRA (copie da aba Itens)";
        row++;
        worksheet.Cells[row, 1].Value = "  - Quantidade: 10 (quantidade emprestada)";
        row++;
        worksheet.Cells[row, 1].Value = "  - QuantidadeRecebida: 0 (ainda não foi devolvido)";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataCriacao: 2025-01-15 10:30:00";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataAlteracao: 2025-01-15 10:30:00";
        row += 2;
        
        worksheet.Cells[row, 1].Value = "PASSO 4: ATUALIZE o estoque na aba 'Itens'";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Reduza 'QuantidadeEstoque' pela quantidade emprestada";
        row++;
        worksheet.Cells[row, 1].Value = "  - Exemplo: Se tinha 50 cadeiras e emprestou 10, deixe 40";
        row++;
        worksheet.Cells[row, 1].Value = "  - Atualize 'DataAlteracao' para a data/hora atual";
        row += 2;
        
        // Seção 9: Como fazer devolução manual
        worksheet.Cells[row, 1].Value = "9. COMO FAZER UMA DEVOLUÇÃO MANUALMENTE";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
        row++;
        
        worksheet.Cells[row, 1].Value = "PASSO 1: Localize o empréstimo na aba 'Emprestimos'";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Anote o Id do empréstimo (ex: 6)";
        row++;
        worksheet.Cells[row, 1].Value = "  - Verifique que o Status é 1 (Em Andamento)";
        row += 2;
        
        worksheet.Cells[row, 1].Value = "PASSO 2: Adicione um registro na aba 'Recebimentos'";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Id: Próximo número sequencial";
        row++;
        worksheet.Cells[row, 1].Value = "  - Nome: RECEBIMENTO - CADEIRA";
        row++;
        worksheet.Cells[row, 1].Value = "  - NomeRecebedor: JOÃO SILVA (copie do empréstimo)";
        row++;
        worksheet.Cells[row, 1].Value = "  - NomeQuemRecebeu: MARIA SANTOS (quem recebeu de volta)";
        row++;
        worksheet.Cells[row, 1].Value = "  - EmprestimoId: 6 (o Id do empréstimo)";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataEmprestimo: 2025-01-15 10:30:00 (copie do empréstimo)";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataRecebimento: 2025-01-20 14:00:00 (data atual)";
        row++;
        worksheet.Cells[row, 1].Value = "  - RecebimentoParcial: FALSE (se devolver tudo) ou TRUE (parcial)";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataCriacao: 2025-01-20 14:00:00";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataAlteracao: 2025-01-20 14:00:00";
        row += 2;
        
        worksheet.Cells[row, 1].Value = "PASSO 3: Adicione registros na aba 'RecebimentoItens'";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Id: Próximo número sequencial";
        row++;
        worksheet.Cells[row, 1].Value = "  - RecebimentoEmprestimoId: (Id do recebimento criado no passo 2)";
        row++;
        worksheet.Cells[row, 1].Value = "  - EmprestimoItemId: (busque na aba EmprestimoItens)";
        row++;
        worksheet.Cells[row, 1].Value = "  - ItemId: 1";
        row++;
        worksheet.Cells[row, 1].Value = "  - ItemNome: CADEIRA";
        row++;
        worksheet.Cells[row, 1].Value = "  - QuantidadeRecebida: 10 (quanto está devolvendo)";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataCriacao: 2025-01-20 14:00:00";
        row++;
        worksheet.Cells[row, 1].Value = "  - DataAlteracao: 2025-01-20 14:00:00";
        row += 2;
        
        worksheet.Cells[row, 1].Value = "PASSO 4: ATUALIZE a aba 'EmprestimoItens'";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Localize o registro correspondente ao empréstimo";
        row++;
        worksheet.Cells[row, 1].Value = "  - Aumente 'QuantidadeRecebida' pela quantidade devolvida";
        row++;
        worksheet.Cells[row, 1].Value = "  - Exemplo: Se estava 0 e devolveu 10, deixe 10";
        row++;
        worksheet.Cells[row, 1].Value = "  - Atualize 'DataAlteracao'";
        row += 2;
        
        worksheet.Cells[row, 1].Value = "PASSO 5: ATUALIZE o estoque na aba 'Itens'";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Aumente 'QuantidadeEstoque' pela quantidade devolvida";
        row++;
        worksheet.Cells[row, 1].Value = "  - Exemplo: Se tinha 40 cadeiras e recebeu 10, deixe 50";
        row++;
        worksheet.Cells[row, 1].Value = "  - Atualize 'DataAlteracao'";
        row += 2;
        
        worksheet.Cells[row, 1].Value = "PASSO 6: ATUALIZE o status na aba 'Emprestimos' (se necessário)";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        row++;
        worksheet.Cells[row, 1].Value = "  - Se todos os itens foram devolvidos, altere Status para 2 (Devolvido)";
        row++;
        worksheet.Cells[row, 1].Value = "  - Se foi devolução parcial, mantenha Status 1 (Em Andamento)";
        row++;
        worksheet.Cells[row, 1].Value = "  - Atualize 'DataAlteracao'";
        row += 2;
        
        // Seção 10: Avisos importantes
        worksheet.Cells[row, 1].Value = "10. AVISOS IMPORTANTES";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.Red);
        worksheet.Cells[row, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
        row++;
        
        worksheet.Cells[row, 1].Value = "?? NÃO DELETE as linhas de cabeçalho (primeira linha de cada aba)";
        row++;
        worksheet.Cells[row, 1].Value = "?? NÃO altere os nomes das abas (Itens, Congregacoes, etc.)";
        row++;
        worksheet.Cells[row, 1].Value = "?? Use SEMPRE o formato de data: yyyy-MM-dd HH:mm:ss";
        row++;
        worksheet.Cells[row, 1].Value = "?? IDs devem ser números ÚNICOS e sequenciais";
        row++;
        worksheet.Cells[row, 1].Value = "?? Referências entre abas (IDs) devem ser VÁLIDAS";
        row++;
        worksheet.Cells[row, 1].Value = "?? O estoque NUNCA pode ficar negativo";
        row++;
        worksheet.Cells[row, 1].Value = "?? QuantidadeRecebida NUNCA pode ser maior que Quantidade";
        row++;
        worksheet.Cells[row, 1].Value = "?? SEMPRE atualize DataAlteracao ao modificar um registro";
        row++;
        worksheet.Cells[row, 1].Value = "?? É RECOMENDADO usar o sistema ao invés de editar manualmente";
        row += 2;
        
        // Seção 11: Códigos de Status
        worksheet.Cells[row, 1].Value = "11. CÓDIGOS DE STATUS (Aba Emprestimos)";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 12;
        worksheet.Cells[row, 1].Style.Fill.SetBackground(System.Drawing.Color.LightGray);
        row++;
        
        worksheet.Cells[row, 1].Value = "Código";
        worksheet.Cells[row, 2].Value = "Significado";
        worksheet.Cells[row, 1, row, 2].Style.Font.Bold = true;
        row++;
        
        worksheet.Cells[row, 1].Value = "1";
        worksheet.Cells[row, 2].Value = "Em Andamento (empréstimo ativo)";
        row++;
        
        worksheet.Cells[row, 1].Value = "2";
        worksheet.Cells[row, 2].Value = "Devolvido (todos os itens foram devolvidos)";
        row++;
        
        worksheet.Cells[row, 1].Value = "3";
        worksheet.Cells[row, 2].Value = "Cancelado (empréstimo foi cancelado)";
        row += 2;
        
        // Ajustar largura das colunas
        worksheet.Column(1).Width = 30;
        worksheet.Column(2).Width = 15;
        worksheet.Column(3).Width = 60;
        worksheet.Column(4).Width = 30;
        
        // Proteger a aba contra alterações acidentais (opcional)
        // worksheet.Protection.IsProtected = true;
        // worksheet.Protection.AllowSelectLockedCells = true;
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
        worksheet.Cells[1, 8].Value = "QuemLiberou";
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
        
        // Verificar e criar aba de instruções se não existir
        if (package.Workbook.Worksheets[INSTRUCOES_SHEET] == null)
        {
            CreateInstrucoesSheet(package);
        }
        
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
            worksheet.Cells[row, 8].Value = emprestimo.QuemLiberou;
            worksheet.Cells[row, 9].Value = emprestimo.DataCriacao.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 10].Value = emprestimo.DataAlteracao.ToString("yyyy-MM-dd HH:mm:ss");
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
                        QuemLiberou = emprestimosSheet.Cells[row, 8].Value?.ToString() ?? string.Empty,
                        DataCriacao = DateTime.Parse(emprestimosSheet.Cells[row, 9].Value?.ToString() ?? DateTime.Now.ToString()),
                        DataAlteracao = DateTime.Parse(emprestimosSheet.Cells[row, 10].Value?.ToString() ?? DateTime.Now.ToString())
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
