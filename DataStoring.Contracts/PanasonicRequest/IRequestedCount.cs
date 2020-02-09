using System.Collections.Generic;

namespace DataStoring.Contracts.PanasonicRequest
{
    public interface IRequestedCount
    {
        List<string> Dt { get; set; }
        string Text { get; set; }
    }
}