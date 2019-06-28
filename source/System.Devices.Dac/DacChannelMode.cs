//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Devices.Dac
{
    /// <summary>
    /// Describes the channel modes that the DAC controller can use for output.
    /// </summary>
    public enum DacChannelMode
    {
        /// <summary>
        /// Simple value of a particular pin.
        /// </summary>
        SingleEnded = 0,
        /// <summary>
        /// Difference between two pins.
        /// </summary>
        Differential
    }
}
