using System;

namespace CSCore.Codecs
{
    public class ConnectionEstablishedEventArgs : EventArgs
    {
        public Uri Uri { get; set; }

        public bool Success { get; set; }

        public ConnectionEstablishedEventArgs(Uri uri, bool success)
        {
            Uri = uri;
            Success = success;
        }
    }
}