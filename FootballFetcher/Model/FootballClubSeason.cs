using System.ComponentModel.DataAnnotations.Schema;

namespace FootballFetcher.Model
{
    class FootballClubSeason
    {
        public string ClubId { get; set; }
        [ForeignKey("ClubId")]
        public FootballClub Club { get; set; }
        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public Season Season { get; set; }
    }
}
