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
// (http://apex.codeplex.com/), written by David Kerr and licensed by a
// MIT style license.
//
// ===========================================================================
//
// <copyright file="SafeObservableCollection.cs" company="Tethys">
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

namespace Tethys.Silverlight.MVVM
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Threading;
  using System.Windows;
  using System.Windows.Threading;

  /// <summary>
  /// A safe observable collection class.
  /// </summary>
  /// <typeparam name="T">A type.</typeparam>
  public class SafeObservableCollection<T> : IList<T>, INotifyCollectionChanged
  {
    #region PRIVATE PROPERTIES
    /// <summary>
    /// The internal representation.
    /// </summary>
    private readonly IList<T> collection = new List<T>();

    /// <summary>
    /// The dispatcher.
    /// </summary>
    private readonly Dispatcher dispatcher;

    /// <summary>
    /// Synchronization object.
    /// </summary>
    private readonly ReaderWriterLock sync = new ReaderWriterLock();
    #endregion // PRIVATE PROPERTIES

    //// -----------------------------------------------------------------------

    #region PUBLIC PROPERTIES
    /// <summary>
    /// Occurs when the collection changes.
    /// </summary>
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>The count.</value>
    public int Count
    {
      get
      {
        this.sync.AcquireReaderLock(Timeout.Infinite);
        var result = this.collection.Count;
        this.sync.ReleaseReaderLock();
        return result;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is read only.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
    /// </value>
    public bool IsReadOnly
    {
      get { return this.collection.IsReadOnly; }
    }

    /// <summary>
    /// Gets or sets the <see cref="T" /> at the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>
    /// An object.
    /// </returns>
    public T this[int index]
    {
      get
      {
        this.sync.AcquireReaderLock(Timeout.Infinite);
        var result = this.collection[index];
        this.sync.ReleaseReaderLock();
        return result;
      }

      set
      {
        this.sync.AcquireWriterLock(Timeout.Infinite);
        if (this.collection.Count == 0 || this.collection.Count <= index)
        {
          this.sync.ReleaseWriterLock();
          return;
        } // if

        this.collection[index] = value;
        this.sync.ReleaseWriterLock();
      }
    }
    #endregion // PUBLIC PROPERTIES

    //// -----------------------------------------------------------------------

    #region CONSTRUCTION
    /// <summary>
    /// Initializes a new instance of the
    ///  <see cref="SafeObservableCollection&lt;T&gt;"/> class.
    /// </summary>
    public SafeObservableCollection()
    {
      this.dispatcher = Application.Current.Dispatcher;
    } // SafeObservableCollection()
    #endregion // CONSTRUCTION

    //// -----------------------------------------------------------------------

    #region PUBLIC METHODS
    /// <summary>
    /// Adds the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    public void Add(T item)
    {
      if (Thread.CurrentThread == this.dispatcher.Thread)
      {
        this.DoAdd(item);
      }
      else
      {
        this.dispatcher.BeginInvoke((Action)(() => { this.DoAdd(item); }));
      } // if
    } // Add()

    /// <summary>
    /// Clears this instance.
    /// </summary>
    public void Clear()
    {
      if (Thread.CurrentThread == this.dispatcher.Thread)
      {
        this.DoClear();
      }
      else
      {
        this.dispatcher.BeginInvoke((Action)(() => { this.DoClear(); }));
      } // if
    } // Clear()

    /// <summary>
    /// Determines whether the collection contains the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    /// <c>true</c> if the specified item exists; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(T item)
    {
      this.sync.AcquireReaderLock(Timeout.Infinite);
      var result = this.collection.Contains(item);
      this.sync.ReleaseReaderLock();
      return result;
    } // Contains()

    /// <summary>
    /// Copies the collection to an array.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <param name="arrayIndex">Index of the array.</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
      this.sync.AcquireWriterLock(Timeout.Infinite);
      this.collection.CopyTo(array, arrayIndex);
      this.sync.ReleaseWriterLock();
    } // CopyTo()

    /// <summary>
    /// Removes the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    /// true, if the value has been successfully removed.
    /// </returns>
    public bool Remove(T item)
    {
      if (Thread.CurrentThread == this.dispatcher.Thread)
      {
        return this.DoRemove(item);
      } // if
      var op = this.dispatcher.BeginInvoke(new Func<T, bool>(this.DoRemove), item);
      if (op.Result == null)
      {
        return false;
      } // if
      return (bool)op.Result;
    } // Remove()

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>An enumerator.</returns>
    public IEnumerator<T> GetEnumerator()
    {
      return this.collection.GetEnumerator();
    } // GetEnumerator()

    /// <summary>
    /// Indexes the of.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>The index of the item.</returns>
    public int IndexOf(T item)
    {
      this.sync.AcquireReaderLock(Timeout.Infinite);
      var result = this.collection.IndexOf(item);
      this.sync.ReleaseReaderLock();
      return result;
    } // IndexOf()

    /// <summary>
    /// Inserts the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="item">The item.</param>
    public void Insert(int index, T item)
    {
      if (Thread.CurrentThread == this.dispatcher.Thread)
      {
        this.DoInsert(index, item);
      }
      else
      {
        this.dispatcher.BeginInvoke((Action)(() => { this.DoInsert(index, item); }));
      } // if
    } // Insert()

    /// <summary>
    /// Removes at.
    /// </summary>
    /// <param name="index">The index.</param>
    public void RemoveAt(int index)
    {
      if (Thread.CurrentThread == this.dispatcher.Thread)
      {
        this.DoRemoveAt(index);
      }
      else
      {
        this.dispatcher.BeginInvoke((Action)(() => { this.DoRemoveAt(index); }));
      } // if
    } // RemoveAt()
    #endregion // PUBLIC METHODS

    //// -----------------------------------------------------------------------

    #region PRIVATE METHODS
    /// <summary>
    /// Does the add.
    /// </summary>
    /// <param name="item">The item.</param>
    private void DoAdd(T item)
    {
      this.sync.AcquireWriterLock(Timeout.Infinite);
      this.collection.Add(item);
      if (this.CollectionChanged != null)
      {
        this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
          NotifyCollectionChangedAction.Add, item));
      } // if

      this.sync.ReleaseWriterLock();
    } // DoAdd()

    /// <summary>
    /// Does the clear.
    /// </summary>
    private void DoClear()
    {
      this.sync.AcquireWriterLock(Timeout.Infinite);
      this.collection.Clear();
      if (this.CollectionChanged != null)
      {
        this.CollectionChanged(this,
            new NotifyCollectionChangedEventArgs(
              NotifyCollectionChangedAction.Reset));
      } // if

      this.sync.ReleaseWriterLock();
    } // DoClear()

    /// <summary>
    /// Does the remove.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>true when the item has been removed.</returns>
    private bool DoRemove(T item)
    {
      this.sync.AcquireWriterLock(Timeout.Infinite);
      var index = this.collection.IndexOf(item);
      if (index == -1)
      {
        this.sync.ReleaseWriterLock();
        return false;
      } // if
      var result = this.collection.Remove(item);
      if (result && this.CollectionChanged != null)
      {
        this.CollectionChanged(this, new
            NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction.Reset));
      } // if
      this.sync.ReleaseWriterLock();
      return result;
    } // DoRemove()

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that
    /// can be used to iterate through the collection.
    /// </returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.collection.GetEnumerator();
    }

    /// <summary>
    /// Does the insert.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="item">The item.</param>
    private void DoInsert(int index, T item)
    {
      this.sync.AcquireWriterLock(Timeout.Infinite);
      this.collection.Insert(index, item);
      if (this.CollectionChanged != null)
      {
        this.CollectionChanged(this,
            new NotifyCollectionChangedEventArgs(
              NotifyCollectionChangedAction.Add, item, index));
      } // if
      this.sync.ReleaseWriterLock();
    } // DoInsert()

    /// <summary>
    /// Does the remove at.
    /// </summary>
    /// <param name="index">The index.</param>
    private void DoRemoveAt(int index)
    {
      this.sync.AcquireWriterLock(Timeout.Infinite);
      if (this.collection.Count == 0 || this.collection.Count <= index)
      {
        this.sync.ReleaseWriterLock();
        return;
      } // if
      this.collection.RemoveAt(index);
      if (this.CollectionChanged != null)
      {
        this.CollectionChanged(this,
            new NotifyCollectionChangedEventArgs(
              NotifyCollectionChangedAction.Reset));
      } // if
      this.sync.ReleaseWriterLock();
    } // DoRemoveAt()
    #endregion // PRIVATE METHODS
  } // SafeObservableCollection
} // Tethys.Silverlight.MVVM
