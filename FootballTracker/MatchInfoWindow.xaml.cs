using FootballTracker.Controllers;
using FootballTracker.Models;
using lesson1;
using lesson1.Model;
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
using System.Windows.Threading;

namespace FootballTracker
{
    /// <summary>
    /// Логика взаимодействия для MatchInfoWindow.xaml
    /// </summary>
    public partial class MatchInfoWindow : Window
    {
        DispatcherTimer dispatcherTimer;
        DataBaseManager dbManager;
        MatchRow match;
        ColorWorker colorWorker;
        public MatchInfoWindow()
        {
            InitializeComponent();
        }

        public MatchInfoWindow(MatchRow match) : this()
        {
            this.match = match;
            this.dbManager = DataBaseManager.GetInstance();
            this.colorWorker = ColorWorker.GetInstance();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var items = colorWorker.GetBannerColors(dbManager.GetCompetitionByCompetitionId(match.Season.CompetitionId).ImgSource);
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
            Title = match.Label;
            tbTitle.Text = match.Label;
            spHome.DataContext = match.Home;
            spAway.DataContext = match.Away;
            SetData();
            if (!match.Status.Contains("Завершен"))
            {
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(UpdateData);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 60);
                dispatcherTimer.Start();
            }
        }

        private void SetData()
        {
            match = new MatchRow(dbManager.GetMatchByMatchId(match.Match.Id));
            match.Statistics.ForEach(x => { x.Events = dbManager.GetMatchEventsByStatisticsId(x); });
            spScore.DataContext = match;
            var tableStats = new MatchStatisticsTable(match.Statistics[0], match.Statistics[1]);
            dgStatistics.ItemsSource = tableStats.result;

            var tableEvents = new MatchEventsTable(match.Statistics[0].Events, match.Statistics[1].Events);
            dgEvents.ItemsSource = tableEvents.result;

            var typesHome = dbManager.GetSquadTypesByStatistics(match.Statistics[0]);
            var typesAway = dbManager.GetSquadTypesByStatistics(match.Statistics[1]);
            if (match.Status == "Ожидается")
            {
                gStatistics.Visibility = Visibility.Collapsed;
            }
            if (typesHome.Count == 0 && typesAway.Count == 0)
            {
                spHomeSquad.Visibility = Visibility.Collapsed;
                spAwaySquad.Visibility = Visibility.Collapsed;
            }
            cbHomeSquad.ItemsSource = typesHome;
            cbAwaySquad.ItemsSource = typesAway;
            cbHomeSquad.SelectedIndex = 0;
            cbAwaySquad.SelectedIndex = 0;
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

        private void tbPlayerName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            Player player = null;
            if (element.DataContext is MatchSquadPlayers ps)
            {
                player = ps.Player;
            }
            else if (element.DataContext is RowInMatchEvents row)
            {
                if(row.HomePlayer is Player p1)
                {
                    player = p1;
                }
                else if (row.AwayPlayer is Player p2)
                {
                    player = p2;
                }
            }
            if(player != null)
            {
                var playerInfoWindow = new PlayerInfoWindow(player);
                playerInfoWindow.Owner = this;
                this.Hide();
                playerInfoWindow.Show();
            }

        }

        private void cbHomeSquad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbHomeSquad.SelectedItem == null)
            {
                cbHomeSquad.SelectedIndex = 0;
            }
            else
            {
                var squadType = (SquadType)cbHomeSquad.SelectedItem;
                var players = dbManager.GetSquadPlayersByStatistics(match.Statistics[0], squadType);
                dgHomeSquad.ItemsSource = players;
            }

        }

        private void cbAwaySquad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbAwaySquad.SelectedItem == null)
            {
                cbAwaySquad.SelectedIndex = 0;
            }
            else
            {
                var squadType = (SquadType)cbAwaySquad.SelectedItem;
                var players = dbManager.GetSquadPlayersByStatistics(match.Statistics[1], squadType);
                dgAwaySquad.ItemsSource = players;
            }
        }

        private void UpdateData(object sender, EventArgs e)
        {
            if (match.Status.Contains("Завершен"))
            {
                dispatcherTimer.Stop();
            }
            else
            {
                SetData();
                GC.Collect();
            }
        }
    }
}
