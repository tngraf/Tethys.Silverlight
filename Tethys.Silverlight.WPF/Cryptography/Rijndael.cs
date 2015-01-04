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
// This file contains the implementation of the classes Rijndael and Aes which
// implement the AES encryption algorithm.
//
// The core encryption functions are (c) James McCaffrey
// http://msdn.microsoft.com/msdnmag/issues/03/11/AES/print.asp
// http://msdn.microsoft.com/msdnmag/issues/03/11/AES/default.aspx
// Article title: Keep Your Data Secure with the New Advanced Encryption
// Standard.
// 
// Some of the methods for the support of the ICryptoTransform interface of the
// Aes class are quite similar to the methods of the Mono SymmetricAlgorithm
// class.
//
// ===========================================================================
//
// <copyright file="Rijndael.cs" company="Tethys">
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
    using System.Security.Cryptography;

#if SILVERLIGHT
    /// <summary>
    /// Access class for the Rijndael encryption algorithm (AES).
    /// </summary>
    public class Rijndael : SymmetricAlgorithm
    {
        // ReSharper disable RedundantCast
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Cipher mode used in the symmetric algorithm.
        /// </summary>
        private CipherMode modeValue = CipherMode.CBC;

        /// <summary>
        /// The padding mode.
        /// </summary>
        private PaddingMode paddingValue;
        #endregion // PRIVATE PROPERTIES

        //// -----------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the cipher mode used in the symmetric algorithm.
        /// We support here only ECB and CBC mode.
        /// </summary>
        public CipherMode Mode
        {
            get
            {
                return this.modeValue;
            }

            set
            {
                if (value == CipherMode.CFB)
                {
                    throw new NotSupportedException("CFB is not supported");
                } // if

                if (value == CipherMode.CTS)
                {
                    throw new NotSupportedException("CTS is not supported");
                } // if

                if (value == CipherMode.OFB)
                {
                    throw new NotSupportedException("OFB is not supported");
                } // if

                this.modeValue = value;
            } // set
        } // Mode

        /// <summary>
        /// Gets or sets the padding mode.
        /// </summary>
        public PaddingMode Padding
        {
            get
            {
                return this.paddingValue;
            }

            set
            {
                if (value == PaddingMode.ANSIX923)
                {
                    throw new NotSupportedException("ANSIX923 is not supported");
                } // if

                if (value == PaddingMode.ISO10126)
                {
                    throw new NotSupportedException("ISO10126 is not supported");
                } // if
                this.paddingValue = value;
            }
        }
        #endregion // PUBLIC PROPERTIES

        //// -----------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the Rijndael class.
        /// </summary>
        public Rijndael()
        {
            this.modeValue = CipherMode.CBC;
            this.paddingValue = PaddingMode.PKCS7;

            this.KeySizeValue = 256;
            this.BlockSizeValue = 128;

            this.LegalKeySizesValue = new KeySizes[1];
            this.LegalKeySizesValue[0] = new KeySizes(128, 256, 64);

            this.LegalBlockSizesValue = new KeySizes[1];
            this.LegalBlockSizesValue[0] = new KeySizes(128, 256, 64);
        } // Rijndael()
        #endregion // CONSTRUCTION

        //// -----------------------------------------------------------------------

        #region SYMMETRICALGORITHM METHODS
        /// <summary>
        /// Creates a symmetric Rijndael decryptor object with the specified Key
        /// and initialization vector (IV).
        /// </summary>
        /// <param name="rgbKey">The secret key to be used for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to be used for the symmetric algorithm. </param>
        /// <returns>A <see cref="ICryptoTransform"/> decryptor.</returns>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new Aes(this, false, rgbKey, rgbIV);
        } // CreateDecryptor()

        /// <summary>
        /// Creates a symmetric Rijndael encryptor object with the specified Key and
        /// initialization vector (IV).
        /// </summary>
        /// <param name="rgbKey">The secret key to be used for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to be used for the symmetric algorithm. </param>
        /// <returns>A <see cref="ICryptoTransform"/> encryptor.</returns>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new Aes(this, true, rgbKey, rgbIV);
        } // CreateEncryptor()

        /// <summary>
        /// Generates a random initialization vector (IV) to be used for
        /// the algorithm.
        /// </summary>
        public override void GenerateIV()
        {
            throw new NotSupportedException("Rijndael.GenerateIV()");
        } // GenerateIV()

        /// <summary>
        /// Generates a random Key to be used for the algorithm.
        /// </summary>
        public override void GenerateKey()
        {
            throw new NotSupportedException("Rijndael.GenerateKey()");
        } // GenerateKey()
        #endregion // SYMMETRICALGORITHM METHODS
    } // Rijndael

    // -----------------------------------------------------------------------

    /// <summary>
    /// Advanced Encryption Standard.
    /// </summary>
    internal class Aes : ICryptoTransform
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// Encryption algorithm object.
        /// </summary>
        private readonly Rijndael alg;

        /// <summary>
        /// Flag "encrypt/decrypt".
        /// </summary>
        private readonly bool encrypt = true;

        /// <summary>
        /// Working buffer.
        /// </summary>
        private readonly byte[] workBuff;

        /// <summary>
        /// Output buffer.
        /// </summary>
        private readonly byte[] workout;

        /// <summary>
        /// Block size in bytes.
        /// </summary>
        private readonly int blockSizeBytes;

        /// <summary>
        /// the seed key. size will be 4 * keySize from constructor.
        /// </summary>
        private readonly byte[] key;

        /// <summary>
        /// Feedback buffer for CBC mode.
        /// </summary>
        private byte[] feedback;

        /// <summary>
        /// Feedback buffer for CBC mode.
        /// </summary>
        private byte[] feedback2;

        /// <summary>
        /// The last block.
        /// </summary>
        private bool lastBlock;

        #region ENCRYPTION CORE PROPERTIES
        /// <summary>
        /// block size in 32-bit words.  Always 4 for AES.  (128 bits).
        /// </summary>
        private int nb;

        /// <summary>
        /// key size in 32-bit words.  4, 6, 8.  (128, 192, 256 bits).
        /// </summary>
        private int nk;

        /// <summary>
        /// number of rounds. 10, 12, 14.
        /// </summary>
        private int nr;

        /// <summary>
        /// Substitution box
        /// </summary>
        private byte[,] sbox;

        /// <summary>
        /// inverse Substitution box
        /// </summary>
        private byte[,] iSbox;

        /// <summary>
        /// key schedule array.
        /// </summary>
        private byte[,] w;

        /// <summary>
        /// Round constants.
        /// </summary>
        private byte[,] rcon;

        /// <summary>
        /// State matrix
        /// </summary>
        private byte[,] state;
        #endregion ENCRYPTION CORE PROPERTIES
        #endregion // PRIVATE PROPERTIES

        //// -----------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="Aes"/> class.
        /// </summary>
        /// <param name="symAlg">Algorithm object.</param>
        /// <param name="encrypt">flag encrypt or decrypt</param>
        /// <param name="key">encryption key</param>
        /// <param name="iv">initialization vector</param>
        public Aes(Rijndael symAlg, bool encrypt, byte[] key, byte[] iv)
        {
            this.alg = symAlg;
            this.encrypt = encrypt;

            this.blockSizeBytes = (this.alg.BlockSize >> 3);

            // allocate buffers for CBC mode
            this.feedback = new byte[this.blockSizeBytes];
            Buffer.BlockCopy(iv, 0, this.feedback, 0,
              Math.Min(this.blockSizeBytes, iv.Length));
            this.feedback2 = new byte[this.blockSizeBytes];

            // transform buffers
            this.workBuff = new byte[this.blockSizeBytes];
            this.workout = new byte[this.blockSizeBytes];

            // prepare algorithm
            this.SetNbNkNr(key.Length);

            this.key = new byte[this.nk * 4];  // 16, 24, 32 bytes
            key.CopyTo(this.key, 0);

            this.BuildSbox();
            this.BuildInvSbox();
            this.BuildRcon();
            this.KeyExpansion();  // expand the seed key into a key schedule and store in w
        } // Aes()

        /// <summary>
        /// Finalizes an instance of the <see cref="Aes"/> class.
        /// </summary>
        ~Aes()
        {
            this.Dispose(false);
        } // ~Aes()
        #endregion // CONSTRUCTION

        //// -----------------------------------------------------------------------

        #region ENCRYPTION CORE METHODS (MICROSOFT CODE)
        /// <summary>
        /// Encrypt data (encipher 16-bit input)
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <param name="output">The output data.</param>
        public void Cipher(byte[] input, byte[] output)
        {
            // state = input
            this.state = new byte[4, this.nb];  // always [4,4]
            for (int i = 0; i < (4 * this.nb); ++i)
            {
                this.state[i % 4, i / 4] = input[i];
            } // for

            this.AddRoundKey(0);

            // main round loop
            for (int round = 1; round <= (this.nr - 1); ++round)
            {
                this.SubBytes();
                this.ShiftRows();
                this.MixColumns();
                this.AddRoundKey(round);
            }  // main round loop

            this.SubBytes();
            this.ShiftRows();
            this.AddRoundKey(this.nr);

            // output = state
            for (int i = 0; i < (4 * this.nb); ++i)
            {
                output[i] = this.state[i % 4, i / 4];
            } // for
        }  // Cipher()

        /// <summary>
        /// Decrypt data 8 8decipher 16-bit input9:
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <param name="output">The output data.</param>
        public void InvCipher(byte[] input, byte[] output)
        {
            // state = input
            this.state = new byte[4, this.nb];  // always [4,4]
            for (var i = 0; i < (4 * this.nb); ++i)
            {
                this.state[i % 4, i / 4] = input[i];
            } // for

            this.AddRoundKey(this.nr);

            // main round loop
            for (var round = this.nr - 1; round >= 1; --round)
            {
                this.InvShiftRows();
                this.InvSubBytes();
                this.AddRoundKey(round);
                this.InvMixColumns();
            } // end main round loop for InvCipher

            this.InvShiftRows();
            this.InvSubBytes();
            this.AddRoundKey(0);

            // output = state
            for (var i = 0; i < (4 * this.nb); ++i)
            {
                output[i] = this.state[i % 4, i / 4];
            } // for
        }  // InvCipher()

        /// <summary>
        /// Set number of rounds, etc.
        /// </summary>
        /// <param name="keySize">The key size.</param>
        private void SetNbNkNr(int keySize)
        {
            this.nb = 4;     // block size always = 4 words = 16 bytes = 128 bits for AES

            if (keySize == 16 /*Bits128*/)
            {
                this.nk = 4;   // key size = 4 words = 16 bytes = 128 bits
                this.nr = 10;  // rounds for algorithm = 10
            }
            else if (keySize == 24 /*Bits192*/)
            {
                this.nk = 6;   // 6 words = 24 bytes = 192 bits
                this.nr = 12;
            }
            else if (keySize == 32 /*Bits256*/)
            {
                this.nk = 8;   // 8 words = 32 bytes = 256 bits
                this.nr = 14;
            } // if
        }  // SetNbNkNr()

        /// <summary>
        /// Build the S-Box.
        /// </summary>
        private void BuildSbox()
        {
            this.sbox = new byte[,]
      {  // populate the Sbox matrix
    /* 0     1     2     3     4     5     6     7     8     9     a     b     c     d     e     f */
    /*0*/ { 0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76 },
    /*1*/ { 0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0 },
    /*2*/ { 0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15 },
    /*3*/ { 0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75 },
    /*4*/ { 0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84 },
    /*5*/ { 0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf },
    /*6*/ { 0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8 },
    /*7*/ { 0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2 },
    /*8*/ { 0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73 },
    /*9*/ { 0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb },
    /*a*/ { 0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79 },
    /*b*/ { 0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08 },
    /*c*/ { 0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a },
    /*d*/ { 0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e },
    /*e*/ { 0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf },
    /*f*/ { 0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16 } 
      };
        }  // BuildSbox() 

        /// <summary>
        /// Build the inverted S-Box.
        /// </summary>
        private void BuildInvSbox()
        {
            this.iSbox = new byte[,] 
            {  // populate the iSbox matrix
    /* 0     1     2     3     4     5     6     7     8     9     a     b     c     d     e     f */
    /*0*/ { 0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb },
    /*1*/ { 0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb },
    /*2*/ { 0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e },
    /*3*/ { 0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25 },
    /*4*/ { 0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92 },
    /*5*/ { 0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84 },
    /*6*/ { 0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06 },
    /*7*/ { 0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b },
    /*8*/ { 0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73 },
    /*9*/ { 0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e },
    /*a*/ { 0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b },
    /*b*/ { 0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4 },
    /*c*/ { 0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f },
    /*d*/ { 0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef },
    /*e*/ { 0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61 },
    /*f*/ { 0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d } 
            };
        }  // BuildInvSbox()

        /// <summary>
        /// Build ???
        /// </summary>
        private void BuildRcon()
        {
            this.rcon = new byte[,] 
            {
                { 0x00, 0x00, 0x00, 0x00 },  
                { 0x01, 0x00, 0x00, 0x00 },
                { 0x02, 0x00, 0x00, 0x00 },
                { 0x04, 0x00, 0x00, 0x00 },
                { 0x08, 0x00, 0x00, 0x00 },
                { 0x10, 0x00, 0x00, 0x00 },
                { 0x20, 0x00, 0x00, 0x00 },
                { 0x40, 0x00, 0x00, 0x00 },
                { 0x80, 0x00, 0x00, 0x00 },
                { 0x1b, 0x00, 0x00, 0x00 },
                { 0x36, 0x00, 0x00, 0x00 }
            };
        }  // BuildRcon()

        /// <summary>
        /// ??? Unknown ???
        /// </summary>
        /// <param name="round">Number of rounds.</param>
        private void AddRoundKey(int round)
        {
            for (int r = 0; r < 4; ++r)
            {
                for (int c = 0; c < 4; ++c)
                {
                    this.state[r, c] = (byte)((int)this.state[r, c] ^ (int)this.w[(round * 4) + c, r]);
                } // for
            } // for
        }  // AddRoundKey()

        /// <summary>
        /// ??? Unknown ???
        /// </summary>
        private void SubBytes()
        {
            for (int r = 0; r < 4; ++r)
            {
                for (int c = 0; c < 4; ++c)
                {
                    this.state[r, c] = this.sbox[(this.state[r, c] >> 4), (this.state[r, c] & 0x0f)];
                } // for
            } // for
        }  // SubBytes

        /// <summary>
        /// ??? Unknown ???
        /// </summary>
        private void InvSubBytes()
        {
            for (int r = 0; r < 4; ++r)
            {
                for (int c = 0; c < 4; ++c)
                {
                    this.state[r, c] = this.iSbox[(this.state[r, c] >> 4), (this.state[r, c] & 0x0f)];
                } // for
            } // for
        }  // InvSubBytes

        /// <summary>
        /// ??? Unknown ???
        /// </summary>
        private void ShiftRows()
        {
            var temp = new byte[4, 4];

            // copy State into temp[]
            for (int r = 0; r < 4; ++r)
            {
                for (int c = 0; c < 4; ++c)
                {
                    temp[r, c] = this.state[r, c];
                } // for
            } // for

            // shift temp into State
            for (int r = 1; r < 4; ++r)
            {
                for (int c = 0; c < 4; ++c)
                {
                    this.state[r, c] = temp[r, (c + r) % this.nb];
                } // for
            } // for
        }  // ShiftRows()

        /// <summary>
        /// ??? Unknown ???
        /// </summary>
        private void InvShiftRows()
        {
            var temp = new byte[4, 4];

            // copy State into temp[]
            for (int r = 0; r < 4; ++r)
            {
                for (int c = 0; c < 4; ++c)
                {
                    temp[r, c] = this.state[r, c];
                } // for
            } // for

            // shift temp into State
            for (var r = 1; r < 4; ++r)
            {
                for (var c = 0; c < 4; ++c)
                {
                    this.state[r, (c + r) % this.nb] = temp[r, c];
                } // for
            } // for
        }  // InvShiftRows()

        /// <summary>
        /// Mixes the columns.
        /// </summary>
        private void MixColumns()
        {
            var temp = new byte[4, 4];

            // copy State into temp[]
            for (var r = 0; r < 4; ++r)
            {
                for (var c = 0; c < 4; ++c)
                {
                    temp[r, c] = this.state[r, c];
                } // for
            } // for

            for (var c = 0; c < 4; ++c)
            {
                this.state[0, c] = (byte)((int)Gfmultby02(temp[0, c]) ^ (int)Gfmultby03(temp[1, c]) ^
                                           (int)Gfmultby01(temp[2, c]) ^ (int)Gfmultby01(temp[3, c]));
                this.state[1, c] = (byte)((int)Gfmultby01(temp[0, c]) ^ (int)Gfmultby02(temp[1, c]) ^
                                           (int)Gfmultby03(temp[2, c]) ^ (int)Gfmultby01(temp[3, c]));
                this.state[2, c] = (byte)((int)Gfmultby01(temp[0, c]) ^ (int)Gfmultby01(temp[1, c]) ^
                                           (int)Gfmultby02(temp[2, c]) ^ (int)Gfmultby03(temp[3, c]));
                this.state[3, c] = (byte)((int)Gfmultby03(temp[0, c]) ^ (int)Gfmultby01(temp[1, c]) ^
                                           (int)Gfmultby01(temp[2, c]) ^ (int)Gfmultby02(temp[3, c]));
            } // for
        }  // MixColumns

        /// <summary>
        /// Inverted mix columns method.
        /// </summary>
        private void InvMixColumns()
        {
            var temp = new byte[4, 4];

            // copy State into temp[]
            for (var r = 0; r < 4; ++r)
            {
                for (var c = 0; c < 4; ++c)
                {
                    temp[r, c] = this.state[r, c];
                } // for
            } // for

            for (var c = 0; c < 4; ++c)
            {
                this.state[0, c] = (byte)((int)Gfmultby0E(temp[0, c]) ^ (int)Gfmultby0B(temp[1, c]) ^
                                           (int)Gfmultby0D(temp[2, c]) ^ (int)Gfmultby09(temp[3, c]));
                this.state[1, c] = (byte)((int)Gfmultby09(temp[0, c]) ^ (int)Gfmultby0E(temp[1, c]) ^
                                           (int)Gfmultby0B(temp[2, c]) ^ (int)Gfmultby0D(temp[3, c]));
                this.state[2, c] = (byte)((int)Gfmultby0D(temp[0, c]) ^ (int)Gfmultby09(temp[1, c]) ^
                                           (int)Gfmultby0E(temp[2, c]) ^ (int)Gfmultby0B(temp[3, c]));
                this.state[3, c] = (byte)((int)Gfmultby0B(temp[0, c]) ^ (int)Gfmultby0D(temp[1, c]) ^
                                           (int)Gfmultby09(temp[2, c]) ^ (int)Gfmultby0E(temp[3, c]));
            } // for
        }  // InvMixColumns

        #region BYTE OPERATIONS
        /// <summary>
        /// Multiplication function.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns>The multiplication result.</returns>
        private static byte Gfmultby01(byte b)
        {
            return b;
        } // Gfmultby01()

        /// <summary>
        /// Multiplication function.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns>The multiplication result.</returns>
        private static byte Gfmultby02(byte b)
        {
            if (b < 0x80)
            {
                return (byte)(int)(b << 1);
            } // if
            return (byte)((int)(b << 1) ^ (int)(0x1b));
        }

        /// <summary>
        /// Multiplication function.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns>The multiplication result.</returns>
        private static byte Gfmultby03(byte b)
        {
            return (byte)((int)Gfmultby02(b) ^ (int)b);
        }

        /// <summary>
        /// Multiplication function.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns>The multiplication result.</returns>
        private static byte Gfmultby09(byte b)
        {
            return (byte)((int)Gfmultby02(Gfmultby02(Gfmultby02(b))) ^
                           (int)b);
        }

        /// <summary>
        /// Multiplication function.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns>The multiplication result.</returns>
        private static byte Gfmultby0B(byte b)
        {
            return (byte)((int)Gfmultby02(Gfmultby02(Gfmultby02(b))) ^
                           (int)Gfmultby02(b) ^
                           (int)b);
        }

        /// <summary>
        /// Multiplication function.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns>The multiplication result.</returns>
        private static byte Gfmultby0D(byte b)
        {
            return (byte)((int)Gfmultby02(Gfmultby02(Gfmultby02(b))) ^
                           (int)Gfmultby02(Gfmultby02(b)) ^
                           (int)(b));
        }

        /// <summary>
        /// Multiplication function.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns>The multiplication result.</returns>
        private static byte Gfmultby0E(byte b)
        {
            return (byte)((int)Gfmultby02(Gfmultby02(Gfmultby02(b))) ^
                           (int)Gfmultby02(Gfmultby02(b)) ^
                           (int)Gfmultby02(b));
        }
        #endregion // BYTE OPERATIONS

        /// <summary>
        /// Keys the expansion method.
        /// </summary>
        private void KeyExpansion()
        {
            this.w = new byte[this.nb * (this.nr + 1), 4];  // 4 columns of bytes corresponds to a word

            for (int row = 0; row < this.nk; ++row)
            {
                this.w[row, 0] = this.key[4 * row];
                this.w[row, 1] = this.key[(4 * row) + 1];
                this.w[row, 2] = this.key[(4 * row) + 2];
                this.w[row, 3] = this.key[(4 * row) + 3];
            } // for

            byte[] temp = new byte[4];

            for (int row = this.nk; row < this.nb * (this.nr + 1); ++row)
            {
                temp[0] = this.w[row - 1, 0]; 
                temp[1] = this.w[row - 1, 1];
                temp[2] = this.w[row - 1, 2]; 
                temp[3] = this.w[row - 1, 3];

                if (row % this.nk == 0)
                {
                    temp = this.SubWord(RotWord(temp));

                    temp[0] = (byte)((int)temp[0] ^ (int)this.rcon[row / this.nk, 0]);
                    temp[1] = (byte)((int)temp[1] ^ (int)this.rcon[row / this.nk, 1]);
                    temp[2] = (byte)((int)temp[2] ^ (int)this.rcon[row / this.nk, 2]);
                    temp[3] = (byte)((int)temp[3] ^ (int)this.rcon[row / this.nk, 3]);
                }
                else if (this.nk > 6 && (row % this.nk == 4))
                {
                    temp = this.SubWord(temp);
                } // if

                // w[row] = w[row-Nk] xor temp
                this.w[row, 0] = (byte)((int)this.w[row - this.nk, 0] ^ (int)temp[0]);
                this.w[row, 1] = (byte)((int)this.w[row - this.nk, 1] ^ (int)temp[1]);
                this.w[row, 2] = (byte)((int)this.w[row - this.nk, 2] ^ (int)temp[2]);
                this.w[row, 3] = (byte)((int)this.w[row - this.nk, 3] ^ (int)temp[3]);
            }  // for
        }  // KeyExpansion()

        /// <summary>
        /// ??? Unknown ???
        /// </summary>
        /// <param name="word">The byte array.</param>
        /// <returns>The resulting byte array.</returns>
        private byte[] SubWord(byte[] word)
        {
            byte[] result = new byte[4];
            result[0] = this.sbox[word[0] >> 4, word[0] & 0x0f];
            result[1] = this.sbox[word[1] >> 4, word[1] & 0x0f];
            result[2] = this.sbox[word[2] >> 4, word[2] & 0x0f];
            result[3] = this.sbox[word[3] >> 4, word[3] & 0x0f];
            return result;
        } // SubWord()

        /// <summary>
        /// ??? Unknown ???
        /// </summary>
        /// <param name="word">The byte array.</param>
        /// <returns>The resulting byte array.</returns>
        private static byte[] RotWord(byte[] word)
        {
            byte[] result = new byte[4];
            result[0] = word[1];
            result[1] = word[2];
            result[2] = word[3];
            result[3] = word[0];
            return result;
        } // RotWord()

        /*
      public  void Dump()
        {
          Console.WriteLine("Nb = " + Nb + " Nk = " + Nk + " Nr = " + Nr);
          Console.WriteLine("\nThe key is \n" + DumpKey() );
          Console.WriteLine("\nThe Sbox is \n" + DumpTwoByTwo(Sbox));
          Console.WriteLine("\nThe w array is \n" + DumpTwoByTwo(w));
          Console.WriteLine("\nThe State array is \n" + DumpTwoByTwo(State));
        }

        public string DumpKey()
        {
          string s = "";
          for (int i = 0; i < key.Length; ++i)
            s += key[i].ToString("x2") + " ";
          return s;
        }

        public string DumpTwoByTwo(byte[,] a)
        {
          string s ="";
          for (int r = 0; r < a.GetLength(0); ++r)
          {
            s += "["+r+"]" + " ";
            for (int c = 0; c < a.GetLength(1); ++c)
            {
              s += a[r,c].ToString("x2") + " " ;
            }
            s += "\n";
          }
          return s;
        }
      */
        #endregion // ENCRYPTION CORE METHODS (MICROSOFT CODE)

        //// -----------------------------------------------------------------------

        #region SUPPORT METHODS FOR ICRYPTOTRANSFORM
        /// <summary>
        /// Encrypt using electronic code book (ECB) mode.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <param name="output">The output data.</param>
        protected void ECB(byte[] input, byte[] output)
        {
            if (this.encrypt)
            {
                this.Cipher(input, output);
            }
            else
            {
                this.InvCipher(input, output);
            } // if
        } // ECB()

        /// <summary>
        /// Encrypt using cipher block chaining (CBC) mode.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <param name="output">The output data.</param>
        protected void CBC(byte[] input, byte[] output)
        {
            if (this.encrypt)
            {
                // Xor with previous block
                for (int i = 0; i < this.blockSizeBytes; i++)
                {
                    this.feedback[i] ^= input[i];
                } // for

                // core encryption
                this.ECB(this.feedback, output);
                Buffer.BlockCopy(output, 0, this.feedback, 0, this.blockSizeBytes);
            }
            else
            {
                Buffer.BlockCopy(input, 0, this.feedback2, 0, this.blockSizeBytes);

                // core encryption
                this.ECB(input, output);

                // Xor operation
                for (int i = 0; i < this.blockSizeBytes; i++)
                {
                    output[i] ^= this.feedback[i];
                } // for
                Buffer.BlockCopy(this.feedback2, 0, this.feedback, 0, this.blockSizeBytes);
            } // if
        } // CBC()

        /// <summary>
        /// Transform data according to cipher mode.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <param name="output">The output data.</param>
        protected void Transform(byte[] input, byte[] output)
        {
            switch (this.alg.Mode)
            {
                case CipherMode.ECB:
                    this.ECB(input, output);
                    break;
                case CipherMode.CBC:
                    this.CBC(input, output);
                    break;
                default:
                    throw new NotSupportedException("Not supported CipherMode" + this.alg.Mode);
            } // switch
        } // Transform()

        /// <summary>
        /// Gets a value indicating whether to keep the last block.
        /// Taken from Mono source.
        /// </summary>
        private bool KeepLastBlock
        {
            get
            {
                return ((!this.encrypt) 
                    && (this.alg.Mode != CipherMode.ECB) 
                    && (this.alg.Padding != PaddingMode.None));
            }
        } // KeepLastBlock

        /// <summary>
        /// xxx
        /// Taken from Mono source.
        /// </summary>
        /// <param name="inputBuffer">The input buffer.</param>
        /// <param name="inputOffset">The input offset.</param>
        /// <param name="inputCount">The input length.</param>
        /// <param name="outputBuffer">The output buffer.</param>
        /// <param name="outputOffset">The output offset.</param>
        /// <returns>Number of bytes transformed.</returns>
        private int InternalTransformBlock(byte[] inputBuffer, int inputOffset, 
            int inputCount, byte[] outputBuffer, int outputOffset)
        {
            int offs = inputOffset;
            int full;

            // this way we don't do a modulo every time we're called
            // and we may save a division
            if (inputCount != this.blockSizeBytes)
            {
                if ((inputCount % this.blockSizeBytes) != 0)
                {
                    throw new Exception("Invalid input block size.");
                } // if

                full = inputCount / this.blockSizeBytes;
            }
            else
            {
                full = 1;
            } // if

            if (this.KeepLastBlock)
            {
                full--;
            } // if

            int total = 0;

            if (this.lastBlock)
            {
                this.Transform(this.workBuff, this.workout);
                Buffer.BlockCopy(this.workout, 0, outputBuffer, outputOffset, this.blockSizeBytes);
                outputOffset += this.blockSizeBytes;
                total += this.blockSizeBytes;
                this.lastBlock = false;
            } // if

            for (int i = 0; i < full; i++)
            {
                Buffer.BlockCopy(inputBuffer, offs, this.workBuff, 0, this.blockSizeBytes);
                
                // TODO: CF problem
                this.Transform(this.workBuff, this.workout);
                Buffer.BlockCopy(this.workout, 0, outputBuffer, outputOffset, this.blockSizeBytes);
                offs += this.blockSizeBytes;
                outputOffset += this.blockSizeBytes;
                total += this.blockSizeBytes;
            } // for

            if (this.KeepLastBlock)
            {
                Buffer.BlockCopy(inputBuffer, offs, this.workBuff, 0, this.blockSizeBytes);
                this.lastBlock = true;
            } // if

            return total;
        } // InternalTransformBlock()

        /// <summary>
        /// xxx
        /// Taken from Mono source.
        /// </summary>
        /// <param name="inputBuffer">The input buffer.</param>
        /// <param name="inputOffset">The input offset.</param>
        /// <param name="inputCount">The input count.</param>
        /// <returns>The resulting byte array.</returns>
        private byte[] FinalEncrypt(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            // are there still full block to process ?
            int full = (inputCount / this.blockSizeBytes) * this.blockSizeBytes;
            int rem = inputCount - full;
            int total = full;

            switch (this.alg.Padding)
            {
                case PaddingMode.PKCS7:
                    // we need to add an extra block for padding
                    total += this.blockSizeBytes;
                    break;
                default:
                    if (inputCount == 0)
                    {
                        return new byte[0];
                    } // if
                    if (rem != 0)
                    {
                        if (this.alg.Padding == PaddingMode.None)
                        {
                            throw new Exception("invalid block length");
                        } // if

                        // zero padding the input (by adding a block for the partial data)
                        byte[] paddedInput = new byte[full + this.blockSizeBytes];
                        Buffer.BlockCopy(inputBuffer, inputOffset, paddedInput, 0, inputCount);
                        inputBuffer = paddedInput;
                        inputOffset = 0;
                        inputCount = paddedInput.Length;
                        total = inputCount;
                    } // if

                    break;
            } // switch

            var res = new byte[total];
            var outputOffset = 0;

            // process all blocks except the last (final) block
            while (total > this.blockSizeBytes)
            {
                this.InternalTransformBlock(inputBuffer, inputOffset, 
                    this.blockSizeBytes, res, outputOffset);
                inputOffset += this.blockSizeBytes;
                outputOffset += this.blockSizeBytes;
                total -= this.blockSizeBytes;
            } // while

            // now we only have a single last block to encrypt
            var padding = (byte)(this.blockSizeBytes - rem);
            switch (this.alg.Padding)
            {
                case PaddingMode.PKCS7:
                    // XX 07 07 07 07 07 07 07 (padding length)
                    for (int i = res.Length; --i >= (res.Length - padding);)
                    {
                        res[i] = padding;
                    } // if
                    Buffer.BlockCopy(inputBuffer, inputOffset, res, full, rem);
                    
                    // the last padded block will be transformed in-place
                    this.InternalTransformBlock(res, full, this.blockSizeBytes, res, full);
                    break;
                default:
                    this.InternalTransformBlock(inputBuffer, inputOffset,
                        this.blockSizeBytes, res, outputOffset);
                    break;
            } // switch
            return res;
        } // FinalEncrypt()

        /// <summary>
        /// xxx
        /// Taken from Mono source.
        /// </summary>
        /// <param name="inputBuffer">The input buffer.</param>
        /// <param name="inputOffset">The input offset.</param>
        /// <param name="inputCount">The input count.</param>
        /// <returns>The resulting byte array.</returns>
        private byte[] FinalDecrypt(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if ((inputCount % this.blockSizeBytes) > 0)
            {
                throw new Exception("Invalid input block size.");
            } // if

            var total = inputCount;
            if (this.lastBlock)
            {
                total += this.blockSizeBytes;
            } // if

            var res = new byte[total];
            var outputOffset = 0;

            while (inputCount > 0)
            {
                var len = this.InternalTransformBlock(inputBuffer, inputOffset, this.blockSizeBytes, res, outputOffset);
                inputOffset += this.blockSizeBytes;
                outputOffset += len;
                inputCount -= this.blockSizeBytes;
            } // while

            if (this.lastBlock)
            {
                this.Transform(this.workBuff, this.workout);
                Buffer.BlockCopy(this.workout, 0, res, outputOffset, this.blockSizeBytes);
                this.lastBlock = false;
            } // if

            // total may be 0 (e.g. PaddingMode.None)
            var padding = ((total > 0) ? res[total - 1] : (byte)0);
            switch (this.alg.Padding)
            {
                case PaddingMode.PKCS7:
                    total -= padding;
                    break;
                case PaddingMode.None:  // nothing to do - it's a multiple of block size
                case PaddingMode.Zeros:  // nothing to do - user must unpad himself
                    break;
            } // switch

            // return output without padding
            if (total > 0)
            {
                var data = new byte[total];
                Buffer.BlockCopy(res, 0, data, 0, total);
                
                // zeroize decrypted data (copy with padding)
                Array.Clear(res, 0, res.Length);
                return data;
            } // if
            return new byte[0];
        } // FinalDecrypt()

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both 
        /// managed and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (this.feedback == null)
            {
                // already disposed
                return;
            } // if

            if (disposing)
            {
                // clear buffers and free
                Array.Clear(this.feedback, 0, this.blockSizeBytes);
                this.feedback = null;
                Array.Clear(this.feedback2, 0, this.blockSizeBytes);
                this.feedback2 = null;
            } // if
        } // Dispose()
        #endregion // SUPPORT METHODS FOR ICRYPTOTRANSFORM

        //// -----------------------------------------------------------------------

        #region ICRYPTOTRANSFORM MEMBERS
        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets a value indicating whether the current transform can be reused.
        /// </summary>
        public bool CanReuseTransform
        {
            get { return true; }
        } // CanReuseTransform()

        /// <summary>
        /// Gets a value indicating whether multiple
        /// blocks can be transformed.
        /// </summary>
        public bool CanTransformMultipleBlocks
        {
            get { return true; }
        } // CanTransformMultipleBlocks()

        /// <summary>
        /// Gets the input block size.
        /// </summary>
        public int InputBlockSize
        {
            get { return this.blockSizeBytes; }
        } // InputBlockSize()

        /// <summary>
        /// Gets the output block size.
        /// </summary>
        public int OutputBlockSize
        {
            get { return this.blockSizeBytes; }
        } // OutputBlockSize()
        #endregion // PUBLIC PROPERTIES

        #region PUBLIC METHODS
        /// <summary>
        /// Computes the hash value for the specified region of the input byte array
        /// and copies the resulting hash value to the specified region of the output
        /// byte array. 
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the hash code.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <param name="outputBuffer">The output to which to write the hash code.</param>
        /// <param name="outputOffset">The offset into the output byte array from which to begin writing data.</param>
        /// <returns>The number of bytes written.</returns>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            if (this.feedback == null)
            {
                // already disposed
                throw new ObjectDisposedException("Object is disposed");
            } // if

            // check parameters
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            } // if
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset", "inputOffset < 0");
            } // if
            if (inputCount < 0)
            {
                throw new ArgumentOutOfRangeException("inputCount", "inputCount < 0");
            } // if
            if (outputBuffer == null)
            {
                throw new ArgumentNullException("outputBuffer");
            } // if
            if (outputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("outputOffset", "outputOffset < 0");
            } // if

            return this.InternalTransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
        } // TransformBlock()

        /// <summary>
        /// Computes the hash value for the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the hash code.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <returns>The number of bytes written.</returns>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (this.feedback == null)
            {
                // already disposed
                throw new ObjectDisposedException("Object is disposed");
            } // if

            if (this.encrypt)
            {
                return this.FinalEncrypt(inputBuffer, inputOffset, inputCount);
            } // if
            return this.FinalDecrypt(inputBuffer, inputOffset, inputCount);
        } // TransformFinalBlock()
        #endregion // PUBLIC METHODS
        #endregion // ICRYPTOTRANSFORM MEMBERS

        //// -----------------------------------------------------------------------

        #region IDISPOSABLE MEMBERS
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            
            // no need for finalization
            GC.SuppressFinalize(this);
        } // Dispose()
        #endregion // IDISPOSABLE MEMBERS
        // ReSharper restore RedundantCast
    } // class Aes
#endif
} // Tethys.Silverlight.Cryptography