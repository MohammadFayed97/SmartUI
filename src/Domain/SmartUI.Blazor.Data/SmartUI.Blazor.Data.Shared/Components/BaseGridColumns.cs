namespace SmartUI.Blazor.Data.Shared;

using Microsoft.AspNetCore.Components;
using SmartUI.Blazor.Data.Shared.Abstractions;
using SmartUI.Blazor.Shared;

public abstract class BaseGridColumns<TColumn> : BaseComponent, IBaseGridColumns<TColumn>
    where TColumn : BaseGridColumn
{
    protected List<TColumn> columns = new List<TColumn>();
    public event EventHandler? GridColumnsStateChanged;

    /// <summary>
    /// Used to know what columns is used in <see cref="ChildContent"/>
    /// </summary>
    /// <param name="column"></param>
    public virtual void AddColumn(TColumn column)
    {
        columns ??= new List<TColumn>();
        columns.Add(column);
    }
    protected override void StateHasChanged(object sender, EventArgs args) => RaiseStateChanged();
    protected void RaiseStateChanged() => GridColumnsStateChanged?.Invoke(this, new EventArgs());

    /// <summary>
    /// Retrive all columns used inside <see cref="ChildContent"/>
    /// </summary>
    public List<TColumn> GetAllColumns() => columns ?? new List<TColumn>();

    /// <summary>
    /// Specifies the content to be rendered inside <see cref="ChildContent"/>.
    /// </summary>
    [Parameter] public RenderFragment ChildContent { get; set; }
}