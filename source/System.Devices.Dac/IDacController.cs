﻿//
// Copyright (c) 2019 The nanoFramework project contributors
// Copyright (c) 2020 .NET Foundation. All rights reserved.  
// See LICENSE file in the project root for full license information.
//

namespace System.Devices.Dac
{
    internal interface IDacController
    {
        int ChannelCount { get; }
        int ResolutionInBits { get; }

        DacChannel OpenChannel(int channelNumber);
    }
}
