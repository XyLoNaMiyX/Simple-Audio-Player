using System;
using System.Threading;

namespace CSCore.SoundOut.DirectSound
{
    public class DirectSoundNotifyEventArgs : EventArgs
    {
        public int HandleIndex { get; set; }

        public int SampleOffset { get; set; }

        public int BufferSize { get; set; }

        public bool IsTimeOut { get; set; }

        public bool DSoundBufferStopped { get; set; }

        /// <summary>
        /// Set this to stop the notification thread
        /// </summary>
        public bool RequestStopPlayback { get; set; }

        public DirectSoundNotifyEventArgs(int handleIndex, int sampleOffset, int bufferSize, bool isTimeOut, bool bufferStopped)
        {
            HandleIndex = handleIndex;
            SampleOffset = sampleOffset;
            BufferSize = bufferSize;
            IsTimeOut = isTimeOut;
            DSoundBufferStopped = bufferStopped;
        }
    }
}