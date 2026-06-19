namespace LiveStockManagement.Pages;

public partial class RemoveDataPage : ContentPage
{

    private readonly DatabaseService _dbs;

    public RemoveDataPage(DatabaseService dbs)
	{
		InitializeComponent();
        _dbs = dbs;
		BindingContext = this;
    }


    private void Remove(object sender, EventArgs e)
    {
        // Remove an item by its ID.
        if (string.IsNullOrWhiteSpace(IdEntry.Text))
        {
            DisplayAlert("Error", "Please enter an ID.", "OK");
            return;
        }

        if (!int.TryParse(IdEntry.Text, out int id))
        {
            DisplayAlert("Error", "Invalid input. ID must be a valid integer.", "OK");
            return;
        }

        // Confirmation warning before deletion
        Task<bool> confirm = DisplayAlert("Confirm Deletion", $"Are you sure you want to delete the item with ID {id}?", "Yes", "No");
        // Return if user cancels the deletion
        if (!confirm.Result)
        {
            DisplayAlert("Cancellation", "Deletion canceled.", "OK");
            return;
        }


        // Get the item from the database by ID.
        var item = _dbs.GetItemById(id);

        // Show an alert if item doesn't exist.
        int deletedCount = 0;
        if (item == null)
        {
            DisplayAlert("Not Found", $"Non-existent livestock ID {id}.", "OK"); return;
        }
        else
        {
            deletedCount = _dbs.DeleteItem(item);
        }

        if (deletedCount > 0)
        {
            DisplayAlert("Success", $"Item with ID {id} removed.", "OK");
            WeakReferenceMessenger.Default.Send(new DBUpdatedMessage(true));
            IdEntry.Text = string.Empty; 
        }
        else
        {
            DisplayAlert("Error", "Failed to remove the record. Please try again.", "OK");
        }


    }

}