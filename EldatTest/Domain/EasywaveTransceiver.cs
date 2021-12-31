using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Text;

namespace Autohmation.Domain
{
    public sealed class EasywaveTransceiver : IDisposable
    {
        private readonly string _portName;
        private uint _addresses;
        private bool _isOpen;
        private SerialPort _port;

        public EasywaveTransceiver(string port)
        {
            if (string.IsNullOrWhiteSpace(port))
                throw new ArgumentNullException(nameof(port));
            _portName = port;
        }

        public string VendorId { get; private set; }
        public string DeviceId { get; private set; }

        public uint Version { get; private set; }

        public event EventHandler<Telegram> Received;

        public void Transmit(uint address, KeyCode code)
        {
            if (!_isOpen)
                throw new NotSupportedException("Not open");
            if (address >= _addresses)
                throw new ArgumentOutOfRangeException(nameof(address),
                    $"Cannot transmit to address {address}.  The adapter only supports {_addresses} addresses.");
            var text = $"TXP,{address:x2},{code}\r";
            Debug.WriteLine(">" + text);
            _port.Write(text);
            Received?.Invoke(this, new Telegram(address, code));
        }

        public void Open()
        {
            _port = new SerialPort(_portName, 57600, Parity.None, 8, StopBits.One)
            {
                Handshake = Handshake.None,
                DtrEnable = true,
                RtsEnable = true,
                Encoding = Encoding.ASCII
            };
            _port.DataReceived += P_DataReceived;
            _port.ErrorReceived += Port_ErrorReceived;
            _port.Open();
            _port.Write("GETP?\r");
            _port.Write("ID?\r");
            _isOpen = true;
        }

        public void Close()
        {
            if (!_isOpen) return;
            _port.DataReceived -= P_DataReceived;
            _port.ErrorReceived -= Port_ErrorReceived;
            _port.Close();
            _port.Dispose();
        }

        private static void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void P_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort) sender;
            var input = port.ReadExisting();
            var lines = input.Split('\r');
            foreach (var line in lines)
                ProcessLine(line);
        }

        private void ProcessLine(string line)
        {
            Debug.WriteLine($"<{line}");
            if (string.IsNullOrWhiteSpace(line)) return;
            var parts = line.Split(',', '\t', '\r');
            if (parts.Length == 0) return;
            switch (parts[0])
            {
                case "ID":
                    VendorId = parts[1];
                    DeviceId = parts[2];
                    Version = uint.Parse(parts[3], NumberStyles.HexNumber);
                    break;
                case "GETP":
                    _addresses = uint.Parse(parts[1], NumberStyles.HexNumber);
                    break;
                case "REC":
                    var address = uint.Parse(parts[1], NumberStyles.HexNumber);
                    var code = (KeyCode) Enum.Parse(typeof(KeyCode), parts[2]);
                    Received?.Invoke(this, new Telegram(address, code));
                    break;
                case "OK":
                    break;
                default:
                    Debug.WriteLine($"Received unexpected {line}");
                    break;
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_isOpen)
                        Close();
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
            Dispose(true);

        }

    }

}