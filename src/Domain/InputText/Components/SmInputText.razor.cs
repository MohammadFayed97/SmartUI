namespace SmartUI.Blazor.Forms;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SmartUI.Blazor.Forms.Shared;
using SmartUI.Blazor.Forms.Shared.Models;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

public partial class SmInputText : BaseInputComponent<string>
{
    protected override string InternalValue { get; set; }
    protected override async Task<ParseValue<string>> ParseValueFromString(string value) => await Task.FromResult(new ParseValue<string>(true, value, string.Empty));
    private string BindValueEventName => IsChangeOnKeyPress ? "oninput" : "onchange";
    
    private FieldIdentifier _fieldIdentifier;
    private string FieldStateCssClass => ParentEditContext.FieldCssClass(_fieldIdentifier);
    private StringBuilder _cssClassBuilder = new StringBuilder();
    protected override void OnInitialized()
    {
        _fieldIdentifier = FieldIdentifier.Create<string>(ValueExpression);
        if(ParentEditContext != null)
            ParentEditContext.OnValidationStateChanged += OnValidationChanged;
    }
    protected override void OnParametersSet() => InternalValue = Value;

    protected override async Task OnInternalValueChanged(string value)
    {
        await ValueChanged.InvokeAsync(value);
        ParentEditContext?.NotifyFieldChanged(_fieldIdentifier);
        await OnChange.InvokeAsync(value);
    }
    private void OnValidationChanged(Object? sender, EventArgs? args) 
    {
        _cssClassBuilder.Clear();
        _cssClassBuilder.Append(FieldStateCssClass);
        if (FieldStateCssClass.Contains(" invalid"))
            _cssClassBuilder.Append(" is-invalid");
        else if (FieldStateCssClass.Contains(" valid"))
            _cssClassBuilder.Append(" is-valid");
    }


    [CascadingParameter]
    public EditContext ParentEditContext { get; set; }
    /// <summary>
    /// Gets or sets the value inside the input field.
    /// </summary>
    [Parameter]
    public string Value { get; set; }
    /// <summary>
    /// Occurs after text has changed.
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }
    /// <summary>
    /// Occurs after text has changed.
    /// </summary>
    [Parameter]
    public Expression<Func<string>> ValueExpression { get; set; }
    /// <summary>
    /// fire when value inside the input changed
    /// </summary>
    [Parameter]
    public EventCallback<string> OnChange { get; set; }
    /// <summary>
    /// If true the event for binding is "oninput" otherwise "onchange"
    /// </summary>
    [Parameter]
    public bool IsChangeOnKeyPress { get; set; }
}
