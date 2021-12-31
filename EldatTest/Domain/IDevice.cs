namespace Autohmation.Domain
{
    /// <summary>
    /// Interface for any device in our system.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Gets or sets the device name.
        /// </summary>
        string Name { get; set; }
        void TurnOn();
        void TurnOff();
    }
}