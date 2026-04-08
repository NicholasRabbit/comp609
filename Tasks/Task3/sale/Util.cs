namespace sale;

public class Util
{
    public static readonly int ROUND = 2;
    public static readonly string BAD_STRING = string.Empty;
    public static readonly int BAD_INT = Int32.MinValue;
    public static readonly double BAD_DOUBLE = Double.MinValue;

    public static int ParseInt(Object? o)
    {
        if (o is null || int.TryParse(o.ToString(), out int n) == false)
            return BAD_INT;
        return n;
    }

    public static double ParseDouble(Object? o)
    {
        if (o is null || double.TryParse(o.ToString(), out double d) == false)
            return BAD_DOUBLE;
        return d;
    }

    public static string ToTitleCase(string s)
    {
        // reasTaurant --> Restaurant 
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
    }

    public static string? VerifyType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type)) return null;
        var types = new List<string>() { "Restaurant", "Salon" };
        string t = ToTitleCase(type);
        return types.Contains(t) ? t : null;
    }


}

