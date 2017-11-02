﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.Win32;

namespace CSCore.SoundOut
{
    /// <summary>
    ///     Provides audioplayback through Wasapi.
    ///     Minimum supported OS: Windows Vista (see <see cref="IsSupportedOnCurrentPlatform" /> property).
    /// </summary>
    public class WasapiOut : ISoundOut
    {
        readonly bool _eventSync;
        readonly ThreadPriority _playbackThreadPriority;
        readonly AudioClientShareMode _shareMode;
        readonly SynchronizationContext _syncContext;
        AudioClient _audioClient;
        bool _createdResampler;
        MMDevice _device;
        bool _disposed;
        EventWaitHandle _eventWaitHandle;
        bool _isInitialized = false;

        int _latency;
        WaveFormat _outputFormat;
        volatile PlaybackState _playbackState;
        Thread _playbackThread;
        AudioRenderClient _renderClient;
        VolumeSource _volumeSource;
        IWaveSource _source;

        readonly object _lockObj = new object();

        /// <summary>
        ///     Initializes an new instance of <see cref="WasapiOut" /> class.
        ///     EventSyncContext = SynchronizationContext.Current.
        ///     PlaybackThreadPriority = AboveNormal.
        ///     Latency = 100ms.
        ///     EventSync = False.
        ///     ShareMode = Shared.
        /// </summary>
        public WasapiOut()
            : this(false, AudioClientShareMode.Shared, 100) //100 ms default
        {
        }

        /// <summary>
        ///     Initializes an new instance of <see cref="WasapiOut" /> class.
        ///     EventSyncContext = SynchronizationContext.Current.
        ///     PlaybackThreadPriority = AboveNormal.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior.</param>
        /// <param name="shareMode">
        ///     Specifies how to open the audio device. Note that if exclusive mode is used, only one single
        ///     playback for the specified device is possible at once.
        /// </param>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency)
            : this(eventSync, shareMode, latency, ThreadPriority.AboveNormal)
        {
        }

        /// <summary>
        ///     Initializes an new instance of <see cref="WasapiOut" /> class.
        ///     EventSyncContext = SynchronizationContext.Current.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior.</param>
        /// <param name="shareMode">
        ///     Specifies how to open the audio device. Note that if exclusive mode is used, only one single
        ///     playback for the specified device is possible at once.
        /// </param>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        /// <param name="playbackThreadPriority">
        ///     ThreadPriority of the playbackthread which runs in background and feeds the device
        ///     with data.
        /// </param>
        public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency,
            ThreadPriority playbackThreadPriority)
            : this(eventSync, shareMode, latency, playbackThreadPriority, SynchronizationContext.Current)
        {
        }

