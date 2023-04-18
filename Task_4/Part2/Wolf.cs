
namespace Task4.Part2
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
