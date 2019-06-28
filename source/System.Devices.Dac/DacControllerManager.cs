//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;

namespace System.Devices.Dac
{
    internal sealed class DacControllerManager
    {
        private static object _syncLock;

        // backing field for ControllersCollection
        // to store the controllers that are open
        private static Hashtable s_controllersCollection;

        /// <summary>
        /// <see cref="DacController"/> collection.
        /// </summary>
        /// <remarks>
        /// This collection is for internal use only.
        /// </remarks>
        internal static Hashtable ControllersCollection
        {
            get
            {
                if (s_controllersCollection == null)
                {
                    if (_syncLock == null)
                    {
                        _syncLock = new object();
                    }

                    lock (_syncLock)
                    {
                        if (s_controllersCollection == null)
                        {
                            s_controllersCollection = new Hashtable();
                        }
                    }
                }

                return s_controllersCollection;
            }

            set
            {
                s_controllersCollection = value;
            }
        }
    }
}
