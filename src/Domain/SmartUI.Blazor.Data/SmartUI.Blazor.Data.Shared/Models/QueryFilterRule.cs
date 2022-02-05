namespace SmartUI.Blazor.Data.Shared;

using AntiRap.Core.DynamicFilter;

public class QueryFilterRule
{
    public string? Field { get; set; }
    public int DisplayOrder { get; set; }
    public FilterOperation FilterOperation { get; set; }
    public dynamic? Value1 { get; set; }
    public dynamic? Value2 { get; set; }
}