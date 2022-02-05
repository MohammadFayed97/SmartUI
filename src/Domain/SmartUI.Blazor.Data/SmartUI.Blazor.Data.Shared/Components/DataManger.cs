namespace SmartUI.Blazor.Data.Shared;

using SmartUI.Blazor.Data.Shared.Abstractions;
using Microsoft.AspNetCore.Components;
using System;

public class DataManger : ComponentBase, IDataManger
{
    protected override void OnInitialized()
    {
        if (Parent is null)
            throw new ArgumentNullException(nameof(IEnumarableComponentBase), "DataManger must be include in EnumarableComponentBase");

        Parent.AddDataManger(this);
    }

    /// <summary>
    /// Gets or sets the cascaded parent IEnumarableComponentBase component.
    /// </summary>
    [CascadingParameter] public IEnumarableComponentBase Parent { get; set; }
    
    /// <summary>
    /// Spacify the url that used to fetch data from some endpoint
    /// </summary>
    [Parameter] public string Url { get; set; }
}