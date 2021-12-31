using System;
using System.IO.Ports;

namespace EldatTest
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Opening port");
        //    using (var port = new SerialPort("COM3", 57600, Parity.None, 8, StopBits.One))
        //    {
        //        port.Handshake = Handshake.None;
        //        port.DtrEnable = true;
        //        port.RtsEnable = true;
        //        port.NewLine = "\r";
        //        port.Open();
        //        port.ErrorReceived += Port_ErrorReceived;
        //        Console.WriteLine("Port is open");
        //        do
        //        {
        //            var c = Console.ReadKey(true).KeyChar;
        //            switch (c)
        //            {
        //                case '1':
        //                case '2':
        //                case '3':
        //                case '4':
        //                case '5':
        //                case '6':
        //                case '7':
        //                case '8':
        //                case '9':
        //                    Stopwatch w = Stopwatch.StartNew();
        //                    for (var i = 48; i < c; i++)
        //                    {
        //                        port.Send("TXP,41,B");
        //                    }
        //                    w.Stop();
        //                    Console.WriteLine(w.ElapsedMilliseconds);
        //                    break;
        //                case 'q':
        //                    return;
        //                case 'o':
        //                    port.Send("TXP,41,A");
        //                    break;
        //            }
        //        } while (true);
        //    }
        //}

        static void Main(string[] args)
        {
            int scanner = 0;
            Console.WriteLine("Opening port");
            Console.WriteLine("k: keukentafel licht aan (10C)");
            Console.WriteLine("K: keukentafel licht uit (10D)");
            Console.WriteLine("g: keukengootsteen licht aan (10A)");
            Console.WriteLine("G: keukengootsteen licht uit (10B)");
            Console.WriteLine("w: wasplaats aan (20A)");
            Console.WriteLine("W: wasplaats uit (20B)");
            Console.WriteLine("h: hal aan (30A)");
            Console.WriteLine("H: hal uit (30B)");
            Console.WriteLine("n: nachthal aan (40A)");
            Console.WriteLine("N: nachthal uit (40B)");
            Console.WriteLine("&: slaapkamer aan (41A)");
            Console.WriteLine("1: slaapkamer uit (41B)");
            Console.WriteLine("é: dressing aan (41C)");
            Console.WriteLine("2: dressing uit (41C)");
            Console.WriteLine("t: buitenlicht terras aan (70A)");
            Console.WriteLine("T: buitenlicht terras uit (70B)");
            Console.WriteLine("x: test aan (01A)");
            Console.WriteLine("X: test uit (01B)");

            using (var port = new SerialPort("COM3", 57600, Parity.None, 8, StopBits.One))
            {
                port.Handshake = Handshake.None;
                port.DtrEnable = true;
                port.RtsEnable = true;
                port.Open();
                port.DataReceived += P_DataReceived;
                port.ErrorReceived += Port_ErrorReceived;
                Console.WriteLine("Port is open");
                while (true)
                {
                    var k = Console.ReadKey(true);
                    switch (k.KeyChar)
                    {
                        case 'Q':
                        case 'q':
                            return;
                        case 'l':
                            Console.WriteLine("Querying led");
                            port.Write("LED?\r");
                            break;
                        case 'o':
                            Console.WriteLine("Switching led on");
                            port.Write("LED,ON\r");
                            break;
                        case 'O':
                            Console.WriteLine("Switching led off");
                            port.Write("LED,OFF\r");
                            break;
                        case 'r':
                            Console.WriteLine("Reading");
                            Console.WriteLine(port.ReadExisting());
                            break;
                        case 'a':
                            Console.WriteLine("Sending A on id 0");
                            port.Write("TXP,00,A\r");
                            break;
                        case 'A':
                            Console.WriteLine("Sending A on id 1");
                            port.Write("TXP,01,A\r");
                            break;
                        case 'b':
                            port.Write("TXP,00,B\r");
                            break;
                        case 'B':
                            Console.WriteLine("Sending B on id 1");
                            port.Write("TXP,01,B\r");
                            break;
                        case 'k':
                            port.Write("TXP,10,C\r");
                            break;
                        case 'K':
                            port.Write("TXP,10,D\r");
                            break;
                        case 'g':
                            port.Write("TXP,10,A\r");
                            break;
                        case 'G':
                            port.Write("TXP,10,B\r");
                            break;
                        case 'w':
                            port.Write("TXP,20,A\r");
                            break;
                        case 'W':
                            port.Write("TXP,20,B\r");
                            break;
                        case 'h':
                            port.Write("TXP,30,A\r");
                            break;
                        case 'H':
                            port.Write("TXP,30,B\r");
                            break;
                        case 'n':
                            port.Write("TXP,40,A\r");
                            break;
                        case 'N':
                            port.Write("TXP,40,B\r");
                            break;
                        case '&':
                            port.Write("TXP,41,A\r");
                            break;
                        case '1':
                            port.Write("TXP,41,B\r");
                            break;
                        case 'é':
                            port.Write("TXP,41,C\r");
                            break;
                        case '2':
                            port.Write("TXP,41,D\r");
                            break;
                        case 's':
                            string val = string.Format("TXP,{0:X2},A\r", scanner++);
                            Console.WriteLine(val);
                            port.Write(val);
                            break;
                        case 'S':
                            port.Write(string.Format("TXP,{0:X2},B\r",scanner-1));
                            break;
                        case 't':
                            port.Write("TXP,70,A\r");
                            break;
                        case 'T':
                            port.Write("TXP,70,B\r");
                            break;
                        case 'x':
                            port.Write("TXP,01,A\r");
                            break;
                        case 'X':
                            port.Write("TXP,01,B\r");
                            break;
                    }
                }
            }
        }

        private static void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine($"Error received: {e.EventType}");
        }

        private static void P_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort) sender;
            Console.WriteLine(port.ReadExisting());

        }
    }

    internal static class Extensions
    {
        public static void Send(this SerialPort port, string message)
        {
            port.WriteLine(message);
            Console.Write($"{DateTime.Now:o} {message}:");
            Console.WriteLine(port.ReadLine());
        }
    }
}