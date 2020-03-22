namespace DataStoring.Contracts
{
    public interface ISettings
    {
        double DeviceDiscoveringTime { get; set; }
        string LocalMoviesPath { get; set; }
    }
}
