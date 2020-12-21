using System;
using System.Threading.Tasks;

namespace Image_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            Task mainAsync = MainAsync(args);
            mainAsync.Wait();

            Console.WriteLine("End...");
            Console.ReadKey();
        }

        private static async Task MainAsync(string[] args)
        {
            FlickerAPI.Answer answer = await FlickerAPI.API.GetAll("pizza");
            Console.WriteLine("Was it a success? (" + answer.IsASuccess + ").");
            Console.WriteLine("Respons are:\n" + answer.Json);

            Console.WriteLine();
        }
    }
}
