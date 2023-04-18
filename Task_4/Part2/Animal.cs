
namespace Task4.Part2
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
