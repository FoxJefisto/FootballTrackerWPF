using FootballFetcher.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AppContext = FootballFetcher.Model.AppContext;

namespace FootballFetcher.Database
{
    class DatabaseManager
    {
        private static DatabaseManager instance;

        public static DatabaseManager GetInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseManager();
            }
            return instance;
        }

        public List<Season> GetSeasonsByCompetitionsId(List<string> compsId, int limit = 0)
        {
            using (AppContext db = new AppContext())
            {
                var result = new List<Season>();
                if (limit == 0)
                {
                    foreach (var compId in compsId)
                    {
                        result.AddRange(db.Seasons.Where(x => x.CompetitionId == compId).Include(x => x.Competition));
                    }
                }
                else
                {
                    foreach (var compId in compsId)
                    {
                        result.AddRange(db.Seasons.Where(x => x.CompetitionId == compId).Include(x => x.Competition).Take(limit));
                    }
                }

                return result;
            }
        }

        public bool IsCurrentSeason(string year)
        {
            var currentYear = DateTime.Today.Year.ToString();
            var regex = Regex.Match(year, @"(\d+)(-(\d+))*");
            if (regex.Groups.Count == 2)
            {
                return regex.Groups[1].Value == currentYear;
            }
            else if (regex.Groups.Count == 4)
            {
                if (DateTime.Today.Month < 7)
                {
                    return regex.Groups[3].Value == currentYear;
                }
                else
                {
                    return regex.Groups[1].Value == currentYear;
                }
            }
            else return false;
        }

        public List<Season> GetCurrentSeasons()
        {
            using (var db = new AppContext())
            {
                var result = db.Seasons.AsEnumerable().Where(x => IsCurrentSeason(x.Year)).ToList();
                return result;
            }
        }

        private DatabaseManager() { }

    }
}
