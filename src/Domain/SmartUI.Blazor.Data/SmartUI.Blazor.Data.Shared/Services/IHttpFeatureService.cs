namespace SmartUI.Blazor.Data.Shared;

public interface IHttpFeatureService<TModel>
        where TModel : class
{
    Task<PagedResponse<TModel>> GetAsync(string url, FilterQuery filterQuery = null);
    Task<IEnumerable<TModel>> GetDataAsync(string url);
    Task<IEnumerable<TModel>> GetByPredicateAsync(string url, List<QueryFilterRule> queryFilterRules);
}