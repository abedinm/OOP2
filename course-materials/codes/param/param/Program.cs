using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace param
{
    internal class Program
    {
        static void ShowStudents(params string[] names)
        {
            foreach (string name in names)
            {
                Console.WriteLine(name);
            }
        }
        static void Main(string[] args)
        {
            ShowStudents("Rahim", "Karim");

            Console.WriteLine("-----");

            ShowStudents("Ayesha", "Nila", "Sakib", "Rafi");
            Console.WriteLine("-----");

            ShowStudents("SIAM", "KABIR", "FARIA", "PROMI","MESSI");

            Console.ReadLine();
        }
    }
}
