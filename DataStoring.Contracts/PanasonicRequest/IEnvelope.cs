namespace DataStoring.Contracts.PanasonicRequest
{
    public interface IEnvelope
    {
        IRequestBody Body { get; set; }
        string EncodingStyle { get; set; }
        string SOAPENV { get; set; }
        void SetStartingIndex(int startingIndex);
        void SetRequestedCount(int requestedCount);
    }
}