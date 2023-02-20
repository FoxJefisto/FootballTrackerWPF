using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FootballTracker.Model
{
    public class Competition
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string ImgSource { get; set; }
        public ObservableCollection<Season> Seasons { get; set; } = new ObservableCollection<Season>();
    }
}
