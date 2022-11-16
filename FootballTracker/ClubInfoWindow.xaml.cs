using FootballTracker.Controllers;
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


            Title = club.Name;
            tbTitle.Text = club.Name;
            spInfo.DataContext = club;
            cbSeasons.ItemsSource = dbManager.GetSeasonsYearsByClub(club);
            cbSeasons.SelectedIndex = 0;
            var country = dbManager.GetCountryByName(club.Country);
            if(country == null)
            {
                spCountry.DataContext = new {Name = club.Country};
            }
            else
            {
                spCountry.DataContext = country;
            }
        }

        private void cbSeasons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var seasonYear = cbSeasons.SelectedItem as string;
            var players = dbManager.GetSquadPlayers(club, seasonYear);
            dgSquad.ItemsSource = players;
            var comps = dbManager.GetCompetitionsByClubYear(club, seasonYear);
            dgComps.ItemsSource = comps;
        }

        private void tbPlayerName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element.DataContext is PlayerStatistics ps && ps.PlayerName is Player player)
            {
                var clubInfoWindow = new PlayerInfoWindow(player);
                clubInfoWindow.Owner = this;
                this.Hide();
                clubInfoWindow.Show();
            }

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

        private void tbClubName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            FootballClub club = null;
            if (element.DataContext is FootballClub fc)
            {
                club = fc;
            }
            else if (element.DataContext is Player p)
            {
                club = dbManager.GetCountryByName(p.Citizenship);
            }
            if (club != null)
            {
                var clubInfoWindow = new ClubInfoWindow(club);
                clubInfoWindow.Owner = this;
                this.Hide();
                clubInfoWindow.Show();
            }
        }

        private void tbCompetition_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element.DataContext is Competition comp)
            {
                var competitionWindow = new CompetitionWindow(comp);
                competitionWindow.Owner = this;
                this.Hide();
                competitionWindow.Show();
            }
        }
    }
}
