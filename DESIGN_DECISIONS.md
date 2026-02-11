# Design Decisions

## Data Model

### Property Naming Convention

All entity models (Item, Emprestimo, RecebimentoEmprestimo, Congregacao) share the same properties:
- `Id` (int)
- `Name` (string)
- `QuantityInStock` (int)

**Rationale**: The requirements explicitly stated that all items should have "Name" and "quanttyinstock" properties. This design follows the specification exactly as provided.

**Note**: While semantically it might be more accurate to have different property names for different entities (e.g., `QuantityLoaned` for Emprestimo, `QuantityReturned` for RecebimentoEmprestimo, `MemberCount` for Congregacao), the current implementation prioritizes consistency and adherence to the stated requirements.

### Future Enhancements

If the application evolves, consider:
1. Creating a base `Entity` class with `Id` and `Name`
2. Adding entity-specific properties (e.g., `LoanDate` for Emprestimo, `ReturnDate` for RecebimentoEmprestimo)
3. Renaming `QuantityInStock` to more semantically appropriate names per entity type
4. Adding relationships between entities (e.g., tracking which items are in which loans)

## Data Storage

### In-Memory Repository

The application uses an in-memory singleton repository for data storage.

**Advantages**:
- Simple implementation
- No external dependencies
- Fast access
- Perfect for demo/prototype

**Limitations**:
- Data is lost when application closes
- No concurrent access support
- Limited scalability

**Future Enhancements**:
- Add database persistence (SQL Server, SQLite)
- Implement repository pattern with multiple providers
- Add data export/import functionality (JSON, CSV)
- Implement data validation and business rules

## User Interface

### Modal Dialogs

All list and detail forms open as modal dialogs using `ShowDialog()`.

**Advantages**:
- Forces user to complete or cancel current operation
- Prevents confusion with multiple windows
- Simple navigation flow

**Considerations**:
- Might limit workflow for power users
- Consider MDI (Multiple Document Interface) for advanced scenarios
