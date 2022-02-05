namespace SmartUI.Blazor.Data;

using AntiRap.Core.DynamicFilter;
using AntiRap.Core.Pagination;
using Microsoft.AspNetCore.Components;
using SmartUI.Blazor.Data.Extentions;
using SmartUI.Blazor.Data.Shared;
using SmartUI.Blazor.Data.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class BaseOperationsGrid<TSource, TColumns, TColumn> : BaseGrid<TSource, TColumns, TColumn>, IBaseOperationGrid<TColumns, TColumn>
    where TSource : class
    where TColumns : BaseGridColumns<TColumn>
    where TColumn : BaseGridColumn
{
    protected FilterQuery _filterQuery = new FilterQuery();
    protected MetaData _responseMetaData = new MetaData();
    protected List<FilterRule<TSource>> _filterRules = new List<FilterRule<TSource>>();

    protected void PerformClientSideDataManipulations()
    {
        _filterQuery.FilterRules = GetQueryFilterRulesToAppliedFilterRules();

        var data = DataSource?.AsQueryable();

        if (data is not null && data.Count() > 0)
        {
            if (AllowFilter) data = data.Filter(_filterQuery.FilterRules);
            if (AllowSorting) data = data.Sort(_filterQuery.OrderBy);
            if (AllowPagination)
            {
                PagedList<TSource> pagedList = data.ToPagedList(_filterQuery.PageSize, _filterQuery.PageNumber);
                data = pagedList.AsQueryable();
                _responseMetaData = pagedList.MetaData;
            }
        }
        _items = data?.ToList();
        StateHasChanged();
    }
    protected override async Task GetDataSource()
    {
        await Loading(async () =>
        {
            if (_dataManger is not null)
                await PerformServerSideDataManipulations(() => _httpFeatureService.GetAsync(_dataManger.Url, _filterQuery));
            else
                PerformClientSideDataManipulations();
        });
    }
    protected async Task PerformServerSideDataManipulations(Func<Task<PagedResponse<TSource>>> getData)
    {
        PagedResponse<TSource>? pagedDatasource = await getData();

        DataSource = pagedDatasource.Items;
        _items = pagedDatasource.Items;
        _responseMetaData = pagedDatasource.MetaData;

        _items = DataSource.AsQueryable().Filter(GetQueryFilterRulesToAppliedFilterRules());
    }
    protected void ApplyFilterOnDataSource() => _items = DataSource.AsQueryable().Filter(GetQueryFilterRulesToAppliedFilterRules());
    private IEnumerable<QueryFilterRule> GetQueryFilterRulesToAppliedFilterRules()
    {
        List<FilterRule<TSource>> appliedFilterRules = _filterRules?.Where(r => r.IsApplied)?.ToList();
        return appliedFilterRules?.Select(r => new QueryFilterRule
        {
            Field = r.PropertyName,
            Value1 = r.FilterValue1,
            Value2 = r.FilterValue2,
            FilterOperation = r.FilterType.FilterName,
            DisplayOrder = r.Order
        }).ToList();
    }

    [Parameter] public bool AllowPagination { get; set; }
    [Parameter] public bool AllowSorting { get; set; }
    [Parameter] public bool AllowFilter { get; set; }
    [Parameter] public Func<DataTableRequestArgs<TSource>, Task<PagedResponse<TSource>>> FetchData { get; set; }
}