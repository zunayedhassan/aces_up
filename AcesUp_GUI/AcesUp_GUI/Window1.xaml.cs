using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AcesUp_CoreLibrary;

namespace AcesUp_GUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private AcesUp _acesUp = new AcesUp();

        private Canvas mainCanvas;
        private Border deckBorder;
        private CustomImage dealCustomImage;
        private static Color mouseEnterBorderColor = Colors.DarkOrange,
                             mouseLeaveBorderColor = Colors.Transparent;

        public Window1()
        {
            InitializeComponent();

            this.Title = "Aces Up";
            this.Width = 500;
            this.Height = 600;
            this.ResizeMode = ResizeMode.NoResize | ResizeMode.CanMinimize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Icon.ico", UriKind.RelativeOrAbsolute));

            mainCanvas = new Canvas();
            mainCanvas.Background = new SolidColorBrush(Colors.DarkGreen);

            deal();

            this.Content = mainCanvas;
        }

        private void dealCustomImage_MouseLeftButtonDown(Object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _acesUp.Deal();
            this.mainCanvas.Children.Clear();
            Simulate();
            deal();
        }

        private void deal()
        {
            dealCustomImage = new CustomImage("Cards/deck.png", 82, 106);

            deckBorder = new Border();
            deckBorder.BorderThickness = new Thickness(2);
            deckBorder.BorderBrush = new SolidColorBrush(mouseLeaveBorderColor);
            deckBorder.Width = dealCustomImage.Width + (deckBorder.BorderThickness.Left + 2);
            deckBorder.Height = dealCustomImage.Height + (deckBorder.BorderThickness.Left + 2);
            deckBorder.Margin = new Thickness(this.Width - dealCustomImage.Width - 30, this.Height - dealCustomImage.Height - 50, 0, 0);

            TextBlock scoreTextBlock = new TextBlock();
            scoreTextBlock.Foreground = new SolidColorBrush(Colors.DarkOrange);
            scoreTextBlock.FontSize = 16;
            scoreTextBlock.FontWeight = FontWeights.Bold;
            scoreTextBlock.Text = "SCORE: " + _acesUp.Score;
            scoreTextBlock.Margin = new Thickness(10, this.Width + 40, 0, 0);

            TextBlock aboutTextBlock = new TextBlock();
            aboutTextBlock.Text = "About";
            aboutTextBlock.Margin = new Thickness(deckBorder.Margin.Left + 30, 20, 0, 0);
            aboutTextBlock.FontSize = scoreTextBlock.FontSize;
            aboutTextBlock.Foreground = new SolidColorBrush(Colors.White);

            mainCanvas.Children.Add(deckBorder);
            deckBorder.Child = dealCustomImage;
            mainCanvas.Children.Add(scoreTextBlock);
            mainCanvas.Children.Add(aboutTextBlock);

            dealCustomImage.MouseLeftButtonDown += new MouseButtonEventHandler(dealCustomImage_MouseLeftButtonDown);
            deckBorder.MouseEnter += new MouseEventHandler(deckBorder_MouseEnter);
            deckBorder.MouseLeave += new MouseEventHandler(deckBorder_MouseLeave);
            aboutTextBlock.MouseEnter += new MouseEventHandler(aboutTextBlock_MouseEnter);
            aboutTextBlock.MouseLeave += new MouseEventHandler(aboutTextBlock_MouseLeave);
            aboutTextBlock.MouseLeftButtonDown += new MouseButtonEventHandler(aboutTextBlock_MouseLeftButtonDown);
        }

        private void Simulate()
        {
            try
            {
                for (int i = 0; i < _acesUp.StackOfPiles.Length; i++)
                {
                    int x = (i * 96) + 20;
                    int y = 10;

                    for (int j = 0; j < _acesUp.StackOfPiles[i].Count; j++)
                    {
                        CustomImage currentCardImage = new CustomImage(_acesUp.StackOfPiles[i][j]);
                        currentCardImage.Margin = new Thickness(x, y, 0, 0);
                        currentCardImage.pileIndex = i + 1;
                        y += 30;

                        if (j == _acesUp.StackOfPiles[i].Count() - 1)
                        {
                            currentCardImage.MouseLeftButtonDown += new MouseButtonEventHandler(delegate(object sender, MouseButtonEventArgs mouseButtonEventArgs)
                            {
                                if (_acesUp.IsGameOver())
                                {
                                    mouseEnterBorderColor
                                        =
                                        mouseLeaveBorderColor
                                        = Colors.Red;
                                    deckBorder.BorderBrush = new SolidColorBrush(mouseEnterBorderColor);

                                    if (_acesUp.Won)
                                        MessageBox.Show(
                                            "Game Over: You won.");
                                    else
                                        MessageBox.Show(
                                        "GAME OVER: No more moves");
                                }
                                else
                                {
                                    bool validToMove =
                                        false;


                                    int count = 0;
                                    foreach (var pile in _acesUp.StackOfPiles)
                                    {
                                        count++;
                                        if (pile.Count == 0)
                                        {
                                            validToMove =
                                                true;
                                            break;
                                        }
                                    }

                                    if (validToMove)
                                        _acesUp.MoveCard(currentCardImage.pileIndex, count);
                                    else
                                        _acesUp.RemoveCardFromPile(currentCardImage.pileIndex);
                                    this.Title =
                                        "ACES UP: Score : " +
                                        _acesUp.Score;
                                }

                                this.mainCanvas.Children.Clear();
                                deal();

                                Simulate();
                            });

                            currentCardImage.MouseEnter += new MouseEventHandler(delegate(object sender, MouseEventArgs mouseEventArgs)
                            {
                                currentCardImage.Cursor =
                                    Cursors.Hand;
                            });
                        }

                        mainCanvas.Children.Add(currentCardImage);
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing...
            }
        }

        private void deckBorder_MouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            deckBorder.BorderBrush = new SolidColorBrush(mouseEnterBorderColor);
        }

        private void deckBorder_MouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            deckBorder.BorderBrush = new SolidColorBrush(mouseLeaveBorderColor);
        }

        private void aboutTextBlock_MouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Black);
        }

        private void aboutTextBlock_MouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.White);
        }

        private void aboutTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            MessageBox.Show("Aces Up 1.0\n" +
                            "\n" +
                            "Mohammad Zunayed Hassan\n" +
                            "Email: zunayed-hassan@live.com", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
