using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballTracker.Model
{
    public class Season
    {
        public int Id { get; set; }

        public string? Year { get; set; }

        public string? CompetitionId { get; set; }
        [ForeignKey("CompetitionId")]
        public Competition? Competition { get; set; }
        public ObservableCollection<PlayerStatistics>? PlayerStatistics { get; set; }
        public ObservableCollection<FootballMatch> Matches { get; set; }
        public ObservableCollection<CompetitionTable> Table { get; set; } = new ObservableCollection<CompetitionTable>();
        public ObservableCollection<FootballClubSeason> ClubsSeasons { get; set; } = new ObservableCollection<FootballClubSeason>();
    }
}
