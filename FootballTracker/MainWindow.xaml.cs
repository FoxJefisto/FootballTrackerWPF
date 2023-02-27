using FootballTracker.Database;
using FootballTracker.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FootballTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataBaseManager dbManager;
        DispatcherTimer dispatcherTimer;
        public MainWindow()
        {
            dbManager = DataBaseManager.GetInstance();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (!dbManager.HasConnection())
            {
                MessageBox.Show("Не удалось подключиться к базе данных", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            else
            {
                dispatcherTimer = new DispatcherTimer();
                dgCompetitions.ItemsSource = dbManager.GetCompetitions();
                dispatcherTimer.Tick += new EventHandler(UpdateData);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 60);
                dispatcherTimer.Start();
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
            GC.Collect(3, GCCollectionMode.Forced);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void dPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbManager.HasConnection())
            {
                SetData(dPicker.SelectedDate.Value);
            }
        }

        private void SetData(DateTime date)
        {
            var matches = dbManager.GetMatchesByDate(date);
            var matchesRow = matches.Select(x => new MatchRow(x)
            {
                Home = dbManager.GetClubByClubId(x.Statistics[0].ClubId),
                Away = dbManager.GetClubByClubId(x.Statistics[1].ClubId)
            });
            dgMatches.ItemsSource = matchesRow;
        }

        private void UpdateData(object sender, EventArgs e)
        {
            SetData(dPicker.SelectedDate.Value);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
                dispatcherTimer = null;
            }
        }
    }
}
