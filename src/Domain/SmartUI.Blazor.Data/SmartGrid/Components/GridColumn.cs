using Microsoft.AspNetCore.Components;

namespace SmartUI.Blazor.Data
{
    public class GridColumn : BaseOperationGridColumn
    {
        private bool isHidden;

        protected override void OnInitialized()
        {
            if (GridColumns is null)
                throw new ArgumentNullException(nameof(GridColumns), $"{nameof(GridColumn)} must be include in {nameof(GridColumns)} Component");

            columnId = Guid.NewGuid();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                GridColumns.AddColumn(this);
        }

        public override string GetColumnHeaderStyle() => @"padding: 0; text-align: left; min-width: 10vw; top: 0; background-color: #FFF; vertical-align: top;";
        public override string GetColumnCellStyle() => @"text-align: left; white-space: nowrap; text-overflow: ellipsis; overflow: hidden;";

        /// <summary>
        /// Gets or sets the cascaded parent table component.
        /// </summary>
        [CascadingParameter] public GridColumns GridColumns { get; set; }

        /// <summary>
        /// Indicates visiblilty to column
        /// </summary>
        [Parameter]
        public bool IsHidden
        {
            get => isHidden;
            set
            {
                if (isHidden == value)
                    return;

                isHidden = value;
                RaiseStateChanged();
            }
        }

        /// <summary>
        /// Set column css class
        /// </summary>
        [Parameter] public string CssClass { get; set; }

        /// <summary>
        /// Set unique column id
        /// </summary>
        [Parameter] public string Id { get; set; }
    }
}
