using System.Collections.Generic;

namespace DataStoring.Contracts.PanasonicResponse
{
    public interface IMovieListResponse
    {
        string Dc { get; set; }
        string Dvb { get; set; }
        List<IItem> Items { get; set; }
        string Pxn { get; set; }
        string SchemaLocation { get; set; }
        string Upnp { get; set; }
        string Vli { get; set; }
        string Xmlns { get; set; }
        string Xsi { get; set; }
    }
}