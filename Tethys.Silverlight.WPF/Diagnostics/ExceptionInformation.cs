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
// <copyright file="ExceptionInformation.cs" company="Tethys">
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

namespace Tethys.Silverlight.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;
    using System.Security.Principal;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// Provides some information about an exception.
    /// </summary>
    [Serializable]
    public class ExceptionInformation
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the thread that has thrown the exception.
        /// </summary>
        public Thread Thread { get; set; }

        /// <summary>
        /// Gets or sets the exception for which the information shall get determined.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets the name of the application domain of the exception.
        /// </summary>
        public string AppDomainName { get; private set; }

        /// <summary>
        /// Gets the name of the assembly of the exception.
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Gets the thread identifier of the exception.
        /// </summary>
        public int ThreadId { get; private set; }

        /// <summary>
        /// Gets the thread user of the exception.
        /// </summary>
        public string ThreadUser { get; private set; }

        /// <summary>
        /// Gets the name of the product of the exception.
        /// </summary>
        public string ProductName { get; private set; }

        /// <summary>
        /// Gets the product version of the exception.
        /// </summary>
        public string ProductVersion { get; private set; }

        /// <summary>
        /// Gets the executable path of the exception.
        /// </summary>
        public string ExecutablePath { get; private set; }

        /// <summary>
        /// Gets the name of the company of the exception.
        /// </summary>
        public string CompanyName { get; private set; }

        /// <summary>
        /// Gets the installed operating system of the exception.
        /// </summary>
        public OperatingSystem OperatingSystem { get; private set; }

        /// <summary>
        /// Gets version information of the installed .NET-Framework.
        /// </summary>
        public Version FrameworkVersion { get; private set; }

        /// <summary>
        /// Gets the physical memory of the active process.
        /// </summary>
        public long WorkingSet { get; private set; }
        #endregion // PUBLIC PROPERTIES

        //// ----------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInformation"/>
        ///  class.
        /// </summary>
        public ExceptionInformation()
        {
            this.Thread = Thread.CurrentThread;
        } // ExceptionInformation()

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInformation"/> 
        /// class.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public ExceptionInformation(Exception ex)
            : this(ex, null)
        {
        } // ExceptionInformation()

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInformation"/> 
        /// class.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="threadObject">The thread that has thrown the exception.
        /// </param>
        public ExceptionInformation(Exception ex, Thread threadObject)
            : this()
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            } // if

            this.Exception = ex;
            if (threadObject != null)
            {
                this.Thread = threadObject;
            } // if

            this.Initialize();
        } // ExceptionInformation()
        #endregion // CONSTRUCTION

        //// ----------------------------------------------------------------------

        #region OVERRIDES
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public new string ToString()
        {
            return string.Format("{0}\r\n" +
              "AppDomain: {1}\r\n" +
              "ThreadId {2}\r\n" +
              "ThreadUser: {3}\r\n" +
              "Productname: {4}\r\n" +
              "Productversion: {5}\r\n" +
              "Dateipfad: {6}\r\n" +
              "Betriebssystem: {7}\r\n" +
              ".NET-Framework-Version: {8}\r\n",
              this.Exception,
              this.AppDomainName,
              this.ThreadId,
              this.ThreadUser,
              this.ProductName,
              this.ProductVersion,
              this.ExecutablePath,
              this.OperatingSystem,
              this.FrameworkVersion);
        } // ToString()
        #endregion // OVERRIDES

        //// ----------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>
        /// The method name.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static string GetMethodName(Exception ex)
        {
            string methodName;

            try
            {
                // Erstes StackFrame aus StackTrace ermitteln
                var frame = GetFirstStackFrame(ex);

                // Klassen- und Funktionsnamen aus StackFrame ermitteln
                if (frame != null)
                {
                    var method = frame.GetMethod();
                    methodName = method.Name + "(";

                    // Methoden-Parameter ermitteln
                    var paramInfos = method.GetParameters();
                    var methodParams = string.Empty;

                    foreach (var paramInfo in paramInfos)
                    {
                        methodParams += ", " + paramInfo.ParameterType.Name + " "
                          + paramInfo.Name;
                    } // foreach
                    if (methodParams.Length > 2)
                    {
                        methodName += methodParams.Substring(2);
                    } // if
                    methodName += ")";
                }
                else
                {
                    methodName = GetMethodNameFromStack(ex.StackTrace);
                } // if
            }
            catch
            {
                methodName = string.Empty;
            } // catch

            return methodName;
        } // GetMethodName()

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>
        /// The class name.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static string GetClassName(Exception ex)
        {
            var className = string.Empty;

            try
            {
                // Erstes StackFrame aus StackTrace ermitteln
                var frame = GetFirstStackFrame(ex);

                // Klassennamen aus StackFrame ermitteln
                if (frame != null)
                {
                    var method = frame.GetMethod();
                    if (method.DeclaringType != null)
                    {
                        className = method.DeclaringType.ToString();
                    } // if
                }
                else
                {
                    className = GetClassNameFromStack(ex.StackTrace);
                } // if
            }
            catch
            {
                className = string.Empty;
            } // catch

            return className;
        } // GetClassName()

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>
        /// The filename.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static string GetFileName(Exception ex)
        {
            string fileName;

            try
            {
                // Erstes StackFrame aus StackTrace ermitteln
                var frame = GetFirstStackFrame(ex);

                // Dateinamen aus StackFrame ermitteln
                if (frame != null)
                {
                    fileName = frame.GetFileName();
                    fileName = fileName.Substring(fileName.LastIndexOf("\\", 
                        StringComparison.CurrentCulture) + 1);
                }
                else
                {
                    fileName = GetFileNameFromStack(ex.StackTrace);
                } // if
            }
            catch
            {
                fileName = string.Empty;
            } // catch

            return fileName;
        } // GetFileName()

        /// <summary>
        /// Gets the file column number.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>
        /// The column number.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static int GetFileColumnNumber(Exception ex)
        {
            int column;

            try
            {
                // Erstes StackFrame aus StackTrace ermitteln
                var frame = GetFirstStackFrame(ex);

                // Dateinamen aus StackFrame ermitteln
                column = frame.GetFileColumnNumber();
            }
            catch
            {
                column = 0;
            } // catch

            return column;
        } // GetFileColumnNumber()

        /// <summary>
        /// Gets the file line number.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>
        /// The line number.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static int GetFileLineNumber(Exception ex)
        {
            int line;

            try
            {
                // Erstes StackFrame aus StackTrace ermitteln
                var frame = GetFirstStackFrame(ex);

                // Dateinamen aus StackFrame ermitteln
                if (frame != null)
                {
                    line = frame.GetFileLineNumber();
                }
                else
                {
                    line = Convert.ToInt32(GetLineNumberFromStack(ex.StackTrace), 
                        CultureInfo.CurrentCulture);
                } // if
            }
            catch
            {
                line = 0;
            } // catch

            return line;
        } // GetFileLineNumber()

        /// <summary>
        /// Gets the IL offset.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>
        /// The offset.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static int GetIlOffset(Exception ex)
        {
            int ilOffset;

            try
            {
                // Erstes StackFrame aus StackTrace ermitteln
                var frame = GetFirstStackFrame(ex);

                // Dateinamen aus StackFrame ermitteln
                ilOffset = frame.GetILOffset();
            }
            catch
            {
                ilOffset = 0;
            } // catch

            return ilOffset;
        } // GetIlOffset()

        /// <summary>
        /// Gets the native offset.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>The native offset.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static int GetNativeOffset(Exception ex)
        {
            int nativeOffset;

            try
            {
                // Erstes StackFrame aus StackTrace ermitteln
                var frame = GetFirstStackFrame(ex);

                // Dateinamen aus StackFrame ermitteln
                nativeOffset = frame.GetNativeOffset();
            }
            catch
            {
                nativeOffset = 0;
            } // catch

            return nativeOffset;
        } // GetNativeOffset()

        /// <summary>
        /// Gets the stack trace for the specified exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>The stack trace as string.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static string GetStackTrace(Exception ex)
        {
            string stackTrace;

            try
            {
                stackTrace = ex.StackTrace ?? string.Empty;
            }
            catch
            {
                stackTrace = string.Empty;
            }

            return stackTrace;
        } // GetStackTrace()

        /// <summary>
        /// Gets the class name from stack.
        /// </summary>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>The class name from stack.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static string GetClassNameFromStack(string stackTrace)
        {
            var className = string.Empty;

            try
            {
                var regClass = new Regex(@" at ([_a-zA-Z0-9\.]+\({1})");
                var matchClass = regClass.Match(stackTrace);
                if (matchClass != null)
                {
                    className = matchClass.Value.Replace(" at ", string.Empty);
                    className = className.Substring(0, className.LastIndexOf(".")).Trim();
                } // if
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // ignore
            } // catch

            return className;
        } // GetClassNameFromStack()

        /// <summary>
        /// Gets the method name from stack.
        /// </summary>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>The method name from stack.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static string GetMethodNameFromStack(string stackTrace)
        {
            var methodName = string.Empty;
            try
            {
                var regMethod = new Regex(@"\.[_a-zA-Z0-9\, ]+\({1}[_a-zA-Z0-9\, ]+\)");
                var matchMethod = regMethod.Match(stackTrace);
                if (matchMethod != null)
                {
                    methodName = matchMethod.Value.Replace(".", string.Empty).Trim();
                }
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // ignore
            } // catch

            return methodName;
        } // GetMethodNameFromStack()

        /// <summary>
        /// Gets the file name from stack.
        /// </summary>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>The file name from stack.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static string GetFileNameFromStack(string stackTrace)
        {
            var fileName = string.Empty;
            try
            {
                var regFile = new Regex(@"(\\{1}[_a-zA-Z0-9\.]+\:)");
                var matchFile = regFile.Match(stackTrace);
                if (matchFile != null)
                {
                    fileName = matchFile.Value.Trim().Replace("\\", string.Empty)
                        .Replace(":", string.Empty);
                }
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // ignore
            } // catch

            return fileName;
        } // GetFileNameFromStack()

        /// <summary>
        /// Gets the line number from stack.
        /// </summary>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>The line number from stack.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        public static string GetLineNumberFromStack(string stackTrace)
        {
            var lineNumber = string.Empty;
            try
            {
                var regLine = new Regex(@"(\:{1}line [1-9]+)");
                var matchLine = regLine.Match(stackTrace);
                if (matchLine != null)
                {
                    lineNumber = matchLine.Value.Replace(":line", string.Empty).Trim();
                } // if
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // ignore
            } // catch

            return lineNumber;
        } // GetLineNumberFromStack()
        #endregion

        //// ----------------------------------------------------------------------

        #region Private Functions
        /// <summary>
        /// Gets the first stack frame.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>The <see cref="StackFrame"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Ok here.")]
        private static StackFrame GetFirstStackFrame(Exception ex)
        {
            try
            {
                // StackTrace aus Exception erstellen
                var trace = new StackTrace(ex, true);

                // StackFrame aus StackTrace ermitteln
                var frame = trace.GetFrame(0);
                return frame;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            if (this.Exception != null)
            {
                this.AppDomainName = AppDomain.CurrentDomain.FriendlyName;
                this.AssemblyName = GetAssemblyName();
                this.ThreadId = Thread.CurrentThread.ManagedThreadId;
                this.ThreadUser = GetThreadUser();
                this.ProductName = AssemblyInformation.Product;
                this.ProductVersion = AssemblyInformation.Version;
                this.ExecutablePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                this.CompanyName = AssemblyInformation.Company;
                this.OperatingSystem = Environment.OSVersion;
                this.FrameworkVersion = Environment.Version;
                this.WorkingSet = Environment.WorkingSet;
            } // if
        } // Initialize()

        /// <summary>
        /// Gets the thread user.
        /// </summary>
        /// <returns>The thread user.</returns>
        private static string GetThreadUser()
        {
            var id = WindowsIdentity.GetCurrent();
            if (id != null)
            {
                return id.Name;
            } // if

            return string.Empty;
        } // GetThreadUser()

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <returns>The name of the assembly.</returns>
        private static string GetAssemblyName()
        {
            return Assembly.GetEntryAssembly().GetName(true).Name;
        } // GetAssemblyName()
        #endregion
    } // ExceptionInformation
} // Tethys.Silverlight.Diagnostics
