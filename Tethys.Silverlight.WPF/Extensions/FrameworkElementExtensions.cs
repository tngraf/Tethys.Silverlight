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
// The idea for this code has been taken from the Apex framework 
// (http://apex.codeplex.com/), written by David Kerr and license by an
// MIT style license.
//
// ===========================================================================
//
// <copyright file="FrameworkElementExtensions.cs" company="Tethys">
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

namespace Tethys.Silverlight.Extensions
{
  using System.Windows;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;

  /// <summary>
  /// Extension for the <see cref="FrameworkElement"/> class.
  /// </summary>
  public static class FrameworkElementExtensions
  {
#if !SILVERLIGHT
    /// <summary>
    /// Get the window container of framework element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>A window.</returns>
    public static Window GetParentWindow(this FrameworkElement element)
    {
      DependencyObject dp = element;
      while (dp != null)
      {
        DependencyObject tp = LogicalTreeHelper.GetParent(dp);
        if (tp is Window)
        {
          return tp as Window;
        } // if
        
        dp = tp;
      } // while
      return null;
    } // GetParentWindow()
#endif

    /// <summary>
    /// Gets the top level parent.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="FrameworkElement"/>.</returns>
    public static FrameworkElement GetTopLevelParent(
      this FrameworkElement element)
    {
      FrameworkElement p = element;
      while (p != null)
      {
        if (p.Parent == null)
        {
          return p;
        } // if
        p = p.Parent as FrameworkElement;
      } // while
      return null;
    } // GetTopLevelParent()

    /// <summary>
    /// Renders the bitmap.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="BitmapSource"/>.</returns>
    public static BitmapSource RenderBitmap(this FrameworkElement element)
    {
#if SILVERLIGHT
      //  We'll use the writable bitmap.
      WriteableBitmap wb = new WriteableBitmap((int)element.ActualWidth, 
      (int)element.ActualHeight);
      wb.Render(element, new TranslateTransform());
      wb.Invalidate();
      return wb;
#else
      // We're in WPF, so use the render bitmap.
      
      // Create a visual brush from the element.
      VisualBrush elementBrush = new VisualBrush(element);

      // Create a visual.
      DrawingVisual visual = new DrawingVisual();

      // Open the visual to get a drawing context.
      DrawingContext dc = visual.RenderOpen();

      // Draw the element in the appropriately sized rectangle.
      dc.DrawRectangle(elementBrush, null, new Rect(0, 0, 
        element.ActualWidth, element.ActualHeight));

      // Close the drawing context.
      dc.Close();
      
      // WPF uses 96 DPI - this is defined in System.Windows.SystemParameters.DPI
      //  but it is internal, so we must use a magic number.
      const double SystemDpi = 96;
      
      // Create the bitmap and render it.
      RenderTargetBitmap bitmap = new RenderTargetBitmap(
        (int)element.ActualWidth, (int)element.ActualHeight, SystemDpi, 
        SystemDpi, PixelFormats.Default);
      bitmap.Render(visual);

      // Return the bitmap.
      return bitmap;
#endif
    } // RenderBitmap
  } // FrameworkElementExtensions
} // Tethys.Silverlight.Extensions
