using FootballTracker.Database;
using FootballTracker.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Xml.Linq;
using Application = System.Windows.Application;

namespace FootballTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            WalkDictionary(this.Resources);

            base.OnStartup(e);
        }

        private static void WalkDictionary(ResourceDictionary resources)
        {
            foreach (DictionaryEntry entry in resources)
            {
            }

            foreach (ResourceDictionary rd in resources.MergedDictionaries)
                WalkDictionary(rd);
        }
        DataBaseManager dbManager = DataBaseManager.GetInstance();

        private void tbCompetition_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            Competition competition = null;
            if (element.DataContext is Competition ct)
            {
                competition = ct;
            }
            else if (element.DataContext is MatchRow mr)
            {
                competition = mr.Season.Competition;
            }
            if (competition != null)
            {
                var compWindow = new CompetitionWindow(competition);
                var window = Window.GetWindow(sender as DependencyObject);
                compWindow.Owner = window;
                compWindow.Show();
                window.Hide();
            }
            GC.Collect(3, GCCollectionMode.Forced);
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
            else if (element.DataContext is Player p)
            {
                club = dbManager.GetCountryByName(p.Citizenship);
            }
            else if (element.DataContext is FootballClub c)
            {
                club = c;
            }
            if (club != null)
            {
                var window = Window.GetWindow(sender as DependencyObject);
                var clubInfoWindow = new ClubInfoWindow(club);
                clubInfoWindow.Owner = window;
                window.Hide();
                clubInfoWindow.Show();
            }
            GC.Collect(3, GCCollectionMode.Forced);
        }

        private void tbMatch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element.DataContext is MatchRow mr)
            {
                var window = Window.GetWindow(sender as DependencyObject);
                var matchInfoWindow = new MatchInfoWindow(mr);
                matchInfoWindow.Owner = window;
                window.Hide();
                matchInfoWindow.Show();
            }
            GC.Collect(3, GCCollectionMode.Forced);
        }

        private void tbPlayerName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            Player player = null;
            if (element.DataContext is MatchSquadPlayers msp)
            {
                player = msp.Player;
            }
            else if (element.DataContext is RowInMatchEvents row)
            {
                if (row.HomePlayer is Player p1)
                {
                    player = p1;
                }
                else if (row.AwayPlayer is Player p2)
                {
                    player = p2;
                }
            }
            else if (element.DataContext is PlayerStatistics ps)
            {
                player = ps.PlayerName;
            }
            if (player != null)
            {
                var window = Window.GetWindow(sender as DependencyObject);
                var clubInfoWindow = new PlayerInfoWindow(player);
                clubInfoWindow.Owner = window;
                window.Hide();
                clubInfoWindow.Show();
            }
            GC.Collect(3, GCCollectionMode.Forced);
        }

        private void iconClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Current.Shutdown();
        }

        private void iconBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(sender as DependencyObject);
            window.Owner.Show();
            window.Close();
        }
    }
}
