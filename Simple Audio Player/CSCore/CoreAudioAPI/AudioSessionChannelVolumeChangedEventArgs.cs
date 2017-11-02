using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionChannelVolumeChanged
    /// </summary>
    public class AudioSessionChannelVolumeChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The channel count. This parameter specifies the number of audio channels in the session submix.
        /// </summary>
        public int ChannelCount { get; set; }

        /// <summary>
        /// Each volume level is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume.
        /// </summary>
        public float[] ChannelVolumes { get; set; }

        /// <summary>
        /// Use this value as an index into the <see cref="ChannelVolumes"/>.
        /// If the session submix contains n channels, the channels are numbered from 0 to n� 1. If more than one channel might have changed, the value of ChangedChannel is (DWORD)(�1).
        /// </summary>
        public int ChangedChannel { get; set; }

        /// <summary>
        /// Returns the volume of the specified channel. <see cref="ChannelVolumes"/>
        /// </summary>
        /// <param name="channelIndex"></param>
        /// <returns>Volume level of the specified channelIndex in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume.</returns>
        public float this[int channelIndex]
        {
            get { return ChannelVolumes[channelIndex]; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionChannelVolumeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="channelCount">Number of channels.</param>
        /// <param name="channelVolumes">Volumes of the channels.</param>
        /// <param name="changedChannel">Number of channel volumes changed.</param>
        /// <param name="eventContext">Userdefined event context.</param>
        public AudioSessionChannelVolumeChangedEventArgs(int channelCount, float[] channelVolumes, int changedChannel, Guid eventContext)
            : base(eventContext)
        {
            if (channelVolumes == null)
                throw new ArgumentNullException("channelVolumes");

            if (channelCount < 0 || channelCount != channelVolumes.Length)
                throw new ArgumentOutOfRangeException("channelCount");

            ChannelCount = channelCount;
            ChannelVolumes = channelVolumes;
            ChangedChannel = changedChannel;
        }
    }
}