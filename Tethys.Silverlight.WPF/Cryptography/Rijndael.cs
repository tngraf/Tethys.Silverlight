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
            return new Tethys.Silverlight.Cryptography.Aes(this, false, rgbKey, rgbIV);
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
            return new Tethys.Silverlight.Cryptography.Aes(this, true, rgbKey, rgbIV);
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
#endif
} // Tethys.Silverlight.Cryptography