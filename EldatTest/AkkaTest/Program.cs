using System;
using Akka.Actor;
using AkkaTest.Messages;

namespace AkkaTest
{
    internal class Program
    {
        private static void Main()
        {
            using (var system = ActorSystem.Create("iot-system"))
            {
                // Create top level supervisor
                var supervisor = system.ActorOf(Props.Create<HouseSupervisor>(), "house-supervisor");
                supervisor.Tell(new TurnLampOn("Keukentafel"));
                // Exit the system after ENTER is pressed
                do
                {
                    var key = Console.ReadKey(true);
                    switch (key.KeyChar)
                    {
                        case 's':
                            supervisor.Tell(new EasyWaveButtonPressed("229589", ButtonCode.A));
                            break;
                        case 'S':
                            supervisor.Tell(new EasyWaveButtonPressed("229589", ButtonCode.B));
                            break;
                        case 'q':
                            return;

                    }
                } while (true);
            }
        }
    }
}
