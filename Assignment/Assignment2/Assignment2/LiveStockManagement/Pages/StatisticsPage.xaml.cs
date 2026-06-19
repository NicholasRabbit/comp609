namespace LiveStockManagement.Pages;

public partial class StatisticsPage : ContentPage
{
    private readonly DatabaseService _db;

    public StatisticsPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;

        // Set default selections
        TypePicker.SelectedIndex   = 0;   // "All"
        ColourPicker.SelectedIndex = 0;   // "All"
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        // Validate Days
        if (!int.TryParse(DaysEntry.Text, out int days) || days <= 0)
        {
            ErrorLabel.Text      = "Please enter a valid number of days (positive integer).";
            ErrorLabel.IsVisible = true;
            ResultsFrame.IsVisible   = false;
            BreakdownFrame.IsVisible = false;
            return;
        }
        ErrorLabel.IsVisible = false;

        // Load data 
        var all = _db.ReadItems();
        int totalCount = all.Count;

        string selectedType   = TypePicker.SelectedItem?.ToString()   ?? "All";
        string selectedColour = ColourPicker.SelectedItem?.ToString() ?? "All";

        // Filter by type
        IEnumerable<Livestock> filtered = selectedType switch
        {
            "Cow"   => all.OfType<Cow>(),
            "Sheep" => all.OfType<Sheep>(),
            _       => all.AsEnumerable()
        };

        // Filter by colour
        if (selectedColour != "All")
            filtered = filtered.Where(a =>
                string.Equals(a.Colour, selectedColour, StringComparison.OrdinalIgnoreCase));

        var selected = filtered.ToList();
        int selectedCount = selected.Count;

        // Core calculations
        // Each animal stores its *daily* Expense, Milk, Wool values.
        double sumDailyTax    = selected.Sum(a => a.Expense);
        double sumDailyProfit = selected.Sum(a => DailyProfit(a));
        double sumWeights     = selected.Sum(a => a.Weight);
        double sumDailyMilk   = selected.OfType<Cow>().Sum(c => c.Milk);
        double sumDailyWool   = selected.OfType<Sheep>().Sum(s => s.Wool);

        double totalTax     = sumDailyTax    * days;
        double totalProfit  = sumDailyProfit * days;
        double avgWeight    = selectedCount > 0 ? sumWeights / selectedCount : 0;
        double totalMilk    = sumDailyMilk   * days;
        double totalWool    = sumDailyWool   * days;
        double totalProduce = (sumDailyMilk + sumDailyWool) * days;
        double percentage   = totalCount > 0 ? (double)selectedCount / totalCount * 100.0 : 0;

        // Populate result labels    
        LblCount.Text      = selectedCount.ToString();
        LblPercentage.Text = $"{percentage:F2}%";
        LblTax.Text        = $"{totalTax:F2}";
        LblProfit.Text     = $"{totalProfit:F2}";
        LblProfit.TextColor = totalProfit >= 0 ? Colors.Green : Colors.Red;
        LblAvgWeight.Text  = $"{avgWeight:F2} kg";
        LblProduce.Text    = $"{totalProduce:F2}";
        LblMilk.Text       = $"{totalMilk:F2}";
        LblWool.Text       = $"{totalWool:F2}";

        ResultsFrame.IsVisible = true;

        // Daily Profit Breakdown (All type + All colour only)
        bool showBreakdown = selectedType == "All" && selectedColour == "All";
        BreakdownFrame.IsVisible = showBreakdown;

        if (showBreakdown)
        {
            var cows  = all.OfType<Cow>().ToList();
            var sheep = all.OfType<Sheep>().ToList();

            double totalDailyCowProfit   = cows.Sum(c  => DailyProfit(c));
            double totalDailySheepProfit = sheep.Sum(s => DailyProfit(s));
            double avgDailyCowProfit     = cows.Count  > 0 ? totalDailyCowProfit   / cows.Count  : 0;
            double avgDailySheepProfit   = sheep.Count > 0 ? totalDailySheepProfit / sheep.Count : 0;

            LblTotalDailyCow.Text  = $"{totalDailyCowProfit:F2}";
            LblTotalDailyCow.TextColor  = totalDailyCowProfit  >= 0 ? Colors.Green : Colors.Red;

            LblAvgDailyCow.Text    = $"{avgDailyCowProfit:F2}";
            LblAvgDailyCow.TextColor    = avgDailyCowProfit    >= 0 ? Colors.Green : Colors.Red;

            LblTotalDailySheep.Text = $"{totalDailySheepProfit:F2}";
            LblTotalDailySheep.TextColor = totalDailySheepProfit >= 0 ? Colors.Green : Colors.Red;

            LblAvgDailySheep.Text  = $"{avgDailySheepProfit:F2}";
            LblAvgDailySheep.TextColor  = avgDailySheepProfit  >= 0 ? Colors.Green : Colors.Red;
        }
    }

    // Daily profit for one animal = daily produce income minus daily expense
    private static double DailyProfit(Livestock a) => a switch
    {
        Cow   c => c.Milk - c.Expense,
        Sheep s => s.Wool - s.Expense,
        _       => -a.Expense
    };
}
