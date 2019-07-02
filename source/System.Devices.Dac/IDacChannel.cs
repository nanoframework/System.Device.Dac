//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Devices.Dac
{
    internal interface IDacChannel
    {
        bool WriteValue(double value);

        DacController Controller { get; }
    }
}
