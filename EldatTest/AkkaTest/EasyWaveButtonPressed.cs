namespace AkkaTest
{
    public class EasyWaveButtonPressed
    {
        public string Address { get; }
        public ButtonCode Code { get; }

        public EasyWaveButtonPressed(string address, ButtonCode code)
        {
            Address = address;
            Code = code;
        }
    }
}