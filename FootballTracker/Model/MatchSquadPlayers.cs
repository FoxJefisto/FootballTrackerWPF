using System.ComponentModel.DataAnnotations.Schema;

namespace FootballTracker.Model
{
    public enum SquadType : byte
    {
        Lineup,
        Probably,
        Substitute,
        Unknown
    }
    public class MatchSquadPlayers
    {
        public int Id { get; set; }
        public int? Number { get; set; }
        public string Label { get; set; }
        public SquadType Type { get; set; }
        public bool IsCaptain { get; set; }
        public string PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }
        public int StatisticsId { get; set; }
        [ForeignKey("StatisticsId")]
        public MatchStatistics Statistics { get; set; }
    }
}
