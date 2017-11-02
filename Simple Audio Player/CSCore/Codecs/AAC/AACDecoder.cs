﻿using CSCore.MediaFoundation;
using System.IO;

namespace CSCore.Codecs.AAC
{
    /// <summary>
    /// Mediafoundation AAC decoder.
    /// </summary>
    public class AacDecoder : MediaFoundationDecoder
    {
        static bool? _issupported;

        /// <summary>
        /// Gets a value which indicates whether the Mediafoundation AAC decoder is supported on the current platform.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = MediaFoundationCore.IsSupported && MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.AacDecoder);
                }
                return _issupported.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AacDecoder"/> class.
        /// </summary>
        /// <param name="uri">Url which points to a data source which provides AAC data. This is typically a filename.</param>
        public AacDecoder(string uri)
            : base(uri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AacDecoder"/> class.
        /// </summary>
        /// <param name="stream">Stream which contains AAC data.</param>
        public AacDecoder(Stream stream)
            : base(stream)
        {
        }
    }
}