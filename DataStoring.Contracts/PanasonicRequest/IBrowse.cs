namespace DataStoring.Contracts.PanasonicRequest
{
    public interface IBrowse
    {
        IBrowseFlag BrowseFlag { get; set; }
        IFilter Filter { get; set; }
        string M { get; set; }
        IObjectID ObjectID { get; set; }
        IRequestedCount RequestedCount { get; set; }
        ISortCriteria SortCriteria { get; set; }
        IStartingIndex StartingIndex { get; set; }
    }
}