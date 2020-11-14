using FFmpegStandardWrapper.Abstract.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FFmpegStandardWrapper.Model
{
    public class Format : IFormat
    {
        public static HashSet<IFormat> Formats = new HashSet<IFormat>();

        public bool CanDemux { get; set; }
        public bool CanMux { get; set; }
        public string Identifier { get; set; }
        public string Description { get; private set; }
        public IList<IMuxer> Muxers { get; private set; }

        public Format()
        {
            Muxers = new List<IMuxer>();
        }

        public bool Fill(string helpLine)
        {
            Regex regex = new Regex(" (?<D>.)(?<E>.) (?<Identifier>.*)  (?<Description>.*)");
            var match = regex.Match(helpLine);

            if (match.Success)
            {
                if (match.Groups["D"].ToString() == "D")
                    CanDemux = true;
                if (match.Groups["E"].ToString() == "E")
                    CanMux = true;

                Identifier = match.Groups["Identifier"].ToString().Trim();
                Description = match.Groups["Description"].ToString();

                Muxers = Muxer.Muxers.Where(x => x.Identifier == Identifier).ToList();

                return true;
            }

            return false;
        }

        public static bool TryGetFormat(string demuxerName, out IFormat format)
        {
            format = Formats.FirstOrDefault(x => x.Identifier == demuxerName);
            if (format == null)
                return false;

            return true;
        }
    }
}
