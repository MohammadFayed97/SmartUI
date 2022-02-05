using Microsoft.AspNetCore.Components;

namespace SmartUI.Blazor.Data
{
    public abstract class BaseOperationGridColumn : BaseGridColumn
    {
        public SortDirection sortDirection { get; private set; } = SortDirection.None;
        private bool allowFilter;
        private bool allowSorting;
        
        public void UpdateSortDirection(SortDirection sortDirection)
        {
            this.sortDirection = sortDirection;
            RaiseStateChanged();
        }

        /// <summary>
        /// If true then can filter by this column
        /// </summary>
        [Parameter]
        public bool AllowFilter
        {
            get => allowFilter;
            set
            {
                if (allowFilter == value)
                    return;

                allowFilter = value;
                RaiseStateChanged();
            }
        }

        /// <summary>
        /// Indicates Sorting by this column
        /// </summary>
        [Parameter]
        public bool AllowSorting
        {
            get => allowSorting;
            set
            {
                if (allowSorting == value)
                    return;

                allowSorting = value;
                RaiseStateChanged();
            }
        }
    }
}
