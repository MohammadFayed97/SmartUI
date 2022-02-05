namespace SmartUI.Blazor.Data;

using AntiRap.Core.DynamicFilter;
using Microsoft.AspNetCore.Components;
using SmartUI.Blazor.Shared;
using System;

public partial class GridFilterInput<TModel> : BaseComponent
    where TModel : class
{
    [Parameter]
    public FilterRule<TModel> FilterRule { get; set; }
    [Parameter]
    public EventCallback<Guid> OnFilterApply { get; set; }
    [Parameter]
    public EventCallback<Guid> OnFilterCleard { get; set; }
    [Parameter]
    public bool ShowFilterInput { get; set; }
    private int filterType;

    private object? value1;
    private object? value2;
    protected override void OnParametersSet()
    {
        filterType = FilterRule?.FilterType?.Id ?? 0;
        base.OnParametersSet();
    }
}