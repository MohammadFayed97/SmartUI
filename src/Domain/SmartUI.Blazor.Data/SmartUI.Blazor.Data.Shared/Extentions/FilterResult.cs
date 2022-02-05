namespace SmartUI.Blazor.Data.Extentions;

using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using AntiRap.Core.Pagination;
using SmartUI.Blazor.Data.Shared;
using AntiRap.Core.DynamicFilter;
using AntiRap.Core;

public static class FilterResult
{
    public static PagedList<T> ToPagedList<T>(this IQueryable<T> dataSource, int pageSize, int pageNumber)
        where T : class => PagedList<T>.ToPagedList(dataSource, pageSize, pageNumber);

    public static IQueryable<T> Sort<T>(this IQueryable<T> source, string orderByQueryString)
    {
        if (string.IsNullOrEmpty(orderByQueryString))
            return source;

        string[] orderParams = orderByQueryString.Trim().Split(',');
        PropertyInfo[] propertiesInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        StringBuilder queryBuilder = new StringBuilder();
        foreach (string param in orderParams)
        {
            if (string.IsNullOrEmpty(param))
                continue;

            string queryProperty = param.Split(" ")[0];
            PropertyInfo objectProperty = propertiesInfo?.FirstOrDefault(p => p.Name.Equals(queryProperty, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty is null)
                continue;

            string direction = param.ToLower().EndsWith(" desc") ? "descending" : "ascending";
            queryBuilder.Append($"{objectProperty.Name} {direction}, ");
        }

        string orderQuery = queryBuilder.ToString().TrimEnd(',', ' ');

        return source.OrderBy(orderQuery);
    }

    public static IQueryable<T> Filter<T>(this IQueryable<T> dataSource, IEnumerable<QueryFilterRule> queryFilters)
    {
        if (queryFilters == null || queryFilters.Count() == 0)
            return dataSource;

        Expression<Func<T, bool>> filterExpression = null;
        foreach (QueryFilterRule queryFilter in queryFilters.Where(e => e != null).OrderBy(f => f.DisplayOrder))
        {
            var filterType = ObjectFilter.FromFilterName(queryFilter.FilterOperation);
            var filterRuleExpression = filterType.GenerateExpression<T>(queryFilter.Field, queryFilter.Value1, queryFilter.Value2);

            if (filterExpression is null) filterExpression = filterRuleExpression;
            else filterExpression = PredicateBuilder.And<T>(filterExpression, filterRuleExpression);
        }

        return dataSource.Where(filterExpression);
    }

    public static IQueryable<TModel> Search<TModel>(this IEnumerable<TModel> source, string propertyName, string serchValue)
        where TModel : class
    {
        if (string.IsNullOrWhiteSpace(serchValue))
            return source.AsQueryable();

        return source.AsQueryable().Where(ObjectFilter.Contains.GenerateExpression<TModel>(propertyName, serchValue, null));
    }
}