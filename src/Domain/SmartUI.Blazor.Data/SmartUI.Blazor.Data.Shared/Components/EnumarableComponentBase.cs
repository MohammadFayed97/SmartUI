namespace SmartUI.Blazor.Data.Shared;

using Microsoft.AspNetCore.Components;
using Serilog;
using SmartUI.Blazor.Data.Shared.Abstractions;
using SmartUI.Blazor.Shared;

public class EnumarableComponentBase<TModel> : BaseComponent, IEnumarableComponentBase
    where TModel : class
{
    protected IEnumerable<TModel> _items;
    protected IDataManger _dataManger;
    protected bool isDataLoading;

    protected override void OnParametersSet()
    {
        if (DataSource is null) DataSource = new List<TModel>();

        base.OnParametersSet();
    }
    public void AddDataManger(IDataManger dataManger) => _dataManger = dataManger;

    /// <summary>
    /// To retrive Data from end-point if <see cref="_dataManger"/> is exist and put it in <see cref="DataSource"/>
    /// </summary>
    protected virtual async Task GetDataSource()
    {
        try
        {
            if (_dataManger is not null)
            {
                if (string.IsNullOrEmpty(_dataManger.Url))
                    return;

                await Loading(async () => DataSource = await _httpFeatureService.GetDataAsync(_dataManger.Url));
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception.Message);
        }
    }
    protected async Task Loading(Func<Task> func)
    {
        isDataLoading = true;
        StateHasChanged();

        await func();

        isDataLoading = false;
        StateHasChanged();
    }
    /// <summary>
    /// The HttpService that used to fetch data from spacified endpoint in <see cref="IDataManger.Url"/>
    /// </summary>
    [Inject] public IHttpFeatureService<TModel> _httpFeatureService { get; set; }

    /// <summary>
    /// spacify the DataSource to component
    /// </summary>
    [Parameter] public IEnumerable<TModel> DataSource { get; set; }

    /// <summary>
    /// This value show when no record exist 
    /// </summary>
    [Parameter] public string NoRecordsText { get; set; } = "No Data Available";

    /// <summary>
    /// Specifies the content to be rendered inside <see cref="ChildContent"/>.
    /// </summary>
    [Parameter] public RenderFragment ChildContent { get; set; }
}