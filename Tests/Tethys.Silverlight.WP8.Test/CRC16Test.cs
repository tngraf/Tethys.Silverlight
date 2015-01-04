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
// <copyright file="CRC16Test.cs" company="Tethys">
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

namespace Tethys.Silverlight.WP8.Test
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tethys.Silverlight.Cryptography;
  using Tethys.Silverlight.TestSupport;

  /// <summary>
  /// Unit tests for <see cref="CRC16"/> class.
  /// </summary>
  [TestClass]
  public class CRC16Test
  {
    /// <summary>
    /// Test for <see cref="CRC16"/>.
    /// </summary>
    [TestMethod]
    public void Crc16Test1()
    {
      var hash = new CRC16();

      // test Initialize()
      hash.Initialize();

      // check default settings
      Assert.AreEqual(0x0000, hash.InitValue);
      Assert.AreEqual(0x0000, hash.XorValue);
      Assert.AreEqual(16, hash.HashSize);
      Assert.AreEqual(2, hash.Hash.Length);
      Assert.AreEqual(true, hash.CanReuseTransform);
      Assert.AreEqual(true, hash.CanTransformMultipleBlocks);
      Assert.AreEqual(1, hash.InputBlockSize);
      Assert.AreEqual(1, hash.OutputBlockSize);

      // CRC16 ("") = 00000000
      var testData = ByteArrayConversion.StringToByteArray(string.Empty);
      var result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      TestSupport.CheckResult(result, "0000");

      // CRC16 ("abc") = 9738
      testData = ByteArrayConversion.StringToByteArray("abc");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      TestSupport.CheckResult(result, "9738");

      // CRC32 ("ABC") = 4521
      testData = ByteArrayConversion.StringToByteArray("ABC");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      TestSupport.CheckResult(result, "4521");

      // CRC16 ("CBA") = 4401
      testData = ByteArrayConversion.StringToByteArray("CBA");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      TestSupport.CheckResult(result, "4401");

      // CRC16 ("12345678") = 3C9D
      testData = ByteArrayConversion.StringToByteArray("12345678");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      TestSupport.CheckResult(result, "3C9D");
    }
  } // CRC16Test
} // Tethys.WinRt.Test
