using lesson1;
using lesson1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballTracker.Models
{
    public class MatchRow
    {
        public MatchRow(FootballMatch match)
        {
            Match = match;
            Label = match.Label;
            Season = match.Season;
            Statistics = match.Statistics;
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
            }
            else
            {
                StatusLabel = match.Status;
            }
            Date = match.Date;

        }
        public FootballMatch? Match { get; set; }
        public FootballClub? Home { get; set; }
        public FootballClub? Away { get; set; }
        public string? Score { get; set; }
        public string? Label { get; set; }
        public Season? Season { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }

        public string StatusLabel { get; set; }
        public List<MatchStatistics> Statistics { get; set; }
    }
}
