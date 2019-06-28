//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections.Generic;

namespace System.Devices.Dac.Provider
{
    public interface IDacProvider
    {
        /// <summary>
        /// Gets the DAC controllers available on the system.
        /// </summary>
        /// <returns>
        /// When this method completes it returns a list of all the available controllers on the system.
        /// </returns>
        IReadOnlyList<IAdcControllerProvider> GetControllers()
    }
}
