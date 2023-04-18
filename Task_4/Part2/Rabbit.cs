using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4.Part2
{
    public class Rabbit : Animal, IHerbivore
    {
        public Rabbit(String name) : base(name) { }
        public override string GetName()
        {
            return "Rabbit";
        }

        public void Eat(Herbal food)
        {
            Console.WriteLine($"{name} is eating {food.GetName()}");
        }
    }
}
