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
// <copyright file="XCRCTest.cs" company="Tethys">
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

  using TestSupport = Tethys.Silverlight.WP8.Test.TestSupport;

  /// <summary>
  /// Unit tests for <see cref="XCRC"/> class.
  /// </summary>
  [TestClass]
  public class XCRCTest
  {
    /// <summary>
    /// Test for <see cref="XCRC"/>.
    /// </summary>
    [TestMethod]
    public void XCrcTest1()
    {
      var hash = new XCRC();

      // test Initialize()
      hash.Initialize();

      // check default settings
      Assert.AreEqual(0xA695, hash.InitValue);
      Assert.AreEqual(0xffff, hash.XorValue);
      Assert.AreEqual(16, hash.HashSize);
      Assert.AreEqual(2, hash.Hash.Length);
      Assert.AreEqual(true, hash.CanReuseTransform);
      Assert.AreEqual(true, hash.CanTransformMultipleBlocks);
      Assert.AreEqual(1, hash.InputBlockSize);
      Assert.AreEqual(1, hash.OutputBlockSize);

      // XCRC ("") = 0000
      var testData = ByteArrayConversion.StringToByteArray(string.Empty);
      var result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      Test.TestSupport.CheckResult(result, "6A59");

      // XCRC ("abc") = 7795
      testData = ByteArrayConversion.StringToByteArray("abc");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      Test.TestSupport.CheckResult(result, "7795");

      // CRC32 ("ABC") = 107D
      testData = ByteArrayConversion.StringToByteArray("ABC");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      Test.TestSupport.CheckResult(result, "107D");

      // XCRC ("CBA") = 443E
      testData = ByteArrayConversion.StringToByteArray("CBA");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      Test.TestSupport.CheckResult(result, "443E");

      // XCRC ("12345678") = 8E37
      testData = ByteArrayConversion.StringToByteArray("12345678");
      result = hash.ComputeHash(testData, 0, testData.Length);
      Assert.AreEqual(2, result.Length);
      Test.TestSupport.CheckResult(result, "8E37");
    }
  } // XCRCTest
} // Tethys.WinRt.Test
