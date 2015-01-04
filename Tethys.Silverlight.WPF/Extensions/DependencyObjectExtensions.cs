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
// (http://apex.codeplex.com/), written by David Kerr and licensed by an
// MIT style license.
//
// ===========================================================================
//
// <copyright file="DependencyObjectExtensions.cs" company="Tethys">
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
#if !SILVERLIGHT
  // ReSharper disable RedundantUsingDirective
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Windows;
  using System.Windows.Data;
  using System.Windows.Media;

  // ReSharper restore RedundantUsingDirective
#endif

  /// <summary>
  /// A set of useful extensions for the DependencyObject class.
  /// </summary>
  public static class DependencyObjectExtensions
  {
    /// <summary>
    /// Gets the first parent found of a specific type.
    /// </summary>
    /// <typeparam name="T">The type of parent to find.</typeparam>
    /// <param name="child">The child.</param>
    /// <returns>The first parent found of type 'T' or null if no parent 
    /// of type 'T' is found.</returns>
    public static T GetParent<T>(this DependencyObject child) 
      where T : DependencyObject
    {
      // Get the visual parent.
      DependencyObject dependencyObject = VisualTreeHelper.GetParent(child);

      // If we've got the parent, return it if it is the correct type - otherwise
      //  continue up the tree.
      if (dependencyObject != null)
      {
        return dependencyObject is T 
          ? dependencyObject as T : GetParent<T>(dependencyObject);
      } // if
      return null;
    } // GetParent()

    /// <summary>
    /// Gets the top level parent.
    /// </summary>
    /// <param name="child">The child.</param>
    /// <returns>The <see cref="DependencyObject"/>.</returns>
    public static DependencyObject GetTopLevelParent(
      this DependencyObject child)
    {
      DependencyObject tmp = child;
      DependencyObject parent = null;
      while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
      {
        parent = tmp;
      } // while
      return parent;
    } // GetTopLevelParent()

    /// <summary>
    /// Gets all children of a specified type, through the visual tree.
    /// This is a recursive function.
    /// </summary>
    /// <typeparam name="T">The type of child to get.</typeparam>
    /// <param name="me">The dependency object to get children of.</param>
    /// <returns>All children of type T of the dependency object.</returns>
    public static IEnumerable<T> GetVisualChildren<T>(this DependencyObject me) 
      where T : DependencyObject
    {
      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(me); i++)
      {
        var child = VisualTreeHelper.GetChild(me, i);
        if (child != null && child is T)
        {
          yield return (T)child;
        } // if

        foreach (T childOfChild in child.GetVisualChildren<T>())
        {
          yield return childOfChild;
        } // foreach
      } // for
    } // GetVisualChildren()

    /// <summary>
    /// Gets all children of a specified type, through the logical tree.
    /// This is a recursive function.
    /// </summary>
    /// <typeparam name="T">The type of child to get.</typeparam>
    /// <param name="me">The dependency object to get children of.</param>
    /// <returns>All children of type T of the dependency object.</returns>
    public static IEnumerable<T> GetLogicalChildren<T>(this DependencyObject me)
      where T : DependencyObject
    {
#if SILVERLIGHT
      foreach (var child in Apex.Consistency.LogicalTreeHelper.GetChildren(me))
#else
      foreach (var child in LogicalTreeHelper.GetChildren(me))
#endif
      {
        // If the child is not a dependency object, we can't use it.
        var childDependencyObject = child as DependencyObject;
        if (childDependencyObject == null)
        {
          continue;
        } // if

        if (childDependencyObject is T)
        {
          yield return (T)childDependencyObject;
        } // if

        foreach (T childOfChild in childDependencyObject.GetLogicalChildren<T>())
        {
          yield return childOfChild;
        } // foreach
      } // foreach
    } // GetLogicalChildren()

    /// <summary>
    /// Finds a child element of a specified type with a specified name.
    /// </summary>
    /// <typeparam name="T">The type of child element to find.</typeparam>
    /// <param name="me">The dependency object.</param>
    /// <param name="childName">Name of the child.</param>
    /// <returns>The first child of type T with the specified name, or null of no
    /// children are found.</returns>
    public static T FindChild<T>(this DependencyObject me, string childName) 
      where T : DependencyObject
    {
      // Confirm parent and childName are valid. 
      if (me == null)
      {
        return null;
      } // if

      T foundChild = null;

      int childrenCount = VisualTreeHelper.GetChildrenCount(me);
      for (int i = 0; i < childrenCount; i++)
      {
        var child = VisualTreeHelper.GetChild(me, i);
        
        // If the child is not of the request child type child
        T childType = child as T;
        if (childType == null)
        {
          // recursively drill down the tree
          foundChild = FindChild<T>(child, childName);

          // If the child is found, break so we do not overwrite the found child. 
          if (foundChild != null)
          {
            break;
          } // if
        }
        else if (!string.IsNullOrEmpty(childName))
        {
          var frameworkElement = child as FrameworkElement;
          
          // If the child's name is set for search
          if (frameworkElement != null && frameworkElement.Name == childName)
          {
            // if the child's name is of the request name
            foundChild = (T)child;
            break;
          } // if
        }
        else
        {
          // child element found.
          foundChild = (T)child;
          break;
        } // if
      } // for

      return foundChild;
    } // FindChild()

