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
// <copyright file="WindowResizedBehavior.cs" company="Tethys">
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
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    /// A behavior to handle the 'minimized' event via a command.
    /// </summary>
    public class WindowResizedBehavior : Behavior<Window>
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// Dependency property for the command.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
          DependencyProperty.Register("Command",
          typeof(ICommand), typeof(WindowResizedBehavior));

        /// <summary>
        /// Dependency property for the command parameter.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
              DependencyProperty.Register("CommandParameter",
              typeof(object), typeof(WindowResizedBehavior));

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }
        #endregion // PUBLIC PROPERTIES

        /// <summary>
        /// Called when the behavior is attached.
        /// </summary>
        protected override void OnAttached()
        {
            this.AssociatedObject.SizeChanged += this.OnSizeChanged;
            base.OnAttached();
        } // OnAttached()

        /// <summary>
        /// Handles the size changed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SizeChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            var command = this.Command;
            if (command != null)
            {
                command.Execute(args);
            } // if
        } // OnSizeChanged()
    } // WindowResizedBehavior
} // Tethys.Silverlight.Behavior
