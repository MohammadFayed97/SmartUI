namespace SmartUI.Blazor.Data.Shared;

public class DataTableRequestArgs<TModel>
{
    public FilterQuery filterQuery { get; private set; } = new FilterQuery();

    public DataTableRequestArgs(int pageNumber, int pageSize, List<QueryFilterRule> filterRules)
    {
        filterQuery.PageNumber = pageNumber;
        filterQuery.PageSize = pageSize;
        filterQuery.FilterRules = filterRules;
    }

    public DataTableRequestArgs(FilterQuery parameters) => filterQuery = parameters;
}
