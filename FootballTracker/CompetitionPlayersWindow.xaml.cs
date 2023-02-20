using FootballTracker.Database;
using FootballTracker.Model;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using System.Windows.Threading;

namespace FootballTracker
{
    /// <summary>
    /// Логика взаимодействия для CompetitionPlayersWindow.xaml
    /// </summary>
    public partial class CompetitionPlayersWindow : Window
    {
        DataBaseManager dbManager;
        Season season;
        ColorWorker colorWorker;
        public CompetitionPlayersWindow()
        {
            InitializeComponent();
        }

        public CompetitionPlayersWindow(Season season): this()
        {
            this.season = season;
            dbManager = DataBaseManager.GetInstance();
            colorWorker = ColorWorker.GetInstance();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var items = colorWorker.GetBannerColors(season.Competition.ImgSource);
            gBanner.Background = items.background;
            foreach (var control in gBanner.Children)
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
            Title = $"{season.Competition.Name} Сезон: {season.Year}";
            tbTitle.Text = $"{season.Competition.Name} Сезон: {season.Year}";
            dgPlayerStats.ItemsSource = dbManager.GetPlayerStatisticsBySeason(season); 
        }

        private void tbClubName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            FootballClub club = null;
            if (tb.DataContext is CompetitionTable ct)
            {
                club = ct.Club;
            }
            else if (tb.DataContext is PlayerStatistics ps)
            {
                club = ps.Club;
            }
            var clubInfoWindow = new ClubInfoWindow(club);
            clubInfoWindow.Owner = this;
            this.Hide();
            clubInfoWindow.Show();
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
