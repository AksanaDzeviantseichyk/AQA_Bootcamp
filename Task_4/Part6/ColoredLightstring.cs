
using System.Drawing;
using System.Text;

namespace Task4.Part6
{
    public class ColoredLightstring : Lightstring
    {
        private Color[] colors = { Color.Red, Color.Yellow, Color.Green, Color.Blue };
        private int colorIndex;

        public ColoredLightstring(int numBulbs) : base(numBulbs)
        {
            colorIndex = 0;
            for (int i = 0; i < numBulbs; i++)
            {
                bulbs[i] = new ColoredBulb();
                ((ColoredBulb)bulbs[i]).SetColor(colors[colorIndex]);
                colorIndex = (colorIndex + 1) % colors.Length;
            }
        }

        public override string GetCurrentState()
        {
            StringBuilder state = new StringBuilder();

            for (int i = 0; i < numBulbs; i++)
            {
                state.Append($"{((ColoredBulb)bulbs[i]).GetColor()}\t");
            }
            state.Append("\n");
            for (int i = 0; i < numBulbs; i++)
            {
                state.Append(bulbs[i].IsOn() ? "o\t" :"x\t");
            }
            return state.ToString();
        }
    }
}
