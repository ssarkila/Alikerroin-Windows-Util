// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPresenter.cs" company="Alikerroin">
//   Alikerroin, all rights reserved.
// </copyright>
// <summary>
//   Main presenter of this application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Alikerroin.Presenters
{
    using NLog;

    using Views;

    /// <summary>Main presenter of this application.</summary>
    public class MainPresenter
    {
        /// <summary>The logger object.</summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>The instancing lock.</summary>
        private static readonly object InstancingLock = new object();

        /// <summary>The singleton instance of the main presenter.</summary>
        private static MainPresenter _instance;

        /// <summary>
        /// Prevents a default instance of the <see cref="MainPresenter"/> class from being created. Initializes a new instance of the <see cref="MainPresenter"/> class.
        /// </summary>
        private MainPresenter()
        {
        }

        /// <summary>Gets the singleton instance of MainPresenter.</summary>
        /// <value>The instance.</value>
        public static MainPresenter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstancingLock)
                    {
                        _instance = _instance ?? new MainPresenter();
                    }
                }

                return _instance;
            }
        }

        /// <summary>This is a special method that is called when program is started. The last line in
        /// this method opens the main view. The execution returns to this method, when the main view is
        /// closed.
        /// </summary>
        public void Run()
        {
            ViewFactory.MainView.Display();
        }
    }
}