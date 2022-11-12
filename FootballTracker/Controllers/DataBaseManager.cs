using FootballTracker.Models;
using lesson1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return db.Competitions.ToList();
            }
        }

        public List<Season> GetSeasonsByComp(Competition comp)
        {
            using (AppContext db = new AppContext())
            {
                return db.Seasons.Where(x => comp.Id == x.CompetitionId).Include(x => x.Competition).OrderByDescending(x => x.Year).ToList();
            }
        }

        public List<Season> GetSeasonsByClub(FootballClub club)
        {
            using (AppContext db = new AppContext())
            {
                return db.ClubsSeasons.Where(x => x.ClubId == club.Id)
                    .Include(x => x.Season).OrderByDescending(x => x.Season.Year)
                    .Select(x => x.Season).ToList();
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

        public List<PlayerStatistics> GetSquadPlayers(FootballClub club, Season season)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.PlayerStatistics.Where(x => x.SeasonId == season.Id && x.ClubId == club.Id)
                    .Include(x => x.Club).Include(x => x.PlayerName).ToList();
                table.Sort(new ComparePosition());
                return table;
            }
        }

        public List<PlayerStatistics> GetPlayerStatisticsByPlayer(Player player)
        {
            using (AppContext db = new AppContext())
            {
                var table = db.PlayerStatistics.Where(x => x.PlayerId == player.Id).Include(x => x.Season).Include(x => x.Season.Competition).Include(x => x.Club).OrderByDescending(x => x.Season.Year).ToList();
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
