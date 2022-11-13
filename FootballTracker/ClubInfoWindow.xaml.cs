﻿using FootballTracker.Controllers;
using ImageMagick;
using lesson1;
using MaterialDesignThemes.Wpf;
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

namespace FootballTracker
{
    /// <summary>
    /// Логика взаимодействия для ClubInfoWindow.xaml
    /// </summary>
    public partial class ClubInfoWindow : Window
    {
        DataBaseManager dbManager;
        FootballClub club;
        ColorWorker colorWorker;
        public ClubInfoWindow()
        {
            InitializeComponent();
        }

        public ClubInfoWindow(FootballClub club) : this()
        {
            this.club = club;
            this.dbManager = DataBaseManager.GetInstance();
            colorWorker = ColorWorker.GetInstance();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var items = colorWorker.GetBannerColors(club.ImgSource);
            gBanner.Background = items.background;
            foreach(var control in gBanner.Children)
            {
                if (control is TextBlock tb)
                {
                    tb.Foreground = items.foreground1;
                }
                else if (control is PackIcon pi)
                {
                    pi.Foreground = items.foreground2;
                }
            }

            
            Title = club.Name;
            tbTitle.Text = club.Name;
            spInfo.DataContext = club;
            cbSeasons.ItemsSource = dbManager.GetSeasonsByClub(club);
            cbSeasons.SelectedIndex = 0;
        }

        private void cbSeasons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var season = cbSeasons.SelectedItem as Season;
            dgSquad.ItemsSource = dbManager.GetSquadPlayers(club, season);
        }

        private void tbPlayerName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            var clubInfoWindow = new PlayerInfoWindow((tb.DataContext as PlayerStatistics).PlayerName);
            clubInfoWindow.Owner = this;
            this.Hide();
            clubInfoWindow.Show();
        }

        private void iconClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void iconBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
