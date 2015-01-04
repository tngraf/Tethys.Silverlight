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
// <copyright file="HashSupport.cs" company="Tethys">
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

namespace Tethys.Silverlight.Cryptography
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The class HashSupport implements helper functions for Message
    /// Digest Algorithms.
    /// </summary>
    // ReSharper disable RedundantCast
    public static class HashSupport
    {
        #region 32 BIT ROTATION FUNCTIONS
        /// <summary>
        /// RotateLeft rotates x left n bits.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="n">The n.</param>
        /// <returns>The rotation result.</returns>
        public static UInt32 RotateLeft(UInt32 x, byte n)
        {
            return (((x) << (n)) | ((x) >> (32 - (n))));
        } // RotateLeft()

        /// <summary>
        /// RotateRight rotates x right n bits.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="n">The n.</param>
        /// <returns>The rotation result.</returns>
        public static UInt32 RotateRight(UInt32 x, byte n)
        {
            return (((x) >> (n)) | ((x) << (32 - (n))));
        } // RotateRight()
        #endregion // 32 BIT ROTATION FUNCTIONS

        #region MEMCPY FUNCTIONS
        /// <summary>
        /// C-style <c>memcpy()</c> function.
        /// </summary>
        /// <param name="output">output buffer.</param>
        /// <param name="input">input buffer.</param>
        /// <param name="len">number of byte to be copied.</param>
        public static void MemCpy(byte[] output, byte[] input, int len)
        {
            int i;

            for (i = 0; i < len; i++)
            {
                output[i] = input[i];
            } // for
        } // MemCpy()

        /// <summary>
        /// C-style <c>memcpy()</c> function.
        /// </summary>
        /// <param name="output">output buffer.</param>
        /// <param name="offsetOutput">offset within the output buffer.</param>
        /// <param name="input">input buffer.</param>
        /// <param name="offsetInput">offset within the input buffer.</param>
        /// <param name="len">number of byte to be copied.</param>
        public static void MemCpy(byte[] output, int offsetOutput,
          byte[] input, int offsetInput, int len)
        {
            int i;

            for (i = 0; i < len; i++)
            {
                output[offsetOutput + i] = input[offsetInput + i];
            } // for
        } // MemCpy()

        /// <summary>
        /// C-style <c>memcpy()</c> function.
        /// </summary>
        /// <param name="output">output buffer.</param>
        /// <param name="offsetOutput">offset within the output buffer.</param>
        /// <param name="input">input buffer.</param>
        /// <param name="offsetInput">offset within the input buffer.</param>
        /// <param name="len">number of byte to be copied.</param>
        public static void MemCpy(UInt32[] output, int offsetOutput,
          byte[] input, int offsetInput, int len)
        {
            int i, j;

            for (i = 0, j = 0; j < len; i++, j += 4)
            {
                output[offsetOutput + i] = ((UInt32)input[j]) | (((UInt32)input[j + 1]) << 8) |
                  (((UInt32)input[j + 2]) << 16) | (((UInt32)input[j + 3]) << 24);
            } // for
        } // MemCpy()

        /// <summary>
        /// C-style <c>memcpy()</c> function.
        /// </summary>
        /// <param name="output">output buffer.</param>
        /// <param name="offsetOutput">offset within the output buffer
        /// (in bytes, NOT <c>UInt32</c>).</param>
        /// <param name="input">input buffer.</param>
        /// <param name="offsetInput">offset within the input buffer.</param>
        /// <param name="len">number of byte to be copied.</param>
        public static void MemCpyEx(UInt32[] output, int offsetOutput,
          byte[] input, int offsetInput, int len)
        {
            int j;

            for (j = 0; j < len; j++)
            {
                int outputIndex = ((offsetOutput + j) / 4);
                int outputByte = (offsetOutput + j) % 4;
                switch (outputByte)
                {
                    case 0:
                        output[outputIndex] = (UInt32)input[j];
                        break;
                    case 1:
                        output[outputIndex] |= (((UInt32)input[j]) << 8);
                        break;
                    case 2:
                        output[outputIndex] |= (((UInt32)input[j]) << 16);
                        break;
                    case 3:
                        output[outputIndex] |= (((UInt32)input[j]) << 24);
                        break;
                } // if
            } // for (j)
        } // MemCpyEx()

        /// <summary>
        /// C-style <c>memset()</c> function.
        /// </summary>
        /// <param name="output">buffer to be modified.</param>
        /// <param name="value">new buffer value</param>
        /// <param name="len">buffer length</param>
        public static void MemSet(byte[] output, byte value, int len)
        {
            int i;

            for (i = 0; i < len; i++)
            {
                output[i] = value;
            } // for
        } // MemSet()
        #endregion // MEMCPY FUNCTIONS

        #region ZEROMEMORY FUNCTIONS
        /// <summary>
        /// C-style ZeroMemory() function.
        /// </summary>
        /// <param name="output">buffer to be modified.</param>
        /// <param name="len">buffer length</param>
        public static void ZeroMemory(UInt32[] output, int len)
        {
            int i;

            for (i = 0; i < len; i++)
            {
                output[i] = 0;
            } // for
        } // ZeroMemory()

        /// <summary>
        /// C-style ZeroMemory() function.
        /// </summary>
        /// <param name="output">buffer to be modified.</param>
        /// <param name="len">buffer length</param>
        public static void ZeroMemory(byte[] output, int len)
        {
            int i;

            for (i = 0; i < len; i++)
            {
                output[i] = 0;
            } // for
        } // ZeroMemory()
        #endregion // ZEROMEMORY FUNCTIONS

        #region LITTLE ENDIAN / BIG ENDIAN BUFFER FUNCTIONS
        /// <summary>
        /// Encodes input (<c>UInt32[]</c>) into output (<c>byte[]</c>).
        /// Assumes len is a multiple of 4.<br />
        /// Little endian (least significant byte first)
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="input">The input.</param>
        /// <param name="len">The length.</param>
        public static void Encode(byte[] output, UInt32[] input, int len)
        {
            Encode(output, 0, input, len, true);
        } // Encode()

        /// <summary>
        /// Encodes input (<c>UInt32[]</c>) into output (<c>byte[]</c>).
        /// Assumes len is a multiple of 4.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="outputOffset">The output offset.</param>
        /// <param name="input">The input.</param>
        /// <param name="len">The length.</param>
        /// <param name="littleEndian">if set to <c>true</c> [little endian].</param>
        public static void Encode(byte[] output, int outputOffset, UInt32[] input,
          int len, bool littleEndian)
        {
            int i, j;

            Debug.Assert((len % 4) == 0, "invalid length!");

            if (littleEndian)
            {
                // least significant byte first
                for (i = 0, j = 0; j < len; i++, j += 4)
                {
                    output[outputOffset + j] = (byte)(input[i] & 0xff);
                    output[outputOffset + j + 1] = (byte)((input[i] >> 8) & 0xff);
                    output[outputOffset + j + 2] = (byte)((input[i] >> 16) & 0xff);
                    output[outputOffset + j + 3] = (byte)((input[i] >> 24) & 0xff);
                } // for
            }
            else
            {
                // most significant byte first
                for (i = 0, j = 0; j < len; i++, j += 4)
                {
                    output[outputOffset + j] = (byte)((input[i] >> 24) & 0xff);
                    output[outputOffset + j + 1] = (byte)((input[i] >> 16) & 0xff);
                    output[outputOffset + j + 2] = (byte)((input[i] >> 8) & 0xff);
                    output[outputOffset + j + 3] = (byte)(input[i] & 0xff);
                } // for
            } // if
        } // Encode()

        /// <summary>
        /// Encodes input (<c>UInt32[]</c>) into output (<c>byte[]</c>).
        /// Assumes len is a multiple of 4.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="outputOffset">The output offset.</param>
        /// <param name="input">The input.</param>
        /// <param name="len">The length.</param>
        public static void Encode(byte[] output, int outputOffset,
          UInt32[] input, int len)
        {
            Encode(output, outputOffset, input, len, true);
        } // Encode()

        /// <summary>
        /// Decodes input (<c>byte[]</c>) into output (<c>UInt32[]</c>).
        /// Assumes len is a multiple of 4.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="input">The input.</param>
        /// <param name="len">The length.</param>
        public static void Decode(UInt32[] output, byte[] input, int len)
        {
            int i, j;

            for (i = 0, j = 0; j < len; i++, j += 4)
            {
                output[i] = ((UInt32)input[j]) | (((UInt32)input[j + 1]) << 8) |
                  (((UInt32)input[j + 2]) << 16) | (((UInt32)input[j + 3]) << 24);
            } // for
        } // Decode()

        /// <summary>
        /// Decodes input (<c>byte[]</c>) into output (<c>UInt32[]</c>).
        /// Assumes len is a multiple of 4.
        /// <remark>Not yet implemented!</remark>
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="outputOffset">The output offset.</param>
        /// <param name="input">The input.</param>
        /// <param name="len">The length.</param>
        /// <exception cref="System.NotSupportedException">Method not yet supported!</exception>
        public static void Decode(UInt32[] output, int outputOffset,
          byte[] input, int len)
        {
            throw new NotSupportedException("Method not yet supported!");
        } // Decode()

        /// <summary>
        /// Decodes input (<c>byte[]</c>) into output (<c>UInt32[]</c>).
        /// Assumes len is a multiple of 4.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="outputOffset">The output offset.</param>
        /// <param name="input">The input.</param>
        /// <param name="len">The length.</param>
        /// <param name="littleEndian">if set to <c>true</c> [little endian].</param>
        public static void Decode(UInt32[] output, int outputOffset,
          byte[] input, int len, bool littleEndian)
        {
            int i, j;

            Debug.Assert((len % 4) == 0, "Invalid length!");

            if (littleEndian)
            {
                // least significant byte first
                for (i = 0, j = 0; j < len; i++, j += 4)
                {
                    output[outputOffset + i] = ((UInt32)input[j]) | (((UInt32)input[j + 1]) << 8) |
                      (((UInt32)input[j + 2]) << 16) | (((UInt32)input[j + 3]) << 24);
                } // for
            }
            else
            {
                // most significant byte first
                for (i = 0, j = 0; j < len; i++, j += 4)
                {
                    output[outputOffset + i] = ((UInt32)input[j + 3]) | (((UInt32)input[j + 2]) << 8) |
                      (((UInt32)input[j + 1]) << 16) | (((UInt32)input[j]) << 24);
                } // for
            } // for
        } // Decode()
        #endregion // LITTLE ENDIAN / BIG ENDIAN BUFFER FUNCTIONS
    } // HashSupport
    // ReSharper restore RedundantCast
} // Tethys.Silverlight.Cryptography