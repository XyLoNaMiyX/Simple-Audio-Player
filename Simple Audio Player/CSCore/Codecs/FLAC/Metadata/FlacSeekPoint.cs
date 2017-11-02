namespace CSCore.Codecs.FLAC
{
    public class FlacSeekPoint
    {
        public long Number { get; set; }

        public long Offset { get; set; }

        public int FrameSize { get; set; }

        public FlacSeekPoint()
        {
        }

        public FlacSeekPoint(long number, long offset, int frameSize)
        {
            Number = number;
            Offset = offset;
            FrameSize = frameSize;
        }
    }
}