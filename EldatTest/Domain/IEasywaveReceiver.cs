namespace Autohmation.Domain
{
    /// <summary>
    /// Abstraction for a Niko/Eldat Easywave receiver
    /// </summary>
    public interface IEasywaveReceiver
    {
        /// <summary>
        /// Sends a <see cref="Telegram"/> to the receiver
        /// </summary>
        /// <param name="telegram">The EasyWave <see cref="Telegram"/> that was sent by an EasyWave device.</param>
        void Receive(Telegram telegram);
    }
}
