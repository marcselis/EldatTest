namespace AkkaTest.Messages
{
    public class TurnLampOff
    {
        public TurnLampOff(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}