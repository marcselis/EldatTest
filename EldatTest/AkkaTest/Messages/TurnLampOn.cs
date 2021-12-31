namespace AkkaTest.Messages
{
    public class TurnLampOn
    {
        public TurnLampOn(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
