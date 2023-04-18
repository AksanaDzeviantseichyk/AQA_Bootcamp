
namespace Task_4.Part1
{
    public class Triangle : Shape
    {
        private readonly double lengthAB;
        private readonly double lengthBC;
        private readonly double lengthAC;
        private readonly double semiPerimeter; 

        public Triangle(double lengthAB, double lengthBC, double lengthAC)
        {
            this.lengthAB = lengthAB;
            this.lengthBC = lengthBC;
            this.lengthAC = lengthAC;
            semiPerimeter = (lengthAB + lengthBC + lengthAC) / 2;
        }
        public bool IsExists()
        {
            return lengthAB + lengthBC > lengthAC && 
                lengthAB + lengthBC > lengthAC && 
                lengthBC + lengthAC > lengthAB;
        }

        public override double GetArea()
        {
            return Math.Sqrt(semiPerimeter * (semiPerimeter - lengthAB) * (semiPerimeter - lengthBC) * (semiPerimeter - lengthAC));
        }

        public override double GetPerimeter()
        {
            return lengthAB + lengthBC + lengthAC;
        }
        
    }
}
