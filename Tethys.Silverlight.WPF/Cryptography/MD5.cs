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
// <copyright file="MD5.cs" company="Tethys">
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

/* Copyright (C) 1991, RSA Data Security, Inc. All rights reserved.

   License to copy and use this software is granted provided that it
   is identified as the "RSA Data Security, Inc. MD5 Message-Digest
   Algorithm" in all material mentioning or referencing this software
   or this function.

   License is also granted to make and use derivative works provided
   that such works are identified as "derived from the RSA Data
   Security, Inc. MD5 Message-Digest Algorithm" in all material
   mentioning or referencing the derived work.  
                                                                    
   RSA Data Security, Inc. makes no representations concerning either
   the merchantability of this software or the suitability of this
   software for any particular purpose. It is provided "as is"
   without express or implied warranty of any kind.  
                                                                    
   These notices must be retained in any copies of any part of this
   documentation and/or software.  
 */
#endregion

// In the Windows Phone 7 Framework is no support for cryptography
// and thus no class HasAlgorithm. This implementation supports
// both: the .Net Framework and the .Net Compact Framework.
namespace Tethys.Silverlight.Cryptography
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// The class MD5 implements the <c>RSA Data Security, Inc.</c>
    /// MD5 Message Digest Algorithm.
    /// </summary>
    public sealed class MD5 : HashAlgorithm
    {
        // ===================================================================
        // CHECKSUM RESULTS
        // ===================================================================
        // TEST1 is the three character sequence "ABC".
        // TEST2 is the three character sequence "CBA".
        // TEST3 is the eight character sequence of "12345678"
        // TEST4 is the 1024 character sequence of "12345678"
        // repeated 128 times.
        //
        //                   Value  
        //                 --------- 
        // MD5(TEST1) = 90 2F BD D2 B1 DF 0C 4F    70 B4 A5 D2 35 25 E9 32 
        // MD5(TEST2) = A6 1A 40 94 3E 07 25 7E    08 4E A3 F6 2D FD B1 C8       
        // MD5(TEST3) = 25 D5 5A D2 83 AA 40 0A    F4 64 C7 6D 71 3C 07 AD        
        // MD5(TEST4) =  89 F8 B6 EB 65 9B 80 F8    1B 22 A0 4F 27 E3 A8 63      
        // MD5("")    =  d41d8cd98f00b204e9800998ecf8427e
        // MD5("a")   =  0cc175b9c0f1b6a831c399e269772661 
        // MD5("abc") =  900150983cd24fb0d6963f7d28e17f72  
        // MD5("message digest") = f96b697d7cb7938d525a2f31aaf161d0
        // MD5("abcdefghijklmnopqrstuvwxyz") = c3fcd3d76192e4007dfb496cca67e13b
        // ===================================================================

        // ---- Constants for MD5Transform routine. ---

        /// <summary>
        /// The S11 value.
        /// </summary>
        private const int S11 = 7;

        /// <summary>
        /// The S12 value.
        /// </summary>
        private const int S12 = 12;

        /// <summary>
        /// The S13 value.
        /// </summary>
        private const int S13 = 17;

        /// <summary>
        /// The S14 value.
        /// </summary>
        private const int S14 = 22;

        /// <summary>
        /// The S21 value.
        /// </summary>
        private const int S21 = 5;

        /// <summary>
        /// The S22 value.
        /// </summary>
        private const int S22 = 9;

        /// <summary>
        /// The S23 value.
        /// </summary>
        private const int S23 = 14;

        /// <summary>
        /// The S24 value.
        /// </summary>
        private const int S24 = 20;

        /// <summary>
        /// The S31 value.
        /// </summary>
        private const int S31 = 4;

        /// <summary>
        /// The S32 value.
        /// </summary>
        private const int S32 = 11;

        /// <summary>
        /// The S33 value.
        /// </summary>
        private const int S33 = 16;

        /// <summary>
        /// The S34 value.
        /// </summary>
        private const int S34 = 23;

        /// <summary>
        /// The S41 value.
        /// </summary>
        private const int S41 = 6;

        /// <summary>
        /// The S42 value.
        /// </summary>
        private const int S42 = 10;

        /// <summary>
        /// The S43 value.
        /// </summary>
        private const int S43 = 15;

        /// <summary>
        /// The S44 value.
        /// </summary>
        private const int S44 = 21;

        /// <summary>
        /// Padding buffer.
        /// </summary>
        private static readonly byte[] Padding = 
        {
          0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        /// <summary>
        /// Hash size in bits.
        /// </summary>
        private const int HashSizeBits = 128;

        /// <summary>
        /// Hash size in bytes.
        /// </summary>
        private const int HashSizeBytes = 16;

        /// <summary>
        /// MD5 context: state (ABCD).
        /// </summary>
        private readonly UInt32[] ctxState;

        /// <summary>
        /// MD5 context: number of bits, modulo 2^64 (lsb first).
        /// </summary>
        private readonly UInt32[] ctxCount;

        /// <summary>
        /// MD5 context: input buffer.
        /// </summary>
        private readonly byte[] ctxBuffer;

        #region PUBLIC HASH ALGORITHM METHODS

        /// <summary>
        /// Initializes a new instance of the <see cref="MD5"/> class.
        /// </summary>
        public MD5()
        {
            this.HashSizeValue = HashSizeBits;
            this.HashValue = new byte[HashSizeBytes];

            this.ctxState = new UInt32[4];
            this.ctxCount = new UInt32[2];
            this.ctxBuffer = new byte[64];
            this.Initialize();
        } // MD5()

        /// <summary>
        /// MD5 initialization. Begins an MD5 operation, writing a new context.<br/>
        /// </summary>
        public override void Initialize()
        {
            this.ctxCount[0] = 0;
            this.ctxCount[1] = 0;

            // Load magic initialization constants.
            this.ctxState[0] = 0x67452301;
            this.ctxState[1] = 0xefcdab89;
            this.ctxState[2] = 0x98badcfe;
            this.ctxState[3] = 0x10325476;

            for (int i = 0; i < 64; i++)
            {
                this.ctxBuffer[i] = 0;
            } // for
        } // Initialize()
        #endregion // PUBLIC HASH ALGORITHM METHODS

        #region PROTECTED HASH ALGORITHM METHODS

        #region BASIC MD5 FUNCTIONS
        // -----------------------------------
        // F, G, H and I are basic MD5 functions.
        // -----------------------------------

        /// <summary>
        /// Basic MD5 function F().
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <returns>F() return value.</returns>
        private static UInt32 F(UInt32 x, UInt32 y, UInt32 z)
        {
            return (((x) & (y)) | ((~x) & (z)));
        } // F()

        /// <summary>
        /// Basic MD5 function G().
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <returns>G() return value.</returns>
        private static UInt32 G(UInt32 x, UInt32 y, UInt32 z)
        {
            return (((x) & (z)) | ((y) & (~z)));
        } // G()

        /// <summary>
        /// Basic MD5 function H().
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <returns>H() return value.</returns>
        private static UInt32 H(UInt32 x, UInt32 y, UInt32 z)
        {
            return ((x) ^ (y) ^ (z));
        } // H()

        /// <summary>
        /// Basic MD5 function I().
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <returns>
        /// I() return value.
        /// </returns>
        private static UInt32 I(UInt32 x, UInt32 y, UInt32 z)
        {
            return ((y) ^ ((x) | (~z)));
        } // H()
        #endregion // BASIC MD5 FUNCTIONS

        #region MD5 TRANSFORMATIONS
        // FF, GG and HH are transformations for rounds 1, 2, 3 and 4.
        // Rotation is separate from addition to prevent recomputation

        /// <summary>
        /// MD5 transformation FF().
        /// </summary>
        /// <param name="a">The a value.</param>
        /// <param name="b">The b value.</param>
        /// <param name="c">The c value.</param>
        /// <param name="d">The d value.</param>
        /// <param name="x">The x value.</param>
        /// <param name="s">The s value.</param>
        /// <param name="ac">The ac value.</param>
        private static void FF(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d,
          UInt32 x, byte s, UInt32 ac)
        {
            a += F(b, c, d) + x + ac;
            a = HashSupport.RotateLeft(a, s);
            a += b;
        } // FF()

        /// <summary>
        /// MD5 transformation GG().
        /// </summary>
        /// <param name="a">The a value.</param>
        /// <param name="b">The b value.</param>
        /// <param name="c">The c value.</param>
        /// <param name="d">The d value.</param>
        /// <param name="x">The x value.</param>
        /// <param name="s">The s value.</param>
        /// <param name="ac">The ac value.</param>
        private static void GG(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d,
          UInt32 x, byte s, UInt32 ac)
        {
            a += G(b, c, d) + x + ac;
            a = HashSupport.RotateLeft(a, s);
            a += b;
        } // GG()

        /// <summary>
        /// MD5 transformation HH().
        /// </summary>
        /// <param name="a">The a value.</param>
        /// <param name="b">The b value.</param>
        /// <param name="c">The c value.</param>
        /// <param name="d">The d value.</param>
        /// <param name="x">The x value.</param>
        /// <param name="s">The s value.</param>
        /// <param name="ac">The ac value.</param>
        private static void HH(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d,
          UInt32 x, byte s, UInt32 ac)
        {
            a += H(b, c, d) + x + ac;
            a = HashSupport.RotateLeft(a, s);
            a += b;
        } // HH()

        /// <summary>
        /// MD5 transformation II().
        /// </summary>
        /// <param name="a">The a value.</param>
        /// <param name="b">The b value.</param>
        /// <param name="c">The c value.</param>
        /// <param name="d">The d value.</param>
        /// <param name="x">The x value.</param>
        /// <param name="s">The s value.</param>
        /// <param name="ac">The ac value.</param>
        private static void II(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d,
          UInt32 x, byte s, UInt32 ac)
        {
            a += I(b, c, d) + x + ac;
            a = HashSupport.RotateLeft(a, s);
            a += b;
        } // HH()
        #endregion // MD5 TRANSFORMATIONS

        /// <summary>
        /// MD5 block update operation. Continues an MD5 message-digest operation,
        /// processing another message block, and updating the context.
        /// </summary>
        /// <param name="input">The input for which to compute the hash code. </param>
        /// <param name="offset">The offset into the byte array from which to begin using data. </param>
        /// <param name="inputLen">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] input, int offset, int inputLen)
        {
            int i;

            // Compute number of bytes mod 64
            var index = (int)((this.ctxCount[0] >> 3) & 0x3F);

            // Update number of bits
            this.ctxCount[0] += (UInt32)(inputLen << 3);
            if (this.ctxCount[0] < ((UInt32)inputLen << 3))
            {
                this.ctxCount[1]++;
            } // if
            this.ctxCount[1] += ((UInt32)inputLen >> 29);

            var partLen = 64 - index;

            // Transform as many times as possible.
            if (inputLen >= partLen)
            {
                HashSupport.MemCpy(this.ctxBuffer, index, input, 0, partLen);
                MD5Transform(this.ctxState, this.ctxBuffer);

                for (i = partLen; i + 63 < inputLen; i += 64)
                {
                    MD5Transform(this.ctxState, input);
                } // for

                index = 0;
            }
            else
            {
                i = 0;
            } // if

            // Buffer remaining input
            HashSupport.MemCpy(this.ctxBuffer, index, input, i, inputLen - i);
        } // HashCore()

        /// <summary>
        /// MD5 basic transformation. Transforms state and updates checksum
        /// based on block.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="block">The block.</param>
        private static void MD5Transform(UInt32[] state, byte[] block)
        {
            UInt32 a = state[0];
            UInt32 b = state[1];
            UInt32 c = state[2];
            UInt32 d = state[3];
            UInt32[] x = new UInt32[16];

            HashSupport.Decode(x, block, 64);

            // Round 1
            FF(ref a, b, c, d, x[0], S11, 0xd76aa478); /* 1 */
            FF(ref d, a, b, c, x[1], S12, 0xe8c7b756); /* 2 */
            FF(ref c, d, a, b, x[2], S13, 0x242070db); /* 3 */
            FF(ref b, c, d, a, x[3], S14, 0xc1bdceee); /* 4 */
            FF(ref a, b, c, d, x[4], S11, 0xf57c0faf); /* 5 */
            FF(ref d, a, b, c, x[5], S12, 0x4787c62a); /* 6 */
            FF(ref c, d, a, b, x[6], S13, 0xa8304613); /* 7 */
            FF(ref b, c, d, a, x[7], S14, 0xfd469501); /* 8 */
            FF(ref a, b, c, d, x[8], S11, 0x698098d8); /* 9 */
            FF(ref d, a, b, c, x[9], S12, 0x8b44f7af); /* 10 */
            FF(ref c, d, a, b, x[10], S13, 0xffff5bb1); /* 11 */
            FF(ref b, c, d, a, x[11], S14, 0x895cd7be); /* 12 */
            FF(ref a, b, c, d, x[12], S11, 0x6b901122); /* 13 */
            FF(ref d, a, b, c, x[13], S12, 0xfd987193); /* 14 */
            FF(ref c, d, a, b, x[14], S13, 0xa679438e); /* 15 */
            FF(ref b, c, d, a, x[15], S14, 0x49b40821); /* 16 */

            // Round 2
            GG(ref a, b, c, d, x[1], S21, 0xf61e2562); /* 17 */
            GG(ref d, a, b, c, x[6], S22, 0xc040b340); /* 18 */
            GG(ref c, d, a, b, x[11], S23, 0x265e5a51); /* 19 */
            GG(ref b, c, d, a, x[0], S24, 0xe9b6c7aa); /* 20 */
            GG(ref a, b, c, d, x[5], S21, 0xd62f105d); /* 21 */
            GG(ref d, a, b, c, x[10], S22, 0x2441453); /* 22 */
            GG(ref c, d, a, b, x[15], S23, 0xd8a1e681); /* 23 */
            GG(ref b, c, d, a, x[4], S24, 0xe7d3fbc8); /* 24 */
            GG(ref a, b, c, d, x[9], S21, 0x21e1cde6); /* 25 */
            GG(ref d, a, b, c, x[14], S22, 0xc33707d6); /* 26 */
            GG(ref c, d, a, b, x[3], S23, 0xf4d50d87); /* 27 */
            GG(ref b, c, d, a, x[8], S24, 0x455a14ed); /* 28 */
            GG(ref a, b, c, d, x[13], S21, 0xa9e3e905); /* 29 */
            GG(ref d, a, b, c, x[2], S22, 0xfcefa3f8); /* 30 */
            GG(ref c, d, a, b, x[7], S23, 0x676f02d9); /* 31 */
            GG(ref b, c, d, a, x[12], S24, 0x8d2a4c8a); /* 32 */

            // Round 3
            HH(ref a, b, c, d, x[5], S31, 0xfffa3942); /* 33 */
            HH(ref d, a, b, c, x[8], S32, 0x8771f681); /* 34 */
            HH(ref c, d, a, b, x[11], S33, 0x6d9d6122); /* 35 */
            HH(ref b, c, d, a, x[14], S34, 0xfde5380c); /* 36 */
            HH(ref a, b, c, d, x[1], S31, 0xa4beea44); /* 37 */
            HH(ref d, a, b, c, x[4], S32, 0x4bdecfa9); /* 38 */
            HH(ref c, d, a, b, x[7], S33, 0xf6bb4b60); /* 39 */
            HH(ref b, c, d, a, x[10], S34, 0xbebfbc70); /* 40 */
            HH(ref a, b, c, d, x[13], S31, 0x289b7ec6); /* 41 */
            HH(ref d, a, b, c, x[0], S32, 0xeaa127fa); /* 42 */
            HH(ref c, d, a, b, x[3], S33, 0xd4ef3085); /* 43 */
            HH(ref b, c, d, a, x[6], S34, 0x4881d05); /* 44 */
            HH(ref a, b, c, d, x[9], S31, 0xd9d4d039); /* 45 */
            HH(ref d, a, b, c, x[12], S32, 0xe6db99e5); /* 46 */
            HH(ref c, d, a, b, x[15], S33, 0x1fa27cf8); /* 47 */
            HH(ref b, c, d, a, x[2], S34, 0xc4ac5665); /* 48 */

            // Round
            II(ref a, b, c, d, x[0], S41, 0xf4292244); /* 49 */
            II(ref d, a, b, c, x[7], S42, 0x432aff97); /* 50 */
            II(ref c, d, a, b, x[14], S43, 0xab9423a7); /* 51 */
            II(ref b, c, d, a, x[5], S44, 0xfc93a039); /* 52 */
            II(ref a, b, c, d, x[12], S41, 0x655b59c3); /* 53 */
            II(ref d, a, b, c, x[3], S42, 0x8f0ccc92); /* 54 */
            II(ref c, d, a, b, x[10], S43, 0xffeff47d); /* 55 */
            II(ref b, c, d, a, x[1], S44, 0x85845dd1); /* 56 */
            II(ref a, b, c, d, x[8], S41, 0x6fa87e4f); /* 57 */
            II(ref d, a, b, c, x[15], S42, 0xfe2ce6e0); /* 58 */
            II(ref c, d, a, b, x[6], S43, 0xa3014314); /* 59 */
            II(ref b, c, d, a, x[13], S44, 0x4e0811a1); /* 60 */
            II(ref a, b, c, d, x[4], S41, 0xf7537e82); /* 61 */
            II(ref d, a, b, c, x[11], S42, 0xbd3af235); /* 62 */
            II(ref c, d, a, b, x[2], S43, 0x2ad7d2bb); /* 63 */
            II(ref b, c, d, a, x[9], S44, 0xeb86d391); /* 64 */

            state[0] += a;
            state[1] += b;
            state[2] += c;
            state[3] += d;

            // Zeroize sensitive information.
            HashSupport.ZeroMemory(x, x.Length);
        } // MD5Transform()

        /// <summary>
        /// MD5 finalization. Ends an MD5 message-digest operation, writing the
        /// message digest and setting the context to zero.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            var bits = new byte[8];

            // Save number of bits
            HashSupport.Encode(bits, this.ctxCount, 8);

            // Pad out to 56 mod 64.
            var index = (int)((this.ctxCount[0] >> 3) & 0x3f);
            var padLen = (index < 56) ? (56 - index) : (120 - index);
            this.HashCore(Padding, 0, padLen);

            // Append length (before padding)
            this.HashCore(bits, 0, 8);

            // Store state in digest
            HashSupport.Encode(this.HashValue, this.ctxState, 16);

            // Zeroize sensitive information.
            HashSupport.ZeroMemory(this.ctxState, this.ctxState.Length);
            HashSupport.ZeroMemory(this.ctxCount, this.ctxCount.Length);
            HashSupport.MemSet(this.ctxBuffer, 0, this.ctxBuffer.Length);
            return this.HashValue;
        } // HashFinal()
        #endregion // PROTECTED HASH ALGORITHM METHODS
    } // MD5
} // Tethys.Silverlight.Cryptography