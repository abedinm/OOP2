using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace class_object
{
    class Student
    {
        public int id;
        public string name;
        //public string UniN;

        public Student()
          
        {
           // UniN = "AIUB";

           Console.WriteLine("Default cons");
        
        }
        public Student(int id, string name)
        {
            this.id = id;
            this.name = name;

            Console.WriteLine("Param Constructor called");
        }
       
       /*
        public void Show()
        {
            Console.WriteLine("ID: " + id);
         jy   Console.WriteLine("Name: " + name);
        }

       
        ~Student()
        {
            Console.WriteLine("Destructor called");
        }
       */
        internal class Program
        {
            static void Main(string[] args)
            {

                // Student s2= new Student();
                //s2.UniN = "IUB";
                //Console.WriteLine(s2.UniN);
                // Console.WriteLine(s2.id);
                // Console.WriteLine(s2.name);
                //Student s1 = new Student();
                //Console.WriteLine(s1.UniN);
                //s1.id= 1;
                // s1.name = "Hasan";
                Student s2 = new Student();
                Student s1 = new Student(101, "Rahim");

                Console.WriteLine(s1.id);
                Console.WriteLine(s1.name);
                // s1.Show();

                Console.ReadLine();

            }
        }
    }
}
