
## Usage/Examples

Install form [NuGet](https://www.nuget.org/packages/FilterDropDownList.Blazor/)

#### Add Imports in _Imports.razor file
To avoid having to add using statements for FilterDropDownList.Blazor to lots of components in your project, it's recommended that you add the following to your root _Imports.razor file. This will make the following usings available to all component in that project.

```razor
@using FilterDropDownList.Blazor.GenericFilterSelect
```

#### Basic Usage

```razor
<h3>Component</h3>

<FilterSelectComponent @bind-BindValue="SelectedPerson"
                       TData="PersonModel"
                       Data="PersonList"
                       FilterAgainstPropertyName="@($"{nameof(PersonModel.Name)}")"
                       OnSelectListItem="OnPersonSelected"
                       Label="Persons">
    <RowTemplate>
        @context.ID - @context.Name
    </RowTemplate>
</FilterSelectComponent>

@code {

    public class PersonModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    private List<PersonModel> PersonList { get; set; } = new();
    public PersonModel SelectedPerson { get; set; } = new();

    protected override void OnInitialized()
    {
        PersonList.Add(new PersonModel { ID = 1, Name = "Huciko", Surname = "Zellikan" });
        PersonList.Add(new PersonModel { ID = 2, Name = "John", Surname = "Gray" });
        PersonList.Add(new PersonModel { ID = 3, Name = "Jack", Surname = "Black" });
    }

    // Optional method to execute on item selected
    private void OnPersonSelected(FilterSelectComponentEventCallbackArgs<PersonModel> args)
    {
        // FilterSelectComponentEventCallbackArgs model has two properties
        // SelectedItem, a reference to the selected iten in the dropdown
        // ExtraParameter, an extra optional parameter (object) to pass together the SelectedItem
        string parsonName = args.SelectedItem.Name;
    }
}
```
