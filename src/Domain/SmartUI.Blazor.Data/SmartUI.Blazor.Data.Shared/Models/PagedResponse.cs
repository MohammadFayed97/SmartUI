namespace SmartUI.Blazor.Data.Shared;

using AntiRap.Core.Pagination;

public class PagedResponse<TModel> where TModel : class
{
    public List<TModel> Items { get; set; }
    public MetaData? MetaData { get; set; }
}
