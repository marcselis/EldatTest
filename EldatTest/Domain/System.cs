using System;
using System.Collections.Generic;

namespace Autohmation.Domain
{
    public sealed class System : IDisposable
    {
        public static readonly System Instance = new System();
        private readonly EasywaveTransceiver _transceiver = new EasywaveTransceiver("COM3");

        private readonly Lamp _hal = new Lamp("Hal", "Hal beneden");
        private readonly DimmableLamp _nachtHal = new DimmableLamp("NachtHal", "Hal boven");
        private readonly Lamp _keukenGootsteen = new Lamp("KeukenGootsteen", "Keuken gootsteen");
        private readonly Lamp _keukenTafel = new Lamp("KeukenTafel", "Keuken tafel");
        private readonly DimmableLamp _slaapkamer1 = new DimmableLamp("Slaapkamer1", "Slaapkamer Marc & Saskia");
        private readonly Lamp _terras = new Lamp("Terras", "Terras");
        private readonly Lamp _wasplaats = new Lamp("Wasplaats", "Wasplaats");
        private readonly Lamp _dressing = new Lamp("Dressing","Dressing");
        private readonly Lamp _badkamer = new Lamp("Badkamer", "Badkamer");
        private readonly DimmableLamp _salon = new DimmableLamp("Salon","Salon");
        private readonly DimmableLamp _eetkamer = new DimmableLamp("Eetkamer","Eetkamer");
        
        public Dictionary<string, Lamp> Lamps { get; } = new Dictionary<string, Lamp>();
        private readonly List<Trigger> _triggers = new List<Trigger>();

        private System()
        {
            ConfigureLamp(_keukenGootsteen, (0x10, KeyCode.A),(0X2274e4, KeyCode.A));
            ConfigureLamp(_keukenTafel, (0x10, KeyCode.C),(0X2274e4, KeyCode.C));
            ConfigureLamp(_wasplaats,(0x20, KeyCode.A),(0x2295be, KeyCode.A));
            ConfigureLamp(_hal,(0x30, KeyCode.A));
            ConfigureLamp(_nachtHal,(0x40, KeyCode.A));
            ConfigureLamp(_slaapkamer1,(0x41, KeyCode.A),(0x229589, KeyCode.A));
            ConfigureLamp(_dressing,(0x41, KeyCode.C));
            ConfigureLamp(_terras,(0x70, KeyCode.A),(0x229ad6, KeyCode.A));
            ConfigureLamp(_badkamer);
            ConfigureLamp(_salon);
            ConfigureLamp(_eetkamer);
        }

        private void ConfigureLamp(Lamp lamp, params (uint addres, KeyCode keyCode)[] triggers)
        {
            lamp.TurnedOff += Lamp_TurnedOff;
            lamp.TurnedOn += Lamp_TurnedOn;
            Lamps.Add(lamp.Name, lamp);
            if (triggers.Length <= 0) 
                return;
            lamp.TurnOnAction = () => _transceiver.Transmit(triggers[0].addres, triggers[0].keyCode);
            lamp.TurnOffAction = () => _transceiver.Transmit(triggers[0].addres, triggers[0].keyCode + 1);
            foreach (var (addres, keyCode) in triggers)
            {
                _triggers.Add(new Trigger(addres, keyCode, lamp.TurnOn));
                _triggers.Add(new Trigger(addres, keyCode+1, lamp.TurnOff));
            }
        }


        public void Run()
        {
            _transceiver.Open();
            _transceiver.Received += _transceiver_Received;
        }


        private void _transceiver_Received(object sender, Telegram e)
        {
            Console.WriteLine($"Switch {e.Address:X} has button {e.KeyCode} pressed");
            foreach (var trigger in _triggers)
            {
                if (trigger.KeyCode == e.KeyCode && trigger.Address == e.Address)
                    trigger.Execute();
            }
        }

        private static void Lamp_TurnedOff(object sender, EventArgs e)
        {
            var lamp = (Lamp) sender;
            Console.WriteLine($"{lamp.Name} lamp is uitgeschakeld");
        }

        private static void Lamp_TurnedOn(object sender, EventArgs e)
        {
            var lamp = (Lamp) sender;
            Console.WriteLine($"{lamp.Name} lamp is aangeschakeld");
        }

        public void SetLampState(string name, LampState state)
        {
            if (!Lamps.TryGetValue(name, out var lamp)) throw new LampNotFoundException(name);

            if (state == LampState.On)
                lamp.TurnOnAction?.Invoke();
            else
                lamp.TurnOffAction?.Invoke();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                        _transceiver.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~System()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    [Serializable]
    public class LampNotFoundException : Exception
    {
        public LampNotFoundException(string name) : base($"Lamp {name} is not found")
        {
        }
    }
}