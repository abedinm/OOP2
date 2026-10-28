using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Modifier
{
    class Teacher
    {
        public string name ;
        private int salary = 50000;
        protected string subject = "C#";
        internal string department = "CSE";
        protected internal string room = "9310K";

        public int ShowPrivateSalary()
        {
            return salary;
           // Console.WriteLine("Salary: " + salary);
        }
    }

    class SeniorTeacher : Teacher
    {
        public void ShowProtected()
        {
            Console.WriteLine("Subject: " + subject);
            Console.WriteLine("Room: " + room);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Teacher t = new Teacher();
            t.name = "Hridoy";
            Console.WriteLine(t.name);
            //Console.WriteLine(t.salary);
           // t.salary=10000;

            Console.WriteLine("Name: " + t.name);
            // t.salary=
            //Console.WriteLine("Name: " + t.);
            // t.ShowPrivate();

            // Console.WriteLine("Department: " + t.department);
            // Console.WriteLine("Room: " + t.room);

            // SeniorTeacher s = new SeniorTeacher();
            // s.ShowProtected();
           // Console.WriteLine(t.ShowPrivateSalary());
            // These cause compile-time errors:
            // Console.WriteLine(t.salary);
            // Console.WriteLine(t.subject);

            Console.ReadLine();
        }
    }
}
