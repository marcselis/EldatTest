namespace AkkaTest
{
    public class RequestStatus
    {
        public RequestStatus(long requestId)
        {
            RequestId = requestId;
        }

        public long RequestId { get; }
    }
}