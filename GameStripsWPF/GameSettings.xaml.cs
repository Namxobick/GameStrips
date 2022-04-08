using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameStripsWPF
{
    public partial class GameSettings : UserControl
    {
        public delegate void AcceptSetings(int width, int height, int startStripeCount, int quantitySpawRate, int frequencySpawnRate);
        public event AcceptSetings SetingsAccepted;

        public GameSettings()
        {
            InitializeComponent();
            _widthStripeTextBox.KeyDown += TextBoxKeyPress;
            _heightStripeTextBox.KeyDown += TextBoxKeyPress;
            _startStripeCountTextBox.KeyDown += TextBoxKeyPress;
            _numberOfSpawnStripeTextBox.KeyDown += TextBoxKeyPress;
            _timeOfSpawnStripeLabel.KeyDown += TextBoxKeyPress;

            _acceptingSettingsButton.Click += AcceptingSettingsClick;
        }

        private void AcceptingSettingsClick(object sender, RoutedEventArgs e)
        {
            SetingsAccepted?.Invoke(int.Parse(_widthStripeTextBox.Text), int.Parse(_heightStripeTextBox.Text),
             int.Parse(_startStripeCountTextBox.Text), int.Parse(_numberOfSpawnStripeTextBox.Text), int.Parse(_timeOfSpawnStripeTextBox.Text));
        }

        private void TextBoxKeyPress(object sender, KeyEventArgs e)
        {
            Control(e);
        }
        private void Control(KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && (e.Key <= Key.D9)) || (e.Key >= Key.NumPad0 && (e.Key <= Key.NumPad9)))
                return;

            if (e.Key == Key.Back)
                return;
            e.Handled = true;
        }
    }
}
