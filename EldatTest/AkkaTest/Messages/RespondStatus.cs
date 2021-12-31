namespace AkkaTest
{
    public class RespondStatus
    {
        public long RequestRequestId { get; }
        public LampStatus Status { get; }

        public RespondStatus(long requestRequestId, LampStatus status)
        {
            RequestRequestId = requestRequestId;
            Status = status;
        }
    }
}