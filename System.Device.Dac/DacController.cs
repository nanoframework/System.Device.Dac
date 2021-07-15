﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Device.Dac
{
    /// <summary>
    /// Represents an DAC controller on the system
    /// </summary>
    public sealed class DacController : IDacController
    {
        // this is used as the lock object 
        // a lock is required because multiple threads can access the DacController
        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private object _syncLock;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly int _controllerId;

        // backing field for DeviceCollection
        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private ArrayList s_deviceCollection;

        /// <summary>
        /// Device collection associated with this <see cref="DacController"/>.
        /// </summary>
        /// <remarks>
        /// This collection is for internal use only.
        /// </remarks>
        internal ArrayList DeviceCollection
        {
            get
            {
                if (s_deviceCollection == null)
                {
                    lock (_syncLock)
                    {
                        if (s_deviceCollection == null)
                        {
                            s_deviceCollection = new ArrayList();
                        }
                    }
                }

                return s_deviceCollection;
            }

            set
            {
                s_deviceCollection = value;
            }
        }

        internal DacController(string dacController)
        {
            // the DAC id is an ASCII string with the format 'DACn'
            // need to grab 'n' from the string and convert that to the integer value from the ASCII code (do this by subtracting 48 from the char value)
            _controllerId = dacController[3] - '0';

            // check if this device is already opened
            var myController = FindController(_controllerId);

            if (myController == null)
            {
                // call native init to allow HAL/PAL inits related with DAC hardware
                // this is also used to check if the requested DAC actually exists
                NativeInit();

                // add controller to collection, with the ID as key 
                // *** just the index number ***
                DacControllerManager.ControllersCollection.Add(this);

                _syncLock = new object();
            }
            else
            {
                // this controller already exists: throw an exception
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// The number of channels available on the ADC controller.
        /// </summary>
        /// <value>
        /// Number of channels.
        /// </value>
        public int ChannelCount {
            get
            {
                return NativeGetChannelCount();
            }
        }

        /// <summary>
        /// Gets the resolution of the controller as number of bits it has. For example, if we have a 10-bit DAC, that means it can detect 1024 (2^10) discrete levels.
        /// </summary>
        /// <value>
        /// The number of bits the DAC controller has.
        /// </value>
        public int ResolutionInBits {
            get
            {
                return NativeGetResolutionInBits();
            }
        }

        /// <summary>
        /// Gets the default DAC controller on the system. 
        /// </summary>
        /// <returns>
        /// The default DAC controller on the system, or null if the system has no DAC controller.
        /// </returns>
        public static DacController GetDefault()
        {
            string controllersAqs = GetDeviceSelector();
            string[] controllers = controllersAqs.Split(',');

            if(controllers.Length > 0)
            {
                // the DAC id is an ASCII string with the format 'DACn'
                // need to grab 'n' from the string and convert that to the integer value from the ASCII code (do this by subtracting 48 from the char value)
                var controllerId = controllers[0][3] - '0';

                //////////////////////////////////////////////////////////////////
                // note that, by design, the default controller is              //
                // the first one in the collection returned from the native end //
                //////////////////////////////////////////////////////////////////

                var myController = FindController(controllerId);
                if (myController != null)
                {
                    // controller is already open
                    return myController;
                }
                else
                {
                    // there is no controller yet, create one
                    return new DacController(controllers[0]);
                }
            }

            // the system has no DAC controller 
            return null;
        }

        /// <summary>
        /// Opens a connection to the specified DAC channel.
        /// </summary>
        /// <param name="channelNumber">
        /// The channel to connect to.
        /// </param>
        /// <returns>
        /// The DAC channel.
        /// </returns>
        public DacChannel OpenChannel(Int32 channelNumber)
        {
            NativeOpenChannel(channelNumber);

            return new DacChannel(this, channelNumber);
        }

        internal static DacController FindController(int index)
        {
            for (int i = 0; i < DacControllerManager.ControllersCollection.Count; i++)
            {
                if (((DacController)DacControllerManager.ControllersCollection[i])._controllerId == index)
                {
                    return (DacController)DacControllerManager.ControllersCollection[i];
                }
            }

            return null;
        }

        #region Native Calls

        /// <summary>
        /// Retrieves an string of all the DAC controllers on the system. 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string GetDeviceSelector();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeOpenChannel(Int32 channelNumber);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeGetChannelCount();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeGetResolutionInBits();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeInit();

        #endregion
    }
}
