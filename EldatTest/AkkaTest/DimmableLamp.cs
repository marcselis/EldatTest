using AkkaTest.Messages;

namespace AkkaTest
{
    public class DimmableLamp : Lamp
    {
        private byte _level = 255;

        public byte Level
        {
            get
            {
                if (Status == LampStatus.Off)
                    return 0;
                return _level;
            }
        }

        public DimmableLamp(string name) : base(name)
        {
        }

        protected override void Off()
        {
            base.Off();
            Receive<DimDown>(m => { });
            Receive<DimUp>(m => { Become(On); });
        }

        protected override void On()
        {
            base.On();
            Receive<DimUp>(m =>
            {
                if (_level == 255)
                    return;
                ++_level;
                Log.Debug($"Increasing lamp level to {_level}");
                Context.System.EventStream.Publish(new LampLevelChanged(Name, _level));
            });
            Receive<DimDown>(m =>
            {
                if (_level == 1)
                {
                    Become(Off);
                    return;
                }
                --_level;
                Log.Debug($"Decreasing lamp level to {_level}");
                Context.System.EventStream.Publish(new LampLevelChanged(Name, _level));
            });
        }
    }
}