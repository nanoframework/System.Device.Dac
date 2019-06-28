//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace System.Devices.Dac.Provider
{
    public interface IDacControllerProvider
    {
        /// <summary>
        /// Gets the number of channels available on for the controller.
        /// </summary>
        /// <value>
        /// Number of channels.
        /// </value>
        int ChannelCount { get; }

        /// <summary>
        /// Gets or sets the controller channel mode.
        /// </summary>
        /// <value>
        /// The channel mode.
        /// </value>
        ProviderDacChannelMode ChannelMode { get; set; }

        /// <summary>
        /// Gets the maximum value that the controller can return.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        int MaxValue { get; }

        /// <summary>
        /// Gets the minimum value that the controller can return.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        int MinValue { get; }

        /// <summary>
        /// Acquires a connection to the specified channel.
        /// </summary>
        /// <param name="channel">
        /// Which channel to connect to.
        /// </param>
        void AcquireChannel(Int32 channel);

        /// <summary>
        /// Determines if the specified channel mode is supported by the controller.
        /// </summary>
        /// <param name="channelMode">
        /// The channel mode in question.
        /// </param>
        /// <returns>
        /// True if the specified channel mode is supported, otherwise false.
        /// </returns>
        bool IsChannelModeSupported(ProviderAdcChannelMode channelMode);

        /// <summary>
        /// Gets the digital representation of the analog value on the specified channel.
        /// </summary>
        /// <param name="channelNumber">
        /// Which channel to read from.
        /// </param>
        /// <returns>
        /// The digital representation of the analog value.
        /// </returns>
        int ReadValue(Int32 channelNumber);

        /// <summary>
        /// Releases the channel connection, opening that channel for others to use.
        /// </summary>
        /// <param name="channel">
        /// Which channel to close the connection to.
        /// </param>
        void ReleaseChannel(Int32 channel)
    }
}
