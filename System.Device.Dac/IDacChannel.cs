//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Device.Dac
{
    internal interface IDacChannel
    {
        void WriteValue(UInt16 value);

        DacController Controller { get; }
    }
}
