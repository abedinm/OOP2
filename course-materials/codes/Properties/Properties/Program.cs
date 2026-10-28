using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Properties
{
    class Student
    {
        private int marks;

        public int Numberden
        {
            set
            {
                marks = value;
                /* if (value >= 0 && value <= 100)
                     marks = value;
                 else
                     Console.WriteLine("Sir says: Marks must be 0-100!");
                */
            }
            get
            {
                return marks;
            }
            

        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Student s = new Student();

            s.Numberden = 85;
            Console.WriteLine(s.Numberden);

            s.Numberden = 500;
            Console.WriteLine(s.Numberden);
        }
    }
}
