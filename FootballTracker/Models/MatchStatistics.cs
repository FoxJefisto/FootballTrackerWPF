using FootballTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lesson1.Model
{
    public enum HomeAway : byte
    {
        Home,
        Away
    }

    public class MatchStatistics
    {
        public int Id { get; set; }
        public string? MatchId { get; set; }
        [ForeignKey("MatchId")]
        public FootballMatch? Match { get; set; }
        public string? ClubId { get; set; }
        [ForeignKey("ClubId")]
        public FootballClub? Club { get; set; }
        public List<MatchEvent>? Events { get; set; }
        public List<MatchSquadPlayers>? Squad { get; set; }
        public HomeAway HomeAway { get; set; }
        public int? Goals { get; set; }
        [Statistic]
        public double? Xg { get; set; }
        [Statistic]
        public int? Shots { get; set; }
        [Statistic]
        public int? ShotsOnTarget { get; set; }
        [Statistic]
        public int? ShotsBlocked { get; set; }
        [Statistic]
        public int? Saves { get; set; }
        [Statistic]
        public int? BallPossession { get; set; }
        [Statistic]
        public int? Corners { get; set; }
        [Statistic]
        public int? Fouls { get; set; }
        [Statistic]
        public int? Offsides { get; set; }
        [Statistic]
        public int? YCards { get; set; }
        [Statistic]
        public int? RCards { get; set; }
        [Statistic]
        public int? Attacks { get; set; }
        [Statistic]
        public int? AttacksDangerous { get; set; }
        [Statistic]
        public int? Passes { get; set; }
        [Statistic]
        public double? AccPasses { get; set; }
        [Statistic]
        public int? FreeKicks { get; set; }
        [Statistic]
        public int? Prowing { get; set; }
        [Statistic]
        public int? Crosses { get; set; }
        [Statistic]
        public int? Tackles { get; set; }
    }
}
