using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4.Part_1
{
    public class Circle : Shape
    {
        private readonly double _radius;
        public Circle(double radius)
        {
            _radius = radius;
        }

        public override double GetPerimeter() => 2 * Math.PI * _radius;
        public override double GetArea() => Math.PI * _radius * _radius;
    }
}
