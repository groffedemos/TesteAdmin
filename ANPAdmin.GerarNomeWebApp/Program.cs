using System;

namespace ANPAdmin.GerarNomeWebApp
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("******** IMERSÃO AZURE DEVOPS ********");
            Console.WriteLine("Nome sugerido para uma nova aplicação:");
            Console.WriteLine();
            Console.WriteLine($"imersao-azdo-{Guid.NewGuid().ToString().Substring(0, 8)}");
            Console.WriteLine();
        }
    }
}
