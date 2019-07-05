//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Devices.Dac
{
    internal interface IDacChannel
    {
        bool WriteValue(UInt16 value);

        DacController Controller { get; }
    }
}
