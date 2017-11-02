using CSCore.SoundOut;
using CSCore.SoundOut.MMInterop;
using System.Runtime.InteropServices;

namespace CSCore.SoundIn
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct WaveInCaps
    {
        public static WaveInCaps[] GetCaps()
        {
            uint count = (uint)MMInterops.waveInGetNumDevs();
            WaveInCaps[] caps = new WaveInCaps[count];
            for (uint i = 0; i < count; i++)
            {
                WaveInCaps c;
                var result = MMInterops.waveInGetDevCaps(i, out c, (uint)Marshal.SizeOf(typeof(WaveInCaps)));
                MmException.Try(result, "waveInGetDevCaps");
                caps[i] = c;
            }
            return caps;
        }

        short wMid;
        short wPid;
        uint vDriverVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        string szPname;

        WaveCapsFormats dwFormats;
        short wChannels;
        short wReserved1;

        public int Channels
        {
            get { return wChannels; }
        }

        public WaveCapsFormats Formats
        {
            get { return dwFormats; }
        }

        public string Name
        {
            get { return szPname; }
        }

        public uint DriverVersion
        {
            get { return vDriverVersion; }
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.AppendLine("Name: " + szPname);
            builder.AppendLine("DriverVersion: " + vDriverVersion);
            builder.AppendLine("Formate: " + dwFormats.ToString());

            return builder.ToString();
        }
    }
}