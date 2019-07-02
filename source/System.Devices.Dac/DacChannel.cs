//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
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
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly object _syncLock;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly int  _channelNumber;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DacController _dacController;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _disposed;

        internal DacChannel(DacController controller, int channelNumber)
        {
            _dacController = controller;
            _channelNumber = channelNumber;

            _syncLock = new object();
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
        public bool WriteValue(double value)
        {
            lock (_syncLock)
            {
                // check if pin has been disposed
                if (_disposed) { throw new ObjectDisposedException(); }

                return NativeWriteValue(value);
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
            if (_dacController != null)
            {
                if (disposing)
                {
                    NativeDisposeChannel();
                    _dacController = null;

                }

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
        private extern bool NativeWriteValue(double value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDisposeChannel();

        #endregion

    }
}
