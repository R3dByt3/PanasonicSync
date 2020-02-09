
namespace DataStoring.Contracts.PanasonicResponse
{
    public interface IPanasonicResponseRoot
    {
        IResponseBody Body { get; set; }
        string EncodingStyle { get; set; }
        string S { get; set; }
    }
}