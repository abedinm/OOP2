using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Constructor_all
{
    class Student
    {
        public int id;
        public string name;
        public static string UniN;
        // 1. Static Constructor
         static Student()
        {
             UniN = "AIUB";
             Console.WriteLine("Static Constructor Called");
        }
        
        // 2. Non-Parameterized Constructor
        public Student()
        {
            id = -1;
            name = "TUMI NAM DAO NAI.";
            //UniN = "unknown";


            Console.WriteLine("Non-Parameterized Constructor Called");
        }

        // 3. Parameterized Constructor
        public Student(int id, string name)
        {
            if (id <= 40)
            { this.id = id; }
            else { id=404;
                this.id = id;
            
            }
            this.name = name;
            Console.WriteLine("Parameterized Constructor Called");
        }
      

        // 4. Copy Constructor
           public Student(Student s)
           {
               id = s.id;
               name = s.name;
               Console.WriteLine("Copy Constructor Called");
           }
        

        public void Show()
        {
            Console.WriteLine("ID: " + id);
            Console.WriteLine("Name: " + name);
            Console.WriteLine("University Name: " + UniN);
            Console.WriteLine();

        }

        // Destructor (Finalizer)
         ~Student()
          {
             Console.WriteLine("Destructor Called for " + name);
         }
      }
        
        internal class Program
        {
            static void Main(string[] args)
            {
                Student s1 = new Student();
                s1.id =200 ;
                s1.name = "SIAM";
               
                 s1.Show();
                
                //Console.WriteLine(s1.name); 
                // Console.WriteLine(s1.id);

                Student s2 = new Student(101, "BRIGHT");
                s2.Show();

                 Student s3 = new Student(s2);
                 s3.name = "Adib";
                 s3.Show();

      
                Console.ReadLine();
            }
        }
    }
