namespace Lecture1;

using static System.Console;

public class Program
{


    public static void ReadString()
    {
        string? s;
        while (true)
        {
            Write("Enter a word: ");
            s = ReadLine();
            if (string.IsNullOrWhiteSpace(s))
            {
                WriteLine("Invalid");
            }
            else
            {
                break;
            }
        }

        s = s.Trim();
        WriteLine($"{s} has {s.Length} letters");
    }

    public static int GetNumber(string prompt)
    {
        Write(prompt);

        while (true)
        {
            if (int.TryParse(ReadLine(), out int a))
            {
                return a;
            }
            else
            {
                WriteLine("Invalid");
            }
        }
    }

    public static bool IsPerfectSqure(int number)
    {
        int sqrt = (int)Math.Sqrt(number);
        return sqrt * sqrt == number;
    }
    public static void ProcessNumers()
    {
        int a = GetNumber("Enter first number: ");
        int b = GetNumber("Enter second number: ");

        int start = (a < b) ? a : b;
        int end = (a < b) ? b : start;

        for (int i = start; i < end; i++)
        {
            WriteLine(i + (IsPerfectSqure(i) ? "<--" : ""));
        }
    }

    public static bool IsPalindrome(string word)
    {
        string w = word.ToLower();
        int len = w.Length;
        for (int i = 0; i < len / 2; i++)
        {
            if (w[i] != w[len - (i + 1)]) return false;
        }

        return true;
    }

    public static bool IsPrime(int n)
    {
        if (n < 2) return false;
        int boundary = (int)Math.Floor(Math.Sqrt(n));
        for (int i = 2; i <= boundary; i++)
        {
            if (n % i == 0) return false;
        }
        return true;
    }
    static void Main(string[] args)
    {
        //ReadString();
        ProcessNumers();
        // WriteLine(IsPalindrome("Racecar"));
        // WriteLine(IsPalindrome("Racecat"));
        // WriteLine(IsPrime(17));
        // WriteLine(IsPrime(70));
        //WriteLine(IsPrime(7));
    }

}
