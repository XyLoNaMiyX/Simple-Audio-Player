﻿using System;

namespace CSCore.Codecs.FLAC
{
    [Serializable]
    public class FlacException : Exception
    {
        public FlacLayer Layer { get; set; }

        public FlacException(string message, FlacLayer layer)
            : base(message)
        {
            Layer = layer;
        }

        public FlacException(Exception innerexception, FlacLayer layer)
            : base("See innerexception", innerexception)
        {
            Layer = layer;
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Layer", Layer);
        }
    }
}