using System;
using Akka.Actor;
using Akka.Event;
using AkkaTest.Messages;

namespace AkkaTest
{
    public class Lamp : ReceiveActor
    {
        public string Name { get; }
        public LampStatus Status { get; private set; } = LampStatus.Unknown;
        public ILoggingAdapter Log { get; } = Context.GetLogger();

        public Lamp(string name)
        {
            Name = name;
            Log.Debug($"Creating lamp instance {Self.Path.Name}");
            Become(Off);
        }

        protected virtual void Off()
        {
            Status = LampStatus.Off;
            Receive<TurnOn>(m => Become(On));
            Receive<TurnOff>(m => Log.Debug($"Lamp {Self.Path.Name} is already off"));
            Log.Debug($"Lamp {Self.Path.Name} is now off");
            Context.System.EventStream.Publish(new LampStatusChanged(Name, Status));
            Sender?.Tell(new LampStatusChanged(Name,Status));
        }

        protected virtual void On()
        {
            Status = LampStatus.On;
            Log.Debug($"Lamp {Self.Path.Name} is now on");
            Receive<TurnOn>(m => Log.Debug($"Lamp {Self.Path.Name} is already on"));
            Receive<TurnOff>(m => Become(Off));
            Context.System.EventStream.Publish(new LampStatusChanged(Name, Status));
            Sender?.Tell(new LampStatusChanged(Name,Status));
        }

        protected override void PreStart()
        {
            Log.Debug($"Lamp actor {Self.Path.Name} starting...");
        }

        protected override void PostStop()
        {
            Log.Debug($"Lamp actor {Self.Path.Name} stopping...");
            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Log.Debug($"Restarting lamp actor {Self.Path.Name} due to exception " + reason.Message);
            base.PreRestart(reason, message);

        }

        protected override void PostRestart(Exception reason)
        {
            Log.Debug($"Lamp actor {Self.Path.Name} restarted due to " + reason.Message);
            base.PostRestart(reason);
        }
    }
}