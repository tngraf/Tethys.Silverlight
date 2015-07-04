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

namespace Tethys.Silverlight.Helper
{
    using System;
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

        /// <summary>
        /// Retrieves information about the specified window. The function also retrieves 
        /// the 32-bit (DWORD) value at the specified offset into the extra window memory. 
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        /// If the function succeeds, the return value is the requested value.
        /// If the function fails, the return value is zero. To get extended error 
        /// information, call <c>GetLastError</c>. 
        /// </returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr window, int index);

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the 
        /// 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="index">The index.</param>
        /// <param name="newLong">The new long value.</param>
        /// <returns>
        /// If the function succeeds, the return value is the requested value.
        /// If the function fails, the return value is zero. To get extended error 
        /// information, call <c>GetLastError</c>.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr window, int index,
            int newLong);

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level
        /// window. These windows are ordered according to their appearance on the 
        /// screen. The topmost window receives the highest rank and is the first 
        /// window in the Z order.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="windowInsertAfter">The window insert after.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// If the function succeeds, the return value is the requested value.
        /// If the function fails, the return value is zero. To get extended error 
        /// information, call <c>GetLastError</c>.
        /// </returns>
        [DllImport("user32.dll")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", 
            MessageId = "x", Justification = "This IS correct")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
            MessageId = "y", Justification = "This IS correct")]
        public static extern bool SetWindowPos(IntPtr window, IntPtr windowInsertAfter,
            int x, int y, int width, int height, uint flags);

        /// <summary>
        /// Sends the specified message to a window or windows. The SendMessage 
        /// function calls the window procedure for the specified window and does 
        /// not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="msg">The message to be sent.</param>
        /// <param name="wParam">The word parameter.</param>
        /// <param name="lParam">The long parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; 
        /// it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
            MessageId = "wParam", Justification = "This is Windows standard")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
            MessageId = "lParam", Justification = "This is Windows standard")]
        public static extern IntPtr SendMessage(IntPtr window, uint msg,
            IntPtr wParam, IntPtr lParam);
    } // NativeMethods
} // Tethys.Silverlight.Helper
