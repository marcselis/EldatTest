using System;
using System.Threading;
using Autohmation;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new SimpleSwitch(SimpleReceiverMode.M3);
            sw.StateChanged += (e, s) => Console.WriteLine(s);
            sw.Subscriptions.Add(new Telegram(1,KeyCode.A));
            sw.Receive(new Telegram(1, KeyCode.A));
            Thread.Sleep(1000);
            sw.Receive(new Telegram(1, KeyCode.A));
            Thread.Sleep(10000);
        }

    }
}

