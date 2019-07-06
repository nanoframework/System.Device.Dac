//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Runtime.CompilerServices;

namespace System.Devices.Dac
{
    /// <summary>
    /// Represents a single DAC channel.
    /// </summary>
    public sealed class DacChannel : IDacChannel, IDisposable
    {
        // this is used as the lock object 
        // a lock is required because multiple threads can access the channel
        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly object _syncLock;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly int  _channelNumber;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private DacController _dacController;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private bool _disposed;

        internal DacChannel(DacController controller, int channelNumber)
        {
            // check if this channel ID is already in the collection
            if (!controller.DeviceCollection.Contains(channelNumber))
            {
                // device doesn't exist, create it...

                _dacController = controller;
                _channelNumber = channelNumber;

                // ... and add this device
                controller.DeviceCollection.Add(channelNumber, this);

                _syncLock = new object();
            }
            else
            {
                // this channel is already in use, throw an exception
                throw new DacChannelAlreadyInUseException();
            }
        }

        /// <summary>
        /// Gets the DAC controller for this channel.
        /// </summary>
        /// <value>
        /// The DAC controller.
        /// </value>
       public DacController Controller
       {
            get
            {
                return _dacController;
            }
        }

        /// <summary>
        /// Writes the analogue representation of the digital value to the DAC.
        /// </summary>
        /// <returns>
        /// The success or failure.
        /// </returns>
        public void WriteValue(UInt16 value)
        {
            lock (_syncLock)
            {
                // check if pin has been disposed
                if (_disposed) { throw new ObjectDisposedException(); }

                NativeWriteValue(value);
            }
        }

        private void ThowIfDisposed()
        {
            // check if pin has been disposed
            if (_disposed)
            {
                throw new ObjectDisposedException();
            }
        }

        #region IDisposable Support

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                bool disposeController = false;

                if (disposing)
                {
                    // get the controller Id
                    // it's enough to divide by the device unique id multiplier as we'll get the thousands digit, which is the controller ID
                    var controller = (DacController)DacControllerManager.ControllersCollection[_dacController._controllerId];

                    // remove from device collection
                    controller.DeviceCollection.Remove(_channelNumber);

                    // it's OK to also remove the controller, if there is no other device associated
                    if (controller.DeviceCollection.Count == 0)
                    {
                        DacControllerManager.ControllersCollection.Remove(controller);

                        controller = null;

                        // flag this to native dispose
                        disposeController = true;
                    }

                }

                NativeDispose(disposeController);

                _disposed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (_syncLock)
            {
                if (!_disposed)
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }
        }

        #pragma warning disable 1591
        ~DacChannel()
        {
            Dispose(false);
        }
        #pragma warning restore 1591

        #endregion

        #region Native Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeWriteValue(UInt16 value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDispose(bool disposeController);

        #endregion

    }
}
