using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Startar TestClient...");

            Task task = MainAsync(args);
            task.Wait();

            Console.WriteLine("Avslutar TestClient...");
            Console.ReadKey();
        }
        static async Task MainAsync(string[] args)
        {
            Console.WriteLine();

            FlickerAPIClient.Answer answer = await FlickerAPIClient.API.GetAll(new string[] { "pizza", "mozzarella", "food" });

            Console.WriteLine("Answer var okej: " + answer.IsASuccess);

            Console.WriteLine("Answer json är:\n" + answer.Json);
            try
            {
                foreach (string item in (List<string>)answer.Json)
                {
                    Console.WriteLine(Tab(1) + item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

           

            Console.WriteLine();
        }
        private static string Tab(int j)
        {
            string tabs = string.Empty;
            for (int i = 0; i < j; i++)
            {
                tabs += "\t";
            }
            return tabs;
        }
    }
}
