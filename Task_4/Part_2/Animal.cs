using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4.Part_2
{
    public class Animal : Alive
    {
        protected string name;

        public Animal(string name)
        {
            this.name = name;
        }

        public override string GetName()
        {
            return "Animal";
        }

        public virtual string GetIndividualName()
        {
            return name;
        }
    }
}
