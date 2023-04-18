﻿
namespace Task4.Part2
{
    public class Bear : Animal, ICarnivore, IHerbivore
    {
        public Bear(String name) : base(name) { }
        public override string GetName()
        {
            return "Bear";
        }

        public void Eat(Animal food)
        {
            Console.WriteLine($"{name} is eating {food.GetName()}");
        }

        public void Eat(Herbal food)
        {
            Console.WriteLine($"{name} is eating {food.GetName()}");
        }
    }
}
