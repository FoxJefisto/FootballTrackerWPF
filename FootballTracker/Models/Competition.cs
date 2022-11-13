using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lesson1
{
    public class Competition
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string ImgSource { get; set; }
        public List<Season> Seasons { get; set; } = new List<Season>();
    }
}
