﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    public class MFSample : MFAttributes
    {
        const string c = "IMFSample";

        public MFSample(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Currently no flags are defined. Instead, metadata for samples is defined using
        /// attributes. To get attibutes from a sample, use the IMFAttributes interface, which
        /// IMFSample inherits.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetSampleFlagsNative(out MFSampleFlags sampleFlags)
        {
            fixed (void* ptr = &sampleFlags)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[33]);
            }
        }

        /// <summary>
        /// Currently no flags are defined. Instead, metadata for samples is defined using
        /// attributes. To get attibutes from a sample, use the IMFAttributes interface, which
        /// IMFSample inherits.
        /// </summary>
        public MFSampleFlags GetSampleFlags()
        {
            MFSampleFlags flags;
            MediaFoundationException.Try(GetSampleFlagsNative(out flags), c, "GetSampleFlags");
            return flags;
        }

        /// <summary>
        /// Currently no flags are defined. Instead, metadata for samples is defined using
        /// attributes. To set attibutes on a sample, use the IMFAttributes interface, which
        /// IMFSample inherits.
        /// </summary>
        /// <param name="flags">must be zero</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetSampleFlagsNative(MFSampleFlags flags)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, flags, ((void**)(*(void**)UnsafeBasePtr))[34]);
        }

        /// <summary>
        /// Currently no flags are defined. Instead, metadata for samples is defined using
        /// attributes. To set attibutes on a sample, use the IMFAttributes interface, which
        /// IMFSample inherits.
        /// </summary>
        /// <param name="flags">must be zero</param>
        public void SetSampleFlags(MFSampleFlags flags)
        {
            MediaFoundationException.Try(SetSampleFlagsNative(flags), c, "SetSampleFlags");
        }

        /// <summary>
        /// Retrieves the presentation time of the sample.
        /// </summary>
        /// <param name="hnsSampleTime">Presentation time, in 100-nanosecond units.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSampleTimeNative(out long hnsSampleTime)
        {
            fixed (void* ptr = &hnsSampleTime)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[35]);
            }
        }

        /// <summary>
        /// Retrieves the presentation time of the sample.
        /// </summary>
        /// <returns>Presentation time, in 100-nanosecond units.</returns>
        public long GetSampleTime()
        {
            long res;
            MediaFoundationException.Try(GetSampleTimeNative(out res), c, "GetSampleTime");
            return res;
        }

        public unsafe int SetSampleTimeNative(long hnsSampleTime)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, hnsSampleTime, ((void**)(*(void**)UnsafeBasePtr))[36]);
        }

        public void SetSampleTime(long hnsSampleTime)
        {
            MediaFoundationException.Try(SetSampleTimeNative(hnsSampleTime), c, "SetSampleTime");
        }

        public unsafe int GetSampleDurationNative(out long hnsSampleDuration)
        {
            fixed (void* ptr = &hnsSampleDuration)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[37]);
            }
        }

        public long GetSampleDuration()
        {
            long res;
            MediaFoundationException.Try(GetSampleDurationNative(out res), c, "GetSampleDuration");
            return res;
        }

        public unsafe int SetSampleDurationNative(long hnsSampleDuration)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, hnsSampleDuration, ((void**)(*(void**)UnsafeBasePtr))[38]);
        }

        public void SetSampleDuration(long hnsSampleDuration)
        {
            MediaFoundationException.Try(SetSampleDurationNative(hnsSampleDuration), c, "SetSampleDuration");
        }

        public unsafe int GetBufferCountNative(out int bufferCount)
        {
            fixed (void* ptr = &bufferCount)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[39]);
            }
        }

        public int GetBufferCount()
        {
            int res;
            MediaFoundationException.Try(GetBufferCountNative(out res), c, "GetBufferCount");
            return res;
        }

        public unsafe int GetBufferByIndexNative(int index, out MFMediaBuffer buffer)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, index, &ptr, ((void**)(*(void**)UnsafeBasePtr))[40]);
            buffer = ptr == IntPtr.Zero ? null : new MFMediaBuffer(ptr);
            return result;
        }

        public MFMediaBuffer GetBufferByIndex(int index)
        {
            MFMediaBuffer buffer;
            MediaFoundationException.Try(GetBufferByIndexNative(index, out buffer), c, "GetBufferByIndex");
            return buffer;
        }

        public unsafe int ConvertToContinousBufferNative(out MFMediaBuffer buffer)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, &ptr, ((void**)(*(void**)UnsafeBasePtr))[41]);
            buffer = ptr == IntPtr.Zero ? null : new MFMediaBuffer(ptr);
            return result;
        }

        public MFMediaBuffer ConvertToContinousBuffer()
        {
            MFMediaBuffer buffer;
            MediaFoundationException.Try(ConvertToContinousBufferNative(out buffer), c, "ConvertToContinousBuffer");
            return buffer;
        }

        public unsafe int AddBufferNative(MFMediaBuffer buffer)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)((buffer == null) ? IntPtr.Zero : buffer.BasePtr), ((void**)(*(void**)UnsafeBasePtr))[42]);
        }

        public void AddBuffer(MFMediaBuffer buffer)
        {
            MediaFoundationException.Try(AddBufferNative(buffer), c, "AddBuffer");
        }

        public unsafe int RemoveBufferByIndexNative(int index)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, index, ((void**)(*(void**)UnsafeBasePtr))[43]);
        }

        public void RemoveBufferByIndex(int index)
        {
            MediaFoundationException.Try(RemoveBufferByIndexNative(index), c, "RemoveBufferByIndex");
        }

        public unsafe int RemoveAllBuffersNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[44]);
        }

        public void RemoveAllBuffers()
        {
            MediaFoundationException.Try(RemoveAllBuffersNative(), c, "RemoveAllBuffers");
        }

        public unsafe int GetTotalLengthNative(out int totalLength)
        {
            fixed (void* ptr = &totalLength)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[45]);
            }
        }

        public int GetTotalLength()
        {
            int res;
            MediaFoundationException.Try(GetTotalLengthNative(out res), c, "GetTotalLength");
            return res;
        }

        public unsafe int CopyToBufferNative(MFMediaBuffer buffer)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)((buffer == null) ? IntPtr.Zero : buffer.BasePtr), ((void**)(*(void**)UnsafeBasePtr))[46]);
        }

        public void CopyToBuffer(MFMediaBuffer buffer)
        {
            MediaFoundationException.Try(CopyToBufferNative(buffer), c, "CopyToBuffer");
        }
    }
}