using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Autohmation.Domain
{
    public class DimmableLamp : Lamp
    {
        [NonSerialized]
        [XmlIgnore]
        [JsonIgnore]
        public EventHandler<byte> LevelChanged;

        public DimmableLamp()
        {
        }

        public DimmableLamp(string name, string description) : base(name,description)
        {
            Level = 0;
        }

        public byte Level { get; set; }

        public void DimUp()
        {
            if (Level == 255)
                return;
            ChangeLevel((byte) (Level + 1));
        }

        public void DimDown()
        {
            if (Level == 0)
                return;
            ChangeLevel((byte) (Level - 1));
        }

        private void ChangeLevel(byte newLevel)
        {
            if (newLevel == Level)
                return;
            if (Level > 0 && newLevel == 0)
                TurnOff();
            if (Level == 0 && newLevel > 0)
                TurnOn();
            Level = newLevel;
            LevelChanged?.Invoke(this, Level);
        }
    }
}