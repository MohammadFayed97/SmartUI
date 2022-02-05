namespace SmartUI.Blazor.Data.Shared.Abstractions;

public interface IFilterableGrid<TColumns, TColumn> : IBaseGrid<TColumns, TColumn>
    where TColumns : IBaseGridColumns<TColumn>
    where TColumn : IBaseGridColumn
{
    bool AllowFilter { get; set; }
}
