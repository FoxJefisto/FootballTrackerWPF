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

namespace FootballTracker
{
    /// <summary>
    /// Логика взаимодействия для PlayerInfoWindow.xaml
    /// </summary>
    public partial class PlayerInfoWindow : Window
    {
        Player player;
        DataBaseManager dbManager;
        public PlayerInfoWindow()
        {
            InitializeComponent();
        }

        public PlayerInfoWindow(Player player): this()
        {
            this.player = player;
            this.dbManager = DataBaseManager.GetInstance();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = String.Join(' ', player.FirstName, player.LastName);
            tbTitle.Text = String.Join(' ', player.FirstName, player.LastName); ;
            spInfo.DataContext = player;
            dgStats.ItemsSource = dbManager.GetPlayerStatisticsByPlayer(player);
        }

        private void tbCompetition_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            var compWindow = new CompetitionWindow((tb.DataContext as PlayerStatistics).Season.Competition);
            compWindow.Owner = this;
            compWindow.Show();
        }

        private void tbClubName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            var clubInfoWindow = new ClubInfoWindow((tb.DataContext as PlayerStatistics).Club);
            clubInfoWindow.Owner = this;
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
