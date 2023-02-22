using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballTracker.Model
{
    public class MatchRow
    {
        public MatchRow(FootballMatch match)
        {
            Match = match;
            Label = match.Label;
            Season = match.Season;
            Statistics = match.Statistics;
            IsLive = true;
            if (match.Statistics[0].Goals == null && match.Statistics[1].Goals == null)
            {
                Score = "- : -";
            }
            else
            {
                Score = $"{match.Statistics[0].Goals} : {match.Statistics[1].Goals}";
            }
            Status = match.Status;
            if (match.Status.Contains("Завершен") || match.Status == "Ожидается")
            {
                StatusLabel = $"{match.Date.Value:g}";
                IsLive = false;
            }
            else
            {
                StatusLabel = match.Status;
            }
            Date = match.Date;
            if(Season != null && Season.Competition != null)
            {
                Season.Competition.Name = String.Join(' ', Season.Competition.Name.Split(' ').TakeLast(2));
            }
        }
        public FootballMatch? Match { get; set; }
        public FootballClub? Home { get; set; }
        public FootballClub? Away { get; set; }
        public string? Score { get; set; }
        public string? Label { get; set; }
        public Season? Season { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
        public bool IsLive { get; set; }

        public string StatusLabel { get; set; }
        public ObservableCollection<MatchStatistics> Statistics { get; set; }
    }
}
