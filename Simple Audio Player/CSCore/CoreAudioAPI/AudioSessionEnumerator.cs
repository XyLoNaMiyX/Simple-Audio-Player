﻿using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionEnumerator.
    /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368281(v=vs.85).aspx. 
    /// </summary>
    [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8")]
    public class AudioSessionEnumerator : ComObject, IEnumerable<AudioSessionControl>
    {
        const string InterfaceName = "IAudioSessionEnumerator";

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionEnumerator"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the <see cref="AudioSessionEnumerator"/> object.</param>
        public AudioSessionEnumerator(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Gets the total number of audio sessions.
        /// </summary>
        public int Count
        {
            get
            {
                int count;
                CoreAudioAPIException.Try(GetCountNative(out count), InterfaceName, "GetCount");
                return count;
            }
        }

        /// <summary>
        /// Gets the audio session specified by an index.
        /// </summary>
        /// <param name="index">The session number. If there are n sessions, the sessions are numbered from 0 to n – 1. To get the number of sessions, call the GetCount method.</param>
        public AudioSessionControl this[int index]
        {
            get { return GetSession(index); }
        }

        /// <summary>
        /// The GetCount method gets the total number of audio sessions that are open on the audio device.
        /// </summary>
        /// <param name="count">Receives the total number of audio sessions.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetCountNative(out int count)
        {
            fixed (void* p = &count)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p, ((void**)(*(void**)UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        /// The GetSession method gets the audio session specified by an audio session number.
        /// </summary>
        /// <param name="index">The session number. If there are n sessions, the sessions are numbered from 0 to n – 1. To get the number of sessions, call the GetCount method.</param>
        /// <param name="session">The <see cref="AudioSessionControl"/> of the specified session number.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSessionNative(int index, out AudioSessionControl session)
        {
            IntPtr ptr = IntPtr.Zero;    
            int result =  InteropCalls.CallI(UnsafeBasePtr, index, &ptr, ((void**)(*(void**)UnsafeBasePtr))[4]);
            session = ptr == IntPtr.Zero ? null : new AudioSessionControl(ptr);
            return result;
        }

        /// <summary>
        /// Gets the audio session specified by an audio session number.
        /// </summary>
        /// <param name="index">The session number. If there are n sessions, the sessions are numbered from 0 to n – 1. To get the number of sessions, call the GetCount method.</param>
        public AudioSessionControl GetSession(int index)
        {
            AudioSessionControl session;
            CoreAudioAPIException.Try(GetSessionNative(index, out session), InterfaceName, "GetSession");
            return session;
        }

        /// <summary>
        /// AudioSessionControl enumerator
        /// </summary>
        public IEnumerator<AudioSessionControl> GetEnumerator()
        {
            int c = Count;
            for (int i = 0; i < c; i++)
            {
                yield return GetSession(i);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
