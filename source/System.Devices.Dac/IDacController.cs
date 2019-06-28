//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Devices.Dac
{
    internal interface IDacController
    {
        bool IsChannelModeSupported(DacChannelMode channelMode);
        DacChannel OpenChannel(int channelNumber);

        int ChannelCount { get; }
        DacChannelMode ChannelMode { get; set; }
        int MaxValue { get; }
        int MinValue { get; }
        int ResolutionInBits { get; }
    }
}
