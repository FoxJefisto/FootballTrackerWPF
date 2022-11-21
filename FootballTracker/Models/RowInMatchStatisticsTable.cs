using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballTracker.Models
{
    public class RowInMatchStatisticsTable
    {
        public string? HomeStats { get; set; }
        public string? Label { get; set; }
        public string? AwayStats { get; set; }

        public RowInMatchStatisticsTable(string? homeStats, string? label, string? awayStats)
        {
            this.HomeStats = homeStats;
            this.Label = label;
            this.AwayStats = awayStats;
        }
    }
}
