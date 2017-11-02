﻿namespace CSCore.Codecs.FLAC
{
    public unsafe class FlacSubFrameData
    {
        public int* DestBuffer;
        public int* ResidualBuffer;

        FlacPartitionedRiceContent _content;

        public FlacPartitionedRiceContent Content
        {
            get { return _content ?? (_content = new FlacPartitionedRiceContent()); }
            set { _content = value; }
        }

        public FlacSubFrameData()
        {
        }
    }
}