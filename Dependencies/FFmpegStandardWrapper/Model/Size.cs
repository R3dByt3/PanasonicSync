using FFmpegStandardWrapper.Abstract.Model;

namespace FFmpegStandardWrapper.Model
{
    public class Size : ISize
    {
        public ulong Height { get; set; }
        public ulong Width { get; set; }

        public bool HasValue => Width != 0 && Height != 0;

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }
}