        /// <summary>
        ///     Initializes an new instance of <see cref="WasapiOut" /> class.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior.</param>
        /// <param name="shareMode">
        ///     Specifies how to open the audio device. Note that if exclusive mode is used, only one single
        ///     playback for the specified device is possible at once.
        /// </param>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        /// <param name="playbackThreadPriority">
        ///     <see cref="ThreadPriority"/> of the playbackthread which runs in background and feeds the device
        ///     with data.
        /// </param>
        /// <param name="eventSyncContext">
        ///     The synchronizationcontext which is used to raise any events like the "Stopped"-event.
        ///     If the passed value is not null, the events will be called async through the <see cref="SynchronizationContext.Post"/> method.
        /// </param>
        public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency,
            ThreadPriority playbackThreadPriority, SynchronizationContext eventSyncContext)
        {
            if (!IsSupportedOnCurrentPlatform)
                throw new PlatformNotSupportedException("Wasapi is only supported on Windows Vista and above.");

            if (latency <= 0)
                throw new ArgumentOutOfRangeException("latency");

            _latency = latency;
            _shareMode = shareMode;
            _eventSync = eventSync;
            _playbackThreadPriority = playbackThreadPriority;
            _syncContext = eventSyncContext;
        }

        /// <summary>
        ///     Gets a value which indicates whether Wasapi is supported on the current Platform. True means that the current
        ///     platform supports <see cref="WasapiOut" />; False means that the current platform does not support
        ///     <see cref="WasapiOut" />.
        /// </summary>
        public static bool IsSupportedOnCurrentPlatform
        {
            get { return Environment.OSVersion.Version.Major >= 6; }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Device" /> which should be used for playback.
        ///     The <see cref="Device" /> property has to be set before initializing. The systems default playback device is used
        ///     as default value
        ///     of the <see cref="Device" /> property.
        /// </summary>
        /// <remarks>
        ///     Make sure to set only activated render devices.
        /// </remarks>
        public MMDevice Device
        {
            get
            {
                return _device ?? (_device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Multimedia));
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _device = value;
            }
        }

        /// <summary>
        ///     Gets a random ID based on internal audioclients memory address for debugging purposes.
        /// </summary>
        public long DebuggingId
        {
            get { return _audioClient != null ? _audioClient.BasePtr.ToInt64() : -1; }
        }

        /// <summary>
        ///     Gets or sets the latency of the playback specified in milliseconds.
        /// </summary>
        public int Latency
        {
            get { return _latency; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _latency = value;
            }
        }

        /// <summary>
        ///     Occurs when the playback stops.
        /// </summary>
        public event EventHandler<PlaybackStoppedEventArgs> Stopped;

        /// <summary>
        ///     Initializes WasapiOut and prepares all resources for playback.
        ///     Note that properties like <see cref="Device" />, <see cref="Latency" />,... won't affect WasapiOut after calling
        ///     <see cref="Initialize" />.
        /// </summary>
        /// <param name="source">The source to prepare for playback.</param>
        public void Initialize(IWaveSource source)
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();

                if (source == null)
                    throw new ArgumentNullException("source");

                if (_playbackState != PlaybackState.Stopped)
                {
                    throw new InvalidOperationException(
                        "PlaybackState has to be Stopped. Call WasapiOut::Stop to stop the playback.");
                }

                _playbackThread.WaitForExit();

                //if (_isInitialized)
                //    throw new InvalidOperationException("Wasapi is already initialized. Call WasapiOut::Stop to uninitialize Wasapi.");

                _volumeSource = new VolumeSource(source);
                _source = _volumeSource.ToWaveSource();
                CleanupResources();
                InitializeInternal();
                _isInitialized = true;
            }
        }

        /// <summary>
        ///     Starts the playback.
        ///     Note: <see cref="Initialize" /> has to get called before calling Play.
        ///     If <see cref="PlaybackState" /> is <see cref="SoundOut.PlaybackState.Paused" />, <see cref="Resume" /> will be
        ///     called automatically.
        /// </summary>
        public void Play()
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();
                CheckForIsInitialized();

                if (PlaybackState == PlaybackState.Stopped)
                {
                    using (var waitHandle = new AutoResetEvent(false))
                    {
                        //just to be sure that the thread finished already. Should not be necessary because after Stop(), Initialize() has to be called which already waits until the playbackthread stopped.
                        _playbackThread.WaitForExit();
                        _playbackThread = new Thread(PlaybackProc)
                        {
                            Name = "WASAPI Playback-Thread; ID = " + DebuggingId,
                            Priority = _playbackThreadPriority
                        };

                        _playbackThread.Start(waitHandle);
                        waitHandle.WaitOne();
                    }
                }
                else if (PlaybackState == PlaybackState.Paused)
                {
                    Resume();
                }
            }
        }

        /// <summary>
        ///     Stops the playback and frees all allocated resources.
        ///     After calling the caller has to call <see cref="Initialize" /> again before another playback can be started.
        /// </summary>
        public void Stop()
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();
                //don't check for isinitialized here

                if (_playbackState != PlaybackState.Stopped && _playbackThread != null)
                {
                    _playbackState = PlaybackState.Stopped;
                    _playbackThread.WaitForExit(); //possible deadlock
                    _playbackThread = null;
                }
                else if (_playbackState == PlaybackState.Stopped && _playbackThread != null)
                {
                    /*
                    * On EOF playbackstate is Stopped, but thread is not stopped. => 
                    * New Session can be started while cleaning up old one => unknown behavior. =>
                    * Always call Stop() to make sure, you wait until the thread is finished cleaning up.
                    */
                    _playbackThread.WaitForExit();
                    _playbackThread = null;
                }
