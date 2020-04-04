// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Alikerroin">
//   Alikerroin, all rights reserved.
// </copyright>
// <summary>
//   View factory is used for getting access to the view components used in this application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Alikerroin.Views
{
    /// <summary>View factory is used for getting access to the view components used in this application. </summary>
    public static class ViewFactory
    {
        /// <summary>The instancing lock used when creating singleton views.</summary>
        private static readonly object InstancingLock = new object();

        /// <summary>Singleton instance of the main view.</summary>
        private static IMainView _mainView;

        /// <summary>Gets the main view.</summary>
        /// <value>The main view.</value>
        public static IMainView MainView
        {
            get
            {
                if (_mainView == null)
                {
                    lock (InstancingLock)
                    {
                        _mainView = _mainView ?? new MainView();
                    }
                }

                return _mainView;
            }
        }
    }
}