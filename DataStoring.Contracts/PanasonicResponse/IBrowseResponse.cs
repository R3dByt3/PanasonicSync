namespace DataStoring.Contracts.PanasonicResponse
{
    public interface IBrowseResponse
    {
        string NumberReturned { get; set; }
        IMovieListResponse ParsedResult { get; }
        string Result { get; set; }
        string TotalMatches { get; set; }
        string U { get; set; }
        string UpdateID { get; set; }

        void Parse();
    }
}