using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace @enum
{
    internal class Program
    {
      enum Week { Satur,sun,mon=67,tues,wed,thur=67,fri};
        static void Main(string[] args)
        {
            Week day = Week.sun;        
            Console.WriteLine($"Current difficulty: {day}");
            Console.WriteLine("Current difficulty:" + (int)day);


            Console.ReadLine();
        }
    }
}
