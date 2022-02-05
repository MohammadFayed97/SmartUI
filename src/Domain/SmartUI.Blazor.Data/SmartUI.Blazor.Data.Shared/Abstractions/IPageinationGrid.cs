namespace SmartUI.Blazor.Data.Shared.Abstractions;

public interface IPageinationGrid<TColumns, TColumn> : IBaseGrid<TColumns, TColumn>
    where TColumns : IBaseGridColumns<TColumn>
    where TColumn : IBaseGridColumn
{
    bool AllowPagination { get; set; }
}