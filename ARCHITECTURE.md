# Application Flow Diagram

## Main Application Flow

```
Program.cs (Entry Point)
    ↓
MainForm (Menu Principal)
    ├── Listagem de Bens → ItemListForm
    │       ├── Criar → ItemDetailForm (New)
    │       ├── Editar → ItemDetailForm (Edit)
    │       └── Excluir → Remove from DataRepository
    │
    ├── Emprestimo → EmprestimoListForm
    │       ├── Criar → EmprestimoDetailForm (New)
    │       ├── Editar → EmprestimoDetailForm (Edit)
    │       └── Excluir → Remove from DataRepository
    │
    ├── Recebimento de Emprestimo → RecebimentoListForm
    │       ├── Criar → RecebimentoDetailForm (New)
    │       ├── Editar → RecebimentoDetailForm (Edit)
    │       └── Excluir → Remove from DataRepository
    │
    └── Congregacoes → CongregacaoListForm
            ├── Criar → CongregacaoDetailForm (New)
            ├── Editar → CongregacaoDetailForm (Edit)
            └── Excluir → Remove from DataRepository
```

## Data Model

All entities share the same structure:

```
Item / Emprestimo / RecebimentoEmprestimo / Congregacao
├── Id (int) - Auto-generated unique identifier
├── Name (string) - Name of the item/entity
└── QuantityInStock (int) - Quantity available in stock
```

## Data Storage

```
DataRepository (Singleton)
├── Items (List<Item>)
├── Emprestimos (List<Emprestimo>)
├── RecebimentoEmprestimos (List<RecebimentoEmprestimo>)
└── Congregacoes (List<Congregacao>)
```

## Form Types

### 1. MainForm
- Main menu with 4 options
- Opens modal dialog forms

### 2. List Forms (*ListForm)
- DataGridView displaying all items
- Three buttons: Criar, Editar, Excluir
- Shows data from DataRepository

### 3. Detail Forms (*DetailForm)
- Text input for Name
- Numeric input for QuantityInStock
- Two buttons: Salvar, Cancelar
- Used for both Create and Edit operations

## User Interaction Flow

1. User opens application → MainForm appears
2. User clicks menu option → Corresponding ListForm opens
3. User sees list of items in DataGridView
4. User actions:
   - **Create**: Click "Criar" → DetailForm opens in "new" mode → Fill data → Click "Salvar" → Item added to repository
   - **Edit**: Select row → Click "Editar" → DetailForm opens with data → Modify data → Click "Salvar" → Item updated
   - **Delete**: Select row → Click "Excluir" → Confirmation dialog → Item removed from repository
5. ListForm refreshes to show updated data
6. User closes ListForm → Returns to MainForm
