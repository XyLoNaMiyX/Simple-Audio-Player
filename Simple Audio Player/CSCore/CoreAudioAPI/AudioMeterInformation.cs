﻿using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368227(v=vs.85).aspx
    /// </summary>
    [Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064")]
    public class AudioMeterInformation : ComObject
    {
        const string InterfaceName = "IAudioMeterInformation";

        /// <summary>
        /// Creates a new AudioMeterInformation instance for the given device.
        /// </summary>
        /// <returns>AudioMeterInformation</returns>
        public static AudioMeterInformation FromDevice(MMDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            var ptr = device.Activate(new Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064"), CLSCTX.CLSCTX_ALL, IntPtr.Zero);
            return new AudioMeterInformation(ptr);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AudioMeterInformation"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer.</param>
        public AudioMeterInformation(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Gets the number of channels in the audio stream that are monitored by peak meters.
        /// </summary>
        public int MeteringChannelCount
        {
            get { return GetMeteringChannelCount(); }
        }

        /// <summary>
        /// Gets the peak sample value for the given channelindex.
        /// </summary>
        /// <returns></returns>
        public float this[int channelIndex]
        {
            get
            {
                if (channelIndex >= MeteringChannelCount || channelIndex < 0)
                    throw new IndexOutOfRangeException("channelIndex");
                return GetChannelsPeakValues()[channelIndex];
            }
        }

        /// <summary>
        /// The QueryHardwareSupport method queries the audio endpoint device for its
        /// hardware-supported functions.
        /// </summary>
        public EndpointHardwareSupport HardwareSupport
        {
            get { return QueryHardwareSupport(); }
        }

        /// <summary>
        /// Gets the peak sample value for the channels in the audio stream.
        /// </summary>
        public float PeakValue
        {
            get { return GetPeakValue(); }
        }

        /// <summary>
        /// The GetPeakValue method gets the peak sample value for the channels in the audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetPeakValueNative(out float peak)
        {
            fixed (void* ptr = &peak)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        /// The GetPeakValue method gets the peak sample value for the channels in the audio stream.
        /// </summary>
        public float GetPeakValue()
        {
            float peak;
            CoreAudioAPIException.Try(GetPeakValueNative(out peak), InterfaceName, "GetPeakValue");
            return peak;
        }

        /// <summary>
        /// The GetMeteringChannelCount method gets the number of channels in the audio stream that
        /// are monitored by peak meters.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetMeteringChannelCountNative(out int channelCount)
        {
            fixed (void* ptr = &channelCount)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        /// The GetMeteringChannelCount method gets the number of channels in the audio stream that
        /// are monitored by peak meters.
        /// </summary>
        public int GetMeteringChannelCount()
        {
            int channelCount;
            CoreAudioAPIException.Try(GetMeteringChannelCountNative(out channelCount), InterfaceName, "GetMeteringChannelCount");
            return channelCount;
        }

        /// <summary>
        /// The GetChannelsPeakValues method gets the peak sample values for all the channels in the
        /// audio stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelsPeakValuesNative(int channelCount, out float[] peakValues)
        {
            peakValues = new float[channelCount];
            fixed (void* ptr = &peakValues[0])
            {
                return InteropCalls.CallI(UnsafeBasePtr, channelCount, ptr, ((void**)(*(void**)UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        /// The GetChannelsPeakValues method gets the peak sample values for all the channels in the
        /// audio stream.
        /// </summary>
        public float[] GetChannelsPeakValues(int channelCount)
        {
            float[] val;
            CoreAudioAPIException.Try(GetChannelsPeakValuesNative(channelCount, out val), InterfaceName, "GetChannelsPeakValues");
            return val;
        }

        /// <summary>
        /// The GetChannelsPeakValues method gets the peak sample values for all the channels in the
        /// audio stream.
        /// </summary>
        public float[] GetChannelsPeakValues()
        {
            return GetChannelsPeakValues(GetMeteringChannelCount());
        }

        /// <summary>
        /// The QueryHardwareSupport method queries the audio endpoint device for its
        /// hardware-supported functions.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int QueryHardwareSupportNative(out EndpointHardwareSupport hardwareSupportMask)
        {
            fixed (void* ptr = &hardwareSupportMask)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        /// The QueryHardwareSupport method queries the audio endpoint device for its
        /// hardware-supported functions.
        /// </summary>
        public EndpointHardwareSupport QueryHardwareSupport()
        {
            EndpointHardwareSupport res;
            CoreAudioAPIException.Try(QueryHardwareSupportNative(out res), InterfaceName, "QueryHardwareSupport");
            return res;
        }
    }
}