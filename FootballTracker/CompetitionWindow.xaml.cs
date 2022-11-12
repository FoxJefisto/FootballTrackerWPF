using FootballTracker.Controllers;
using lesson1;
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
        Competition currentComp;
        DataBaseManager dbManager;
        List<Season> seasons;
        public CompetitionWindow()
        {
            InitializeComponent();
        }

        public CompetitionWindow(Competition comp) : this()
        {
            this.currentComp = comp;
            this.dbManager = DataBaseManager.GetInstance();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = currentComp.Name;
            tbTitle.Text = currentComp.Name;
            tbCompName.Text = currentComp.Name;
            tbCompCountry.Text = currentComp.Country;
            seasons = dbManager.GetSeasonsByComp(currentComp);
            cbSeasons.ItemsSource = seasons;
            cbSeasons.SelectedItem = seasons[0];
        }

        private void cbSeasons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var season = cbSeasons.SelectedItem as Season;
            dgCompetitionTable.ItemsSource = dbManager.GetCompetitionTable(season);
            dgBombarders.ItemsSource = dbManager.GetBombarders(season);
            dgAssistants.ItemsSource = dbManager.GetAssistants(season);
            dgRudePlayers.ItemsSource = dbManager.GetRudePlayers(season);
        }

        private void tbClubName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            FootballClub club = null;
            if(tb.DataContext is CompetitionTable ct)
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
    }
}
