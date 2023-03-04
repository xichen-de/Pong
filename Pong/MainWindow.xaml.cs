using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Pong
{
    public partial class MainWindow : Window
    {
        private const double BallSpeed = 5;
        private const double PlayerSpeed = 10;
        private const double ComputerPlayerSpeed = 7;

        private double _ballXDirection = 1;
        private double _ballYDirection = 1;
        private double _ballXPosition = 240;
        private double _ballYPosition = 140;

        private double _player1YPosition = 150;
        private double _player2YPosition = 150;

        private bool _isPlaying = false;

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!_isPlaying)
            {
                return;
            }

            MoveBall();
            MovePlayer1();
            MoveComputerPlayer();
            CheckCollisions();
            CheckGameOver();
        }

        private void MoveBall()
        {
            _ballXPosition += _ballXDirection * BallSpeed;
            _ballYPosition += _ballYDirection * BallSpeed;
            Canvas.SetLeft(Ball, _ballXPosition);
            Canvas.SetTop(Ball, _ballYPosition);
        }

        private void MovePlayer1()
        {
            if (Keyboard.IsKeyDown(Key.W) && _player1YPosition > 0)
            {
                _player1YPosition -= PlayerSpeed;
                Canvas.SetTop(Player1, _player1YPosition);
            }

            if (Keyboard.IsKeyDown(Key.S) && _player1YPosition + Player1.Height < GameCanvas.ActualHeight)
            {
                _player1YPosition += PlayerSpeed;
                Canvas.SetTop(Player1, _player1YPosition);
            }
        }

        private void MoveComputerPlayer()
        {
            double distanceToBall = Math.Abs(_ballYPosition - _player2YPosition - (Player2.Height / 2));
            double speed = ComputerPlayerSpeed;

            if (distanceToBall > Player2.Height / 2)
            {
                speed *= 1.5;
            }
            else if (distanceToBall < Player2.Height / 4)
            {
                speed /= 2;
            }

            if (_ballYPosition < _player2YPosition + (Player2.Height / 2) && _player2YPosition > 0)
            {
                _player2YPosition -= speed;
                Canvas.SetTop(Player2, _player2YPosition);
            }

            if (_ballYPosition > _player2YPosition + (Player2.Height / 2) &&
                _player2YPosition + Player2.Height < GameCanvas.ActualHeight)
            {
                _player2YPosition += speed;
                Canvas.SetTop(Player2, _player2YPosition);
            }
        }

        private void CheckCollisions()
        {
            if (_ballYPosition < 0 || _ballYPosition > GameCanvas.ActualHeight - Ball.Height)
            {
                _ballYDirection *= -1;
            }

            if (Canvas.GetLeft(Ball) < Canvas.GetLeft(Player1) + Player1.Width &&
                Canvas.GetLeft(Ball) + Ball.Width > Canvas.GetLeft(Player1) &&
                Canvas.GetTop(Ball) < _player1YPosition + Player1.Height &&
                Canvas.GetTop(Ball) + Ball.Height > _player1YPosition)
            {
                _ballXDirection *= -1;
            }

            if (Canvas.GetLeft(Ball) < Canvas.GetLeft(Player2) + Player2.Width &&
                Canvas.GetLeft(Ball) + Ball.Width > Canvas.GetLeft(Player2) &&
                Canvas.GetTop(Ball) < _player2YPosition + Player2.Height &&
                Canvas.GetTop(Ball) + Ball.Height > _player2YPosition)
            {
                _ballXDirection *= -1;
            }
        }

        private void ResetBall()
        {
            _ballXPosition = GameCanvas.ActualWidth / 2 - Ball.Width / 2;
            _ballYPosition = GameCanvas.ActualHeight / 2 - Ball.Height / 2;
            _ballXDirection = 1;
            _ballYDirection = 1;

            Canvas.SetLeft(Ball, _ballXPosition);
            Canvas.SetTop(Ball, _ballYPosition);
        }

        private void CheckGameOver()
        {
            if (_ballXPosition < 0 || _ballXPosition > GameCanvas.ActualWidth - Ball.Width)
            {
                _timer.Stop();
                MessageBoxResult result = MessageBox.Show("Game over! Do you want to play again?", "Game Over",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ResetBall();
                    _timer.Start();
                }
                else
                {
                    Close();
                }
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                _isPlaying = true;
                _ballXPosition = 240;
                _ballYPosition = 140;
                _ballXDirection = 1;
                _ballYDirection = 1;
                _player1YPosition = 150;
                _player2YPosition = 150;

                Canvas.SetLeft(Ball, _ballXPosition);
                Canvas.SetTop(Ball, _ballYPosition);
                Canvas.SetTop(Player1, _player1YPosition);
                Canvas.SetTop(Player2, _player2YPosition);

                _timer.Start();
            }
            else if (e.Key == Key.W && Canvas.GetTop(Player2) > 0)
            {
                _player2YPosition -= PlayerSpeed;
                Canvas.SetTop(Player2, _player2YPosition);
            }
            else if (e.Key == Key.S && Canvas.GetTop(Player2) + Player2.Height < GameCanvas.ActualHeight)
            {
                _player2YPosition += PlayerSpeed;
                Canvas.SetTop(Player2, _player2YPosition);
            }
        }
    }
}