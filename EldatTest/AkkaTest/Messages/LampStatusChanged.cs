namespace AkkaTest.Messages
{
    public class LampStatusChanged
    {
        public LampStatus Status { get; }
        public string Name { get; }

        public LampStatusChanged(string name, LampStatus status)
        {
            Status = status;
            Name = name;
        }
    }
}
