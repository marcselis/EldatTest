namespace AkkaTest.Messages
{
    public class DimLampDown
    {
        public string Name { get; }

        public DimLampDown(string name)
        {
            Name = name;
        }
    }
}