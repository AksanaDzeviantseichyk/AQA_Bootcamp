
namespace Task4.Part6
{
    public  abstract class Lightstring
    {
        protected Bulb[] bulbs;
        protected int numBulbs;
        public Lightstring(int numBulbs)
        {
            this.numBulbs = numBulbs;
            bulbs = new Bulb[numBulbs];
            for (int i = 0; i < numBulbs; i++)
            {
                bulbs[i] = new Bulb();
            }
        }

        public void UpdateState()
        {
            int minute = DateTime.Now.Minute;
            for (int i = 0; i < numBulbs; i++)
            {
                bool isOn = (minute % 2 == i % 2);
                bulbs[i].SetState(isOn);
            }
        }
        public abstract string GetCurrentState();
    }
}
