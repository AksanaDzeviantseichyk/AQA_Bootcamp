
namespace Task_4.Part2
{
    public class Herbal : Alive
    {
        protected string name;
        
        public Herbal(string name)
        { this.name = name; }
        public override string GetName()
        {
            return "Herbal";
        }

        public virtual string GetIndividualName()
        {
            return name;
        }
    }
}
