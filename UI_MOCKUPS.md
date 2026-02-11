# UI Screenshots and Flow

Since this is a Windows Forms application running on Linux, I cannot provide actual screenshots. However, here's what each form looks like:

## Main Form
```
┌──────────────────────────────────────────────────────────┐
│ Controle de Emprestimos                           [_][□][X]│
├──────────────────────────────────────────────────────────┤
│ [Listagem de Bens] [Emprestimo] [Recebimento...] [Congre...]│
├──────────────────────────────────────────────────────────┤
│                                                            │
│                                                            │
│                    (Empty Main Area)                       │
│                                                            │
│              Click a menu option to begin                  │
│                                                            │
│                                                            │
└──────────────────────────────────────────────────────────┘
```

## List Form (Example: ItemListForm - "Listagem de Bens")
```
┌──────────────────────────────────────────────────────────┐
│ Listagem de Bens                              [_][□][X]│
├──────────────────────────────────────────────────────────┤
│ ┌────────────────────────────────────────────────────┐ │
│ │ Id │ Name           │ QuantityInStock              │ │
│ ├────┼───────────────┼──────────────────────────────┤ │
│ │ 1  │ Cadeira        │ 50                           │ │
│ │ 2  │ Mesa           │ 20                           │ │
│ │    │                │                              │ │
│ │    │                │                              │ │
│ │    │                │                              │ │
│ └────────────────────────────────────────────────────┘ │
├──────────────────────────────────────────────────────────┤
│ [Criar]  [Editar]  [Excluir]                             │
└──────────────────────────────────────────────────────────┘
```

## Detail Form (Example: ItemDetailForm - "Detalhes do Item")
```
┌────────────────────────────────────────┐
│ Detalhes do Item            [_][X]     │
├────────────────────────────────────────┤
│                                        │
│  Nome:                                 │
│  ┌──────────────────────────────────┐ │
│  │ Cadeira                          │ │
│  └──────────────────────────────────┘ │
│                                        │
│  Quantidade em Estoque:                │
│  ┌──────────┐                         │
│  │ 50     ▲│                          │
│  │        ▼│                          │
│  └──────────┘                         │
│                                        │
│  [Salvar]  [Cancelar]                 │
│                                        │
└────────────────────────────────────────┘
```

## User Interaction Flow

### Creating a New Item
1. User clicks "Listagem de Bens" in main menu
2. ItemListForm opens showing all items
3. User clicks "Criar" button
4. ItemDetailForm opens (empty fields)
5. User enters name and quantity
6. User clicks "Salvar"
7. Detail form closes
8. List form refreshes showing new item

### Editing an Existing Item
1. User clicks "Listagem de Bens" in main menu
2. ItemListForm opens showing all items
3. User selects a row in the DataGridView
4. User clicks "Editar" button
5. ItemDetailForm opens (pre-filled with item data)
6. User modifies name or quantity
7. User clicks "Salvar"
8. Detail form closes
9. List form refreshes showing updated item

### Deleting an Item
1. User clicks "Listagem de Bens" in main menu
2. ItemListForm opens showing all items
3. User selects a row in the DataGridView
4. User clicks "Excluir" button
5. Confirmation dialog appears: "Tem certeza que deseja excluir 'Cadeira'?"
6. User clicks "Yes"
7. Item is removed from repository
8. List form refreshes (item no longer visible)

## Form Navigation Diagram

```
                        ┌─────────────┐
                        │   Program   │
                        │  (Startup)  │
                        └──────┬──────┘
                               │
                        ┌──────▼──────┐
                        │  MainForm   │
                        │   (Menu)    │
                        └──────┬──────┘
                 ┌─────────────┼─────────────┐
                 │             │             │
        ┌────────▼────┐   ┌───▼────┐   ┌───▼────┐   ┌──────────────┐
        │ItemListForm │   │Emprés- │   │Receb.  │   │Congregacao   │
        │             │   │timo    │   │List    │   │ListForm      │
        └──────┬──────┘   │List    │   │Form    │   └───────┬──────┘
               │          └───┬────┘   └───┬────┘            │
        ┌──────▼──────┐  ┌───▼────┐   ┌───▼────┐   ┌────────▼──────┐
        │ItemDetail   │  │Emprés- │   │Receb.  │   │Congregacao    │
        │Form         │  │timo    │   │Detail  │   │DetailForm     │
        │(Create/Edit)│  │Detail  │   │Form    │   │(Create/Edit)  │
        └─────────────┘  └────────┘   └────────┘   └───────────────┘
```

All forms follow the same pattern:
- List forms display data in DataGridView
- List forms have Create, Edit, Delete buttons
- Detail forms are used for both create and edit operations
- Forms use modal dialogs (ShowDialog) for user interaction
