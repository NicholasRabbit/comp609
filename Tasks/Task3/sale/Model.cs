

namespace sale;

public class Employee
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string? Fullname { get; set; }

    public string Age { get; set; }

    public override string ToString()
    {
        return $"{this.GetType().Name,-15}{this.Id,-15}{this.Fullname,-20}{this.Age,-5}";
    }

}

[Table("Salesperson")]
public class Salesperson : Employee
{
    public string? SalesVolume { get; set; }
    public override string ToString()
    {
        return base.ToString() + this.SalesVolume;
    }
}

[Table("Manager")]
public class Manager : Employee
{
    public string? TeamSize { get; set; }
    public override string ToString()
    {
        return base.ToString() + this.TeamSize;
    }
}
