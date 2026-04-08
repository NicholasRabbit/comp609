namespace sale;

public class MainAppConsole
{
    public ObservableCollection<Employee> Emplyees { get; set; }

    public readonly Database database;

    public MainAppConsole()
    {
        Emplyees = [];
        database = new ();
        ReadDB();
    }

    public void ReadDB()
    {
        database.ReadItems().ForEach(x => Emplyees.Add(x));
    }

    public void Print() 
    {
        WriteLine("===Employee List===");
        Emplyees.ToList().ForEach(item => WriteLine(item));
    }

    

    public void QueryById()
    {
        int id;
        Employee? employee;
        while (true) {
            Write("Enter employee ID: ");
            id = Util.ParseInt(ReadLine());
            if (id == Util.BAD_INT)
            {
                WriteLine("Invalid id.");
            }
            else
            { 
                employee = Emplyees.FirstOrDefault(x => x.Id == id);
                if (employee is null)
                    WriteLine("Could not find employee with the ID:" + id);
                else
                    WriteLine(employee);
            }
        }
    }

  

}
