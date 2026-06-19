namespace LiveStockManagement.Pages;

public partial class EditDataPage : ContentPage
{
    private readonly DatabaseService _dbs;
    public List<string> AnimalTypes { get; set; } = ["Cow", "Sheep"];

    private bool _isCow;
    public bool IsCow
    {
        get => _isCow;
        set
        {
            _isCow = value;
            OnPropertyChanged();
        }
    }

    private bool _isSheep;
    public bool IsSheep
    {
        get => _isSheep;
        set
        {
            _isSheep = value;
            OnPropertyChanged();
        }
    }



    public EditDataPage(DatabaseService dbs)
    {
        InitializeComponent();
        _dbs = dbs;
        BindingContext = this;
    }

    
    private void OnAnimalTypeChanged(object sender, EventArgs e)
    {
        string selected = AnimalPicker.SelectedItem?.ToString() ?? "";
        IsCow = selected == "Cow";
        IsSheep = selected == "Sheep";

       
        if (IsCow) WoolEntry.Text = string.Empty;
        if (IsSheep) MilkEntry.Text = string.Empty;
    }

    private void Add(object sender, EventArgs e)
    {
        // Should select an animal type.
        if (AnimalPicker.SelectedItem is null)
        {
            DisplayAlert("Error", "Please select an animal type.", "OK");
            return;
        }
        string animalType = AnimalPicker.SelectedItem.ToString()!;

        // Validate numeric fields
        if (!double.TryParse(ExpenseEntry.Text, out double expense))
        {
            DisplayAlert("Error", "Expense must be a valid number.", "OK");
            return;
        }
        if (!double.TryParse(WeightEntry.Text, out double weight))
        {
            DisplayAlert("Error", "Weight must be a valid number.", "OK");
            return;
        }

        // Validate Colour
        string colour = ColourEntry.Text?.Trim().ToLower() ?? "";
        string[] validColours = ["black", "red", "white"];
        if (!validColours.Contains(colour))
        {
            DisplayAlert("Error", "Colour must be black, red, or white.", "OK");
            return;
        }

        // Validate the type-specific numeric field
        double milk = 0, wool = 0;
        if (animalType == "Cow")
        {
            if (!double.TryParse(MilkEntry.Text, out milk))
            {
                DisplayAlert("Error", "Milk must be a valid number.", "OK");
                return;
            }
        }
        else if (animalType == "Sheep")
        {
            if (!double.TryParse(WoolEntry.Text, out wool))
            {
                DisplayAlert("Error", "Wool must be a valid number.", "OK");
                return;
            }
        }


        // Insert the new animal into the database
        int added = 0;
        if (animalType == "Cow")
        {
            Cow cow = new Cow
            {
                Expense = expense,
                Weight = weight,
                // Captilize the first letter of the colour for consistency
                Colour = char.ToUpper(colour[0]) + colour.Substring(1),
                Milk = milk
            };
            added = _dbs.InsertItem(cow);
        }
        else if (animalType == "Sheep")
        {
            Sheep sheep = new Sheep
            {
                Expense = expense,
                Weight = weight,
                Colour = char.ToUpper(colour[0]) + colour.Substring(1),
                Wool = wool
            };
            added = _dbs.InsertItem(sheep);
        }

        
        if (added > 0)
        {
            DisplayAlert("", $"Added:{animalType} ", "Ok");
            WeakReferenceMessenger.Default.Send(new DBUpdatedMessage(true));
            ClearForm();
        }
        else
        {
            DisplayAlert("Error", "Failed to save the record. Please try again.", "OK");
        }



    }

    private void ClearForm()
    {
        AnimalPicker.SelectedItem = null;
        ExpenseEntry.Text = string.Empty;
        WeightEntry.Text = string.Empty;
        ColourEntry.Text = string.Empty;
        MilkEntry.Text = string.Empty;
        WoolEntry.Text = string.Empty;
        IsCow = false;
        IsSheep = false;
    }

}