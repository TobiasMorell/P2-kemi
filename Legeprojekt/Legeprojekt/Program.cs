using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legeprojekt
{
    class Program
    {
        public static void Main()
        {
            char a = (char)102;
            char b = (char)97;
            char c = (char)114;
            char d = (char)115;
		    Console.WriteLine("Hej Arne. Du har så store løg som en tyr\nTil morgenmad spiser du skyr\nTil aften får du steg\nHvornår vil du elske mig?");
            Console.ReadKey();
            Console.WriteLine("<<33<43<evah3.");
            Console.ReadKey();
            Console.WriteLine("Når du skriver den Password! (hint: forloren skildpadde");
            if (Console.ReadLine().Equals(""+ a + b + c + d))
            {
                Console.WriteLine("YEHS!!! :*");
                Console.ReadKey();
            }
        }
    }
}
