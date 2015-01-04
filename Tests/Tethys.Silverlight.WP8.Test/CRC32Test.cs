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
// <copyright file="CRC32Test.cs" company="Tethys">
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
  /// Unit tests for <see cref="CRC32"/> class.
  /// </summary>
  [TestClass]
  public class CRC32Test
  {
    /// <summary>
    /// Test for <see cref="CRC32"/>.
    /// </summary>
    [TestMethod]
    public void Crc32Test1()
    {
      var hash = new CRC32();

      // test Initialize()
      hash.Initialize();

      // check default settings
      Assert.AreEqual(0xffffffff, hash.InitValue);
      Assert.AreEqual(0xffffffff, hash.XorValue);
      Assert.AreEqual(32, hash.HashSize);
      Assert.AreEqual(4, hash.Hash.Length);
      Assert.AreEqual(true, hash.CanReuseTransform);
      Assert.AreEqual(true, hash.CanTransformMultipleBlocks);
      Assert.AreEqual(1, hash.InputBlockSize);
      Assert.AreEqual(1, hash.OutputBlockSize);
      
      // CRC32 ("") = 00000000
      var testData = ByteArrayConversion.StringToByteArray(string.Empty);
      var result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(4, result.Length);
      TestSupport.CheckResult(result, "00000000");

      // CRC32 ("abc") = 352441C2
      testData = ByteArrayConversion.StringToByteArray("abc");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(4, result.Length);
      TestSupport.CheckResult(result, "352441C2");

      // CRC32 ("ABC") = a3830348
      testData = ByteArrayConversion.StringToByteArray("ABC");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(4, result.Length);
      TestSupport.CheckResult(result, "a3830348");

      // CRC32 ("CBA") = 4e09b60a
      testData = ByteArrayConversion.StringToByteArray("CBA");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(4, result.Length);
      TestSupport.CheckResult(result, "4e09b60a");

      // CRC32 ("12345678") = 4e09b60a
      testData = ByteArrayConversion.StringToByteArray("12345678");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(4, result.Length);
      TestSupport.CheckResult(result, "9ae0daaf");
    }
  } // CRC32Test
} // Tethys.WinRt.Test
