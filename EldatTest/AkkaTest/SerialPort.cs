using System;
using System.IO.Ports;
using Akka.Actor;
using Akka.Event;
using AkkaTest.Messages;

namespace AkkaTest
{
    public class SerialPort : ReceiveActor, IDisposable
    {
        private readonly IActorRef _owner;
        private readonly System.IO.Ports.SerialPort _port;
        public ILoggingAdapter Log { get; } = Context.GetLogger();
        public SerialPort(string portName, IActorRef owner)
        {
            _owner = owner;
            _port = new System.IO.Ports.SerialPort(portName, 57600, Parity.None, 8, StopBits.One)
            {
                Handshake = Handshake.None,
                DtrEnable = true,
                RtsEnable = true
            };
            _port.DataReceived += P_DataReceived;
            _port.ErrorReceived += Port_ErrorReceived;
            _port.Open();
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void P_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Log.Debug("Port is receiving data");
            var port = (System.IO.Ports.SerialPort)sender;
            var text = port.ReadExisting();

            _owner.Tell(new SerialMessageReceived(text));
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

        public void Dispose()
        {
            _port?.Dispose();
        }
    }
}