#if !SILVERLIGHT
    /// <summary>
    /// Gets the binding objects.
    /// </summary>
    /// <param name="me">The dependency object.</param>
    /// <returns>All bindings for the dependency object.</returns>
    public static IEnumerable<Binding> GetBindingObjects(
      this DependencyObject me)
    {
      // Get the dependency properties.
      var properties = GetDependencyProperties(me);

      // Select all of the non-null bindings.
      return properties.Select(dp => BindingOperations.GetBinding(me, dp))
        .Where(b => b != null);
    } // GetBindingObjects()

    /// <summary>
    /// Gets the dependency properties of a dependency object.
    /// </summary>
    /// <param name="me">The dependency object.</param>
    /// <returns>The dependency properties of a dependency object.</returns>
    public static IEnumerable<DependencyProperty> GetDependencyProperties(
      this DependencyObject me)
    {
      // Get appropriate properties.
      var properties = TypeDescriptor.GetProperties(me,
        new Attribute[] 
        { 
          new PropertyFilterAttribute(
          PropertyFilterOptions.All) 
        });

      // Return all non null dependency properties.
      return from PropertyDescriptor pd in properties
             select DependencyPropertyDescriptor.FromProperty(pd) into dpd
             where dpd != null
             select dpd.DependencyProperty;
    } // GetDependencyProperties()
#endif

#if SILVERLIGHT
    /// <summary>
    /// Retrieves all the logical children of a framework element using a 
    /// depth-first search.  A visual element is assumed to be a logical 
    /// child of another visual element if they are in the same namescope.
    /// For performance reasons this method manually manages the stack 
    /// instead of using recursion.
    /// </summary>
    /// <param name="parent">The parent framework element.</param>
    /// <returns>The logical children of the framework element.</returns>
    internal static IEnumerable<FrameworkElement> GetLogicalChildren(
     FrameworkElement parent)
    {
      EnsureName(parent);

      string parentName = parent.Name;
      Stack<FrameworkElement> stack =
          new Stack<FrameworkElement>(parent.GetVisualChildren<FrameworkElement>());

      while (stack.Count > 0)
      {
        FrameworkElement element = stack.Pop();
        if (element.FindName(parentName) == parent)
        {
            yield return element;
        }
        else
        {
          foreach (FrameworkElement visualChild in element.GetVisualChildren<FrameworkElement>())
          {
              stack.Push(visualChild);
          } // foreach
        } // if
      } // while
    } // GetLogicalChildren()

    internal static void EnsureName(FrameworkElement parent)
    {
      if (string.IsNullOrEmpty(parent.Name))
          parent.Name = Guid.NewGuid().ToString();
    } // EnsureName()
#endif
  } // DependencyObjectExtensions
} // Tethys.Silverlight.Extensions
