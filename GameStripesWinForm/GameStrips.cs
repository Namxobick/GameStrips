using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GameStripes
{
    class GameStrips : Control
    {

        private GameSettings _gameSettings;

        private Stripe _mystrip;

        private WinWindow _winWindow;
        private LoseWindow _loseWindow;


        private Button _settingButton;
        
        private Label _timeLabel;
        private Label _mouseCountLabel;

        private Timer _timer = new Timer();
        private Timer _timerForSpawn = new Timer();
        private int _stripWidth = 0, _stripHeight = 0, _startStripCount = 0, _stripQuantityOfSpawnRate = 0, _stripFrequencyOfSpawnRate = 0;
        private int _red = 0, _green = 0, _blue = 0;
        private int _clickCount = 0;
        private int _totalStrips = 0;
        private int _time = 0;
        private bool _stripInverted = false;
        private bool _endGame = false;

        private Random _random;

        public GameStrips()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            MinimumSize = new Size(500, 500);
            Size = new Size(500, 500);

            PoolStripe.Init(2 * _startStripCount);
            PoolStripe.InstancePoolStripe.OnFullPool += GameLose;
            PoolStripe.InstancePoolStripe.OnPoolEmpty += GameWin;


            _gameSettings = new GameSettings();
            Controls.Add(_gameSettings);
            _gameSettings.SetingsAccepted += GamSetingsAccepted;

            _settingButton = new Button();
            _settingButton.Image = Image.FromFile("C:/Users/Namxobick/Desktop/setting.png");
            _settingButton.Size = new Size(26, 26);
            Controls.Add(_settingButton);

            _settingButton.MouseClick += SettingButtonMouseClick;

            _random = new Random();

            AddLabel(ref _timeLabel, 20, 5, "Time: 0");
            AddLabel(ref _mouseCountLabel, 210, 5, "Mouse click: 0");
        }

        private void AddLabel(ref Label label, int posX, int posY, string text)
        {
            label = new Label();
            label.AutoSize = false;
            label.Size = new Size(180, 25);
            label.Font = new Font("Tobota", 16);
            label.ForeColor = Color.Black;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.BackColor = Color.White;
            label.Location = new Point(posX, posY);
            Controls.Add(label);
        }

        private void SettingButtonMouseClick(object sender, MouseEventArgs e)
        {
            ResetAllComponents();
            _gameSettings.Visible = true;
        }

        private void GamSetingsAccepted(int width, int height, int startStripeCount, int quantitySpawRate, int frequencySpawnRate)
        {
            _time = 0;
            _clickCount = 0;
            _totalStrips = 0;
            _stripWidth = width;
            _stripHeight = height;
            _startStripCount = startStripeCount;
            _stripQuantityOfSpawnRate = quantitySpawRate;
            _stripFrequencyOfSpawnRate = frequencySpawnRate;

            PoolStripe.InstancePoolStripe.SetMaxCount(2 * _startStripCount);
            _gameSettings.SetingsAccepted -= GamSetingsAccepted;
            Controls.Remove(_gameSettings);

            _endGame = false;
            AddStipes(_startStripCount);

            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = 25;
            _timer.Start();

            _timerForSpawn.Tick += new EventHandler(TimerForSpawnTick);
            _timerForSpawn.Interval = _stripFrequencyOfSpawnRate;
            _timerForSpawn.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _time += 25;
            _timeLabel.Text = "Time: " + (_time / 1000.0).ToString();

            int clickCount = PoolStripe.InstancePoolStripe.GetClickCount() + _clickCount;
            _mouseCountLabel.Text = "Mouse click: " + clickCount.ToString();
        }

        private void TimerForSpawnTick(object sender, EventArgs e)
        {
            AddStipes(_stripQuantityOfSpawnRate);
        }

        private void ResetAllComponents()
        {
            _timer.Stop();
            _timer.Tick -= new EventHandler(TimerTick);
            _timerForSpawn.Stop();
            _timerForSpawn.Tick -= new EventHandler(TimerForSpawnTick);

            PoolStripe.InstancePoolStripe.RemoveAllStripe();

            _settingButton.MouseClick -= SettingButtonMouseClick;
            _gameSettings.SetingsAccepted -= GamSetingsAccepted;

            Controls.Clear();

            Controls.Add(_settingButton);
            Controls.Add(_gameSettings);

            AddLabel(ref _timeLabel, 20, 5, "Time: 0");
            AddLabel(ref _mouseCountLabel, 210, 5, "Mouse click: 0");

            _settingButton.MouseClick += SettingButtonMouseClick;

            _gameSettings.Location = new Point((Width - _gameSettings.Width) / 2, (Height - _gameSettings.Height) / 2);
            _gameSettings.SetingsAccepted += GamSetingsAccepted;
        }

        private Color RandomColor()
        {
            _red = _random.Next(256);
            _green = _random.Next(256);
            _blue = _random.Next(256);
            return Color.FromArgb(_red, _green, _blue);
        }

        private void AddStipes(int stripeCount)
        {
            List<Control> controls = new List<Control>();
            foreach (Control x in Controls)
                controls.Add(x);

            for (int i = stripeCount; i > 0; i--)
            {
                _totalStrips++;
                Color color = RandomColor();

                _stripInverted = Convert.ToBoolean(_random.Next(2));

                if (_stripInverted)
                {
                    CreateSripe(_stripWidth, _stripHeight, color);
                }
                else
                {
                    CreateSripe(_stripHeight, _stripWidth, color);
                }

                PoolStripe.InstancePoolStripe.AddStripe(_mystrip);
                controls.Insert(0, _mystrip);
                if (_endGame)
                    return;
            }
            Controls.Clear();
            Controls.AddRange(controls.ToArray());
        }

        private void CreateSripe(int stripHeight, int stripWidth, Color color)
        {
            int posX = _random.Next(Width - stripWidth - 10) + 2;
            int posY = _random.Next(Height - stripHeight - (int)(Height * 0.05) - 30) + (int)(Height * 0.05) + 15; ;
            _mystrip = new Stripe(posX, posY, stripWidth, stripHeight, color);
        }

        private void GameLose()
        {
            int clickCount = 0, posX = 0, posY = 0;
            double clickAccuracy = 0;
            SettingGameOver(ref clickCount, ref posX, ref posY);
            if (clickCount != 0) clickAccuracy = ((_totalStrips - 1) - 2 * _startStripCount) / (double)clickCount * 100;
            _loseWindow = new LoseWindow(clickCount, _time / 1000.0, clickAccuracy, posX, posY);
            Controls.Add(_loseWindow);
            _loseWindow.WindowClose += WindowClose;
        }

        private void GameWin()
        {
            int clickCount = 0, posX = 0, posY = 0;
            double clickAccuracy = 0;
            SettingGameOver(ref clickCount, ref posX, ref posY);
            if (clickCount != 0) clickAccuracy = _totalStrips / (double)clickCount * 100;
            _winWindow = new WinWindow(clickCount, _time / 1000.0, clickAccuracy, posX, posY);
            Controls.Add(_winWindow);
            _winWindow.WindowClose += WindowClose;
        }

        private void SettingGameOver(ref int clickCount, ref int posX, ref int posY)
        {
            clickCount = PoolStripe.InstancePoolStripe.GetClickCount() + _clickCount;
            posX = Width / 2 - 190;
            posY = this.Height / 2 - 225;
            ResetAllComponents();
            _gameSettings.Visible = false;
            _endGame = true;
        }

        private void WindowClose()
        {
            if(_winWindow != null) _winWindow.WindowClose -= WindowClose;
            if (_loseWindow != null) _loseWindow.WindowClose -= WindowClose;
            Controls.Remove(_winWindow);
            Controls.Remove(_loseWindow);
            _gameSettings.Visible = true;
        }

        private GraphicsPath RoundedRectangle(Rectangle rect, int RoundSize)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rect.X, rect.Y, RoundSize, RoundSize, 180, 90);
            gp.AddArc(rect.X + rect.Width - RoundSize, rect.Y, RoundSize, RoundSize, 270, 90);
            gp.AddArc(rect.X + rect.Width - RoundSize, rect.Y + rect.Height - RoundSize, RoundSize, RoundSize, 0, 90);
            gp.AddArc(rect.X, rect.Y + rect.Height - RoundSize, RoundSize, RoundSize, 90, 90);
            gp.CloseFigure();

            return gp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.Clear(Parent.BackColor);
            Rectangle rect = new Rectangle(1, 1, Width - 3, Height - 3);
            GraphicsPath rectGP = RoundedRectangle(rect, (int)(Height * 0.05));
            graph.DrawPath(new Pen(Color.Black, 3), rectGP);
            graph.FillPath(new SolidBrush(Color.White), rectGP);

            graph.DrawLine(new Pen(Color.Black, 2), 1, (int)(Height * 0.05) + 10, Width, (int)(Height * 0.05) + 10);
            _settingButton.Location = new Point(Width - 35, (int)((Height * 0.05) + 10) / 5);
            _gameSettings.Location = new Point((Width - _gameSettings.Width) / 2, (Height - _gameSettings.Height) / 2);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            _clickCount++;
        }
    }
}
