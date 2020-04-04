// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsFactory.cs" company="Alikerroin">
//   Alikerroin, all rights reserved.
// </copyright>
// <summary>
//   The factory class for using the correct settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Alikerroin.Models.Settings
{
    using System;

    using NLog;

    /// <summary>The settings factory.</summary>
    public class SettingsFactory
    {
        /// <summary>The logger object.</summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>The instancing lock.</summary>
        private static readonly object InstancingLock = new object();

        /// <summary>The singleton instance of settings.</summary>
        private static ISettings settings;

        /// <summary>Gets the general application settings.</summary>
        public static ISettings Setup
        {
            get
            {
                lock (InstancingLock)
                {
                    if (settings != null)
                    {
                        return settings;
                    }

                    try
                    {
                        settings = settings ?? Settings.FromFile(Defines.SettingsPath);
                        return settings;
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, @"Settings load from json failed.");
                        return new Settings();
                    }
                }
            }
        }

        /// <summary>Reloads settings from files.</summary>
        public static void Reload()
        {
            lock (InstancingLock)
            {
                settings = Settings.FromFile(Defines.SettingsPath);
            }
        }

        /// <summary>Saves the settings.</summary>
        public static void SaveSettings()
        {
            lock (InstancingLock)
            {
                if (settings != null)
                {
                    try
                    {
                        Settings.ToFile(settings as Settings, Defines.SettingsPath);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, @"Settings save to json failed.");
                    }
                }
            }
        }
    }
}
