using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encapsulation
{
    class BonusBox
    {
        private int Bonus;
        
        public void BonusNao(int b)
        {
            Bonus = b;
        }
        public int BonusDao()
        {
            return Bonus;
        }

    }

       // public void ShowBonus()
       // {
       //     Console.WriteLine("Bonus left: " + Bonus);
       // }
    
internal class Program
{
    static void Main(string[] args)
    {
        BonusBox box = new BonusBox();
            box.BonusNao(10);
            Console.WriteLine(box.BonusDao());
           // box.BonusDao();
           
      
        //box.setBonus(1);
        //Console.WriteLine(box.getBonus());

        // box.ShowBonus();
        // box.GiveBonus();
        //box.GiveBonus();
        //box.ShowBonus();
    }
}
}
