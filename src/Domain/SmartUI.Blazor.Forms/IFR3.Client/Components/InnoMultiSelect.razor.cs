namespace SmartUI.Blazor.Forms
{
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading.Tasks;

    public partial class InnoMultiSelect<TItem, TValue>
    {
        [Parameter]
        public List<TItem> DataSource { get; set; }

        [Parameter]
        public string DataSourceUrl { get; set; }

        [Parameter]
        public Expression<Func<TItem, object>> TextField { get; set; }
        [Parameter]
        public Expression<Func<TItem, TValue>> ValueField { get; set; }

        [Parameter]
        public List<TValue> Value { get; set; } = new List<TValue>();

        [Parameter]
        public EventCallback<List<TValue>> ValueChanged { get; set; }

        [Parameter]
        public RenderFragment? NoRecordTemplate { get; set; }

        [Parameter]
        public EventCallback<TItem> OnSelect { get; set; }

        [Inject]
        private HttpClient httpClient { get; set; }


        private List<object> selectedTexts = new List<object>();


        private string searchTerm;

        private bool isOpen;

        private bool isSelectAll;


        private List<TItem> filteredDataSource = new List<TItem>();


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (DataSourceUrl != default)
            {
                DataSource = await httpClient.GetFromJsonAsync<List<TItem>>(DataSourceUrl);
                FilterDataSource();
            }

            Value ??= new List<TValue>();
            DataSource ??= new List<TItem>();
            filteredDataSource = DataSource.ToList();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            parameters = parameters.EnsureOnlyOneParameterIsSet(new[] { nameof(DataSourceUrl), nameof(DataSource) });

            await base.SetParametersAsync(parameters);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Value ??= new List<TValue>();
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

            // Test
            Console.WriteLine(string.Join(", ", filteredDataSource.Select(i => GetPropertyValue(TextField, i))));
        }

        private bool IsSearchMatch(TItem item)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm == default)
                return true;

            object text = GetPropertyValue(TextField, item);
            return text.ToString().Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase);
        }


        private async Task ToggleSelectAll()
        {
            if (isSelectAll)
                DeselectAll();
            else
                SelectAll();

            await ValueChanged.InvokeAsync(Value);
        }

        private void SelectAll()
        {

            DeselectAll();
            filteredDataSource.ForEach(AddItem);
        }

        private void DeselectAll()
        {
            selectedTexts.Clear();
            Value.Clear();
        }



        private async Task OnItemClick(TItem item)
        {
            object text = GetPropertyValue(TextField, item);
            TValue value = ConvertValue(GetPropertyValue(ValueField, item));

            if (Value.Contains(value))
            {
                RemoveItem(text, value);
            }
            else
            {
                AddItem(text, value);
            }

            await ValueChanged.InvokeAsync(Value);

            Console.WriteLine(string.Join(" , ", Value));
        }



        private void AddItem(TItem item)
        {
            object text = GetPropertyValue(TextField, item);
            TValue value = ConvertValue(GetPropertyValue(ValueField, item));

            AddItem(text, value);
        }

        private void AddItem(object text, TValue value)
        {
            selectedTexts.Add(text);
            Value.Add(value);
        }

        private void RemoveItem(TItem item)
        {
            object text = GetPropertyValue(TextField, item);
            TValue value = ConvertValue(GetPropertyValue(ValueField, item));

            RemoveItem(text, value);
        }

        private void RemoveItem(object text, TValue value)
        {
            selectedTexts.Remove(text);
            Value.Remove(value);
        }


        private TValue ConvertValue(object value)
        {
            Type valueType = typeof(TValue);

            if (valueType.IsEnum)
            {
                return (TValue)Enum.Parse(valueType, value?.ToString());
            }
            else
            {
                return (TValue)Convert.ChangeType(value, valueType);
            }
        }

        private static object GetPropertyValue<T>(string propertyName, object? obj) =>
            typeof(T).GetProperty(propertyName).GetValue(obj);

        private static object GetPropertyValue<T, TOut>(Expression<Func<T, TOut>> property, object? obj)
        {
            string properyName = ((MemberExpression)property.Body).Member.Name;
            return GetPropertyValue<T>(properyName, obj);
        }
    }
}
