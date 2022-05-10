namespace SmartUI.Blazor.Forms.InnoDropDownList
{
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading.Tasks;

    public partial class InnoDropDownList<TItem,TValue>
    {
        [Parameter]
        public List<TItem>? DataSource { get; set; } = new();

        [Parameter]
        public string? DataSourceUrl { get; set; }

        //Property Reference
        [Parameter]
        public Expression<Func<TItem, object>> TextField { get; set; }
        [Parameter]
        public Expression<Func<TItem, TValue>> ValueField { get; set; }

        [Parameter]
        public TValue Value { get; set; }

        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        [Parameter]
        public EventCallback<TItem> OnSelect { get; set; }

        [Parameter]
        public RenderFragment? NoRecordTemplate { get; set; }

        [Inject]
        private HttpClient httpClient { get; set; }

        private string selectedText;

        private string searchTerm;

        private bool visible;

        private List<TItem> filteredDataSource = new List<TItem>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (DataSourceUrl != default)
            {
                DataSource = await httpClient.GetFromJsonAsync<List<TItem>>(DataSourceUrl);
                DataSource ??= new List<TItem>();
            }
            FilterDataSource();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            parameters = parameters.EnsureOnlyOneParameterIsSet(new[] { nameof(DataSourceUrl), nameof(DataSource) });

            await base.SetParametersAsync(parameters);
        }

        private async Task OnItemSelected(TItem item)
        {
            Value = (TValue)GetPropertyValue<TItem>(GetPropertyName(ValueField.Body), item);
            await ValueChanged.InvokeAsync(Value);
            await OnSelect.InvokeAsync(item);
            selectedText = (string)GetPropertyValue<TItem>(GetPropertyName(TextField.Body), item);
            visible = false;
        }

        private static object GetPropertyValue<T>(string propertyName, object? obj) =>
            typeof(T).GetProperty(propertyName).GetValue(obj);


        private static string GetPropertyName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member.Name;
                case ExpressionType.Convert:
                    return GetPropertyName(((UnaryExpression)expression).Operand);
                default:
                    throw new NotSupportedException(expression.NodeType.ToString());
            }
        }
        private void OnSearchTermChange(ChangeEventArgs args)
        {
            searchTerm = args.Value?.ToString();

            FilterDataSource();

            StateHasChanged();
        }

        private void FilterDataSource()
        {
            filteredDataSource = DataSource.Where(IsSearchMatch).ToList();
        }
        private bool IsSearchMatch(TItem item)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm == default)
                return true;

            object text = GetPropertyValue<TItem>(GetPropertyName(TextField.Body), item);
            return text.ToString().Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase);
        }


    }
}
