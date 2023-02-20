using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballTracker.Model
{
    public class FootballClub
    {
        public string? Id { get; set; }
        [Column("ClubName")]
        public string? Name { get; set; }
        public string? NameEnglish { get; set; }
        public string? FullName { get; set; }
        public string? MainCoach { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Stadium { get; set; }
        public string? FoundationDate { get; set; }
        public int? Rating { get; set; }
        public string? ImgSource { get; set; }
        public ObservableCollection<FootballClubSeason> ClubsSeasons { get; set; } = new ObservableCollection<FootballClubSeason>();
    }
}
