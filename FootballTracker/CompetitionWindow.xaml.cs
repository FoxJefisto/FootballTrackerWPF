using FootballTracker.Database;
using FootballTracker.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using AppContext = FootballTracker.Model.AppContext;

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
        ObservableCollection<Season> seasons;
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
            GC.Collect(3, GCCollectionMode.Forced);
        }

        private void btnShowPlayerStats_Click(object sender, RoutedEventArgs e)
        {
            var competitionPlayersWindow = new CompetitionPlayersWindow(seasons[cbSeasons.SelectedIndex]);
            competitionPlayersWindow.Owner = this;
            this.Hide();
            competitionPlayersWindow.Show();
            GC.Collect(3, GCCollectionMode.Forced);
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
            GC.Collect(3, GCCollectionMode.Forced);
        }

        private void UpdateData(object sender, EventArgs e)
        {
            SetData();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            dispatcherTimer = null;
            GC.Collect(3, GCCollectionMode.Forced);
        }
    }
}
