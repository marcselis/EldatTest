namespace AkkaTest.Messages
{
    public class DimLampUp
    {
        public string Name { get; }

        public DimLampUp(string name)
        {
            Name = name;
        }
    }
}