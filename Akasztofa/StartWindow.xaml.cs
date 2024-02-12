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
using System.Windows.Shapes;

namespace Akasztofa
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        MainWindow mainWindow;
        string fileName;

       public StartWindow()
        {
            InitializeComponent();
        }

        private void tema1BTN_Click(object sender, RoutedEventArgs e)
        {
            fileName = "videogame.json";
            mainWindow = new MainWindow(fileName);
            this.Close();
            mainWindow.ShowDialog();
        }

        private void tema2BTN_Click(object sender, RoutedEventArgs e)
        {
            fileName = "film.json";
            mainWindow = new MainWindow(fileName);
            this.Close();
            mainWindow.ShowDialog();
        }

        private void tema3BTN_Click(object sender, RoutedEventArgs e)
        {
            fileName = "sport.json";
            mainWindow = new MainWindow(fileName);
            this.Close();
            mainWindow.ShowDialog();
        }

        private void tema4BTN_Click(object sender, RoutedEventArgs e)
        {
            fileName = "brand.json";
            mainWindow = new MainWindow(fileName);
            this.Close();
            mainWindow.ShowDialog();
        }
    }
}
