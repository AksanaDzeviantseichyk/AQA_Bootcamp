using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4.Part_2
{
    public class Wolf : Animal, ICarnivore
    {
        public Wolf(String name) : base(name) { }
        public override string GetName()
        {
            return "Wolf";
        }

        public void Eat(Animal food)
        {
            Console.WriteLine($"{name} is eating {food.GetName()}");
        }
    }
}
