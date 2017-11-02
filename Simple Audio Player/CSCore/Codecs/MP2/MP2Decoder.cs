﻿using CSCore.MediaFoundation;
using System.IO;

namespace CSCore.Codecs.MP2
{
    /// <summary>
    /// Mediafoundation MP2 decoder.
    /// </summary>
    public class Mp2Decoder : MediaFoundationDecoder
    {
        static bool? _issupported;

        /// <summary>
        /// Gets a value which indicates whether the Mediafoundation MP2 decoder is supported on the current platform.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = MediaFoundationCore.IsSupported && MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.MpegAudioDecoder);
                }
                return _issupported.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp2Decoder"/> class.
        /// </summary>
        /// <param name="uri">Url which points to a data source which provides MP2 data. This is typically a filename.</param>
        public Mp2Decoder(string uri)
            : base(uri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp2Decoder"/> class.
        /// </summary>
        /// <param name="stream">Stream which contains MP2 data.</param>
        public Mp2Decoder(Stream stream)
            : base(stream)
        {
        }
    }
}