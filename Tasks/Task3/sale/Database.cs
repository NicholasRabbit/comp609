using System.Collections.Generic;

namespace sale;


public class Database
{
    private readonly SQLiteConnection _conn;
    public Database()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string dbRealPath = Path.Combine(baseDir, @"..\..\..\EmployeeData.db");
        string dbFullPath = Path.GetFullPath(dbRealPath);
        var dbConnStr = new SQLiteConnectionString(dbFullPath);
        _conn = new SQLiteConnection(dbConnStr);
        _conn.CreateTables<Salesperson, Manager>();
    }

    // CRUD
    public List<Employee> ReadItems()
    {
        var emps = new List<Employee>();
        var lst1 = _conn.Table<Salesperson>().ToList();
        emps.AddRange(lst1);
        var lst2 = _conn.Table<Manager>().ToList();
        emps.AddRange(lst2);
        return emps;

    }

    public int InsertItem(Employee item)
    {
        return _conn.Insert(item);
    }

    public int DeleteItem(Employee item)
    {
        return _conn.Delete(item);
    }

    public int UpdateItem(Employee item)
    {
        return _conn.Update(item);
    }
}

