using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autohmation.Domain
{
    public class SimpleSwitch : IEasywaveReceiver
    {
        public event StateChangedHandler StateChanged;

        public State State
        {
            get { return _state; }
            private   set
            {
                if (value == _state)
                    return;
                _state = value;
                StateChanged?.Invoke(this, _state);
            }
        }

        private SimpleReceiverMode _mode;
        private State _state;

        public SimpleSwitch(SimpleReceiverMode mode)
        {
            _mode = mode;
        }

        public List<ITelegram> Subscriptions { get; private set; } = new List<ITelegram>();

        public void Receive(Telegram telegram)
        {
            var sub = Subscriptions.FirstOrDefault(s => s.Address == telegram.Address);
            if (sub == null)
                return;
            switch (_mode)
            {
                case SimpleReceiverMode.M1:
                    if (sub.KeyCode == telegram.KeyCode)
                    {
                        Device.TurnOn();
                        return;
                    }
                    if (sub.KeyCode == telegram.KeyCode + 1)
                    {
                        Device.TurnOff();
                    }
                    return;
                case SimpleReceiverMode.M2:
                    if (sub.KeyCode == telegram.KeyCode)
                    {
                        State = 1 - State;
                    }
                    return;
                case SimpleReceiverMode.M3:
                    if (sub.KeyCode != telegram.KeyCode) return;
                    State = State.On;
                    Task.Delay(TimeSpan.FromSeconds(7)).ContinueWith(t => State = State.Off);
                    return;
            }
        }

        public IDevice Device { get; set; }
    }
}