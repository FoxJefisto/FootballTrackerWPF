using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballTracker.Model
{
    public class FootballMatch
    {
        public string? Id { get; set; }
        public string? Label { get; set; }
        public string? Stage { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public Season Season { get; set; }
        public ObservableCollection<MatchStatistics>? Statistics { get; set; }
    }
}