//                else // HACK Nope :)
//                    Debug.WriteLine("Wasapi is already stopped.");
            }
        }

        /// <summary>
        ///     Resumes the paused playback.
        /// </summary>
        public void Resume()
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();
                CheckForIsInitialized();

                if (_playbackState == PlaybackState.Paused)
                    _playbackState = PlaybackState.Playing;
            }
        }

        /// <summary>
        ///     Pauses the playback.
        /// </summary>
        public void Pause()
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();
                CheckForIsInitialized();

                if (PlaybackState == PlaybackState.Playing)
                    _playbackState = PlaybackState.Paused;
            }
        }

        /// <summary>
        ///     Gets the current <see cref="SoundOut.PlaybackState"/> of the playback.
        /// </summary>
        public PlaybackState PlaybackState
        {
            get { return _playbackState; }
        }

        /// <summary>
        ///     Gets or sets the volume of the playback.
        ///     Valid values are in the range from 0.0 (0%) to 1.0 (100%).
        /// </summary>
        public float Volume
        {
            get { return _volumeSource != null ? _volumeSource.Volume : 1; }
            set
            {
                CheckForDisposed();
                CheckForIsInitialized();
                _volumeSource.Volume = value;
            }
        }

        /// <summary>
        ///     The currently initialized source.
        ///     To change the WaveSource property, call <see cref="Initialize"/>.
        /// </summary>
        /// <remarks>
        ///     The value of the WaveSource might not be the value which was passed to the <see cref="Initialize"/> method, because
        ///     WasapiOut (depending on the waveformat of the source) has to use a DmoResampler.
        /// </remarks>
        public IWaveSource WaveSource
        {
            get { return _source; }
        }

        /// <summary>
        ///     Stops the playback (if playing) and cleans up all used resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void PlaybackProc(object playbackStartedEventWaithandle)
        {
            Exception exception = null;
            IntPtr avrtHandle = IntPtr.Zero;
            try
            {
                int bufferSize = _audioClient.BufferSize;
                int frameSize = _outputFormat.Channels * _outputFormat.BytesPerSample;

                var buffer = new byte[bufferSize * frameSize];

                WaitHandle[] eventWaitHandleArray = { _eventWaitHandle };

                //001
                /*if (!FeedBuffer(_renderClient, buffer, bufferSize, frameSize)) //todo: might cause a deadlock: play() is waiting on eventhandle but FeedBuffer got already called
                {
                    _playbackState = PlaybackState.Stopped;
                    if (playbackStartedEventWaithandle is EventWaitHandle)
                    {
                        ((EventWaitHandle)playbackStartedEventWaithandle).Set();
                        playbackStartedEventWaithandle = null;
                    }
                }
                else
                {*/
                _audioClient.Start();
                _playbackState = PlaybackState.Playing;

                int taskIndex;
                string mmcssType = Latency > 25 ? "Audio" : "Pro Audio";
                avrtHandle = Win32.NativeMethods.AvSetMmThreadCharacteristics(mmcssType, out taskIndex);

                if (playbackStartedEventWaithandle is EventWaitHandle)
                {
                    ((EventWaitHandle)playbackStartedEventWaithandle).Set();
                    playbackStartedEventWaithandle = null;
                }

                while (PlaybackState != PlaybackState.Stopped)
                {
                    //based on the "RenderSharedEventDriven"-Sample: http://msdn.microsoft.com/en-us/library/dd940520(v=vs.85).aspx
                    if (_eventSync)
                    {
                        //3 * latency = see msdn: recommended timeout
                        int eventWaitHandleIndex = WaitHandle.WaitAny(eventWaitHandleArray, 3 * _latency, false);
                        if (eventWaitHandleIndex == WaitHandle.WaitTimeout)
                            continue;
                    }
                    else
                        //based on the "RenderSharedTimerDriven"-Sample: http://msdn.microsoft.com/en-us/library/dd940521(v=vs.85).aspx
                        Thread.Sleep(_latency / 8);

                    if (PlaybackState == PlaybackState.Playing)
                    {


                        int padding;
                        if (_eventSync && _shareMode == AudioClientShareMode.Exclusive)
                            padding = 0;
                        else
                            padding = _audioClient.GetCurrentPadding();

                        int framesReadyToFill = bufferSize - padding;
                        //avoid conversion errors
                        if (framesReadyToFill > 5 &&
                            !(_source is DmoResampler &&
                              ((DmoResampler)_source).OutputToInput(framesReadyToFill * frameSize) <= 0))
                        {
                            if (!FeedBuffer(_renderClient, buffer, framesReadyToFill, frameSize))
                                _playbackState = PlaybackState.Stopped;
                        }

                    }
                }

                Win32.NativeMethods.AvRevertMmThreadCharacteristics(avrtHandle);

                Thread.Sleep(_latency / 2);

                _audioClient.Stop();
                _audioClient.Reset();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (avrtHandle != IntPtr.Zero)
                    NativeMethods.AvRevertMmThreadCharacteristics(avrtHandle);
                //CleanupResources();
                if (playbackStartedEventWaithandle is EventWaitHandle)
                    ((EventWaitHandle)playbackStartedEventWaithandle).Set();
                RaiseStopped(exception);
            }
        }

        void CheckForInvalidThreadCall()
        {
            if (Thread.CurrentThread == _playbackThread)
                throw new InvalidOperationException("You must not access this method from the PlaybackThread.");
        }

        void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("WasapiOut");
        }

        void CheckForIsInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("WasapiOut is not initialized.");
        }

        void InitializeInternal()
        {
            const int reftimesPerMillisecond = 10000;

            _audioClient = AudioClient.FromMMDevice(Device);
            _outputFormat = SetupWaveFormat(_source.WaveFormat, _audioClient);

            long latency = _latency * reftimesPerMillisecond;
        AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED_TRY_AGAIN:
            try
            {

                if (!_eventSync)
                    _audioClient.Initialize(_shareMode, AudioClientStreamFlags.None, latency, 0, _outputFormat,
                        Guid.Empty);
                else //event sync
                {
                    if (_shareMode == AudioClientShareMode.Exclusive) //exclusive
                    {
                        _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlagsEventCallback, latency,
                            latency, _outputFormat, Guid.Empty);
                    }
                    else //shared
                    {
                        _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlagsEventCallback, 0, 0,
                            _outputFormat, Guid.Empty);
                        latency = (int)(_audioClient.StreamLatency / reftimesPerMillisecond);
                    }
                }
            }
            catch (CoreAudioAPIException exception)
            {
                if (exception.ErrorCode == unchecked((int)0x88890019)) //AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED
                {
                    const long reftimesPerSec = 10000000;
                    int framesInBuffer = _audioClient.GetBufferSize();
                    latency = (int)(reftimesPerSec * framesInBuffer / _outputFormat.SampleRate + 0.5);
                    goto AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED_TRY_AGAIN;
                }
                throw;
            }

            Latency = (int) (latency / reftimesPerMillisecond);

            if (_eventSync)
            {
                _eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                _audioClient.SetEventHandle(_eventWaitHandle.SafeWaitHandle.DangerousGetHandle());
            }

            _renderClient = AudioRenderClient.FromAudioClient(_audioClient);
        }

        void CleanupResources()
        {
            if (_createdResampler && _source is DmoResampler)
            {
                ((DmoResampler)_source).DisposeResamplerOnly();
                _source = null;
            }

            if (_renderClient != null)
            {
                _renderClient.Dispose();
                _renderClient = null;
            }
            if (_audioClient != null)
            {
                try
                {
                    _audioClient.StopNative();
                    _audioClient.Reset();
                }
                catch (CoreAudioAPIException ex)
                {
                    if (ex.ErrorCode != unchecked((int)0x88890001)) //AUDCLNT_E_NOT_INITIALIZED
                        throw;
                }
                _audioClient.Dispose();
                _audioClient = null;
            }
            /*if (_simpleAudioVolume != null)
            {
                _simpleAudioVolume.Dispose();
                _simpleAudioVolume = null;
            }*/
            if (_eventWaitHandle != null)
            {
                _eventWaitHandle.Close();
                _eventWaitHandle = null;
            }

            _isInitialized = false;
        }

        WaveFormat SetupWaveFormat(WaveFormat waveFormat, AudioClient audioClient)
        {
            WaveFormat closestMatch;
            WaveFormat finalFormat = waveFormat;
            if (!audioClient.IsFormatSupported(_shareMode, waveFormat, out closestMatch))
            {
                if (closestMatch == null)
                {
                    WaveFormat mixformat = audioClient.GetMixFormat();
                    if (mixformat == null || !audioClient.IsFormatSupported(_shareMode, mixformat))
                    {
                        WaveFormatExtensible[] possibleFormats =
                        {
                            new WaveFormatExtensible(waveFormat.SampleRate, 32, waveFormat.Channels,
                                AudioSubTypes.IeeeFloat),
                            new WaveFormatExtensible(waveFormat.SampleRate, 24, waveFormat.Channels,
                                AudioSubTypes.Pcm),
                            new WaveFormatExtensible(waveFormat.SampleRate, 16, waveFormat.Channels,
                                AudioSubTypes.Pcm),
                            new WaveFormatExtensible(waveFormat.SampleRate, 8, waveFormat.Channels,
                                AudioSubTypes.Pcm)
                        };

                        if (!CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
                        {
                            //no format found...
                            possibleFormats = new[]
                            {
                                new WaveFormatExtensible(waveFormat.SampleRate, 32, 2, AudioSubTypes.IeeeFloat),
                                new WaveFormatExtensible(waveFormat.SampleRate, 24, 2, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 16, 2, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 8, 2, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 32, 1, AudioSubTypes.IeeeFloat),
                                new WaveFormatExtensible(waveFormat.SampleRate, 24, 1, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 16, 1, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 8, 1, AudioSubTypes.Pcm)
                            };

                            if (CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
                                throw new NotSupportedException("Could not find a supported format.");
                        }
                    }

                    finalFormat = mixformat;
                }
                else
                    finalFormat = closestMatch;

                //todo: implement channel matrix
                var resampler = new DmoResampler(_source, finalFormat)
                {
                    Quality = 60
                };
                _source = resampler;
                _createdResampler = true;

                return finalFormat;
            }

            return finalFormat;
        }

        bool CheckForSupportedFormat(AudioClient audioClient, IEnumerable<WaveFormatExtensible> waveFormats,
            out WaveFormat foundMatch)
        {
            foundMatch = null;
            foreach (WaveFormatExtensible format in waveFormats)
            {
                if (audioClient.IsFormatSupported(_shareMode, format))
                {
                    foundMatch = format;
                    return true;
                }
            }
            return false;
        }

        bool FeedBuffer(AudioRenderClient renderClient, byte[] buffer, int numFramesCount, int frameSize)
        {
            int count = numFramesCount * frameSize;
            count -= (count % _source.WaveFormat.BlockAlign);
            if (count <= 0)
                return true;

            int read = _source.Read(buffer, 0, count);

            IntPtr ptr = renderClient.GetBuffer(numFramesCount);
            Marshal.Copy(buffer, 0, ptr, read);
            renderClient.ReleaseBuffer(read / frameSize, AudioClientBufferFlags.None);

            return read > 0;
        }

        void RaiseStopped(Exception exception)
        {
            if (Stopped == null)
                return;

            if (_syncContext != null)
                //since Send could cause deadlocks better use Post instead
                _syncContext.Post(x => Stopped(this, new PlaybackStoppedEventArgs(exception)), null);
            else
                Stopped(this, new PlaybackStoppedEventArgs(exception));
        }

        /// <summary>
        /// Disposes and stops the <see cref="WasapiOut"/> instance.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                if (!_disposed)
                {
//                    Debug.WriteLine("Disposing WasapiOut."); // HACK Nope :)
                    Stop();
                    CleanupResources();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor which calls the <see cref="Dispose(bool)"/> method.
        /// </summary>
        ~WasapiOut()
        {
            Dispose(false);
        }
    }
}