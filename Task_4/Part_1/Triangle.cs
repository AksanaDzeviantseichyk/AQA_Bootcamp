using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4.Part_1
{
    public class Triangle : Shape
    {
        private readonly double _lengthAB;
        private readonly double _lengthBC;
        private readonly double _lengthAC;
        private readonly double _semiperimeter; 

        public Triangle(double lengthAB, double lengthBC, double lengthAC)
        {
            _lengthAB = lengthAB;
            _lengthBC = lengthBC;
            _lengthAC = lengthAC;
            _semiperimeter = (lengthAB + lengthBC + lengthAC) / 2;
        }
        public bool IsExists()
        {
            if (_lengthAB + _lengthBC > _lengthAC && _lengthAB + _lengthBC > _lengthAC && _lengthBC + _lengthAC > _lengthAB)
                return true;
            return false;
        }

        public override double GetArea() => 
            Math.Sqrt(_semiperimeter*(_semiperimeter-_lengthAB)*(_semiperimeter-_lengthBC)*(_semiperimeter-_lengthAC));

        public override double GetPerimeter() =>_lengthAB + _lengthBC + _lengthAC;
        
    }
}
