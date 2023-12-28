﻿using System;
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
        int akasztafaId = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {   
            if(akasztafaId <= 10)
            {
                akasztofaGrid.Children[akasztafaId].Visibility = Visibility;
                akasztafaId++;
            }
            else
            {
                MessageBox.Show("Vége", "Vége", MessageBoxButton.OK);
                akasztafaId = 0;
                for (int i = 0; i <= 10; i++)
                {
                    akasztofaGrid.Children[i].Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
