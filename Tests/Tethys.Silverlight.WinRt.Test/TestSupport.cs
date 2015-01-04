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
// <copyright file="TestSupport.cs" company="Tethys">
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
// Tools .... Microsoft Visual Studio 2012
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.Silverlight.WinRt.Test
{
  using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

  using Tethys.Silverlight.TestSupport;

  /// <summary>
  /// The class TestInfrastructure implements unit tests for the
  /// infrastructure layer of the application.
  /// </summary>
  public class TestSupport
  {
    /// <summary>
    /// Checks the contents of the specified byte array against
    /// the expected result specified as a string containing
    /// hex characters.
    /// "d41d" -&gt; 0xd4, 0x1d.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="resultExpected">The result expected.</param>
    /// <returns><c>true</c> if the specified result matches the expected result;
    /// otherwise <c>false</c>.</returns>
    public static bool CheckResult(byte[] result, string resultExpected) 
    {
      byte[] expected = ByteArrayConversion.HexStringToByteArray(resultExpected);

      Assert.IsTrue(expected.Length <= result.Length);

      // do the comparison
      for (int i = 0; i < expected.Length; i++)
      {
        Assert.AreEqual(expected[i], result[i]);
      } // for

      return true;
    } // CheckResult()
  } // TestSupport
} // Tethys.WinRt.Test

// ==============================
