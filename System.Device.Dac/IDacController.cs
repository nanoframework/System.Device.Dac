//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Device.Dac
{
    internal interface IDacController
    {
        int ChannelCount { get; }
        int ResolutionInBits { get; }

        DacChannel OpenChannel(int channelNumber);
    }
}
