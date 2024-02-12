using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using System.IO;
using Akasztofa;
using Newtonsoft.Json;

namespace Akasztofa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int rightGuesses = 0;
        int akasztofaIndex = 0;
        int score = 0;
        int highscore = 0;
        int wordLength;
        string answer = "";
        List<string> lettersGuessed;
        string fileName;
        string highscoreFileName = "highscore.json";
        string theme;
        Dictionary<string, string> fileThemes = new Dictionary<string, string>
    {
        {"videogame.json", "Videójáték"},
        {"film.json", "Film"},
        {"sport.json", "Sport"},
        {"brand.json", "Márka"}
    };

        public MainWindow(string fileName)
        {
            InitializeComponent();
            wordLength = GenerateTheWord(fileName);
            theme = fileThemes[fileName];
            themeLabel.Content = theme;
            this.fileName = fileName;
            if (File.Exists(highscoreFileName))
            {
                string json = File.ReadAllText(highscoreFileName);
                Dictionary<string, int> highscores = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
                if (highscores.ContainsKey(theme))
                {
                    highscore = highscores[theme];
                }
            }
            highScoreLabel.Content = $"Legnagyobb pontszám: {highscore}";
        }

        private int GenerateTheWord(string fileName)
        {
            string json = File.ReadAllText(fileName);
            List<string> words = JsonConvert.DeserializeObject<List<string>>(json);
            Random r = new Random();
            int index = r.Next(words.Count);
            string word = words[index];
            word = word.ToUpper();
            answer = word;
            lettersGuessed = new List<string>();
            int length = 0;

            foreach (char character in word)
            {
                Label label = new Label();
                label.Content = character;
                label.Width = 30;
                label.Height = 50;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                if (character.ToString() != " ")
                {
                    label.BorderBrush = Brushes.Black;
                    label.BorderThickness = new Thickness(0, 0, 0, 5);
                    label.Margin = new Thickness(0, 0, 10, 0);
                    label.FontSize = 20;
                    label.Foreground = Brushes.Beige;
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
                    if (label.Content.ToString().Equals(letter))
                    {
                        label.Foreground = Brushes.Black;
                        rightGuesses++;
                        score++;
                        scoreLabel.Content = $"Pontszám: {score}";
                    }
                }
            }
            else
            {
                akasztofaGrid.Children[akasztofaIndex].Visibility = Visibility.Visible;
                usedLetters.Content += $"{letter} ";
                akasztofaIndex++;
            }


            if (rightGuesses == wordLength)
            {

                MessageBox.Show("Gratulálok Ön nyert!", "Nyert", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                if (score > highscore)
                {
                    highscore = score;

                    highScoreLabel.Content = $"Legnagyobb pontszám: {highscore}";
                }
                Restart();

            }

            if (akasztofaIndex == 11)
            {
                RevealAnswer();
                GameOverWindow gow = new GameOverWindow();
                gow.ShowDialog();
                //MessageBox.Show($"Game over, Pontszám: {score}", "Vége", MessageBoxButton.OK, MessageBoxImage.Error);
                if (score > highscore)
                {
                    highscore = score;
                    highScoreLabel.Content = $"Legnagyobb pontszám: {highscore}";
                }
                Restart();
                score = 0;
                scoreLabel.Content = $"Pontszám: 0";

            }
        }

        private void RevealAnswer()
        {
            foreach (Label label in wordSP.Children)
            {
                label.Foreground = Brushes.Black;
            }
        }

        private void Restart()
        {
            rightGuesses = 0;
            akasztofaIndex = 0;
            wordSP.Children.Clear();
            usedLetters.Content = "";
            wordLength = GenerateTheWord(fileName);
            foreach (UIElement akasztofaResz in akasztofaGrid.Children)
            {
                akasztofaResz.Visibility = Visibility.Hidden;
            }

        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Biztosan kilép? Elveszti a pontszámát!", "Kérdés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (File.Exists(highscoreFileName))
                {
                    string json = File.ReadAllText(highscoreFileName);
                    Dictionary<string, int> highscores = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
                    highscores[theme] = highscore;
                    json = JsonConvert.SerializeObject(highscores);
                    File.WriteAllText(highscoreFileName, json);
                }

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
