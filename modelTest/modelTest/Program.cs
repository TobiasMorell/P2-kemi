using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            NewModel fisk = new NewModel(new DataPoint(0, 60000, 20000, 500, 0, 0, false));
            for (int i = 0; i <10 ; i++)
            {
                Console.WriteLine(i);
                fisk.Update(1);
            }
            Console.ReadKey();
        }
    }
}
