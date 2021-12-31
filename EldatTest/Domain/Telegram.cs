namespace Autohmation.Domain
{
    public class Telegram : ITelegram
    {
        public Telegram(uint address, KeyCode keyCode)
        {
            Address = address;
            KeyCode = keyCode;
        }
        
        public uint Address { get; }
        public KeyCode KeyCode { get; }
    }
}