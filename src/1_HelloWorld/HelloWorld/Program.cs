namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
            String str = "Hello world";
            Console.Write(str);     
            Console.WriteLine();

            int i = 1;
            Console.WriteLine("i: " + i);
            string? line = Console.ReadLine();
            Console.WriteLine(line);
        }
    }
}
