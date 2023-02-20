using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballTracker.Model
{
    public class MatchEvent
    {
        public int? Id { get; set; }
        public string? Minute { get; set; }
        public string? Type { get; set; }

        public string? Label { get; set; }
        public string? PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player? Player { get; set; }
        public int? StatisticsId { get; set; }
        [ForeignKey("StatisticsId")]
        public MatchStatistics? Statistics { get; set; }
    }
}
