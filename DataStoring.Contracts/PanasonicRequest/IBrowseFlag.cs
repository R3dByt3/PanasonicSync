using System.Collections.Generic;

namespace DataStoring.Contracts.PanasonicRequest
{
    public interface IBrowseFlag
    {
        List<string> Dt { get; set; }
        string Text { get; set; }
    }
}