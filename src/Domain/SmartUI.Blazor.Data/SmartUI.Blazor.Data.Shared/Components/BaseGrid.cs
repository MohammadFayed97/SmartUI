namespace SmartUI.Blazor.Data.Shared;

using SmartUI.Blazor.Data.Shared.Abstractions;

public class BaseGrid<TSource, TColumns, TColumn> : EnumarableComponentBase<TSource>, IBaseGrid<TColumns, TColumn>
    where TSource : class
    where TColumns : BaseGridColumns<TColumn>
    where TColumn : BaseGridColumn
{
    /// <summary>
    /// Child GridColumns Component that contain all columns spacified.
    /// </summary>
    protected TColumns _gridColumns;

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
            return;

        _gridColumns.GridColumnsStateChanged += StateHasChanged;
    }

    public void AddGridColumns(TColumns gridColumns) => _gridColumns = gridColumns;
    protected override void Dispose(bool disposing)
    {
        _gridColumns.GridColumnsStateChanged -= StateHasChanged;
        _gridColumns = null;
    }
}