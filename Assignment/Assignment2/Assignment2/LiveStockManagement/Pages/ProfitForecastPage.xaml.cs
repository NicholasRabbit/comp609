namespace LiveStockManagement.Pages;

public partial class ProfitForecastPage : ContentPage
{
    private readonly DatabaseService _db;

    public ProfitForecastPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;

        TypePicker.SelectedIndex = 0;   // "Cow"
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        // Validate inputs
        ErrorLabel.IsVisible   = false;
        ResultsFrame.IsVisible = false;
        NoDataFrame.IsVisible  = false;

        string selectedType = TypePicker.SelectedItem?.ToString() ?? "Cow";

        if (!int.TryParse(QuantityEntry.Text, out int quantity) || quantity <= 0)
        {
            ErrorLabel.Text      = "Please enter a valid investment quantity (positive integer).";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (!int.TryParse(DaysEntry.Text, out int days) || days <= 0)
        {
            ErrorLabel.Text      = "Please enter a valid number of days (positive integer).";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Load and filter livestock
        var all = _db.ReadItems();

        double avgDailyProfit;
        int    existingCount;

        if (selectedType == "Cow")
        {
            var cows = all.OfType<Cow>().ToList();
            existingCount  = cows.Count;
            avgDailyProfit = existingCount > 0
                ? cows.Sum(c => c.Milk - c.Expense) / existingCount
                : 0;
        }
        else  // Sheep
        {
            var sheep = all.OfType<Sheep>().ToList();
            existingCount  = sheep.Count;
            avgDailyProfit = existingCount > 0
                ? sheep.Sum(s => s.Wool - s.Expense) / existingCount
                : 0;
        }

        // Warn if no existing animals of that type
        if (existingCount == 0)
        {
            LblNoData.Text        = $"No {selectedType} records found in the database. "
                                  + $"The forecast cannot be calculated without existing {selectedType} data.";
            NoDataFrame.IsVisible = true;
            return;
        }

        // Calculate forecast
        // Formula: (Avg Daily Profit * Investment Quantity) * Number of Days
        double estimatedTotalProfit = avgDailyProfit * quantity * days;

        // Populate result labels
        LblAvgLabel.Text   = $"Based on average daily {selectedType} profit of:";
        LblTotalLabel.Text = $"Buying {quantity} {selectedType} would bring in estimated over {days} days profit of ";

        LblAvgProfit.Text      = $"{avgDailyProfit:F2}";
        LblAvgProfit.TextColor = avgDailyProfit >= 0 ? Colors.Green : Colors.Red;

        LblQuantity.Text = quantity.ToString();
        LblDays.Text     = days.ToString();

        LblTotalProfit.Text      = $"{estimatedTotalProfit:F2}";
        LblTotalProfit.TextColor = estimatedTotalProfit >= 0 ? Colors.Green : Colors.Red;

        ResultsFrame.IsVisible = true;
    }
}
