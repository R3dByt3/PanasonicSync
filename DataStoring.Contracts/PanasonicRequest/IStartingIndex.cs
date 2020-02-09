using System.Collections.Generic;

namespace DataStoring.Contracts.PanasonicRequest
{
    public interface IStartingIndex
    {
        List<string> Dt { get; set; }
        string Text { get; set; }
    }
}