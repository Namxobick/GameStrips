using System.Drawing;
using System;
using System.Windows.Forms;

namespace GameStripes
{
    class GameSettings : Control
    {
        private TextBox _widthStripeTextBox;
        private TextBox _heightStripeTextBox;
        private TextBox _startStripeCountTextBox;
        private TextBox _numberOfSpawnStripeTextBox;
        private TextBox _timeOfSpawnStripeTextBox;

        private Label _headerLabel;
        private Label _widthStripeLabel;
        private Label _heightStripeLabel;
        private Label __startStripeCountLabel;
        private Label _numberOfSpawnStripeLabel;
        private Label _timeOfSpawnStripeLabel;

        private Button _acceptingSettingsButton;

        public delegate void AcceptSetings(int width, int height, int startStripeCount, int quantitySpawRate, int frequencySpawnRate);
        public event AcceptSetings SetingsAccepted;

        public GameSettings()
        {
            Size = new Size(251, 301);
            AddLabel(ref _headerLabel, 55, 1, "Setting");
            _headerLabel.TextAlign = ContentAlignment.MiddleCenter;
            _headerLabel.Font = new Font("Tobota", 16);

            AddTextBox(ref _widthStripeTextBox, 146, 50);
            AddLabel(ref _widthStripeLabel, 1, 48, "Width:");
            _widthStripeTextBox.KeyPress += WidthStripeTBKeyPress;

            AddTextBox(ref _heightStripeTextBox, 146, 90);
            AddLabel(ref _heightStripeLabel, 1, 88, "Height:");
            _heightStripeTextBox.KeyPress += WidthStripeTBKeyPress;

            AddTextBox(ref _startStripeCountTextBox, 146, 130);
            AddLabel(ref __startStripeCountLabel, 1, 128, "Start:");
            _heightStripeTextBox.KeyPress += WidthStripeTBKeyPress;

            AddTextBox(ref _numberOfSpawnStripeTextBox, 146, 170);
            AddLabel(ref _numberOfSpawnStripeLabel, 1, 168, "Quantity:");
            _numberOfSpawnStripeTextBox.KeyPress += WidthStripeTBKeyPress;

            AddTextBox(ref _timeOfSpawnStripeTextBox, 146, 210);
            AddLabel(ref _timeOfSpawnStripeLabel, 1, 208, "Frequency(ms):");
            _timeOfSpawnStripeTextBox.KeyPress += WidthStripeTBKeyPress;

            AddButton(ref _acceptingSettingsButton, 80, 255, "Accept");
            _acceptingSettingsButton.MouseClick += AcceptingSettingsButtonMouseClick;

        }

        private void AcceptingSettingsButtonMouseClick(object sender, MouseEventArgs e)
        {
            SetingsAccepted?.Invoke(int.Parse(_widthStripeTextBox.Text), int.Parse(_heightStripeTextBox.Text), 
                int.Parse(_startStripeCountTextBox.Text), int.Parse(_numberOfSpawnStripeTextBox.Text), int.Parse(_timeOfSpawnStripeTextBox.Text));
        }
        
        private void AddButton(ref Button button, int PosX, int posY, string text)
        {
            button = new Button();
            button.Text = text;
            button.Font = new Font("Tobota", 15);
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Size = new Size(89, 35);
            button.Location = new Point(PosX, posY);
            Controls.Add(button);
        }

        private void AddLabel(ref Label label, int posX, int posY, string text)
        {
            label = new Label();

            label.AutoSize = false;
            label.Size = new Size(145, 35);
            label.Font = new Font("Tobota", 14);
            label.ForeColor = Color.Black;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.BackColor = Color.GhostWhite;
            label.Location = new Point(posX, posY);
            Controls.Add(label);
        }

        private void AddTextBox(ref TextBox textBox,int posX, int posY)
        {
            textBox = new TextBox();
            textBox.Font = new Font("Tobota", 14);
            textBox.ForeColor = Color.Black;
            textBox.TextAlign = HorizontalAlignment.Center;
            textBox.Location = new Point(posX, posY);
            Controls.Add(textBox);
        }

        private void WidthStripeTBKeyPress(object sender, KeyPressEventArgs e)
        {
            Control(e, '0', '9');
        }

        private void Control(KeyPressEventArgs e, char first, char last)
        {
            if ((e.KeyChar >= first) && (e.KeyChar <= last))
                return;
            if (Char.IsControl(e.KeyChar))
                return;
            e.Handled = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;

            graph.Clear(Parent.BackColor);

            Rectangle _rect = new Rectangle(0, 0, Width - 1, Height - 1);
            graph.FillRectangle(new SolidBrush(Color.GhostWhite), _rect);
            graph.DrawRectangle(new Pen(Color.Black), _rect);
            
        }
    }
}
