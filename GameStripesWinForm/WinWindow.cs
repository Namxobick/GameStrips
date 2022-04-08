using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameStripes
{
    public partial class WinWindow : UserControl
    {
        public delegate void CloseWinWindow();
        public event CloseWinWindow WindowClose;
        public WinWindow(int mouseClickCount, double time, double clickAccuracy, int PosX, int PosY)
        {
            InitializeComponent();
            Location = new Point(PosX, PosY);
            label3.Text = mouseClickCount.ToString();
            label4.Text = time.ToString();
            label5.Text = Math.Round(clickAccuracy, 2).ToString() + "%";
            Size = new Size(380, 475);
        }

        private void WinWindow_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowClose?.Invoke();
        }
    }
}
