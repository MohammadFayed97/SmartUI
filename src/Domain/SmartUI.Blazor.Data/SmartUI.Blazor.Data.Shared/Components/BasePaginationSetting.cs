using Microsoft.AspNetCore.Components;
using SmartUI.Blazor.Data.Shared.Abstractions;
using SmartUI.Blazor.Shared;

namespace SmartUI.Blazor.Data
{
    public class BasePaginationSetting : BaseComponent, IPaginationSettings
    {
        protected override void OnParametersSet()
        {
            if (LimitOfPages > 10) throw new ArgumentOutOfRangeException("Limit of page must be less than or equal 10");
            base.OnParametersSet();
        }


        [Parameter] public int PageSize { get; set; } = 20;
        [Parameter] public int LimitOfPages { get; set; } = 5;
        /// <summary>
        /// It Render DropDownList in the pager which allow to select PageSize from it 
        /// </summary>
        [Parameter]
        public List<int> PageSizes { get; set; } = new List<int> { 10, 20, 30, 40, 50 };
        
    }
}
