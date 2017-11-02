﻿using CSCore.MediaFoundation;
using System.IO;

namespace CSCore.Codecs.WMA
{
    /// <summary>
    /// Mediafoundation WMA decoder.
    /// </summary>
    public class WmaDecoder : MediaFoundationDecoder
    {
        static bool? _isspeechsupported;
        static bool? _iswmasupported;
        static bool? _iswmaprosupported;

        /// <summary>
        /// Gets a value which indicates whether the Mediafoundation WMA, WMA-Speech and WMA-Professional decoder is supported on the current platform.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                return IsSpeechSupported || IsWmaProfessionalSupported || IsWmaSupported;
            }
        }

        /// <summary>
        /// Gets a value which indicates whether the Mediafoundation WMA-Speech decoder is supported on the current platform.
        /// </summary>
        public static bool IsSpeechSupported
        {
            get
            {
                if (_isspeechsupported == null)
                {
                    _isspeechsupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.WmSpeechDecoder);
                }
                return _isspeechsupported.Value;
            }
        }

        /// <summary>
        /// Gets a value which indicates whether the Mediafoundation WMA-Professional decoder is supported on the current platform.
        /// </summary>
        public static bool IsWmaProfessionalSupported
        {
            get
            {
                if (_iswmaprosupported == null)
                {
                    _iswmaprosupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.WmaProDecoder);
                }
                return _iswmaprosupported.Value;
            }
        }

        /// <summary>
        /// Gets a value which indicates whether the Mediafoundation WMA decoder is supported on the current platform.
        /// </summary>
        public static bool IsWmaSupported
        {
            get
            {
                if (_iswmasupported == null)
                {
                    _iswmasupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.WmAudioDecoder);
                }
                return _iswmasupported.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WmaDecoder"/> class.
        /// </summary>
        /// <param name="uri">Url which points to a data source which provides WMA data. This is typically a filename.</param>
        public WmaDecoder(string uri)
            : base(uri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WmaDecoder"/> class.
        /// </summary>
        /// <param name="stream">Stream which contains WMA data.</param>
        public WmaDecoder(Stream stream)
            : base(stream)
        {
        }
    }
}