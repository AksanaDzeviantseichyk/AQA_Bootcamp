
namespace Task4.Part1
{
    public class Square : Shape
    {
        private readonly double lengthSide;

        public Square(double lengthSide)
        {
             this.lengthSide = lengthSide;
        }

        public override double GetArea()
        {
            return lengthSide * lengthSide;
        }
        public override double GetPerimeter()
        {
            return 4 * lengthSide;
        }
        
    }
}
