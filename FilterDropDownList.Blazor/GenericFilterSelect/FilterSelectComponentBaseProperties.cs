using Microsoft.AspNetCore.Components;
using System.Reflection;
using FilterDropDownList.Blazor.GenericFilterSelect.Models;

namespace FilterDropDownList.Blazor.GenericFilterSelect;


public class FilterSelectComponentBaseProperties<TData> : ComponentBase
{
    /// <summary>
    /// Component parameter Bind value
    /// </summary>
    [Parameter] public TData? BindValue { get; set; }

    /// <summary>
    /// Component parameter Bind value update EventCallback
    /// </summary>
    [Parameter] public EventCallback<TData> BindValueChanged { get; set; }

    /// <summary>
    /// List items of type <see cref="TData"/>
    /// </summary>
    [Parameter, EditorRequired] public IEnumerable<TData> Data { get; set; } = Enumerable.Empty<TData>();

    /// <summary>
    /// Content to render
    /// </summary>
    [Parameter, EditorRequired] public RenderFragment<TData> RowTemplate { get; set; } = default!;

    /// <summary>
    /// EventCallback method to trigger when an item is selected.
    /// </summary>
    [Parameter] public EventCallback<FilterSelectComponentEventCallbackArgs<TData>> OnSelectListItem { get; set; }

    /// <summary>
    /// Property name or comma separated property names to filter against.
    /// e.g. "Name" or "Name, UserId"
    /// </summary>
    [Parameter, EditorRequired] public string FilterAgainstPropertyName { get; set; } = default!;


    /// <summary>
    /// Text for the <see cref="NoItemSelectedItem"/>
    /// </summary>
    [Parameter] public string? InitialSelectText { get; set; } = "None Selected";

    /// <summary>
    /// Label of the component
    /// </summary>
    [Parameter] public string? Label { get; set; } = "Label";

    /// <summary>
    /// Set an extra object together with the <see cref="SelectedListItem"/> for the select component.
    /// Can be accessed via <see cref="OnSelectListItem"/> "EventCallback" when an item is selected.
    /// </summary>
    [Parameter] public object? OnSelectListItemExtraParameter { get; set; }

    /// <summary>
    /// Boolean to disable the component
    /// </summary>
    [Parameter] public bool Disabled { get; set; } = false;

    /// <summary>
    /// Css class(es) for the component
    /// </summary>
    [Parameter] public string? CssClass { get; set; }

    /// <summary>
    /// Css class(es) for the component in <see cref="Popup"/> mode
    /// </summary>
    [Parameter] public string? PopWrapperCssClass { get; set; }

    /// <summary>
    /// Boolean indicating whether the component will popup in UI as a dialog
    /// </summary>
    [Parameter] public bool Popup { get; set; } = false;

    /// <summary>
    /// Boolean whether to render or not the component. False by default
    /// </summary>
    protected bool Render { get; set; } = false;

    /// <summary>
    /// <see cref="RenderFragment"/> to show <see cref="NoItemSelectedItem"/> text
    /// </summary>
    /// <returns></returns>
    protected RenderFragment RenderFragmentInitialSelectText() => builder =>
    {
        builder.AddContent(1, InitialSelectText);
    };

    /// <summary>
    /// Filtered list after search in the list.
    /// </summary>
    protected ICollection<TData> FilteredListItems { get; set; } = default!;

    /// <summary>
    /// Holds the property names to be filtered against
    /// </summary>
    protected List<PropertyInfo> PropertyInfoListForFiltering { get; set; } = new();

    /// <summary>
    /// Hashed list of items for performance on search
    /// </summary>
    protected HashSet<TData> HashedList { get; set; } = new();

    /// <summary>
    /// Selected item on the list
    /// </summary>
    protected TData? SelectedListItem { get; set; }

    /// <summary>
    /// Item representing the "none selected" item. Empty <see cref="TData"/>
    /// </summary>
    protected TData? NoItemSelectedItem { get; set; }

    protected ElementReference SearchBox;
    protected string Visible { get; set; } = string.Empty;
    protected bool PreventDropdownClose { get; set; }
    protected string SearchTerm { get; set; } = string.Empty;
    protected bool IsFixed { get; set; } = false;

    #region Parameter changes Tracking parameters
    /// <summary>
    /// Property to keep track of the previously selected <see cref="TData"/> item
    /// </summary>
    protected TData? _previousBindValue { get; set; }
    protected string _previousdFilterAgainstPropertyName { get; set; } = default!;
    protected string? _previousdInitialSelectText { get; set; } = default!;
    protected string? _previousLabel { get; set; }
    protected object? _previousOnSelectListItemExtraParameter { get; set; }
    protected bool _previousDisabled { get; set; }
    protected string? _previousCssClass { get; set; } = default!;
    protected string? _previousPopWrapperCssClass { get; set; } = default!;
    protected bool _previousPopup { get; set; } = false;
    #endregion
}

