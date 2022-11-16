using FootballTracker.Models;
using lesson1;
using MaterialDesignThemes.Wpf.Converters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Data;
using AppContext = FootballTracker.Models.AppContext;

namespace FootballTracker.Controllers
{
    public class DataBaseManager
    {
        private static DataBaseManager instance;

        public static DataBaseManager GetInstance()
        {
            if (instance == null)
            {
                instance = new DataBaseManager();
            }
            return instance;
        }

        public List<Competition> GetCompetitions()
        {
            using (AppContext db = new AppContext())
            {
                return db.Competitions.OrderByDescending(x => x.Country).ToList();
            }
        }

        public List<Season> GetSeasonsByComp(Competition comp)
        {
            using (AppContext db = new AppContext())
            {
                return db.Seasons.Where(x => comp.Id == x.CompetitionId).Include(x => x.Competition).OrderByDescending(x => x.Year).ToList();
            }
        }

        public List<string> GetSeasonsYearsByClub(FootballClub club)
        {
            using (AppContext db = new AppContext())
            {
                return db.ClubsSeasons.Where(x => x.ClubId == club.Id)
                    .Include(x => x.Season).GroupBy(x => x.Season.Year).OrderByDescending(x => x.Key)
                    .Select(x => x.Key).ToList();
            }
        }
        public List<string?> GetCompetitionGroupNames(Season season)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.CompetitionTable.Where(x => x.SeasonId == season.Id).Select(x => x.GroupName).Distinct().OrderBy(x => x).ToList();
                return table;
            }
        }

        public List<CompetitionTable> GetCompetitionTable(Season season)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.CompetitionTable.Where(x => x.SeasonId == season.Id).Include(x => x.Club).OrderBy(x => x.Position).ToList();
                return table;
            }
        }

        public List<CompetitionTable> GetCompetitionTableByGroupName(Season season, string groupName)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.CompetitionTable.Where(x => x.SeasonId == season.Id && x.GroupName == groupName).Include(x => x.Club).OrderBy(x => x.Position).ToList();
                return table;
            }
        }

        public List<PlayerStatistics> GetBombarders(Season season)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.PlayerStatistics.Where(x => x.SeasonId == season.Id).Include(x => x.Club).Include(x => x.PlayerName).OrderByDescending(x => x.Goals).Take(10).ToList();
                return table;
            }
        }

        public List<PlayerStatistics> GetAssistants(Season season)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.PlayerStatistics.Where(x => x.SeasonId == season.Id).Include(x => x.Club).Include(x => x.PlayerName).OrderByDescending(x => x.Assists).Take(10).ToList();
                return table;
            }
        }

        public List<PlayerStatistics> GetRudePlayers(Season season)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.PlayerStatistics.Where(x => x.SeasonId == season.Id).Include(x => x.Club).Include(x => x.PlayerName).OrderByDescending(x => x.FairPlayScore).Take(10).ToList();
                return table;
            }
        }

        public List<PlayerStatistics> GetSquadPlayers(FootballClub club, string seasonYear)
        {
            using (AppContext db = new AppContext())
            {
                var players = db.PlayerStatistics.Where(x => x.Season.Year == seasonYear && x.ClubId == club.Id)
                    .Include(x => x.Club).Include(x => x.PlayerName).AsEnumerable().DistinctBy(x => x.Label).ToList();
                try
                {
                    players.Sort(new ComparePosition());
                }
                catch { }
                return players;
            }
        }

        public List<PlayerStatistics> GetPlayerStatisticsByPlayer(Player player)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.PlayerStatistics.Where(x => x.PlayerId == player.Id)
                    .Include(x => x.Season).Include(x => x.Season.Competition).Include(x => x.Club)
                    .OrderByDescending(x => x.Season.Year).ThenByDescending(x => x.Matches).ToList();
                return table;
            }
        }

        public List<PlayerStatistics> GetPlayerStatisticsBySeason(Season season)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.PlayerStatistics.Where(x => x.SeasonId == season.Id).Include(x => x.Club).Include(x => x.PlayerName).OrderByDescending(x => x.Goals).ToList();
                return table;
            }
        }

        public List<Competition> GetCompetitionsByClubYear(FootballClub club, string year)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.ClubsSeasons.Include(x => x.Season).Include(x => x.Season.Competition)
                    .Where(x => x.ClubId == club.Id && x.Season.Year == year).Select(x => x.Season.Competition).OrderBy(x => x.Name).ToList();
                return table;
            }
        }

        public List<FootballClub> GetCurrentClubsByPlayer(Player player)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.PlayerStatistics.Where(x => x.PlayerId == player.Id).Include(x => x.Season).Include(x => x.Club).ToList();
                var result = table.Where(x => IsCurrentSeason(x.Season.Year)).OrderByDescending(x => x.Matches).Select(x => x.Club).Distinct().ToList();
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

        public List<PlayerStatistics> GetResultsByPlayerStatistics(List<PlayerStatistics> ps)
        {
            var result = ps.GroupBy(x => x.PlayerId).Select(x => new PlayerStatistics()
            {
                Goals = x.Sum(y => y.Goals),
                Assists = x.Sum(y => y.Assists),
                Matches = x.Sum(y => y.Matches),
                Minutes = x.Sum(y => y.Minutes),
                GoalPlusPass = x.Sum(y => y.GoalPlusPass),
                PenGoals = x.Sum(y => y.PenGoals),
                DoubleGoals = x.Sum(y => y.DoubleGoals),
                HatTricks = x.Sum(y => y.HatTricks),
                AutoGoals = x.Sum(y => y.AutoGoals),
                YellowCards = x.Sum(y => y.YellowCards),
                YellowRedCards = x.Sum(y => y.YellowRedCards),
                RedCards = x.Sum(y => y.RedCards),
                FairPlayScore = x.Sum(y => y.FairPlayScore),
            }).ToList();
            return result;
        }

        public FootballClub GetCountryByName(string countryName)
        {
            using (AppContext db = new AppContext())
            {
                var country = db.Clubs.FirstOrDefault(x => x.Name == countryName);
                return country;
            }
        }

        public string? GetAgeString(DateTime? date)
        {
            if (date is DateTime birthdate)
            {
                var today = DateTime.Today;
                var age = today.Year - birthdate.Year;
                if (birthdate.Date > today.AddYears(-age)) age--;
                string yearStr;
                switch(age % 10)
                {
                    case 1:
                        yearStr = " год";
                        break;
                    case 2:
                    case 3:
                    case 4:
                        yearStr = " года";
                        break;
                    default:
                        yearStr = " лет";
                        break;
                }
                return age.ToString() + yearStr;
            }
            else return null;
        }

        private DataBaseManager() { }


    }

    public class ComparePosition : IComparer<PlayerStatistics>
    {
        private Dictionary<string?, int> positionPriority = new Dictionary<string?, int>()
        {
            { "вратарь", 0 },
            { "защитник", 1 },
            { "полузащитник", 2 },
            { "нападающий", 3 }
        };
        public int Compare(PlayerStatistics x, PlayerStatistics y)
        {
            if (x.PlayerName != null && y.PlayerName != null && x.PlayerName.Position != null && y.PlayerName.Position != null)
            {
                return positionPriority[x.PlayerName.Position].CompareTo(positionPriority[y.PlayerName.Position]);
            }
            else
            {
                return -1;
            }
        }
    }
}
