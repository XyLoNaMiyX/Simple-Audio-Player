using System;
using System.IO;

// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    public class FlacMetadataSeekTable : FlacMetadata
    {
        readonly FlacSeekPoint[] seekPoints;

        public FlacMetadataSeekTable(Stream stream, Int32 length, bool lastBlock)
            : base(FlacMetaDataType.Seektable, lastBlock, length)
        {
            int entryCount = length / 18;
            EntryCount = entryCount;
            seekPoints = new FlacSeekPoint[entryCount];
            BinaryReader reader = new BinaryReader(stream);
            try
            {
                for (int i = 0; i < entryCount; i++)
                {
                    seekPoints[i] = new FlacSeekPoint(reader.ReadInt64(), reader.ReadInt64(), reader.ReadInt16());
                }
            }
            catch (IOException e)
            {
                throw new FlacException(e, FlacLayer.Metadata);
            }
        }

        public int EntryCount { get; set; }

        public FlacSeekPoint[] SeekPoints { get; set; }

        public FlacSeekPoint this[int index]
        {
            get
            {
                return seekPoints[index];
            }
        }
    }
}