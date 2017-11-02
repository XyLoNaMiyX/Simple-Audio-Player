﻿namespace CSCore.SoundOut.DirectSound
{
    /*
    #define DSBPLAY_LOOPING             0x00000001
    #define DSBPLAY_LOCHARDWARE         0x00000002
    #define DSBPLAY_LOCSOFTWARE         0x00000004
    #define DSBPLAY_TERMINATEBY_TIME    0x00000008
    #define DSBPLAY_TERMINATEBY_DISTANCE    0x000000010
    #define DSBPLAY_TERMINATEBY_PRIORITY    0x000000020
     */

    public enum DSBPlayFlags : uint
    {
        None = 0x0,
        DSBPLAY_LOOPING = 0x00000001,
        DSBPLAY_LOCHARDWARE = 0x00000002,
        DSBPLAY_LOCSOFTWARE = 0x00000004,
        DSBPLAY_TERMINATEBY_TIME = 0x00000008,
        DSBPLAY_TERMINATEBY_DISTANCE = 0x000000010,
        DSBPLAY_TERMINATEBY_PRIORITY = 0x000000020
    }
}