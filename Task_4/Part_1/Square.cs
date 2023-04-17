using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4.Part_1
{
    public class Square : Shape
    {
        private readonly double _lengthSide;

        public Square(double lengthSide)
        {
            _lengthSide = lengthSide;
        }

        public override double GetArea() => _lengthSide * _lengthSide;
        public override double GetPerimeter() => 4 * _lengthSide;
        
    }
}
