//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Devices.Dac
{
    internal interface IDacChannel
    {
        bool ReadValue(double value);

        DacController Controller { get; }
    }
}
