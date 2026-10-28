using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jagged_array
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            int[][] marks =
            {
                new int[] { 80, 85, 90 },
                new int[] { 70, 75 },
                new int[] { 60, 65, 70, 75 }
            };

            foreach (int[] row in marks)
            {
                foreach (int mark in row)
                {
                    Console.Write(mark + " ");
                }

                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
