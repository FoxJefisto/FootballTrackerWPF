using FootballTracker.Controllers;
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
using AppContext = FootballTracker.Models.AppContext;

namespace FootballTracker
{
    /// <summary>
    /// Логика взаимодействия для CompetitionWindow.xaml
    /// </summary>
    public partial class CompetitionWindow : Window
    {
        Competition competition;
        DataBaseManager dbManager;
        List<Season> seasons;
        ColorWorker colorWorker;
        public CompetitionWindow()
        {
            InitializeComponent();
        }

        public CompetitionWindow(Competition comp) : this()
        {
            this.competition = comp;
            this.dbManager = DataBaseManager.GetInstance();
            colorWorker = ColorWorker.GetInstance();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var items = colorWorker.GetBannerColors(competition.ImgSource);
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
            Title = competition.Name;
            tbTitle.Text = competition.Name;
            spInfo.DataContext = competition;
            seasons = dbManager.GetSeasonsByComp(competition);
            cbSeasons.ItemsSource = seasons;
            cbSeasons.SelectedItem = seasons[0];
            spCountry.DataContext = dbManager.GetCountryByName(competition.Country);
        }

        private void cbSeasons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var season = cbSeasons.SelectedItem as Season;
            var groups = dbManager.GetCompetitionGroupNames(season);
            if (groups.Count > 1)
            {
                cbGroups.ItemsSource = groups;
                cbGroups.SelectedItem = groups[1];
                cbGroups.SelectedItem = groups[0];
                cbGroups.Visibility = Visibility.Visible;
            }
            else
            {
                dgCompetitionTable.ItemsSource = dbManager.GetCompetitionTable(season);
                cbGroups.Visibility = Visibility.Collapsed;
            }
            dgBombarders.ItemsSource = dbManager.GetBombarders(season);
            dgAssistants.ItemsSource = dbManager.GetAssistants(season);
            dgRudePlayers.ItemsSource = dbManager.GetRudePlayers(season);
        }

        private void tbClubName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            FootballClub club = null;
            if(element.DataContext is CompetitionTable ct)
            {
                club = ct.Club;
            } 
            else if (element.DataContext is PlayerStatistics ps)
            {
                club = ps.Club;
            }
            else if (element.DataContext is FootballClub c)
            {
                club = c;
            }
            if (club != null)
            {
                var clubInfoWindow = new ClubInfoWindow(club);
                clubInfoWindow.Owner = this;
                this.Hide();
                clubInfoWindow.Show();
            }

        }

        private void tbPlayerName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            if(element.DataContext is PlayerStatistics ps && ps.PlayerName is Player player)
            {
                var clubInfoWindow = new PlayerInfoWindow(player);
                clubInfoWindow.Owner = this;
                this.Hide();
                clubInfoWindow.Show();
            }
        }

        private void btnShowPlayerStats_Click(object sender, RoutedEventArgs e)
        {
            var competitionPlayersWindow = new CompetitionPlayersWindow(seasons[cbSeasons.SelectedIndex]);
            competitionPlayersWindow.Owner = this;
            this.Hide();
            competitionPlayersWindow.Show();
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

        private void cbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var groupName = cbGroups.SelectedItem as string;
            var season = cbSeasons.SelectedItem as Season;
            var table = dbManager.GetCompetitionTableByGroupName(season, groupName);
            dgCompetitionTable.ItemsSource = table;
        }
    }
}
