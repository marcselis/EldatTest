namespace AkkaTest.Messages
{
    public class LampLevelChanged
    {
        public byte Level { get; }
        public string Name { get; }

        public LampLevelChanged(string name, byte level)
        {
            Level = level;
            Name = name;
        }
    }
}