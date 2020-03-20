using DataStoring.Contracts.UpnpResponse;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DataStoring.UpnpResponse
{
    public class PanasonicDevice : IPanasonicDevice
    {
        public bool IsChecked { get; set; }
        public string Location { get; set; }
        public string ServiceType { get; set; }
        public DateTime LastSeen { get; set; }
        public string UUID { get; set; }

        public void FillFromString(string input)
        {
            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.StartsWith("DATE: "))
                {
                    LastSeen = DateTime.ParseExact(line.Replace("DATE: ", string.Empty), "ddd, dd MMM yyyy HH:mm:ss GMT", CultureInfo.InvariantCulture);
                }
                else if (line.StartsWith("LOCATION: "))
                {
                    Location = line.Replace("LOCATION: ", string.Empty);
                }
                else if (line.StartsWith("ST: "))
                {
                    ServiceType = line.Replace("ST: ", string.Empty);
                }
                else if (line.StartsWith("USN: "))
                {
                    string pattern = @"\:\:.*$";
                    UUID = Regex.Replace(line.Replace("USN: ", string.Empty), pattern, string.Empty);
                }
            }
        }

        public override string ToString()
        {
            return $"{Location} - {UUID} - {LastSeen}";
        }
    }
}
