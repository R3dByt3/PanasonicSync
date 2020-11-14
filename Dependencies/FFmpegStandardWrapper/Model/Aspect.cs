using FFmpegStandardWrapper.Abstract.Model;

namespace FFmpegStandardWrapper.Model
{
    public class Aspect : IAspect
    {
        public ulong Height { get; set; }
        public ulong Width { get; set; }
        public bool HasValue => Width != 0 && Height != 0;

        public override string ToString()
        {
            return $"{Width}:{Height}";
        }
    }
}
