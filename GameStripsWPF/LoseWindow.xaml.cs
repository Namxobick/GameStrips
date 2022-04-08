using System.Windows;
using System.Windows.Controls;

namespace GameStripsWPF
{
    public partial class LoseWindow : UserControl
    {
        public delegate void CloseWinWindow();
        public event CloseWinWindow WindowClose;

        public LoseWindow(int mouseClickCount, string time, double clickAccuracy, int PosX, int PosY)
        {
            InitializeComponent();
            Margin = new Thickness(PosX, PosY, 0, 0);
            _mouseClickLabel.Content = mouseClickCount.ToString();
            _timeLabel.Content = time;
            _accurasyLabel.Content = clickAccuracy.ToString() + "%";
        }

        private void _closeWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowClose?.Invoke();
        }
    }
}

