using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameStripsWPF
{
    public partial class MainWindow : Window
    {
        private Rectangle _gameWindow;

        private GameSettings _gameSettings;

        private Random _random = new Random();

        private WinWindow _winWindow;
        private LoseWindow _loseWindow;

        private DispatcherTimer _timerSpawnStrips = new DispatcherTimer();


        private byte _red = 0, _green = 0, _blue = 0;

        private int _stripWidth = 0, _stripHeight = 0, _startStripCount = 0, _stripQuantityOfSpawnRate = 0, _stripFrequencyOfSpawnRate = 0;   
        private int _countStrips = 0;

        private int _clickCount = 0, _totalStrips = 0, _deleteStripCount = 0;
        DateTime _timeStart, _timeEnd;

        private bool _stripInverted = false;

        public MainWindow()
        {
            InitializeComponent();
            _gameWindow = new Rectangle();
            _gameWindow.Width = Width;
            _gameWindow.Height = Height;
            _gameWindow.Fill = new SolidColorBrush(Colors.White);
            grid.Children.Add(_gameWindow);
            _gameWindow.MouseLeftButtonDown += GameWindowMouseLeftButtonDown;
            _gameSettings = new GameSettings();
            grid.Children.Add(_gameSettings);
            _gameSettings.SetingsAccepted += GamSetingsAccepted;

        }

        private void GamSetingsAccepted(int width, int height, int startStripeCount, int quantitySpawRate, int frequencySpawnRate)
        {
            _clickCount = 0;
            _totalStrips = 0;
            _deleteStripCount = 0;

            _stripWidth = width;
            _stripHeight = height;
            _startStripCount = startStripeCount;
            _stripQuantityOfSpawnRate = quantitySpawRate;
            _stripFrequencyOfSpawnRate = frequencySpawnRate;

            _gameSettings.SetingsAccepted -= GamSetingsAccepted;
            grid.Children.Remove(_gameSettings);

            _timeStart = DateTime.Now;
            AddStipes(_startStripCount);

            _timerSpawnStrips.Tick += TimerSpawnStripsTick;
            _timerSpawnStrips.Interval = TimeSpan.FromMilliseconds(_stripFrequencyOfSpawnRate);
            _timerSpawnStrips.Start();

        }

        private void TimerSpawnStripsTick(object sender, EventArgs e)
        {
            AddStipes(_stripQuantityOfSpawnRate);

            if (_countStrips > 2 * _startStripCount)
                GameLose();
        }

        private void AddStipes(int stripeCount)
        {
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
                _countStrips++;
            }
        }

        private void CreateSripe(int stripHeight, int stripWidth, Color color)
        {
            Rectangle rect = new Rectangle();
            rect.Width = stripWidth;
            rect.Height = stripHeight;

            rect.Fill = new SolidColorBrush(color);
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.HorizontalAlignment = HorizontalAlignment.Left;
            rect.VerticalAlignment = VerticalAlignment.Top;

            _gameWindow.Width = Width;
            _gameWindow.Height = Height;

            int posX = _random.Next((int)(this.Width - rect.Width) - 50);
            int posY = _random.Next((int)(this.Height - rect.Height) - 50);
            rect.Margin = new Thickness(posX, posY, 0, 0);

            rect.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => _clickCount++;
            rect.MouseLeftButtonDown += RectMouseLeftButtonDown;
            grid.Children.Add(rect);
        }

        private Color RandomColor()
        {
            _red = (byte)_random.Next(256);
            _green = (byte)_random.Next(256);
            _blue = (byte)_random.Next(256);
            return Color.FromArgb(255, _red, _green, _blue);
        }

        private void RectMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            Rectangle rect = (Rectangle)sender;
            int index = grid.Children.IndexOf(rect);
            Rect mainRect = new Rect(rect.Margin.Left, rect.Margin.Top, rect.Width, rect.Height);

            for (int i = index + 1; i < grid.Children.Count; i++)
            {
                Rectangle rectI = (Rectangle)grid.Children[i];
                Rect secondaryRect = new Rect(rectI.Margin.Left, rectI.Margin.Top, rectI.Width, rectI.Height);

                if (mainRect.IntersectsWith(secondaryRect)) return;
            }
            grid.Children.Remove(rect);
            _deleteStripCount++;
            _countStrips--;

            if (_countStrips == 0)
                GameWin();
        }

        private void ResetAllComponents()
        {
            _timerSpawnStrips.Stop();
            _timerSpawnStrips.Tick -= TimerSpawnStripsTick;

            _gameSettings.SetingsAccepted -= GamSetingsAccepted;
            _countStrips = 0;

            grid.Children.Clear();

            grid.Children.Add(_gameWindow);
            grid.Children.Add(_gameSettings);
            _gameSettings.SetingsAccepted += GamSetingsAccepted;
            _gameSettings.Visibility = Visibility.Hidden;


        }

        private void GameLose()
        {
            _timeEnd = DateTime.Now;
            string time = Math.Round((_timeEnd - _timeStart).TotalMilliseconds / 1000, 3).ToString();
            double clickAccuracy = 0;
            if (_clickCount != 0) clickAccuracy = Math.Round(_deleteStripCount / (double)_clickCount * 100, 2);
            ResetAllComponents();

            _loseWindow = new LoseWindow(_clickCount, time, clickAccuracy, (int)(Width / 2 - 199), (int)(Height / 2 - 225));
            grid.Children.Add(_loseWindow);
            _loseWindow.WindowClose += WindowClose;
        }

        private void GameWin()
        {
            _timeEnd = DateTime.Now;
            string time = Math.Round((_timeEnd - _timeStart).TotalMilliseconds / 1000, 3).ToString();
            double clickAccuracy = 0;
            if (_clickCount != 0) clickAccuracy = Math.Round( _totalStrips / (double)_clickCount * 100, 2);
            ResetAllComponents();
            _winWindow = new WinWindow(_clickCount, time, clickAccuracy, (int)(Width / 2 - 199), (int)(Height / 2 - 225));
            grid.Children.Add(_winWindow);
            _winWindow.WindowClose += WindowClose;

        }

        private void WindowClose()
        {
            grid.Children.Remove(_winWindow);
            grid.Children.Remove(_loseWindow);
            _gameSettings.Visibility = Visibility.Visible;
        }

        private void GameWindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _clickCount++;
        }
    }
}