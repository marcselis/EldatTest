namespace AkkaTest.Messages
{
    public class SerialMessageReceived
    {
        public string Message { get; }

        public SerialMessageReceived(string message)
        {
            Message = message;
        }
    }
}