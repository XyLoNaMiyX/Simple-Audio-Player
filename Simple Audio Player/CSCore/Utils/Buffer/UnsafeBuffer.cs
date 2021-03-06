﻿using System;
using System.Runtime.InteropServices;

namespace CSCore.Utils.Buffer
{
    //Based on this idea: http://stackoverflow.com/questions/11390645/structlayout-to-convert-byte-array-to-short
    [Obsolete]
    [StructLayout(LayoutKind.Explicit, Pack = 2)]
    public class UnsafeBuffer
    {
        [FieldOffset(0)]
        byte[] _byteBuffer;

        [FieldOffset(0)]
        short[] _shortBuffer;

        [FieldOffset(0)]
        float[] _floatBuffer;

        [FieldOffset(0)]
        int[] _intBuffer;

        public UnsafeBuffer()
        {
        }

        public UnsafeBuffer(byte[] byteBuffer)
        {
            _byteBuffer = byteBuffer;
        }

        public byte[] ByteBuffer
        {
            get { return _byteBuffer; }
            set { _byteBuffer = value; }
        }

        public short[] ShortBuffer
        {
            get { return _shortBuffer; }
            set { _shortBuffer = value; }
        }

        public float[] FloatBuffer
        {
            get { return _floatBuffer; }
            set { _floatBuffer = value; }
        }

        public int[] IntBuffer
        {
            get { return _intBuffer; }
            set { _intBuffer = value; }
        }
    }
}