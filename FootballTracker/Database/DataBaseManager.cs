using FootballTracker.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace FootballTracker.Database
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

        public bool HasConnection()
        {
            using (var db = new ContextApp())
            {
                return db.Database.CanConnect();
            }
        }
        public ObservableCollection<Competition> GetCompetitions()
        {
            using (ContextApp db = new ContextApp())
            {
                return db.Competitions.OrderByDescending(x => x.Country).ToObservableCollection();
            }
        }

        public ObservableCollection<Season> GetSeasonsByComp(Competition comp)
        {
            using (ContextApp db = new ContextApp())
            {
                return db.Seasons.Where(x => comp.Id == x.CompetitionId).Include(x => x.Competition).OrderByDescending(x => x.Year).ToObservableCollection();
            }
        }

        public ObservableCollection<string> GetSeasonsYearsByClub(FootballClub club)
        {
            using (ContextApp db = new ContextApp())
            {
                return db.ClubsSeasons.Where(x => x.ClubId == club.Id)
                    .Include(x => x.Season).GroupBy(x => x.Season.Year).OrderByDescending(x => x.Key)
                    .Select(x => x.Key).ToObservableCollection();
            }
        }
        public ObservableCollection<string?> GetCompetitionGroupNames(Season season)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.CompetitionTable.Where(x => x.SeasonId == season.Id).Select(x => x.GroupName).Distinct().OrderBy(x => x).ToObservableCollection();
                return table;
            }
        }

        public ObservableCollection<CompetitionTable> GetCompetitionTable(Season season)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.CompetitionTable.Where(x => x.SeasonId == season.Id).Include(x => x.Club).OrderBy(x => x.Position).ToObservableCollection();
                return table;
            }
        }

        public ObservableCollection<CompetitionTable> GetCompetitionTableByGroupName(Season season, string groupName)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.CompetitionTable.Where(x => x.SeasonId == season.Id && x.GroupName == groupName).Include(x => x.Club).OrderBy(x => x.Position).ToObservableCollection();
                return table;
            }
        }

        public ObservableCollection<PlayerStatistics> GetBombarders(Season season)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.PlayerStatistics.Where(x => x.SeasonId == season.Id).Include(x => x.Club).Include(x => x.PlayerName).OrderByDescending(x => x.Goals).Take(10).ToObservableCollection();
                return table;
            }
        }

        public ObservableCollection<PlayerStatistics> GetAssistants(Season season)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.PlayerStatistics.Where(x => x.SeasonId == season.Id).Include(x => x.Club).Include(x => x.PlayerName).OrderByDescending(x => x.Assists).Take(10).ToObservableCollection();
                return table;
            }
        }

        public ObservableCollection<PlayerStatistics> GetRudePlayers(Season season)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.PlayerStatistics.Where(x => x.SeasonId == season.Id).Include(x => x.Club).Include(x => x.PlayerName).OrderByDescending(x => x.FairPlayScore).Take(10).ToObservableCollection();
                return table;
            }
        }

        public ObservableCollection<PlayerStatistics> GetSquadPlayers(FootballClub club, string seasonYear)
        {
            using (ContextApp db = new ContextApp())
            {
                var players = db.PlayerStatistics.Where(x => x.Season.Year == seasonYear && x.ClubId == club.Id)
                    .Include(x => x.Club).Include(x => x.PlayerName).AsEnumerable().DistinctBy(x => x.Label).ToList();
                try
                {
                    players.Sort(new ComparePosition());
                }
                catch { }
                return players.ToObservableCollection();
            }
        }

        public ObservableCollection<PlayerStatistics> GetPlayerStatisticsByPlayer(Player player)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.PlayerStatistics.Where(x => x.PlayerId == player.Id)
                    .Include(x => x.Season).Include(x => x.Season.Competition).Include(x => x.Club)
                    .OrderByDescending(x => x.Season.Year).ThenByDescending(x => x.Matches).ToObservableCollection();
                return table;
            }
        }

        public ObservableCollection<PlayerStatistics> GetPlayerStatisticsBySeason(Season season)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.PlayerStatistics.Where(x => x.SeasonId == season.Id).Include(x => x.Club).Include(x => x.PlayerName).OrderByDescending(x => x.Goals).ToObservableCollection();
                return table;
            }
        }

        public ObservableCollection<Competition> GetCompetitionsByClubYear(FootballClub club, string year)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.ClubsSeasons.Include(x => x.Season).Include(x => x.Season.Competition)
                    .Where(x => x.ClubId == club.Id && x.Season.Year == year).Select(x => x.Season.Competition).OrderBy(x => x.Name).ToObservableCollection();
                return table;
            }
        }

        public ObservableCollection<FootballClub> GetCurrentClubsByPlayer(Player player)
        {
            using (ContextApp db = new ContextApp())
            {
                var table = db.PlayerStatistics.Where(x => x.PlayerId == player.Id).Include(x => x.Season).Include(x => x.Club).ToObservableCollection();
                var result = table.Where(x => IsCurrentSeason(x.Season.Year)).OrderByDescending(x => x.Matches).Select(x => x.Club).Distinct().ToObservableCollection();
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

        public ObservableCollection<PlayerStatistics> GetResultsByPlayerStatistics(ObservableCollection<PlayerStatistics> ps)
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
                OwnGoals = x.Sum(y => y.OwnGoals),
                YellowCards = x.Sum(y => y.YellowCards),
                YellowRedCards = x.Sum(y => y.YellowRedCards),
                RedCards = x.Sum(y => y.RedCards),
                FairPlayScore = x.Sum(y => y.FairPlayScore),
            }).ToObservableCollection();
            return result;
        }

        public FootballClub GetCountryByName(string countryName)
        {
            using (ContextApp db = new ContextApp())
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
                switch (age % 10)
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

        public ObservableCollection<FootballClub> GetClubsBySeason(Season season)
        {
            using (var db = new ContextApp())
            {
                var result = db.ClubsSeasons.Include(x => x.Club).Where(x => x.SeasonId == season.Id).Select(x => x.Club).OrderBy(x => x.Name).ToObservableCollection();
                return result;
            }
        }

        public FootballMatch GetMatchByMatchId(string? matchId)
        {
            using (var db = new ContextApp())
            {
                var result = db.Matches.Include(x => x.Statistics).First(x => x.Id == matchId);
                return result;
            }
        }

        public ObservableCollection<MatchEvent> GetMatchEventsByStatisticsId(MatchStatistics stats)
        {
            using (var db = new ContextApp())
            {
                var result = db.MatchEvents.Where(x => x.StatisticsId == stats.Id).Include(x => x.Player).ToObservableCollection();
                return result;
            }
        }

        public ObservableCollection<FootballMatch> GetMatchesBySeason(Season season)
        {
            using (var db = new ContextApp())
            {
                var matches = db.Matches.Where(x => x.SeasonId == season.Id).Include(x => x.Statistics).Include(x => x.Season).ToObservableCollection();
                return matches;
            }
        }

        public FootballClub GetClubByClubId(string clubId)
        {
            using (var db = new ContextApp())
            {
                var club = db.Clubs.Find(clubId);
                return club;
            }
        }

        public Competition GetCompetitionByCompetitionId(string competitionId)
        {
            using (var db = new ContextApp())
            {
                var competition = db.Competitions.Find(competitionId);
                return competition;
            }
        }

        public ObservableCollection<SquadType> GetSquadTypesByStatistics(MatchStatistics ms)
        {
            using (var db = new ContextApp())
            {
                var result = db.MatchSquad.Where(x => x.StatisticsId == ms.Id).Select(x => x.Type).Distinct().ToObservableCollection();
                return result;
            }
        }

        public ObservableCollection<MatchSquadPlayers> GetSquadPlayersByStatistics(MatchStatistics ms, SquadType st)
        {
            using (var db = new ContextApp())
            {
                var players = db.MatchSquad.Where(x => x.StatisticsId == ms.Id && x.Type == st).Include(x => x.Player).AsEnumerable().OrderBy(x => x.Player, new ComparePosition()).ToObservableCollection();
                return players;
            }
        }

        public ObservableCollection<FootballMatch> GetMatchesByDate(DateTime date)
        {
            using (var db = new ContextApp())
            {
                var matches = db.Matches.Where(x => x.Date.Value.Day == date.Day && x.Date.Value.Month == date.Month && x.Date.Value.Year == date.Year)
                    .Include(x => x.Statistics).Include(x => x.Season).Include(x => x.Season.Competition).OrderBy(x => x.Season.Competition.Name).ThenBy(x => x.Date).ToObservableCollection();
                return matches;
            }
        }

        private DataBaseManager() { }


    }

    public class ComparePosition : IComparer<PlayerStatistics>, IComparer<Player>
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

        public int Compare(Player x, Player y)
        {
            if (x != null && y != null && x.Position != null && y.Position != null)
            {
                return positionPriority[x.Position].CompareTo(positionPriority[y.Position]);
            }
            else
            {
                return -1;
            }
        }
    }
}
