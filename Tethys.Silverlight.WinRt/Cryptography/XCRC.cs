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
// <copyright file="XCRC.cs" company="Tethys">
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
  using System.IO;
  using System.Runtime.InteropServices.WindowsRuntime;

  using Windows.Storage.Streams;

  /// <summary>
  /// The class XCRC implements a fast CRC-16 checksum algorithm,
  /// that uses the following polynomial:<br/>
  /// <code>
  /// Polynom = x**16 + x**15 + x**10 + x**8 + x**7
  /// + x**5 + x**3 + 1
  /// (= 0x85A5)
  /// </code>
  /// Default initial value = 0xA695,<br/>
  /// default final XOR value = 0xFFFF.
  /// </summary>
  public sealed class XCRC
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
    //                 byte[0]  byte[1]  
    //                 -------  -------  
    // X-CRC(TEST1) =    0x10     0x7D
    // X-CRC(TEST2) =    0x44     0x3E
    // X-CRC(TEST3) =    0x8E     0x37
    // X-CRC(TEST4) =    0x75     0x75
    // ===================================================================

    /// <summary>
    /// Constants used to compute the CRC-16 checksum.
    /// </summary>
    private static readonly ushort[] Tab;

    /// <summary>
    /// Hash algorithm name.
    /// </summary>
    private const string AlgorithmNameXcrc = "XCRC";

    /// <summary>
    /// Hash size in bytes.
    /// </summary>
    private const ushort HashSizeBytesXcrc = 2;

    /// <summary>
    /// Default initial value.
    /// </summary>
    public const ushort DefaultInit = 0xA695;

    /// <summary>
    /// Default final XOR value.
    /// </summary>
    public const ushort DefaultXor = 0xFFFF;

    /// <summary>
    /// Initial value.
    /// </summary>
    private ushort initValue;

    /// <summary>
    /// Final XOR value.
    /// </summary>
    private ushort xorValue;

    /// <summary>
    /// 16 bit CRC value.
    /// </summary>
    private ushort crc;

    #region PUBLIC HASH ALGORITHM METHODS
    /// <summary>
    /// Gets or sets the initial value.
    /// </summary>
    public ushort InitValue
    {
      get
      {
        return this.initValue;
      }

      set
      {
        this.initValue = value;
        this.Initialize();
      }
    } // InitValue

    /// <summary>
    /// Gets or sets the final XOR value.
    /// </summary>
    public ushort XorValue
    {
      get { return this.xorValue; }
      set { this.xorValue = value; }
    } // XorValue

    /// <summary>
    /// Gets the name of the open hash algorithm.
    /// </summary>
    public string AlgorithmName
    {
      get
      {
        return AlgorithmNameXcrc;
      }
    } // AlgorithmName

    /// <summary>
    /// Gets the length, in bytes, of the hash.
    /// </summary>
    public int HashLength
    {
      get
      {
        return HashSizeBytesXcrc;
      }
    } // HashLength

    /// <summary>
    /// Initializes static members of the <see cref="XCRC"/> class.
    /// </summary>
    static XCRC()
    {
      Tab = BuildTable();
    } // XCRC()

    /// <summary>
    /// Initializes the CRC table.
    /// </summary>
    /// <returns>The CRC table.</returns>
    private static ushort[] BuildTable()
    {
      ushort[] tab = new ushort[256];
      const ushort G = 0x85A5;
      int i;

      for (i = 0; i < 256; i++)
      {
        ushort help;
        int j;
        for (help = (ushort)(i << 8), j = 0; j < 8; j++)
        {
          if ((help & 0x8000) > 0)
          {
            help = (ushort)((help << 1) ^ G);
          }
          else
          {
            help = (ushort)(help << 1);
          } // if
        } // for (help)
        tab[i] = help;
      } // for (i)

      return tab;
    } // BuildTable()

    /// <summary>
    /// Initializes a new instance of the <see cref="XCRC"/> class.
    /// </summary>
    public XCRC()
    {
      this.initValue = DefaultInit;
      this.xorValue = DefaultXor;
      this.Initialize();
    } // XCRC()

    /// <summary>
    /// Initializes an implementation of HashAlgorithm.
    /// </summary>
    public void Initialize()
    {
      this.crc = this.initValue;
    } // Initialize()

    /// <summary>
    /// Hashes the data.
    /// </summary>
    /// <param name="data">Data to be hashed.</param>
    /// <returns>Hashed data.</returns>
    public byte[] HashData(IBuffer data)
    {
      if (data == null)
      {
        throw new ArgumentNullException("data");
      } // if

      this.Initialize();

      // cast IBuffer to something we can use
      Stream stream = data.AsStream();

      for (int o = 0; o < data.Length; o++)
      {
        // calculate CRC for next byte
        // using table with 256 entries
        this.crc = (ushort)((this.crc << 8) ^ Tab[(this.crc >> 8) ^ stream.ReadByte()]);
      } // for

      return this.HashFinal();
    } // HashData()

    /// <summary>
    /// Hashes the data.
    /// </summary>
    /// <param name="buffer">The input for which to compute the hash code. </param>
    /// <param name="offset">The offset into the byte array from which to begin using data. </param>
    /// <param name="count">The number of bytes in the byte array to use as data.</param>
    /// <returns>Hashed data.</returns>
    public byte[] HashData(byte[] buffer, int offset, int count)
    {
      if (buffer == null)
      {
        throw new ArgumentNullException("buffer");
      } // if

      this.Initialize();

      this.HashCore(buffer, offset, count);

      return this.HashFinal();
    } // HashData()
    #endregion // PUBLIC HASH ALGORITHM METHODS

    #region PROTECTED HASH ALGORITHM METHODS
    /// <summary>
    /// Routes data written to the object into the hash
    /// algorithm for computing the hash.<br/>
    /// This function calculates the CRC-16-checksum via the specified partial block.
    /// The CRC-16 value of the previous calculation is updated the specified block.<br/>
    /// </summary>
    /// <param name="buffer">The input for which to compute the hash code. </param>
    /// <param name="offset">The offset into the byte array from which to begin using data. </param>
    /// <param name="count">The number of bytes in the byte array to use as data.</param>
    private void HashCore(byte[] buffer, int offset, int count)
    {
      for (; count != 0; count--, offset++)
      {
        // calculate CRC for next byte
        // using table with 256 entries
        this.crc = (ushort)((this.crc << 8) ^ Tab[(this.crc >> 8) ^ buffer[offset]]);
      } // for
    } // HashCore()

    /// <summary>
    /// Finalizes the hash computation after the last data is processed
    /// by the cryptographic stream object.
    /// </summary>
    /// <returns>The computed hash code.</returns>
    private byte[] HashFinal()
    {
      ushort crcRet = (ushort)(this.crc ^ this.xorValue);

      // save new calculated value
      byte[] hashValue = new byte[HashSizeBytesXcrc];
      hashValue[1] = (byte)((crcRet & 0xff00) >> 8);
      hashValue[0] = (byte)(crcRet & 0x00ff);

      return hashValue;
    } // HashFinal()
    #endregion // PROTECTED HASH ALGORITHM METHODS
  } // XCRC
} // Tethys.Silverlight.Cryptography