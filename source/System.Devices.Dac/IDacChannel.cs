//
// Copyright (c) 2019 The nanoFramework project contributors
// Copyright (c) 2020 .NET Foundation. All rights reserved.  
// See LICENSE file in the project root for full license information.
//

namespace System.Devices.Dac
{
    internal interface IDacChannel
    {
        void WriteValue(UInt16 value);

        DacController Controller { get; }
    }
}
