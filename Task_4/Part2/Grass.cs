using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4.Part2
{
    public class Grass : Herbal
    {
        public Grass(String name) : base(name) { }
        public override string GetName()
        {
            return "Grass";
        }
    }
}
