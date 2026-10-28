using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structt
{
    struct StudentInfo
    {
        public int id;
        public string name;
        public double cgpa;
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            StudentInfo s;

            s.id = 101;
            s.name = "Rahim";
            s.cgpa = 3.75;

            Console.WriteLine(s);
           // Console.WriteLine(s.name);
            //Console.WriteLine(s.cgpa);

            Console.ReadLine();
        }
    }
}
