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
// <copyright file="UpdateSourceOnTextChangedBehavior.cs" company="Tethys">
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

namespace Tethys.Silverlight.Behavior
{
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Interactivity;

    /// <summary>
    /// Behavior to update the source of a binding for a textbox directly when the
    /// content of the text box has changed (and not only on FocusLost which is the 
    /// default).<para />
    /// This is especially important for Windows Phone where we do not have 
    /// <code>UpdateSourceTrigger=PropertyChanged</code>.
    /// </summary>
    public class UpdateSourceOnTextChangedBehavior : Behavior<TextBox>
    {
        #region PROTECTED METHODS
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.TextChanged += this.OnTextChanged;
        } // OnAttached()

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, 
        /// but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.TextChanged -= this.OnTextChanged;
        } // OnDetaching()
        #endregion // PROTECTED METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Called when the text in the textbox has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing 
        /// the event data.</param>
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression be =
              this.AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
        } // OnTextChanged()
        #endregion // PRIVATE METHODS
    } // UpdateSourceOnTextChangedBehavior
} // Tethys.Silverlight.Behavior
