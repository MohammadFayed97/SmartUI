namespace SmartUI.Blazor.Data.Shared.Abstractions;

public interface IBaseGridColumns<TColumn>
    where TColumn : IBaseGridColumn
{
    void AddColumn(TColumn column);
    List<TColumn> GetAllColumns();
}