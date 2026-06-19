namespace LiveStockManagement;

public class Livestock 
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public double Expense { get; set; }
    public double Weight { get; set; }
    public string? Colour { get; set; }

}

[Table("Cow")]
public class Cow : Livestock
{
    public double Milk { get; set; }
}

[Table("Sheep")]
public class Sheep : Livestock
{
    public double Wool { get; set; }
}


