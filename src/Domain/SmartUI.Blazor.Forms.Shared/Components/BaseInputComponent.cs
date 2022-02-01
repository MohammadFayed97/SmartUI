namespace SmartUI.Blazor.Forms.Shared;

using Microsoft.AspNetCore.Components;
using SmartUI.Blazor.Forms.Shared.Models;
using SmartUI.Blazor.Shared;

public abstract class BaseInputComponent<TValue> : BaseComponent
{
    /// <summary>
    /// Gets or sets the internal edit value.
    /// </summary>
    /// <remarks>
    /// The reason for this to be abstract is so that input components can have
    /// their own specialized parameters that can be binded(Text, Date, Value etc.)
    /// </remarks>
    protected abstract TValue InternalValue { get; set; }
    protected abstract Task OnInternalValueChanged(TValue value);
    protected abstract Task<ParseValue<TValue>> ParseValueFromString(string value);
    protected virtual string FormatValueAsString(TValue value) => value?.ToString();
    /// <summary>
    /// Handles the parsing of an input value.
    /// </summary>
    /// <param name="value">Input value to be parsed.</param>
    /// <returns>Returns the awaitable task.</returns>
    private async Task CurrentValueHandler(string value)
    {
        var result = await ParseValueFromString(value);
        if (result.Success)
            CurrentValue = result.ParsedValue;
    }
    protected TValue CurrentValue
    {
        get
        {
            return InternalValue;
        }
        set
        {
            if (!value.Equals(InternalValue))
            {
                InternalValue = value;
                InvokeAsync(() => OnInternalValueChanged(value));
            }
        }
    }
    /// <summary>
    /// Gets or sets the current input value represented as a string.
    /// </summary>
    protected string CurrentValueAsString
    {
        get => FormatValueAsString(CurrentValue);
        set => InvokeAsync(() => CurrentValueHandler(value));
    }

    #region properties

    /// <summary>
    /// spacify id to component
    /// </summary>
    [Parameter]
    public string Id { get; set; }

    /// <summary>
    /// spacify class to component
    /// </summary>
    [Parameter]
    public string CssClass { get; set; }

    /// <summary>
    /// spacify placeholder to component
    /// </summary>
    [Parameter]
    public string Placeholder { get; set; }
    /// <summary>
    /// If true then input will be disabled
    /// </summary>
    [Parameter]
    public string Disabled { get; set; }

    /// <summary>
    /// Captures all the custom attribute that are not part of Blazorise component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> Attributes { get; set; }
    #endregion
}
