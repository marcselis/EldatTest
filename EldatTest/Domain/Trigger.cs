using System;

namespace Autohmation.Domain
{
    public class Trigger
    {

        public Trigger(uint address, KeyCode keyCode, Action action)
        {
            Address = address;
            KeyCode = keyCode;
            Action = action;
        }

        public uint Address { get; set; }
        public KeyCode KeyCode { get; set; }
        public Action Action { get; set; }

        public void Execute()
        {
            Action();
        }
    }
}