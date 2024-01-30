using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Akasztofa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> words = new List<string> { "EEE EEEEEEE EEEE", "AAAA" };
        int rightGuesses = 0;
        int akasztofaIndex = 0;
        int score = 0;
        int wordLength;
        string answer = "";
        List<string> lettersGuessed;
        public MainWindow(string theme)
        {
            InitializeComponent();
            wordLength = GenerateTheWord();
            themeLabel.Content = theme;
        }

        private int GenerateTheWord() //max 16 char
        {
            Random r = new Random();
            int index = r.Next(words.Count);
            int length = 0;
            string word = words[index];
            answer = word;
            lettersGuessed = new List<string>();

            foreach (char character in word)
            {
                Label label = new Label();
                label.Content = character;
                label.Width = 30;
                label.Height = 50;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                if(character.ToString() != " ")
                {
                    label.BorderBrush = Brushes.Black;
                    label.BorderThickness = new Thickness(0, 0, 0, 5);
                    label.Margin = new Thickness(0, 0, 10, 0);
                    label.FontSize = 20;
                    label.Foreground = Brushes.White;
                    length++;
                }

                wordSP.Children.Add(label);
                
            }
            return length;
        }

        private void guessBtn_Click(object sender, RoutedEventArgs e)
        {
            string letter = wordTB.Text.ToUpper();
            if (!string.IsNullOrEmpty(wordTB.Text) && letter.All(Char.IsLetter))
            {
                if (!lettersGuessed.Contains(letter))
                {
                    lettersGuessed.Add(letter);
                    CheckifLetter_inAnswer(letter);
                    wordTB.Text = "";
                    wordTB.Focus();
                }
                else
                {
                    MessageBox.Show("Ezt a betűt már próbálta!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    wordTB.Text = "";
                    wordTB.Focus();
                }
            }
            else
            {
                MessageBox.Show("Csak betűket lehet megadni!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                wordTB.Text = "";
                wordTB.Focus();
            }

        }

        private void CheckifLetter_inAnswer(string letter)
        {
            if (answer.Contains(letter))
            {
                foreach (Label label in wordSP.Children)
                {
                    if (label.Content.ToString() == letter)
                    {
                        label.Foreground = Brushes.Black;
                        rightGuesses++;
                        score++;
                        scoreLabel.Content = $"Pontszám: {score}";
                    }
                }
            }
            else //nincs benne ilyen betű
            {
                akasztofaGrid.Children[akasztofaIndex].Visibility = Visibility.Visible;
                usedLetters.Content += $"{letter} ";
                akasztofaIndex++;
            }


            //győzelem
            if (rightGuesses == wordLength)
            {

                MessageBox.Show("Nyert", "Nyert", MessageBoxButton.OK);
                Restart();

            }

            //Game over
            if (akasztofaIndex == 11)
            {
                MessageBox.Show($"Game over, Pontszám: {score}", "Vége", MessageBoxButton.OK, MessageBoxImage.Error);
                Restart();
                score = 0;
                scoreLabel.Content = $"Pontszám: 0";

            }
        }

        private void Restart()
        {
            rightGuesses = 0;
            akasztofaIndex = 0;
            wordSP.Children.Clear();
            usedLetters.Content = "";
            wordLength = GenerateTheWord();

            foreach (UIElement akasztofaResz in akasztofaGrid.Children)
            {
                akasztofaResz.Visibility = Visibility.Hidden;
            }

        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Biztosan kilép? Elveszti a pontszámát!", "Kérdés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                StartWindow startWindow = new StartWindow();
                this.Close();
                startWindow.ShowDialog();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                guessBtn_Click(sender,e);
            }

            if(e.Key == Key.Escape)
            {
                closeBtn_Click(sender, e);
            }
        }
    }
}
