
using System.Drawing;

namespace Task4.Part6
{
    public class ColoredBulb : Bulb
    {
        private Color color;

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public string GetColor()
        {
            return color.Name;
        }
    }
}
