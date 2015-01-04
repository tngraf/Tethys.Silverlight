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
// <copyright file="AESTest.cs" company="Tethys">
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
  using System.Security.Cryptography;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  /// <summary>
  /// Unit tests for <see cref="Cryptography.Rijndael"/> class.
  /// </summary>
  [TestClass]
  public class AESTest
  {
    /// <summary>
    /// A test for <see cref="AesManaged"/>.
    /// </summary>
    [TestMethod]
    public void RijndaelSingleBlockTest()
    {
      var key = new byte[]
      {
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 
        0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f
      };
      var iv = new byte[]
      {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 
      };
      var plaindata = new byte[]
      {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 
      };
      var cryptdata = new byte[]
      {
        0xc6, 0xa1, 0x3b, 0x37, 0x87, 0x8f, 0x5b, 0x82, 
        0x6f, 0x4f, 0x81, 0x62, 0xa1, 0xc8, 0xd8, 0x79 
      };

      SymmetricAlgorithm sa = new AesManaged();
      sa.Mode = CipherMode.CBC;
      sa.Padding = PaddingMode.PKCS7;
      ICryptoTransform trans = sa.CreateEncryptor(key, iv);
      byte[] tempdata = trans.TransformFinalBlock(plaindata, 0, plaindata.Length);

      bool ok = !Silverlight.TestSupport.TestSupport.ArraysAreDifferent(tempdata, cryptdata, cryptdata.Length);
      Assert.IsTrue(ok);

      ICryptoTransform transBack = sa.CreateDecryptor(key, iv);
      byte[] tempdata2 = transBack.TransformFinalBlock(tempdata, 0, tempdata.Length);
      ok = !Silverlight.TestSupport.TestSupport.ArraysAreDifferent(tempdata2, plaindata, plaindata.Length);
      Assert.IsTrue(ok);
    }

    /// <summary>
    /// A test for <see cref="AesManaged"/>.
    /// </summary>
    [TestMethod]
    public void RijndaelSmallTest()
    {
      var key = new byte[]
      { 
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
        0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f
      };
      var iv = new byte[]
      {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 
      };
      var plaindata = new byte[]
      {
        0x41, 0x42, 0x43
      };
      var cryptdata = new byte[]
      {
        0xac, 0xd4, 0xd6, 0x98, 0xf0, 0x96, 0x5b, 0xc9, 
        0xfd, 0xdd, 0x70, 0xba, 0x1f, 0x99, 0xdf, 0x28
      };

      SymmetricAlgorithm sa = new AesManaged();
      sa.Mode = CipherMode.CBC;
      sa.Padding = PaddingMode.PKCS7;
      ICryptoTransform trans = sa.CreateEncryptor(key, iv);
      byte[] tempdata = trans.TransformFinalBlock(plaindata, 0, plaindata.Length);

      bool ok = !Silverlight.TestSupport.TestSupport.ArraysAreDifferent(tempdata, cryptdata, cryptdata.Length);
      Assert.IsTrue(ok);

      ICryptoTransform transBack = sa.CreateDecryptor(key, iv);
      byte[] tempdata2 = transBack.TransformFinalBlock(tempdata, 0, tempdata.Length);
      ok = !Silverlight.TestSupport.TestSupport.ArraysAreDifferent(tempdata2, plaindata, plaindata.Length);
      Assert.IsTrue(ok);
    }

    /// <summary>
    /// A test for <see cref="AesManaged"/>.
    /// </summary>
    [TestMethod]
    public void RijndaelLargeTest()
    {
      var key = new byte[]
      {
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 
        0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f
      };
      var iv = new byte[]
      {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 
      };
      var plaindata = new byte[]
      {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
      };
      var cryptdata = new byte[]
      {
        0xc6, 0xa1, 0x3b, 0x37, 0x87, 0x8f, 0x5b, 0x82, 
        0x6f, 0x4f, 0x81, 0x62, 0xa1, 0xc8, 0xd8, 0x79, 
        0x6e, 0xa9, 0x4b, 0xd1, 0xca, 0x5c, 0xe1, 0x22, 
        0x1b, 0x88, 0x17, 0x1e, 0xb4, 0x46, 0xf5, 0x14
      };

      SymmetricAlgorithm sa = new AesManaged();
      sa.Mode = CipherMode.CBC;
      sa.Padding = PaddingMode.PKCS7;
      ICryptoTransform trans = sa.CreateEncryptor(key, iv);
      byte[] tempdata = trans.TransformFinalBlock(plaindata, 0, plaindata.Length);

      bool ok = !Silverlight.TestSupport.TestSupport.ArraysAreDifferent(tempdata, cryptdata, cryptdata.Length);
      Assert.IsTrue(ok);

      ICryptoTransform transBack = sa.CreateDecryptor(key, iv);
      byte[] tempdata2 = transBack.TransformFinalBlock(tempdata, 0, tempdata.Length);
      ok = !Silverlight.TestSupport.TestSupport.ArraysAreDifferent(tempdata2, plaindata, plaindata.Length);
      Assert.IsTrue(ok);
    }

    /// <summary>
    /// A test for <see cref="AesManaged"/>.
    /// </summary>
    [TestMethod]
    public void RijndaelLargeTest2()
    {
      var key = new byte[]
      {
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 
        0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f
      };
      var iv = new byte[]
      {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 
      };
      var plaindata = new byte[]
      {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
      };
      var cryptdata = new byte[]
      {
        0xc6, 0xa1, 0x3b, 0x37, 0x87, 0x8f, 0x5b, 0x82, 
        0x6f, 0x4f, 0x81, 0x62, 0xa1, 0xc8, 0xd8, 0x79, 
        0xaf, 0x9d, 0x99, 0x26, 0xf7, 0xda, 0xc8, 0x71,
        0x92, 0xb1, 0xc4, 0x14, 0x3a, 0xd9, 0x89, 0x58,
        0x81, 0xf9, 0x3d, 0xe6, 0x17, 0xac, 0xc4, 0x7f,
        0x8c, 0xa9, 0x68, 0x24, 0x2c, 0xee, 0x1d, 0x99
      };

      SymmetricAlgorithm sa = new AesManaged();
      sa.Mode = CipherMode.CBC;
      sa.Padding = PaddingMode.PKCS7;
      ICryptoTransform trans = sa.CreateEncryptor(key, iv);
      byte[] tempdata = trans.TransformFinalBlock(plaindata, 0, plaindata.Length);

      bool ok = !Silverlight.TestSupport.TestSupport.ArraysAreDifferent(tempdata, cryptdata, cryptdata.Length);
      Assert.IsTrue(ok);

      ICryptoTransform transBack = sa.CreateDecryptor(key, iv);
      byte[] tempdata2 = transBack.TransformFinalBlock(tempdata, 0, tempdata.Length);
      ok = !Silverlight.TestSupport.TestSupport.ArraysAreDifferent(tempdata2, plaindata, plaindata.Length);
      Assert.IsTrue(ok);
    }
  } // AESTest
} // Tethys.WP8.Test
