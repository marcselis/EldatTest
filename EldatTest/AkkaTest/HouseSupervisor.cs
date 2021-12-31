using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using AkkaTest.Messages;

namespace AkkaTest
{
    public enum HouseState
    {
        Occupied,
        Sleeping,
        Away
    }

    public class HouseSupervisor : ReceiveActor
    {
        public HouseState State { get; } = HouseState.Occupied;

        private readonly Dictionary<string,IActorRef> _lamps = new Dictionary<string, IActorRef>();
        private ILoggingAdapter Log { get; } = Context.GetLogger();

        protected override void PreStart() => Log.Info("IoT Application started");
        protected override void PostStop() => Log.Info("IoT Application stopped");

        public HouseSupervisor()
        {
            //Context.ActorOf(Props.Create(() => new EasyWaveTransceiver(Self)), "EasyWaveTransceiver");
            AddLamp("Keukentafel");
            AddLamp("Gootsteen");
            AddLamp("Terras");
            AddLamp("Hall");
            AddLamp("Dressing");
            AddLamp("Wasplaats");
            AddLamp("Badkamer");
            AddDimmableLamp("Slaapkamer1");
            AddDimmableLamp("Nachthal");
            Receive<TurnLampOn>(Handle);
            Receive<TurnLampOff>(Handle);
            Receive<DimLampUp>(Handle);
            Receive<DimLampDown>(Handle);
            Receive<EasyWaveButtonPressed>(Handle);
            Context.System.EventStream.Subscribe<LampLevelChanged>(Self);
            Context.System.EventStream.Subscribe<LampStatus>(Self);
            Receive<LampLevelChanged>(Handle);
            Receive<LampStatusChanged>(Handle);
        }

        private bool Handle(LampLevelChanged message)
        {
            Log.Info($"{message.Name}: {message.Level} ");
            return true;
        }

        private bool Handle(LampStatusChanged message)
        {
            Log.Info($"Lamp {message.Name} is now {message.Status}");
            return true;
        }

        private void AddLamp(string lampName)
        {
            _lamps.Add(lampName, Context.ActorOf(Props.Create(() => new Lamp(lampName)), lampName));

        }
        private void AddDimmableLamp(string lampName)
        {
            _lamps.Add(lampName, Context.ActorOf(Props.Create(() => new DimmableLamp(lampName)), lampName));
        }
        private bool Handle(TurnLampOn message)
        {
            if (!_lamps.TryGetValue(message.Name, out var value))
            {
                return false;
            }
            value.Tell(new TurnOn());
            return true;
        }

        private bool Handle(TurnLampOff message)
        {
            if (!_lamps.TryGetValue(message.Name, out var value))
            {
                return false;
            }
            value.Tell(new TurnOff());
            return true;
        }

        private bool Handle(DimLampDown message)
        {
            if (!_lamps.TryGetValue(message.Name, out var value))
            {
                return false;
            }
            value.Tell(new DimDown());
            return true;
        }

        private bool Handle(DimLampUp message)
        {
            if (!_lamps.TryGetValue(message.Name, out var value))
            {
                return false;
            }
            value.Tell(new DimUp());
            return true;
        }

        private bool Handle(EasyWaveButtonPressed message)
        {
            Log.Debug($"Switch {message.Address} has button {message.Code} pressed");
            switch (message.Address)
            {
                case "2274e4":
                    switch (message.Code)
                    {
                        case ButtonCode.A:
                            Self.Tell(new TurnLampOn("Gootsteen"));
                            return true;
                        case ButtonCode.B:
                            Self.Tell(new TurnLampOff("Gootsteen"));
                            return true;
                        case ButtonCode.C:
                            Handle(new TurnLampOn("Keukentafel"));
                            return true;
                        case ButtonCode.D:
                            Handle(new TurnLampOff("Keukentafel"));
                            return true;
                    }
                    break;
                case "229ad6":
                    switch (message.Code)
                    {
                        case ButtonCode.A:
                            Self.Tell(new TurnLampOn("Terras"));
                            return true;
                        case ButtonCode.B:
                            Self.Tell(new TurnLampOff("Terras"));
                            return true;
                    }
                    break;
                case "2295be":
                    switch (message.Code)
                    {
                        case ButtonCode.A:
                            Self.Tell(new TurnLampOn("Wasplaats"));
                            return true;
                        case ButtonCode.B:
                            Self.Tell(new TurnLampOff("Wasplaats"));
                            return true;
                    }
                    break;
                case "229589":
                    switch (message.Code)
                    {
                        case ButtonCode.A:
                            Self.Tell(new DimLampUp("Slaapkamer1"));
                            return true;
                        case ButtonCode.B:
                            Self.Tell(new DimLampDown("Slaapkamer1"));
                            return true;
                    }
                    break;
            }
            return false;
        }
        // No need to handle any messages
    }
}