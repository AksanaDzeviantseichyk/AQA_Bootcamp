using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4.Part_2
{
    public class Rose : Herbal
    {
        public Rose(String name) : base(name) { }
        public override string GetName()
        {
            return "Rose";
        }
    }
}
