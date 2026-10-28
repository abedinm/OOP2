using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ref_out
{
    internal class Program
    {
        static void sowaaaaaap(int f,int h,out int p, out int q)
        {

            int t = 0;
            p= f;
            q= h;
            t = p;
            p= q;
            q= t;
            Console.WriteLine("P="+p);
            Console.WriteLine("Q="+q);

        }
        static void Main(string[] args)
        {
            int a = 5;
            int b = 10;
            sowaaaaaap( a, b,out a,out b);
            Console.WriteLine("A=" + a);
            Console.WriteLine("B=" + b);


            Console.ReadLine();
        }
    }
}
