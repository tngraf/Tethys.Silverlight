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
// <copyright file="CryptSupport.cs" company="Tethys">
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
  using System.Diagnostics.CodeAnalysis;

  /// <summary>
  /// Specifies the block cipher mode to use for encryption.
  /// </summary>
  /// <remarks>Take from MSDN .Net help.</remarks>
  public enum CipherMode
  {
    /// <summary>
    /// Cypher block chaining mode.
    /// </summary>
    [SuppressMessage("Microsoft.Naming",
      "CA1709:IdentifiersShouldBeCasedCorrectly", 
      MessageId = "CBC", Justification = "Ok here.")]
    CBC,

    /// <summary>
    /// Electronic code book mode.
    /// </summary>
    [SuppressMessage("Microsoft.Naming",
      "CA1709:IdentifiersShouldBeCasedCorrectly",
      MessageId = "ECB", Justification = "Ok here.")]
    ECB,

    /// <summary>
    /// Output feedback mode.
    /// </summary>
    [SuppressMessage("Microsoft.Naming",
      "CA1709:IdentifiersShouldBeCasedCorrectly",
      MessageId = "OFB", Justification = "Ok here.")]
    OFB,

    /// <summary>
    /// Cipher feedback mode.
    /// </summary>
    [SuppressMessage("Microsoft.Naming",
      "CA1709:IdentifiersShouldBeCasedCorrectly",
      MessageId = "CFB", Justification = "Ok here.")]
    CFB,

    /// <summary>
    /// Cipher Text Stealing mode.
    /// </summary>
    [SuppressMessage("Microsoft.Naming",
      "CA1709:IdentifiersShouldBeCasedCorrectly",
      MessageId = "CTS", Justification = "Ok here.")]
    CTS
  } // CipherMode

  /// <summary>
  /// Specifies the type of padding to apply when the message data block is 
  /// shorter than the full number of bytes needed for a cryptographic operation.
  /// </summary>
  /// <remarks>Take from MSDN .Net help.</remarks>
  public enum PaddingMode
  {
    /// <summary>
    /// No padding.
    /// </summary>
    None,

    /// <summary>
    /// The PKCS #7 padding string consists of a sequence of bytes, each
    /// of which is equal to the total number of padding bytes added. 
    /// </summary>
    PKCS7,

    /// <summary>
    /// Fill with zeros.
    /// </summary>
    Zeros,

    /// <summary>
    /// The ANSIX923 padding string consists of a sequence of bytes
    /// filled with zeros before the length.
    /// </summary>
    ANSIX923,

    /// <summary>
    /// The ISO10126 padding string consists of random data before the length.
    /// </summary>
    ISO10126
  } // PaddingMode

  /// <summary>
  /// Crypt support class.
  /// </summary>
  public class CryptSupport
  {
  }
} // Tethys.Silverlight.Cryptography