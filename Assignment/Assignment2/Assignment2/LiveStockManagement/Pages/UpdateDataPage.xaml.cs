namespace LiveStockManagement.Pages;

public partial class UpdateDataPage : ContentPage
{
    private readonly DatabaseService _dbs;

    public UpdateDataPage(DatabaseService dbs)
	{  
		InitializeComponent();
        _dbs = dbs;
        BindingContext = this;
    }

    // The livestock record currently being edited (Cow or Sheep)
    private Livestock? _currentItem;

    // Visibility flags 
    private bool _isRecordFound;
    public bool IsRecordFound
    {
        get => _isRecordFound;
        set { _isRecordFound = value; OnPropertyChanged(); }
    }

    private bool _isCow;
    public bool IsCow
    {
        get => _isCow;
        set { _isCow = value; OnPropertyChanged(); }
    }

    private bool _isSheep;
    public bool IsSheep
    {
        get => _isSheep;
        set { _isSheep = value; OnPropertyChanged(); }
    }

    // Summary label shown above the form
    private string _recordSummary = string.Empty;
    public string RecordSummary
    {
        get => _recordSummary;
        set { _recordSummary = value; OnPropertyChanged(); }
    }

    // Bound entry fields 
    private string _expenseText = string.Empty;
    public string ExpenseText
    {
        get => _expenseText;
        set { _expenseText = value; OnPropertyChanged(); }
    }

    private string _weightText = string.Empty;
    public string WeightText
    {
        get => _weightText;
        set { _weightText = value; OnPropertyChanged(); }
    }

    private string _colourText = string.Empty;
    public string ColourText
    {
        get => _colourText;
        set { _colourText = value; OnPropertyChanged(); }
    }

    private string _milkText = string.Empty;
    public string MilkText
    {
        get => _milkText;
        set { _milkText = value; OnPropertyChanged(); }
    }

    private string _woolText = string.Empty;
    public string WoolText
    {
        get => _woolText;
        set { _woolText = value; OnPropertyChanged(); }
    }

    
    // Step 1: Find record by ID
    private void OnFindClicked(object sender, EventArgs e)
    {
        // Reset the form first
        IsRecordFound = false;
        _currentItem = null;

        if (string.IsNullOrWhiteSpace(IdEntry.Text))
        {
            DisplayAlert("Error", "Please enter an ID.", "OK");
            return;
        }

        if (!int.TryParse(IdEntry.Text, out int id))
        {
            DisplayAlert("Error", "ID must be a valid integer.", "OK");
            return;
        }

        var item = _dbs.GetItemById(id);
        if (item is null)
        {
            DisplayAlert("Not Found", $"No livestock found with ID {id}.", "OK");
            return;
        }

        // Populate the form from the retrieved record
        _currentItem = item;
        ExpenseText = item.Expense.ToString();
        WeightText = item.Weight.ToString();
        ColourText = item.Colour ?? string.Empty;

        if (item is Cow cow)
        {
            IsCow = true;
            IsSheep = false;
            MilkText = cow.Milk.ToString();
            WoolText = string.Empty;
            RecordSummary = $"Editing Cow  (ID {item.Id})";
        }
        else if (item is Sheep sheep)
        {
            IsCow = false;
            IsSheep = true;
            WoolText = sheep.Wool.ToString();
            MilkText = string.Empty;
            RecordSummary = $"Editing Sheep  (ID {item.Id})";
        }

        IsRecordFound = true;
    }

    //  Step 2: Validate and save 
    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (_currentItem is null) return;

        // Validate shared fields
        if (!double.TryParse(ExpenseText, out double expense))
        {
            DisplayAlert("Error", "Expense must be a valid number.", "OK");
            return;
        }
        if (!double.TryParse(WeightText, out double weight))
        {
            DisplayAlert("Error", "Weight must be a valid number.", "OK");
            return;
        }

        string colour = ColourText.Trim().ToLower();
        string[] validColours = ["black", "red", "white"];
        if (!validColours.Contains(colour))
        {
            DisplayAlert("Error", "Colour must be black, red, or white.", "OK");
            return;
        }

        // Apply common fields
        _currentItem.Expense = expense;
        _currentItem.Weight = weight;
        _currentItem.Colour = char.ToUpper(colour[0]) + colour.Substring(1);

        // Apply type-specific field and update
        int updated = 0;
        if (_currentItem is Cow cow)
        {
            if (!double.TryParse(MilkText, out double milk))
            {
                DisplayAlert("Error", "Milk must be a valid number.", "OK");
                return;
            }
            cow.Milk = milk;
            updated = _dbs.UpdateItem(cow);
        }
        else if (_currentItem is Sheep sheep)
        {
            if (!double.TryParse(WoolText, out double wool))
            {
                DisplayAlert("Error", "Wool must be a valid number.", "OK");
                return;
            }
            sheep.Wool = wool;
            updated = _dbs.UpdateItem(sheep);
        }

        if (updated > 0)
        {
            DisplayAlert("Success", $"ID {_currentItem.Id} updated.", "OK");
            
            WeakReferenceMessenger.Default.Send(new DBUpdatedMessage(true));
            
            ClearForm();
        }
        else
        {
            DisplayAlert("Error", "Failed to update the record. Please try again.", "OK");
        }
    }

    
    private void ClearForm()
    {
        IdEntry.Text = string.Empty;
        _currentItem = null;
        IsRecordFound = false;
        IsCow = false;
        IsSheep = false;
        ExpenseText = string.Empty;
        WeightText = string.Empty;
        ColourText = string.Empty;
        MilkText = string.Empty;
        WoolText = string.Empty;
        RecordSummary = string.Empty;
    }



}