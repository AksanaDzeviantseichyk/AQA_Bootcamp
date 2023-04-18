
namespace Task4.Part1
{
    public class Circle : Shape
    {
        private readonly double radius;
        public Circle(double radius)
        {
            this.radius = radius;
        }

        public override double GetPerimeter()
        {
            return 2 * Math.PI * radius;
        }
        public override double GetArea()
        {
           return Math.PI * radius * radius;
        }
    }
}
