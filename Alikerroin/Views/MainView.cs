// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainView.cs" company="Alikerroin">
//   Alikerroin, all rights reserved.
// </copyright>
// <summary>
//   The main view component.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using Alikerroin.Models.Settings;

namespace Alikerroin.Views
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    public enum ReturnPercentageUsage
    {
        Unknown,

        Odds1,

        Odds2,

        CalculateReturnPercentage
    }

    public partial class MainView : Form, IMainView
    {
        private bool _isUpdating;
        private decimal _odds;
        private decimal _oddsLimit;
        private decimal _probability;
        private decimal _kellyDivider;
        private decimal _bankroll;
        private decimal _maxPartOfBankroll;
        private decimal _units;
        private decimal _maxUnits;
        private decimal _odds121;
        private decimal _odds122;
        private decimal _odds1X21;
        private decimal _odds1X2X;
        private decimal _odds1X22;
        private decimal _probability121;
        private decimal _probability122;
        private decimal _probability1X21;
        private decimal _probability1X2X;
        private decimal _probability1X22;
        private decimal _probability1X2121;
        private decimal _probability1X2122;
        private decimal _returnPercentage12;
        private decimal _calculatedReturnPercentage12;
        private decimal _calculatedReturnPercentage1X2;

        public MainView()
        {
            InitializeComponent();
        }

        public void Display()
        {
            InitializeComponent();
            string version = Application.ProductVersion;
            string restrictedVersion = version.Replace(".0", string.Empty);
            base.Text = $@"Alikerroin util {restrictedVersion}";
            LoadSettings();
            _odds121 = _numericUpDownOdds121.Value;
            _odds122 = _numericUpDownOdds122.Value;
            _odds1X21 = _numericUpDownOdds1X21.Value;
            _odds1X2X = _numericUpDownOdds1X2X.Value;
            _odds1X22 = _numericUpDownOdds1X22.Value;
            _probability121 = _numericUpDownProbability121.Value;
            _probability122 = _numericUpDownProbability122.Value;
            _returnPercentage12 = 1;
            CalculateBetResult();
            ReturnPercentageCheck12();
            Calculate1X2Probabilities();
            Application.Run(this);
        }

        private void LoadSettings()
        {
            try
            {
                _odds = (decimal)SettingsFactory.Setup.Odds;
                _oddsLimit = (decimal)SettingsFactory.Setup.OddsLimit;
                _probability = (decimal)SettingsFactory.Setup.Probability;
                _kellyDivider = SettingsFactory.Setup.KellyDivider;
                _checkBoxBetfair.Checked = SettingsFactory.Setup.IsBetfair;
                _bankroll = (decimal)SettingsFactory.Setup.Bankroll;
                _maxPartOfBankroll = (decimal)SettingsFactory.Setup.MaxPartOfBankroll;
                _units = (decimal)SettingsFactory.Setup.Units;
                _maxUnits = (decimal)SettingsFactory.Setup.MaxUnits;
                _checkBoxReturnPercentage12.Checked = SettingsFactory.Setup.IsReturnPercentage;
                _returnPercentage12 = (decimal)SettingsFactory.Setup.ReturnPercentage;
                _checkBoxCalculateReturnPercentage12.Checked = SettingsFactory.Setup.IsCalculateReturnPercentage;
                SetValuesToUi();
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Failed to load settings! {Environment.NewLine} {Environment.NewLine} {e.Message}", @"Alikerroin");
            }
        }

        private void SaveSettings()
        {

            SettingsFactory.Setup.Odds = (double) _numericUpDownOdds.Value;
            SettingsFactory.Setup.OddsLimit = (double)_numericUpDownOddsLimit.Value;
            SettingsFactory.Setup.Probability = (double)_numericUpDownProbability.Value;
            SettingsFactory.Setup.KellyDivider = (int)_numericUpDownKellyDivider.Value;
            SettingsFactory.Setup.IsBetfair = _checkBoxBetfair.Checked;
            SettingsFactory.Setup.Bankroll = (double)_numericUpDownBankroll.Value;
            SettingsFactory.Setup.MaxPartOfBankroll = (double)_numericUpDownMaxPartOfBankroll.Value;
            SettingsFactory.Setup.Units = (double)_numericUpDownUnits.Value;
            SettingsFactory.Setup.MaxUnits = (double)_numericUpDownMaxUnits.Value;
            SettingsFactory.Setup.IsReturnPercentage = _checkBoxReturnPercentage12.Checked;
            SettingsFactory.Setup.ReturnPercentage = (double)_numericUpDownReturnPercentage12.Value;
            SettingsFactory.Setup.IsCalculateReturnPercentage = _checkBoxCalculateReturnPercentage12.Checked;

            SettingsFactory.SaveSettings();
        }

        private void SetValuesToUi()
        {
            _numericUpDownOdds.Value = _odds;
            _numericUpDownOddsLimit.Value = _oddsLimit;
            _numericUpDownProbability.Value = _probability;
            _numericUpDownKellyDivider.Value = _kellyDivider;
            _numericUpDownBankroll.Value = _bankroll;
            _numericUpDownMaxPartOfBankroll.Value = _maxPartOfBankroll;
            _numericUpDownUnits.Value = _units;
            _numericUpDownMaxUnits.Value = _maxUnits;
            _numericUpDownReturnPercentage12.Value = _returnPercentage12;
        }

        private void CalculateBetResult()
        {
            if (_checkBoxBetfair.Checked)
            {
                double betfairReducer = 0.98;
                _textBoxExpectedValue.Text = Math.Round((((double)_probability / 100.0) * ((double)_odds * betfairReducer)), 3).ToString(CultureInfo.InvariantCulture);
                double kelly = (100.0 * ((((double)_probability / 100.0) * (double)_odds * betfairReducer - 1) / ((double)_odds * betfairReducer - 1) / (double)_kellyDivider));
                _textBoxKelly.Text = Math.Round(kelly, 2).ToString(CultureInfo.InvariantCulture);
                _textBoxBet1.Text = Math.Round(((kelly / 100.0) * (double)_bankroll), 2).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                _textBoxExpectedValue.Text = Math.Round((((double)_probability / 100.0) * (double)_odds), 3).ToString(CultureInfo.InvariantCulture);
                double kelly = (100 * ((((double)_probability / 100.0) * (double)_odds - 1) / ((double)_odds - 1) / (double)_kellyDivider));
                _textBoxKelly.Text = Math.Round(kelly, 2).ToString(CultureInfo.InvariantCulture);
                _textBoxBet1.Text = Math.Round(((kelly / 100.0) * (double)_bankroll), 2).ToString(CultureInfo.InvariantCulture);
            }

            _textBoxPartOfMaxUnits.Text = Math.Round(100 * (_numericUpDownUnits.Value / _numericUpDownMaxUnits.Value), 1).ToString(CultureInfo.InvariantCulture);
            _textBoxBet2.Text = Math.Round((double)_bankroll * (((double)_numericUpDownMaxPartOfBankroll.Value / 100.0) * (double)_numericUpDownUnits.Value / (double)_numericUpDownMaxUnits.Value), 1).ToString(CultureInfo.InvariantCulture);
        }

        private void Update12(ReturnPercentageUsage rcu = ReturnPercentageUsage.Unknown)
        {
            switch (rcu)
            {
                case ReturnPercentageUsage.Odds1:
                    _numericUpDownOdds121.Value = Math.Round(_odds121, 3);
                    _numericUpDownOdds122.Value = Math.Round(_odds122 * _returnPercentage12 * _returnPercentage12, 3);
                    break;
                case ReturnPercentageUsage.Odds2:
                    _numericUpDownOdds121.Value = Math.Round(_odds121 * _returnPercentage12 * _returnPercentage12, 3);
                    _numericUpDownOdds122.Value = Math.Round(_odds122, 3);
                    break;
                case ReturnPercentageUsage.CalculateReturnPercentage:
                    _numericUpDownOdds121.Value = Math.Round(_odds121 * _returnPercentage12 * _returnPercentage12, 3);
                    _numericUpDownOdds122.Value = Math.Round(_odds122, 3);
                    _textBoxCalculateReturnPercentage12.Text = Math.Round(_calculatedReturnPercentage12, 2).ToString(CultureInfo.InvariantCulture);
                    break;
                default:
                    _numericUpDownOdds121.Value = Math.Round(_odds121 * _returnPercentage12, 3);
                    _numericUpDownOdds122.Value = Math.Round(_odds122 * _returnPercentage12, 3);
                    break;
            }

            _numericUpDownProbability121.Value = Math.Round(_probability121, 1);
            _numericUpDownProbability122.Value = Math.Round(_probability122, 1);

            Pen blankPen = new Pen(SystemColors.Control) { Width = 2 };
            int variance = 2;

            if (_checkBoxReturnPercentage12.Checked)
            {
                _numericUpDownReturnPercentage12.Enabled = true;
                _textBoxCalculateReturnPercentage12.Enabled = false;
                Pen greenPen = new Pen(Color.LawnGreen) { Width = 2 };
                Graphics g = _groupBox12.CreateGraphics();

                g.DrawRectangle(blankPen, new Rectangle(_textBoxCalculateReturnPercentage12.Location.X - variance, _textBoxCalculateReturnPercentage12.Location.Y - variance, _textBoxCalculateReturnPercentage12.Width + (2 * variance), _textBoxCalculateReturnPercentage12.Height + (2 * variance)));
                g.DrawRectangle(greenPen, new Rectangle(_numericUpDownReturnPercentage12.Location.X - variance, _numericUpDownReturnPercentage12.Location.Y - variance, _numericUpDownReturnPercentage12.Width + (2 * variance), _numericUpDownReturnPercentage12.Height + (2 * variance)));
                g.DrawRectangle(greenPen, new Rectangle(_numericUpDownOdds121.Location.X - variance, _numericUpDownOdds121.Location.Y - variance, _numericUpDownOdds121.Width + (2 * variance), _numericUpDownOdds121.Height + (2 * variance)));
                g.DrawRectangle(greenPen, new Rectangle(_numericUpDownOdds122.Location.X - variance, _numericUpDownOdds122.Location.Y - variance, _numericUpDownOdds122.Width + (2 * variance), _numericUpDownOdds122.Height + (2 * variance)));
            }

            if (_checkBoxCalculateReturnPercentage12.Checked)
            {
                _numericUpDownReturnPercentage12.Enabled = false;
                _textBoxCalculateReturnPercentage12.Enabled = true;
                Pen bluePen = new Pen(Color.DeepSkyBlue) { Width = 2 };
                Graphics g = _groupBox12.CreateGraphics();
                g.DrawRectangle(blankPen, new Rectangle(_numericUpDownReturnPercentage12.Location.X - variance, _numericUpDownReturnPercentage12.Location.Y - variance, _numericUpDownReturnPercentage12.Width + (2 * variance), _numericUpDownReturnPercentage12.Height + (2 * variance)));
                g.DrawRectangle(bluePen, new Rectangle(_textBoxCalculateReturnPercentage12.Location.X - variance, _textBoxCalculateReturnPercentage12.Location.Y - variance, _textBoxCalculateReturnPercentage12.Width + (2 * variance), _textBoxCalculateReturnPercentage12.Height + (2 * variance)));
                g.DrawRectangle(bluePen, new Rectangle(_numericUpDownOdds121.Location.X - variance, _numericUpDownOdds121.Location.Y - variance, _numericUpDownOdds121.Width + (2 * variance), _numericUpDownOdds121.Height + (2 * variance)));
                g.DrawRectangle(bluePen, new Rectangle(_numericUpDownOdds122.Location.X - variance, _numericUpDownOdds122.Location.Y - variance, _numericUpDownOdds122.Width + (2 * variance), _numericUpDownOdds122.Height + (2 * variance)));
            }

            if (!_checkBoxReturnPercentage12.Checked && !_checkBoxCalculateReturnPercentage12.Checked)
            {
                _numericUpDownReturnPercentage12.Enabled = false;
                _textBoxCalculateReturnPercentage12.Enabled = false;
                Graphics g = _groupBox12.CreateGraphics();
                g.DrawRectangle(blankPen, new Rectangle(_numericUpDownReturnPercentage12.Location.X - variance, _numericUpDownReturnPercentage12.Location.Y - variance, _numericUpDownReturnPercentage12.Width + (2 * variance), _numericUpDownReturnPercentage12.Height + (2 * variance)));
                g.DrawRectangle(blankPen, new Rectangle(_textBoxCalculateReturnPercentage12.Location.X - variance, _textBoxCalculateReturnPercentage12.Location.Y - variance, _textBoxCalculateReturnPercentage12.Width + (2 * variance), _textBoxCalculateReturnPercentage12.Height + (2 * variance)));
                g.DrawRectangle(blankPen, new Rectangle(_numericUpDownOdds121.Location.X - variance, _numericUpDownOdds121.Location.Y - variance, _numericUpDownOdds121.Width + (2 * variance), _numericUpDownOdds121.Height + (2 * variance)));
                g.DrawRectangle(blankPen, new Rectangle(_numericUpDownOdds122.Location.X - variance, _numericUpDownOdds122.Location.Y - variance, _numericUpDownOdds122.Width + (2 * variance), _numericUpDownOdds122.Height + (2 * variance)));
            }
        }

        private void Update1X2()
        {
            _numericUpDownOdds1X21.Value = Math.Round(_odds1X21, 3);
            _numericUpDownOdds1X2X.Value = Math.Round(_odds1X2X, 3);
            _numericUpDownOdds1X22.Value = Math.Round(_odds1X22, 3);
            _textBoxCalculateReturnPercentage1X2.Text = Math.Round(_calculatedReturnPercentage1X2, 2).ToString(CultureInfo.InvariantCulture);

            _textBoxProbability1X21.Text = Math.Round(_probability1X21, 1).ToString(CultureInfo.InvariantCulture);
            _textBoxProbability1X2X.Text = Math.Round(_probability1X2X, 1).ToString(CultureInfo.InvariantCulture);
            _textBoxProbability1X22.Text = Math.Round(_probability1X22, 1).ToString(CultureInfo.InvariantCulture);
            _textBoxProbability1.Text = Math.Round(_probability1X2121, 1).ToString(CultureInfo.InvariantCulture);
            _textBoxProbability2.Text = Math.Round(_probability1X2122, 1).ToString(CultureInfo.InvariantCulture);
        }

        private void ReturnPercentageCheck12()
        {
            if (!_isUpdating)
            {
                _isUpdating = true;
                if (_checkBoxReturnPercentage12.Checked)
                {
                    _returnPercentage12 = _numericUpDownReturnPercentage12.Value / (decimal)100.0;
                }
                else
                {
                    _returnPercentage12 = 1;
                }

                Update12();
                _isUpdating = false;
            }
        }

        private void Calculate1X2Probabilities()
        {
            _calculatedReturnPercentage1X2 = (decimal)100.0 / ((1 / _odds1X21) + (1 / _odds1X2X) + (1 / _odds1X22));
            _probability1X21 = (decimal)100.0 / ((_odds1X21) * ((decimal)100.0 / _calculatedReturnPercentage1X2));
            _probability1X2X = (decimal)100.0 / ((_odds1X2X) * ((decimal)100.0 / _calculatedReturnPercentage1X2));
            _probability1X22 = (decimal)100.0 / ((_odds1X22) * ((decimal)100.0 / _calculatedReturnPercentage1X2));
            decimal scaleFactorTo100 = (decimal)100.0 / (_probability1X21 + _probability1X22);
            _probability1X2121 = _probability1X21 * scaleFactorTo100;
            _probability1X2122 = _probability1X22 * scaleFactorTo100;
        }

        private decimal RoundUp(decimal input, int places)
        {
            decimal multiplier = (decimal)Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        private decimal RoundDown(decimal input, int places)
        {
            decimal multiplier = (decimal)Math.Pow(10, Convert.ToDouble(places));
            return Math.Floor(input * multiplier) / multiplier;
        }

        private void _numericUpDownOddsLimit_ValueChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;

                if (_oddsLimit > _numericUpDownOddsLimit.Value)
                {
                    if (Math.Abs(Math.Round(_oddsLimit, 3) - _numericUpDownOddsLimit.Value - _numericUpDownOddsLimit.Increment) == 0 && Math.Abs(_numericUpDownOddsLimit.Value - Math.Round(_numericUpDownOddsLimit.Value, 2)) != 0)
                    {
                        _numericUpDownOddsLimit.Value = RoundDown(_oddsLimit, 2);
                    }
                }
                else
                {
                    if (Math.Abs(Math.Round(_oddsLimit, 3) - _numericUpDownOddsLimit.Value + _numericUpDownOddsLimit.Increment) == 0 && Math.Abs(_numericUpDownOddsLimit.Value - Math.Round(_numericUpDownOddsLimit.Value, 2)) != 0)
                    {
                        _numericUpDownOddsLimit.Value = RoundUp(_oddsLimit, 2);
                    }
                }

                _oddsLimit = _numericUpDownOddsLimit.Value;
                _probability = (decimal)100.0 / _oddsLimit;
                _numericUpDownProbability.Value = Math.Round(_probability, 1);
                CalculateBetResult();
                _isUpdating = false;
            }
        }

        private void _numericUpDownProbability_ValueChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;

                if (_probability > _numericUpDownProbability.Value)
                {
                    if (Math.Abs(Math.Round(_probability, 1) - _numericUpDownProbability.Value - _numericUpDownProbability.Increment) == 0 && Math.Abs(_numericUpDownProbability.Value - Math.Round(_numericUpDownProbability.Value, 0)) != 0)
                    {
                        _numericUpDownProbability.Value = RoundDown(_probability,0);
                    }
                }
                else
                {
                    if (Math.Abs(Math.Round(_probability, 1) - _numericUpDownProbability.Value + _numericUpDownProbability.Increment) == 0 && Math.Abs(_numericUpDownProbability.Value - Math.Round(_numericUpDownProbability.Value, 0)) != 0)
                    {
                        _numericUpDownProbability.Value = RoundUp(_probability,0);
                    }
                }

                _probability = _numericUpDownProbability.Value;
                _oddsLimit = 100 / _numericUpDownProbability.Value;
                _numericUpDownOddsLimit.Value = Math.Round(_oddsLimit, 3);
                CalculateBetResult();
                _isUpdating = false;
            }
        }

        private void _numericUpDownOdds_ValueChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;

                if (_odds > _numericUpDownOdds.Value)
                {
                    if (Math.Abs(Math.Round(_odds, 3) - _numericUpDownOdds.Value - _numericUpDownOdds.Increment) == 0 && Math.Abs(_numericUpDownOdds.Value - Math.Round(_numericUpDownOdds.Value, 2)) != 0)
                    {
                        _numericUpDownOdds.Value = RoundDown(_odds, 2);
                    }
                }
                else
                {
                    if (Math.Abs(Math.Round(_odds, 3) - _numericUpDownOdds.Value + _numericUpDownOdds.Increment) == 0 && Math.Abs(_numericUpDownOdds.Value - Math.Round(_numericUpDownOdds.Value, 2)) != 0)
                    {
                        _numericUpDownOdds.Value = RoundUp(_odds, 2);
                    }
                }

                _odds = _numericUpDownOdds.Value;
                CalculateBetResult();
                _isUpdating = false;
            }
        }

        private void _numericUpDownKellyDivider_ValueChanged(object sender, EventArgs e)
        {
            _kellyDivider = _numericUpDownKellyDivider.Value;
            CalculateBetResult();
        }

        private void _numericUpDownMaxPartOfBankroll_ValueChanged(object sender, EventArgs e)
        {
            _maxPartOfBankroll = _numericUpDownMaxPartOfBankroll.Value;
            CalculateBetResult();
        }

        private void _numericUpDownUnits_ValueChanged(object sender, EventArgs e)
        {
            if (_numericUpDownUnits.Value > _maxUnits)
            {
                _numericUpDownMaxUnits.Value = _units;
            }

            _units = _numericUpDownUnits.Value;
            _maxUnits = _numericUpDownMaxUnits.Value;

            CalculateBetResult();
        }

        private void _numericUpDownMaxUnits_ValueChanged(object sender, EventArgs e)
        {
            if (_numericUpDownUnits.Value > _maxUnits)
            {
                _numericUpDownUnits.Value  = _maxUnits;
            }

            _units = _numericUpDownUnits.Value;
            _maxUnits = _numericUpDownMaxUnits.Value;

            CalculateBetResult();
        }

        private void _numericUpDownBankroll_ValueChanged(object sender, EventArgs e)
        {
            _bankroll = _numericUpDownBankroll.Value;
            CalculateBetResult();
        }

        private void _buttonLoad_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void _buttonSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void _numericUpDownProbability121_ValueChanged(object sender, EventArgs e)
        {
            if (!_checkBoxCalculateReturnPercentage12.Checked)
            {
                if (!_isUpdating)
                {
                    _isUpdating = true;
                    _probability121 = _numericUpDownProbability121.Value;
                    _probability122 = (decimal) 100.0 - _probability121;
                    _odds121 = (decimal) 100.0 / _probability121;
                    _odds122 = (decimal) 100.0 / ((decimal) 100.0 - _probability121);
                    Update12();
                    _isUpdating = false;
                }
            }
        }

        private void _numericUpDownProbability122_ValueChanged(object sender, EventArgs e)
        {
            if (!_checkBoxCalculateReturnPercentage12.Checked)
            {
                if (!_isUpdating)
                {
                    _isUpdating = true;
                    _probability122 = _numericUpDownProbability122.Value;
                    _probability121 = (decimal) 100.0 - _probability122;
                    _odds121 = (decimal) 100.0 / ((decimal) 100.0 - _probability122);
                    _odds122 = (decimal) 100.0 / _probability122;
                    Update12();
                    _isUpdating = false;
                }
            }
        }

        private void _numericUpDownOdds121_ValueChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;

                if (_odds121 > _numericUpDownOdds121.Value)
                {
                    if (Math.Abs(Math.Round(_odds121, 3) - _numericUpDownOdds121.Value - _numericUpDownOdds121.Increment) == 0 && Math.Abs(_numericUpDownOdds121.Value - Math.Round(_numericUpDownOdds121.Value, 2)) != 0)
                    {
                        _numericUpDownOdds121.Value = RoundDown(_odds121, 2);
                    }
                }
                else
                {
                    if (Math.Abs(Math.Round(_odds121, 3) - _numericUpDownOdds121.Value + _numericUpDownOdds121.Increment) == 0 && Math.Abs(_numericUpDownOdds121.Value - Math.Round(_numericUpDownOdds121.Value, 2)) != 0)
                    {
                        _numericUpDownOdds121.Value = RoundUp(_odds121, 2);
                    }
                }

                _odds121 = _numericUpDownOdds121.Value;

                if (_checkBoxCalculateReturnPercentage12.Checked)
                {
                    _calculatedReturnPercentage12 = (decimal)100.0 / ((1 / _odds121) + (1 / _odds122));
                    _probability121 = (decimal)100.0 / ((_odds121) * ((decimal)100.0 / _calculatedReturnPercentage12));
                    _probability122 = (decimal) 100.0 - _probability121;
                    Update12(ReturnPercentageUsage.CalculateReturnPercentage);
                }
                else
                {
                    _probability121 = (decimal)100.0 / _odds121;
                    _probability122 = (decimal)100.0 - _probability121;
                    _odds122 = (decimal)100.0 / ((decimal)100.0 - _probability121);
                    Update12(ReturnPercentageUsage.Odds1);
                }

                _isUpdating = false;
            }
        }

        private void _numericUpDownOdds122_ValueChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;

                if (_odds122 > _numericUpDownOdds122.Value)
                {
                    if (Math.Abs(Math.Round(_odds122, 3) - _numericUpDownOdds122.Value - _numericUpDownOdds122.Increment) == 0 && Math.Abs(_numericUpDownOdds122.Value - Math.Round(_numericUpDownOdds122.Value, 2)) != 0)
                    {
                        _numericUpDownOdds122.Value = RoundDown(_odds122, 2);
                    }
                }
                else
                {
                    if (Math.Abs(Math.Round(_odds122, 3) - _numericUpDownOdds122.Value + _numericUpDownOdds122.Increment) == 0 && Math.Abs(_numericUpDownOdds122.Value - Math.Round(_numericUpDownOdds122.Value, 2)) != 0)
                    {
                        _numericUpDownOdds122.Value = RoundUp(_odds122, 2);
                    }
                }

                _odds122 = _numericUpDownOdds122.Value;

                if (_checkBoxCalculateReturnPercentage12.Checked)
                {
                    _odds122 = _numericUpDownOdds122.Value;
                    _calculatedReturnPercentage12 = (decimal)100.0 / ((1 / _odds122) + (1 / _odds121));
                    _probability122 = (decimal)100.0 / ((_odds122) * ((decimal)100.0 / _calculatedReturnPercentage12));
                    _probability121 = (decimal)100.0 - _probability122;
                    Update12(ReturnPercentageUsage.CalculateReturnPercentage);
                }
                else
                {
                    _odds122 = _numericUpDownOdds122.Value;
                    _probability122 = (decimal)100.0 / _odds122;
                    _probability121 = (decimal)100.0 - _probability122;
                    _odds121 = (decimal)100.0 / ((decimal)100.0 - _probability122);
                    Update12(ReturnPercentageUsage.Odds2);
                }

                _isUpdating = false;
            }
        }

        private void _numericUpDownReturnPercentage_ValueChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;
                _returnPercentage12 = _numericUpDownReturnPercentage12.Value / (decimal)100.0;
                Update12();
                _isUpdating = false;
            }
        }

        private void _checkBoxBetfair_CheckedChanged(object sender, EventArgs e)
        {
            CalculateBetResult();
        }

        private void _checkBoxReturnPercentage_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;
                if (_checkBoxReturnPercentage12.Checked)
                {
                    _checkBoxCalculateReturnPercentage12.Checked = false;
                    _numericUpDownProbability121.ReadOnly = false;
                    _numericUpDownProbability122.ReadOnly = false;
                    _numericUpDownProbability121.Controls[0].Visible = true;
                    _numericUpDownProbability122.Controls[0].Visible = true;
                }

                _isUpdating = false;
            }

            ReturnPercentageCheck12();
            Update12();
        }

        private void _checkBoxCalculateReturnPercentage_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;
                if (_checkBoxCalculateReturnPercentage12.Checked)
                {
                    _checkBoxReturnPercentage12.Checked = false;
                    _numericUpDownProbability121.ReadOnly = true;
                    _numericUpDownProbability122.ReadOnly = true;
                    _numericUpDownProbability121.Controls[0].Visible = false;
                    _numericUpDownProbability122.Controls[0].Visible = false;
                    _calculatedReturnPercentage12 = (decimal)100.0 / ((1 / _odds121) + (1 / _odds122));
                    Update12(ReturnPercentageUsage.CalculateReturnPercentage);
                }
                else
                {
                    _numericUpDownProbability121.ReadOnly = false;
                    _numericUpDownProbability122.ReadOnly = false;
                    _numericUpDownProbability121.Controls[0].Visible = true;
                    _numericUpDownProbability122.Controls[0].Visible = true;
                    Update12();
                }

                ReturnPercentageCheck12();
                _isUpdating = false;
            }
        }

        private void _numericUpDownOdds1X21_ValueChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;

                if (_odds1X21 > _numericUpDownOdds1X21.Value)
                {
                    if (Math.Abs(Math.Round(_odds1X21, 3) - _numericUpDownOdds1X21.Value - _numericUpDownOdds1X21.Increment) == 0 && Math.Abs(_numericUpDownOdds1X21.Value - Math.Round(_numericUpDownOdds1X21.Value, 2)) != 0)
                    {
                        _numericUpDownOdds1X21.Value = RoundDown(_odds1X21, 2);
                    }
                }
                else
                {
                    if (Math.Abs(Math.Round(_odds1X21, 3) - _numericUpDownOdds1X21.Value + _numericUpDownOdds1X21.Increment) == 0 && Math.Abs(_numericUpDownOdds1X21.Value - Math.Round(_numericUpDownOdds1X21.Value, 2)) != 0)
                    {
                        _numericUpDownOdds1X21.Value = RoundUp(_odds1X21, 2);
                    }
                }

                _odds1X21 = _numericUpDownOdds1X21.Value;
                Calculate1X2Probabilities();
                Update1X2();
                _isUpdating = false;
            }
        }

        private void _numericUpDownOdds1X2X_ValueChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;

                if (_odds1X2X > _numericUpDownOdds1X2X.Value)
                {
                    if (Math.Abs(Math.Round(_odds1X2X, 3) - _numericUpDownOdds1X2X.Value - _numericUpDownOdds1X2X.Increment) == 0 && Math.Abs(_numericUpDownOdds1X2X.Value - Math.Round(_numericUpDownOdds1X2X.Value, 2)) != 0)
                    {
                        _numericUpDownOdds1X2X.Value = RoundDown(_odds1X2X, 2);
                    }
                }
                else
                {
                    if (Math.Abs(Math.Round(_odds1X2X, 3) - _numericUpDownOdds1X2X.Value + _numericUpDownOdds1X2X.Increment) == 0 && Math.Abs(_numericUpDownOdds1X2X.Value - Math.Round(_numericUpDownOdds1X2X.Value, 2)) != 0)
                    {
                        _numericUpDownOdds1X2X.Value = RoundUp(_odds1X2X, 2);
                    }
                }

                _odds1X2X = _numericUpDownOdds1X2X.Value;
                Calculate1X2Probabilities();
                Update1X2();
                _isUpdating = false;
            }
        }

        private void _numericUpDownOdds1X22_ValueChanged(object sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;

                if (_odds1X22 > _numericUpDownOdds1X22.Value)
                {
                    if (Math.Abs(Math.Round(_odds1X22, 3) - _numericUpDownOdds1X22.Value - _numericUpDownOdds1X22.Increment) == 0 && Math.Abs(_numericUpDownOdds1X22.Value - Math.Round(_numericUpDownOdds1X22.Value, 2)) != 0)
                    {
                        _numericUpDownOdds1X22.Value = RoundDown(_odds1X22, 2);
                    }
                }
                else
                {
                    if (Math.Abs(Math.Round(_odds1X22, 3) - _numericUpDownOdds1X22.Value + _numericUpDownOdds1X22.Increment) == 0 && Math.Abs(_numericUpDownOdds1X22.Value - Math.Round(_numericUpDownOdds1X22.Value, 2)) != 0)
                    {
                        _numericUpDownOdds1X22.Value = RoundUp(_odds1X22, 2);
                    }
                }

                _odds1X22 = _numericUpDownOdds1X22.Value;
                Calculate1X2Probabilities();
                Update1X2();
                _isUpdating = false;
            }
        }
    }
}
