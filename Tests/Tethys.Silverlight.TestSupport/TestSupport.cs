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
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.Silverlight.TestSupport
{
  /// <summary>
  /// The class TestInfrastructure implements unit tests for the
  /// infrastructure layer of the application.
  /// </summary>
  public class TestSupport
  {
    /// <summary>
    /// Compares two array and returns true if they are different.
    /// </summary>
    /// <param name="data1">first array</param>
    /// <param name="data2">second array</param>
    /// <param name="count">number of elements to compare</param>
    /// <returns><c>true</c> if both arrays are different; otherwise
    /// <c>false</c>.</returns>
    public static bool ArraysAreDifferent(byte[] data1, byte[] data2, int count) 
    {
      bool different = false;

      for (int i = 0; i < count; i++)
      {
        if (data1[i] != data2[i]) 
        {
          different = true;
          break;
        } // if
      } // for
      
      return different;
    } // ArraysAreDifferent()
  } // TestSupport
} // Tethys.Silverlight.TestSupport

// ==============================
