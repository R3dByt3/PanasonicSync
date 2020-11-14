using FFmpegStandardWrapper.Abstract.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FFmpegStandardWrapper.Model
{
    public class Muxer : IMuxer
    {
        public static HashSet<IMuxer> Muxers = new HashSet<IMuxer>();

        public bool CanDemux { get; private set; }
        public bool CanMux { get; private set; }
        public string Identifier { get; private set; }
        public string Description { get; private set; }

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

                return true;
            }

            return false;
        }

        public static bool TryGetMuxer(string muxerName, out IMuxer muxer)
        {
            muxer = Muxers.FirstOrDefault(x => x.Identifier == muxerName);
            if (muxer == null)
                return false;

            return true;
        }
    }
}
