using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alikerroin.Models.Settings;
using Newtonsoft.Json;
using NLog;

namespace Alikerroin.Models.Settings
{
    /// <summary>JSON Implementation of the settings.</summary>
    public class Settings : ISettings
    {
        /// <summary>The logger instance.</summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public double Odds { get; set; }
        public double OddsLimit { get; set; }
        public double Probability { get; set; }
        public int KellyDivider { get; set; }
        public bool IsBetfair { get; set; }
        public double Bankroll { get; set; }
        public double MaxPartOfBankroll { get; set; }
        public double Units { get; set; }
        public double MaxUnits { get; set; }
        public bool IsReturnPercentage { get; set; }
        public double ReturnPercentage { get; set; }
        public bool IsCalculateReturnPercentage { get; set; }

        public static Settings FromFile(string file)
        {
            if (!File.Exists(file))
            {
                Logger.Warn($"File {file} does not exist");
                return null;
            }

            return FromJson(File.ReadAllText(file));
        }

        /// <summary>Converts settings object to a file.</summary>
        /// <param name="settings">   A settings object to save. </param>
        /// <param name="file">The file name. </param>
        public static void ToFile(Settings settings, string file)
        {
            File.WriteAllText(file, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }

        /// <summary>Initializes settings object from the given from JSON.</summary>
        /// <param name="json">The JSON. </param>
        /// <returns>The settings object.</returns>
        public static Settings FromJson(string json)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error
                };

                return JsonConvert.DeserializeObject<Settings>(json, settings);
            }
            catch (Exception e)
            {
                Logger.Error($"Could not deserialize settings object from the given json: caught {nameof(e)}, {e.Message}");
                throw;
            }
        }
    }
}
