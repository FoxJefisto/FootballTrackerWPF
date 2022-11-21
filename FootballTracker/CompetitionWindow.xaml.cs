using FootballTracker.Controllers;
using FootballTracker.Models;
using lesson1;
using lesson1.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
using AppContext = FootballTracker.Models.AppContext;

namespace FootballTracker
{
    /// <summary>
    /// Логика взаимодействия для CompetitionWindow.xaml
    /// </summary>
    public partial class CompetitionWindow : Window
    {
        DispatcherTimer dispatcherTimer;
        Competition competition;
        DataBaseManager dbManager;
        List<Season> seasons;
        Season season;
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
            season = seasons[0];
            cbSeasons.ItemsSource = seasons;
            cbSeasons.SelectedItem = season;
            spCountry.DataContext = dbManager.GetCountryByName(competition.Country);
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(UpdateData);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 60);
            dispatcherTimer.Start();
        }

        private void cbSeasons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            season = cbSeasons.SelectedItem as Season;
            dgClubs.ItemsSource = dbManager.GetClubsBySeason(season);
            SetData();
        }

        private void SetData()
        {
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
            var matches = dbManager.GetMatchesBySeason(season);
            var matchesRow = matches.Select(x => new MatchRow(x)
            {
                Home = dbManager.GetClubByClubId(x.Statistics[0].ClubId),
                Away = dbManager.GetClubByClubId(x.Statistics[1].ClubId)
            });
            dgMatches.ItemsSource = matchesRow.OrderByDescending(x => x.Date);
        }

        private void tbClubName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            FootballClub club = null;
            if (element.DataContext is CompetitionTable ct)
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
            if (element.DataContext is PlayerStatistics ps && ps.PlayerName is Player player)
            {
                var clubInfoWindow = new PlayerInfoWindow(player);
                clubInfoWindow.Owner = this;
                this.Hide();
                clubInfoWindow.Show();
            }
        }


        private void MatchName_DataGridRowSelected(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element.DataContext is MatchRow mr)
            {
                var matchInfoWindow = new MatchInfoWindow(mr);
                matchInfoWindow.Owner = this;
                this.Hide();
                matchInfoWindow.Show();
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

        private void UpdateData(object sender, EventArgs e)
        {
            SetData();
            GC.Collect();
        }
    }
}
