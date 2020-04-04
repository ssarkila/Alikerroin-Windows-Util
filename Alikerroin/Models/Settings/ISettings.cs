// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISettings.cs" company="Alikerroin">
//   Alikerroin, all rights reserved.
// </copyright>
// <summary>
//   The settings interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Alikerroin.Models.Settings
{
    /// <summary> Interface definition of the settings.</summary>
    public interface ISettings
    {
        double Odds { get; set; }
        double OddsLimit { get; set; }
        double Probability { get; set; }
        int KellyDivider { get; set; }
        bool IsBetfair { get; set; }
        double Bankroll { get; set; }
        double MaxPartOfBankroll { get; set; }
        double Units { get; set; }
        double MaxUnits { get; set; }
        bool IsReturnPercentage { get; set; }
        double ReturnPercentage { get; set; }
        bool IsCalculateReturnPercentage { get; set; }
    }
}