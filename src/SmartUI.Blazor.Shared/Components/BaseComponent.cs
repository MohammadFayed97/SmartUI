namespace SmartUI.Blazor.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

public class BaseComponent : ComponentBase, IDisposable
{
    /// <summary>
    /// Creates a new instance of <see cref="DotNetObjectReference{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    /// <param name="value">The reference of the tracked object.</param>
    /// <returns>An instance of <see cref="DotNetObjectReference{T}"/>.</returns>
    protected DotNetObjectReference<T> CreateDotNetObjectRef<T>(T value) where T : class 
        => DotNetObjectReference.Create(value);
    /// <summary>
    /// Destroys the instance of <see cref="DotNetObjectReference{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    /// <param name="value">The reference of the tracked object.</param>
    protected void DisposeDotNetObjectRef<T>(DotNetObjectReference<T> value) where T : class 
        => value?.Dispose();
    /// <summary>
    /// Event handler for when certain important things happen to component
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected virtual void StateChanged(Object? sender, EventArgs args) => StateHasChanged();
    /// <inheritdoc/>
    public void Dispose() => Dispose(true);

    protected virtual void Dispose(bool disposing)
    {
        return;
    }

    /// <summary>
    /// Gets or sets the reference to the rendered element.
    /// </summary>
    public ElementReference ElementRef { get; set; }
}
