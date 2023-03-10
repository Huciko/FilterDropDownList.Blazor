namespace FilterDropDownList.Blazor.GenericFilterSelect.Models;

public class FilterSelectComponentEventCallbackArgs<TData>
{
    public TData? SelectedItem { get; set; } = default!;
    public object? ExtraParameter { get; set; }
}
