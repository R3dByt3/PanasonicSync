using System.Collections.Generic;

namespace DataStoring.Contracts.PanasonicRequest
{
    public interface ISortCriteria
    {
        List<string> Dt { get; set; }
    }
}