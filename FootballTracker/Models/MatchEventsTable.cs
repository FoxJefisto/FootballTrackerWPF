using lesson1.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballTracker.Models
{
    public class MatchEventsTable
    {
        public List<RowInMatchEvents> result { get; set; }
        public MatchEventsTable(List<MatchEvent> home, List<MatchEvent> away)
        {
            result = new List<RowInMatchEvents>();
            foreach(var row in home.OrderBy(x => x.Minute))
            {
                result.Add(new RowInMatchEvents(row.Player, row.Label, row.Type, row.Minute, null, null));
            }
            foreach(var row in away.OrderBy(x => x.Minute))
            {
                result.Add(new RowInMatchEvents(null, null, row.Type, row.Minute, row.Player, row.Label));
            }
            result = result.OrderBy(x => x.Minute).ThenByDescending(x => x.Type).ToList();
        }
    }
}
