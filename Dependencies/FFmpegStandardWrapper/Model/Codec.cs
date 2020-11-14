using FFmpegStandardWrapper.Abstract.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FFmpegStandardWrapper.Model
{
    public class Codec : ICodec
    {
        public static HashSet<ICodec> Codecs = new HashSet<ICodec>();

        public bool CanDecode { get; private set; }
        public bool CanEncode { get; private set; }
        public bool IsVideoCodec { get; private set; }
        public bool IsAudioCodec { get; private set; }
        public bool IsSubtitleCodec { get; private set; }
        public bool IsIntraFrameOnlyCodec { get; private set; }
        public bool HasLossyCompression { get; private set; }
        public bool HasLosslessCompression { get; private set; }
        public string Identifier { get; private set; }
        public string Description { get; private set; }


        public bool Fill(string helpLine)
        {
            Regex regex = new Regex("(?<D>.)(?<E>.)(?<C>.)(?<I>.)(?<L>.)(?<S>.) (?<Identifier>.*)  (?<Description>.*)");
            var match = regex.Match(helpLine);

            if (match.Success)
            {
                if (match.Groups["D"].ToString() == "D")
                    CanDecode = true;
                if (match.Groups["E"].ToString() == "E")
                    CanEncode = true;
                if (match.Groups["C"].ToString() == "V")
                    IsVideoCodec = true;
                if (match.Groups["C"].ToString() == "A")
                    IsAudioCodec = true;
                if (match.Groups["C"].ToString() == "S")
                    IsSubtitleCodec = true;
                if (match.Groups["I"].ToString() == "I")
                    IsIntraFrameOnlyCodec = true;
                if (match.Groups["L"].ToString() == "L")
                    HasLossyCompression = true;
                if (match.Groups["S"].ToString() == "S")
                    HasLosslessCompression = true;

                Identifier = match.Groups["Identifier"].ToString().Trim();
                Description = match.Groups["Description"].ToString();
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Identifier;
        }


        public static bool TryGetCodec(string codecName, out ICodec codec)
        {
            codec = Codecs.FirstOrDefault(x => x.Identifier == codecName);
            if (codec == null)
                return false;

            return true;
        }
    }
}
