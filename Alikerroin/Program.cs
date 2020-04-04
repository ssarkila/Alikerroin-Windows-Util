// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Alikerroin">
//   Alikerroin, all rights reserved.
// </copyright>
// <summary>
//   The Main Program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Alikerroin
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using Presenters;
    using NLog;

    static class Program
    {
        /// <summary>The logger instance.</summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            MainPresenter.Instance.Run();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Logger.Error(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error((Exception)e.ExceptionObject);
        }
    }
}
