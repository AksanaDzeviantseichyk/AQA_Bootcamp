
namespace Task4.Part6
{
    public class Bulb
    {
        private bool isOn;

        public void SetState(bool isOn)
        {
            this.isOn = isOn;
        }

        public bool IsOn()
        {
            return isOn;
        }
    }
}
