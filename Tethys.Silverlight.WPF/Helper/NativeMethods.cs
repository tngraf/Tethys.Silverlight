#region Header
// --------------------------------------------------------------------------
// Tethys.Silverlight
// ==========================================================================
//
// This library contains common code for WPF, Silverlight, Windows Phone and
// Windows 8 projects.
//
// ===========================================================================
//
// <copyright file="NativeMethods.cs" company="Tethys">
// Copyright  2010-2015 by Thomas Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
//
// System ... Microsoft .Net Framework 4.5
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

using System;

namespace Tethys.Silverlight.Helper
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides access to native Win32 methods.
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// The style flag value.
        /// </summary>
        public const int Style = -16;

        /// <summary>
        /// The ext style flag value.
        /// </summary>
        public const int ExtStyle = -20;

        /// <summary>
        /// The minimize box flag value.
        /// </summary>
        public const int MinimizeBox = 0x20000;

        /// <summary>
        /// The maximize box flag value.
        /// </summary>
        public const int MaximizeBox = 0x10000;

        /// <summary>
        /// The context help flag value.
        /// </summary>
        public const int ContextHelp = 0x400;

        /// <summary>
        /// The dialog modal frame flag value.
        /// </summary>
        public const int DialogModalFrame = 0x0001;

        /// <summary>
        /// The no size flag value.
        /// </summary>
        public const int NoSize = 0x0001;

        /// <summary>
        /// The no move flag value.
        /// </summary>
        public const int NoMove = 0x0002;

        /// <summary>
        /// The no z order flag value.
        /// </summary>
        public const int NoZOrder = 0x0004;

        /// <summary>
        /// The frame changed flag value.
        /// </summary>
        public const int SwpFramechanged = 0x0020;

        /// <summary>
        /// The set icon flag value.
        /// </summary>
        public const uint Seticon = 0x0080;

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr window, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr window, int index,
            int newLong);

        [DllImport("user32.dll")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
            MessageId = "x", Justification = "This IS correct")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
            MessageId = "y", Justification = "This IS correct")]
        public static extern bool SetWindowPos(IntPtr window, IntPtr windowInsertAfter,
            int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
            MessageId = "wParam", Justification = "This is Windows standard")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
            MessageId = "lParam", Justification = "This is Windows standard")]
        public static extern IntPtr SendMessage(IntPtr window, uint msg,
            IntPtr wParam, IntPtr lParam);
    } // NativeMethods
} // Tethys.Silverlight.Helper
