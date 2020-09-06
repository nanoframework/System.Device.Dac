//
// Copyright (c) 2019 The nanoFramework project contributors
// Copyright (c) 2020 .NET Foundation. All rights reserved.  
// See LICENSE file in the project root for full license information.
//

namespace System.Devices.Dac
{

    /// <summary>
    /// Exception thrown when a check in <see cref="DacChannel"/> constructor finds that the channel is already in use.
    /// </summary>

    [Serializable]
    public class DacChannelAlreadyInUseException : Exception
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() { return base.Message; }
    }
}
