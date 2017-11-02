using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionDisplayNameChangedEventArgs
    /// </summary>
    public class AudioSessionDisplayNameChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// Gets the new display name the session.
        /// </summary>
        public string NewDisplayName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionDisplayNameChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newDisplayName">Thew new display name of the session.</param>
        /// <param name="eventContext">The event context value.</param>
        public AudioSessionDisplayNameChangedEventArgs(string newDisplayName, Guid eventContext)
            : base(eventContext)
        {
            NewDisplayName = newDisplayName;
        }
    }
}