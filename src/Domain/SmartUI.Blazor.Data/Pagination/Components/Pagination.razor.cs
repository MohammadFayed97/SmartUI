namespace SmartUI.Blazor.Data;

using AntiRap.Core.Pagination;
using Microsoft.AspNetCore.Components;
using SmartUI.Blazor.Shared;

public partial class Pagination : BaseComponent
{
    #region fields

    private List<PaginationLink> _links;
    private int prevNavigationPages;
    private int nextNavigationPages;
    
    #endregion

    #region methods
    protected override void OnParametersSet()
    {
        //MetaData = MetaData ?? new MetaData();
        CreatePaginationLinks();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            nextNavigationPages = LimitOfVisiblePages;
            CreatePaginationLinks();
        }

        base.OnAfterRender(firstRender);
    }

    private void CreatePaginationLinks()
    {
        _links = new List<PaginationLink>();

        if (MetaData.CurrentPage == 1)
        {
            prevNavigationPages = 0;
            nextNavigationPages = LimitOfVisiblePages;
        }

        _links.Add(new PaginationLink(1, MetaData.HasPrevious, "<<"));
        _links.Add(new PaginationLink(MetaData.CurrentPage - 1, MetaData.HasPrevious, "<"));

        for (int i = 1; i <= MetaData.TotalPages; i++)
        {
            if (i >= prevNavigationPages && i <= nextNavigationPages)
            {
                _links.Add(new PaginationLink(i, true, i.ToString())
                {
                    Active = MetaData.CurrentPage == i,
                });
            }
        }

        _links.Add(new PaginationLink(MetaData.CurrentPage + 1, MetaData.HasNext, ">"));
        _links.Add(new PaginationLink(MetaData.TotalPages, MetaData.HasNext, ">>"));

        StateHasChanged();
    }

    private async Task OnPageSelected(PaginationLink link)
    {
        if (MetaData.CurrentPage == link.Page || !link.Enabled)
            return;

        if (MetaData.CurrentPage < link.Page)
        {
            if (link.Page >= nextNavigationPages && MetaData.TotalPages != link.Page)
            {
                prevNavigationPages = nextNavigationPages;
                nextNavigationPages += LimitOfVisiblePages;
            }
            else if (link.Page == MetaData.TotalPages)
            {
                nextNavigationPages = MetaData.TotalPages;
                prevNavigationPages = MetaData.TotalPages - LimitOfVisiblePages;
            }
        }
        else
        {
            if (link.Page <= prevNavigationPages && link.Page != 1)
            {
                nextNavigationPages = prevNavigationPages;
                prevNavigationPages -= LimitOfVisiblePages;
            }
            else if (link.Page == 1)
            {
                nextNavigationPages = LimitOfVisiblePages;
                prevNavigationPages = 0;
            }
        }
        MetaData.CurrentPage = link.Page;

        await SelectedPage.InvokeAsync(link.Page);
    }


    private string getAlignment()
    {
        switch (Alignment)
        {
            case Alignment.Right:
                return "start";
            case Alignment.Center:
                return "center";
            case Alignment.Left:
                return "end";
            default:
                return "end";

        }
    }
    #endregion

    #region properties
    /// <summary>
    /// set MetaData To Pagination
    /// </summary>
    [Parameter]
    public MetaData MetaData { get; set; } = new();
    /// <summary>
    /// set how many pages you will see
    /// </summary>
    [Parameter]
    public int LimitOfVisiblePages { get; set; }
    /// <summary>
    /// Triggers when page is selected
    /// </summary>
    [Parameter]
    public EventCallback<int> SelectedPage { get; set; }
    [Parameter]
    public Alignment Alignment { get; set; } = Alignment.Right;
    #endregion
}
