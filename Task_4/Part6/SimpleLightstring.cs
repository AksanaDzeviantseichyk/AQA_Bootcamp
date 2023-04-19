
using System.Text;

namespace Task4.Part6
{
    public class SimpleLightstring : Lightstring
    {
        public SimpleLightstring(int numBulbs) : base(numBulbs) { }

        public override string GetCurrentState()
        {
            StringBuilder state = new StringBuilder();
            for (int i = 0; i < numBulbs; i++)
            {
                state.Append(bulbs[i].IsOn() ? "o\t" : "x\t");
            }
            return state.ToString();
        }
    }
}
