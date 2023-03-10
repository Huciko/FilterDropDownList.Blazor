using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using FilterDropDownList.Blazor.GenericFilterSelect.Models;

namespace FilterDropDownList.Blazor.GenericFilterSelect;


public class FilterSelectComponentBase<TData> : FilterSelectComponentBaseProperties<TData>
{
    protected override async Task OnInitializedAsync()
    {
        await Initialize();
    }

    protected override async Task OnParametersSetAsync()
    {
        // Check if "Data" has changed
        if (!HashedList.SetEquals(Data))
        {
            Render = true;
            await Initialize();
        }

        // Check if "BindValue" is different from the "_previousBindValue"
        // To render properly when "BindValue" is changed outside the component
        if (!ReferenceEquals(BindValue, _previousBindValue))
        {
            Render = true;
            SelectedListItem = _previousBindValue = BindValue;
        }

        if (!ReferenceEquals(OnSelectListItemExtraParameter, _previousOnSelectListItemExtraParameter))
        {
            Render = true;
            _previousOnSelectListItemExtraParameter = OnSelectListItemExtraParameter;
        }

        // Also check for the rest of the value type parameters of the component 
        if (FilterAgainstPropertyName != _previousdFilterAgainstPropertyName ||
            InitialSelectText != _previousdInitialSelectText ||
            Label != _previousLabel ||
            Disabled != _previousDisabled ||
            CssClass != _previousCssClass ||
            PopWrapperCssClass != _previousPopWrapperCssClass ||
            Popup != _previousPopup)
        {
            // Do something when any parameter value changes
            Render = true;
            _previousdFilterAgainstPropertyName = FilterAgainstPropertyName;
            _previousdInitialSelectText = InitialSelectText;
            _previousLabel = Label;
            _previousDisabled = Disabled;
            _previousCssClass = CssClass;
            _previousPopWrapperCssClass = PopWrapperCssClass;
            _previousPopup = Popup;
        }
    }

    protected override bool ShouldRender()
    {
        if (Render)
        {
            Render = false;
            return true;
        }
        return false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (SearchBox.Id is not null && Visible == "show")
        {
            await SearchBox.FocusAsync();
        }
    }

    /// <summary>
    /// To trigger on initialization or when <see cref="ListItems"/> changes
    /// </summary>
    private async Task Initialize()
    {
        InstantiateNoItemSelectedItem();
        PopulatePropertyInfoListForFiltering();
        if (Data is not null)
        {
            HashedList = Data.ToHashSet();
            Filter();
            await SetDefaultSelectedItem();
        }
    }

    /// <summary>
    /// Populates the <see cref="PropertyInfoListForFiltering"/> with the set <see cref="FilterAgainstPropertyName"/> property names of <see cref="TData"/> 
    /// to be used when filtering the list
    /// </summary>
    private void PopulatePropertyInfoListForFiltering()
    {
        PropertyInfoListForFiltering.Clear();

        foreach (var propertyName in FilterAgainstPropertyName.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
        {
            PropertyInfoListForFiltering.Add(HashedList?.FirstOrDefault()?.GetType()?.GetProperty(propertyName.Trim()) ?? NoItemSelectedItem?.GetType().GetProperty(propertyName.Trim()));
        }
    }

    /// <summary>
    /// Instantiates a new instance of TData model and references (assigns) it to <see cref="NoItemSelectedItem"/><br/>
    /// Sets the default values for the <see cref="TData"/> on the <see cref="NoItemSelectedItem"/>
    /// </summary>
    /// <returns></returns>
    private void InstantiateNoItemSelectedItem()
    {
        // Instantiate a new instance of TData model and assign it to "NoItemSelectedItem"
        Type type = typeof(TData);
        var obj = Activator.CreateInstance(type);
        NoItemSelectedItem = (TData)obj!;
    }

    /// <summary>
    /// Sets the default selected item on initialization
    /// </summary>
    /// <returns></returns>
    private async Task SetDefaultSelectedItem()
    {
        SelectedListItem = NoItemSelectedItem;
        await BindValueChanged.InvokeAsync(SelectedListItem);
    }

    /// <summary>
    /// Trigger when an item is selected
    /// </summary>
    /// <param name="listItemIdentifierPropertyValue"></param>
    /// <returns></returns>
    protected async Task OnSelectListItemAsync(TData selectedItem)
    {
        Render = true;
        SelectedListItem = selectedItem;
        ToggleDropdown();
        await BindValueChanged.InvokeAsync(SelectedListItem);

        if (OnSelectListItem.HasDelegate)
        {
            var eventCallbackArgs = new FilterSelectComponentEventCallbackArgs<TData> { SelectedItem = SelectedListItem, ExtraParameter = OnSelectListItemExtraParameter };
            await OnSelectListItem.InvokeAsync(eventCallbackArgs);
        }

        if (Popup)
        {
            IsFixed = false;
        }
    }

    /// <summary>
    /// Triggers when search term changes
    /// </summary>
    protected void SearchTermChanged(ChangeEventArgs args)
    {
        Render = true;
        SearchTerm = args?.Value?.ToString() ?? "";
        Filter();
    }

    /// <summary>
    /// Filters the list for all properties in <see cref="PropertyInfoListForFiltering"/>
    /// </summary>
    /// <returns></returns>
    private void Filter()
    {
        Render = true;

        if (HashedList is null || HashedList.Any() == false)
        {
            FilteredListItems = new List<TData>();
            return;
        }

        FilteredListItems = HashedList?.Where(c => SearchValueListItemFilter(c)).ToList() ?? default!;
    }

    /// <summary>
    /// Returns a boolean indicating whether the propertyValue of any property in <see cref="PropertyInfoListForFiltering"/> of the <see cref="listItem"/> matches the <see cref="SearchTerm"/>
    /// </summary>
    /// <param name="listItem"></param>
    /// <returns></returns>
    private bool SearchValueListItemFilter(TData listItem)
    {
        foreach (var propertyInfo in PropertyInfoListForFiltering)
        {
            if ((propertyInfo?.GetValue(listItem)?.ToString() ?? "").Contains(SearchTerm.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    protected void ClosePop(MouseEventArgs args)
    {
        Render = true;

        if (Popup)
        {
            IsFixed = false;
        }
    }

    protected void DropdownFocusOutHandler(FocusEventArgs e)
    {
        Render = true;
        if (PreventDropdownClose)
        {
            PreventDropdownClose = false;
            return;
        }
        Visible = "";
    }

    protected void ToggleDropdown()
    {
        Render = true;
        Visible = Visible == "show" ? "" : "show";
        PreventDropdownClose = Visible == "show";
        if (Popup)
        {
            IsFixed = true;
        }
    }

}