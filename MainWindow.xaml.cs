using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MATCHGAME
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Drawing.Color color;
        private bool findingMatch = false;
        private TextBlock lastTextBlockClicked;
        private int matchesFound;
        private bool playButtonClicked = false;
        private int tenthsOfSecondElapsed;
        private DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
        }

        // create a list of emojis or words at random. Populates the 16 grids squares.
        public void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
             {
              "✔","✔",
              "❤","❤",
              "🎶","🎶",
              "🐱‍🐉","🐱‍🐉",
              "💋","💋",
              "👀","👀",
              "🎂","🎂",
              "🐱‍👤","🐱‍👤"
            };
            //if they check the play hard box thislist of items will be used instead.
            if (ChkBx.IsChecked == true)
            {
                animalEmoji = new List<string>()
             {
              "Red","Red",
              "Green","Green",
              "Blue","Blue",
              "Yellow","Yellow",
              "Orange","Orange",
              "Purple","Purple",
              "Teal","Teal",
              "Brown","Brown"
              };
            }

            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
                if (textBlock.Name != "timerTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                    //if they check the play hard box the colors will be chosen at random.
                    if (ChkBx.IsChecked == true)
                    {
                        Random randomGen = new Random();
                        KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                        KnownColor randomColorName = names[randomGen.Next(names.Length)];

                        System.Drawing.Color randomColor = System.Drawing.Color.FromKnownColor(randomColorName);
                        color = randomColor;
                        System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);

                        Brush fColor = new SolidColorBrush(newColor);

                        textBlock.Foreground = fColor;
                        textBlock.Background = new SolidColorBrush(Colors.Beige);
                    }
                    else
                    {
                        textBlock.Foreground = new SolidColorBrush(Colors.Black);
                        textBlock.Background = new SolidColorBrush(Colors.White);
                    };
                }
            tenthsOfSecondElapsed = 0;
            matchesFound = 0;
        }

        // click reset to set everything back to default
        private void Is_Reset(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            tenthsOfSecondElapsed = 0;
            timerTextBlock.Text = "0.00";
            playPause.Content = "Play";
            SetUpGame();
        }

        // play pause toggle
        private void Play_Button(object sender, RoutedEventArgs e)
        {
            playButtonClicked = !playButtonClicked;

            if (playButtonClicked == true & tenthsOfSecondElapsed == 0.00 || matchesFound == 8)
            {
                SetUpGame();
                timer.Start();
                playPause.Content = "Pause";
            }
            else if (playButtonClicked == false & tenthsOfSecondElapsed > 0.00)
            {
                timer.Stop();
                playPause.Content = "Play";
            }
            else
            {
                timer.Start();
                playPause.Content = "Pause";
            }
        }

        // controls matching in the grid.
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (playPause.Content == "Pause")
            {
                TextBlock textBlock = sender as TextBlock;
                if (findingMatch == false)
                {
                    textBlock.Visibility = Visibility.Hidden;
                    lastTextBlockClicked = textBlock;
                    findingMatch = true;
                }
                else if (textBlock.Text == lastTextBlockClicked.Text)
                {
                    matchesFound++;
                    textBlock.Visibility = Visibility.Hidden;
                    findingMatch = false;
                }
                else
                {
                    lastTextBlockClicked.Visibility = Visibility.Visible;
                    findingMatch = false;
                }
            }
        }

        // controls the timer text box and timmer output
        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondElapsed++;
            timerTextBlock.Text = (tenthsOfSecondElapsed / 10f).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timerTextBlock.Text = timerTextBlock.Text + " - Play again ?";
                playPause.Content = "Play";
            }
        }
        // the play again when all matches are met.
        private void timerTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}