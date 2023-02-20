using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballTracker.Model
{
    public class Player
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }
        public string? WorkingLeg { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string? OriginalName { get; set; }
        public string? Citizenship { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? ImgSource { get; set; }
        public ObservableCollection<PlayerStatistics>? PlayerStatistics { get; set; }
    }
}
