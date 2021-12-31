using System;
using Akka.Actor;
using Akka.Event;
using AkkaTest.Messages;

namespace AkkaTest
{

    class EasyWaveTransceiver : ReceiveActor
    {
        private readonly IActorRef _owner;
        private IActorRef _port;
        public ILoggingAdapter Log { get; } = Context.GetLogger();

        public EasyWaveTransceiver(IActorRef owner)
        {
            _owner = owner;

            _port = Context.ActorOf(Props.Create(() => new SerialPort("COM3", Self)), "COM3");
            Receive<SerialMessageReceived>(m => Reveived(m));
        }

        private void Reveived(SerialMessageReceived m)
        {
            if (string.IsNullOrEmpty(m?.Message))
                return;
            var parts = m.Message.Split(',', '\r');
            if (parts?.Length > 2)
            {
                Enum.TryParse(parts[2], out ButtonCode code);
                _owner.Tell(new EasyWaveButtonPressed(parts[1], code));
            }
            Console.WriteLine(m.Message);
        }

        protected override void PreStart()
        {
            Log.Debug($"EasyWaveTransceiver {Self.Path.Name} starting...");
        }

        protected override void PostStop()
        {
            Log.Debug($"EasyWaveTransceiver {Self.Path.Name} stopping...");
            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Log.Debug($"Restarting EasyWaveTransceiver {Self.Path.Name} due to exception " + reason.Message);
            base.PreRestart(reason, message);

        }

        protected override void PostRestart(Exception reason)
        {
            Log.Debug($"EasyWaveTransceiver {Self.Path.Name} restarted due to " + reason.Message);
            base.PostRestart(reason);
        }

    }
}