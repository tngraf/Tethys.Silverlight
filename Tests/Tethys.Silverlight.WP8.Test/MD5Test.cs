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
// <copyright file="MD5Test.cs" company="Tethys">
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
  /// Unit tests for <see cref="MD5"/> class.
  /// </summary>
  [TestClass]
  public class MD5Test
  {
    /// <summary>
    /// Test for <see cref="CRC16"/>.
    /// </summary>
    [TestMethod]
    public void TestMD5()
    {
      var hash = new MD5();

      // test Initialize()
      hash.Initialize();

      // check default settings
      Assert.AreEqual(0x80, hash.HashSize);
      Assert.AreEqual(true, hash.CanReuseTransform);
      Assert.AreEqual(true, hash.CanTransformMultipleBlocks);
      Assert.AreEqual(1, hash.InputBlockSize);
      Assert.AreEqual(1, hash.OutputBlockSize);

      // use test values from original RFC1321:
      // MD5 ("") = d41d8cd98f00b204e9800998ecf8427e
      var testData = new byte[0];
      var result = hash.ComputeHash(testData);
      TestSupport.CheckResult(result, "d41d8cd98f00b204e9800998ecf8427e");

      // MD5 ("a") = 0cc175b9c0f1b6a831c399e269772661
      testData = ByteArrayConversion.StringToByteArray("a");
      result = hash.ComputeHash(testData);
      TestSupport.CheckResult(result, "0cc175b9c0f1b6a831c399e269772661");

      // MD5 ("abc") = 900150983cd24fb0d6963f7d28e17f72
      testData = ByteArrayConversion.StringToByteArray("abc");
      result = hash.ComputeHash(testData);
      TestSupport.CheckResult(result, "900150983cd24fb0d6963f7d28e17f72");

      // MD5 ("message digest") = f96b697d7cb7938d525a2f31aaf161d0
      testData = ByteArrayConversion.StringToByteArray("message digest");
      result = hash.ComputeHash(testData);
      TestSupport.CheckResult(result, "f96b697d7cb7938d525a2f31aaf161d0");

      // MD5 ("abcdefghijklmnopqrstuvwxyz") = c3fcd3d76192e4007dfb496cca67e13b
      testData = ByteArrayConversion.StringToByteArray("abcdefghijklmnopqrstuvwxyz");
      result = hash.ComputeHash(testData);
      TestSupport.CheckResult(result, "c3fcd3d76192e4007dfb496cca67e13b");

      // MD5 ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789") = d174ab98d277d9f5a5611c2c9f419d9f
      testData = ByteArrayConversion.StringToByteArray("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
      result = hash.ComputeHash(testData);
      TestSupport.CheckResult(result, "d174ab98d277d9f5a5611c2c9f419d9f");
    } // TestMD5()
  } // MD5Test
} // Tethys.WP8.Test
