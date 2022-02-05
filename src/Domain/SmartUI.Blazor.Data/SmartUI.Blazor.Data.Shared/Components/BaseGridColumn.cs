namespace SmartUI.Blazor.Data
{
    using AntiRap.Core.Abstractions;
    using Microsoft.AspNetCore.Components;
    using SmartUI.Blazor.Data.Shared.Abstractions;
    using SmartUI.Blazor.Shared;

    public abstract class BaseGridColumn : BaseComponent, IBaseGridColumn
    {
        public Guid columnId;
        public event EventHandler? ColumnStateChanged;
        protected string propertyName;
        
        public override Task SetParametersAsync(ParameterView parameters)
        {
            HeaderTemplate = builder =>
            {
                builder.AddContent(1, string.IsNullOrEmpty(Title) ? PropertyName : Title);
            };
            RowTemplate = (model) => (builder) =>
            {
                builder.AddContent(1, _propertyHandler.GetPropertyValue(model, PropertyName));
            };
            return base.SetParametersAsync(parameters);
        }
        
        protected void RaiseStateChanged() => ColumnStateChanged?.Invoke(this, new EventArgs());

        /// <summary>
        /// Gets style for column header
        /// </summary>
        public abstract string GetColumnHeaderStyle();
        /// <summary>
        /// Gets style for column cell
        /// </summary>
        public abstract string GetColumnCellStyle();


        [Inject] public IPropertyHandler _propertyHandler { get; set; }

        

        /// <summary>
        /// Spacify Property that exist in 
        /// </summary>
        /// <remarks>
        /// Note: if title not set then the column title will be PropertyName property that set to this column
        /// </remarks>
        [Parameter]
        public string PropertyName
        {
            get => propertyName;
            set
            {
                if (propertyName == value)
                    return;

                propertyName = value;
                RaiseStateChanged();
            }
        }

        /// <summary>
        /// Specifies the content to be rendered inside this <see cref="HeaderTemplate"/>.
        /// </summary>
        [Parameter] public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// Specifies the content to be rendered inside this <see cref="RowTemplate"/>.
        /// </summary>
        [Parameter] public RenderFragment<object> RowTemplate { get; set; }

        /// <summary>
        /// Set title to column
        /// </summary>
        /// <remarks>
        /// Note: if title not set then the column title will be <see cref="PropertyName"/> that set to this column
        /// </remarks>
        [Parameter] public string Title { get; set; }
    }
}
