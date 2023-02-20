using FootballTracker.Database;
using FootballTracker.Model;
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
    /// Логика взаимодействия для PlayerInfoWindow.xaml
    /// </summary>
    public partial class PlayerInfoWindow : Window
    {
        Player player;
        DataBaseManager dbManager;
        ColorWorker colorWorker;
        public PlayerInfoWindow()
        {
            InitializeComponent();
        }

        public PlayerInfoWindow(Player player) : this()
        {
            this.player = player;
            this.dbManager = DataBaseManager.GetInstance();
            colorWorker = ColorWorker.GetInstance();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var playerStatistics = dbManager.GetPlayerStatisticsByPlayer(player);
            var items = colorWorker.GetBannerColors(playerStatistics.First().Club.ImgSource);
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
            Title = String.Join(' ', player.FirstName, player.LastName);
            tbTitle.Text = String.Join(' ', player.FirstName, player.LastName); ;
            spInfo.DataContext = player;
            dgStats.ItemsSource = playerStatistics;
            dgResultStats.ItemsSource = dbManager.GetResultsByPlayerStatistics(playerStatistics);
            dgClubs.ItemsSource = dbManager.GetCurrentClubsByPlayer(player);
            var country = dbManager.GetCountryByName(player.Citizenship);
            if(country is null)
            {
                country = new FootballClub { Name = player.Citizenship };
            }
            spCountry.DataContext = country;
            tbAge.Text = $"({dbManager.GetAgeString(player.DateOfBirth)})";
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
