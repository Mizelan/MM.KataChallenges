namespace Reversi.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataLoader = new DataProcessor();
            dataLoader.ProcessFromDataFolder(@".\data", @".\output");
        }
    }
}
