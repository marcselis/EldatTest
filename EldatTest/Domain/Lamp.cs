using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Autohmation.Domain
{
    [Serializable]
    [XmlInclude(typeof(DimmableLamp))]
    public class Lamp
    { 
        [XmlIgnore]
        [JsonIgnore]
        public  EventHandler TurnedOn;
        [XmlIgnore]
        [JsonIgnore]
        public  EventHandler TurnedOff;
        [XmlIgnore]
        [JsonIgnore]
        public Action TurnOnAction { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public Action TurnOffAction { get; set; }

        public Lamp()
        {

        }
        public Lamp(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public LampState State { get; set; } = LampState.Off;

        public void TurnOn()
        {
            if (State == LampState.On)
            {
                return;
            }
            State = LampState.On;
            TurnedOn?.Invoke(this, EventArgs.Empty);
        }

        public virtual void TurnOff()
        {
            if (State == LampState.Off)
            {
                return;
            }
            State = LampState.Off;
            TurnedOff?.Invoke(this, EventArgs.Empty);
        }
    }
}