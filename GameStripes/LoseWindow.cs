using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameStripes
{
    public partial class LoseWindow : UserControl
    {
        public delegate void CloseLoseWindow();
        public event CloseLoseWindow WindowClose;
        public LoseWindow(int mouseClickCount, double time, double clickAccuracy, int PosX, int PosY)
        {
            InitializeComponent();
            Location = new Point(PosX, PosY);
            label3.Text = mouseClickCount.ToString();
            label4.Text = time.ToString();
            label5.Text = Math.Round(clickAccuracy, 2).ToString() + "%";
            Size = new Size(380, 475);
        }

        private void LoseWindow_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowClose?.Invoke();
        }
    }
}
