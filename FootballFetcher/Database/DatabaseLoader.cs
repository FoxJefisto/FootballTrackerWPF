﻿using FootballFetcher.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FootballFetcher.Database.DataFetcher;

namespace FootballFetcher.Database
{
    class DatabaseLoader
    {
        private static DatabaseLoader instance;
        public DatabaseManager dbManager;
        public DataFetcher dataFetcher;
        public int clubsExpectedLength;
        public int playersExpectedLength;
        public int matchesExpectedLength;

        public static DatabaseLoader GetInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseLoader();
            }
            return instance;
        }

        public void LoadStatistics(List<Season> seasons)
        {
            LoadSeasonsPlayerStatisticsAsync(seasons).GetAwaiter().GetResult();
            LoadSeasonsCompetitionTableAsync(seasons).GetAwaiter().GetResult();
        }

        public string[] FilterPlayersId(List<string> playersId)
        {
            using (ContextApp db = new ContextApp())
            {
                var result = playersId.Distinct().Except(db.Players.Select(x => x.Id)).ToArray();
                playersExpectedLength = result.Length + db.Players.Count();
                return result;
            }
        }

        public string[] FilterClubsId(List<string> clubsId)
        {
            using (ContextApp db = new ContextApp())
            {
                var result = clubsId.Distinct().Except(db.Clubs.Select(x => x.Id)).ToArray();
                clubsExpectedLength = result.Length + db.Clubs.Count();
                return result;
            }
        }

        public string[] FilterPastMatchesId(List<string> matchesId)
        {
            using (ContextApp db = new ContextApp())
            {
                var result = matchesId.Except(db.Matches.Where(x => x.Status.Contains("Завершен")).Select(x => x.Id)).ToArray();
                matchesExpectedLength = result.Length + db.Matches.Count();
                return result;
            }
        }

        public string[] FilterUpcomingMatchesId(List<string> matchesId)
        {
            using (ContextApp db = new ContextApp())
            {
                var now = DateTime.Now;
                var result = matchesId.Intersect(db.Matches.Where(x => x.Date.Value.Date == now.Date).Select(x => x.Id)).ToArray();
                matchesExpectedLength = result.Length + db.Matches.Count();
                return result;
            }
        }

        public void LoadNewDataByCompetitionId(List<string> compsId, int limit)
        {
            using (ContextApp db = new ContextApp())
            {
                LoadCompetitionInfo(compsId);
                var seasons = dbManager.GetSeasonsByCompetitionsId(compsId, limit);
                LoadPlayersAndClubsInfo(seasons);
                LoadStatistics(seasons);
                LoadPastMatches(seasons);
                LoadUpcomingMatches();
            }
        }

        public void UpdateStatistics()
        {
            using (ContextApp db = new ContextApp())
            {
                var seasons = dbManager.GetCurrentSeasons();
                LoadPlayersAndClubsInfo(seasons);
                LoadStatistics(seasons);
                db.SaveChanges();
            }
        }

        public void UpdateCurrentMatches()
        {
            using (ContextApp db = new ContextApp())
            {
                var seasons = dbManager.GetCurrentSeasons();
                LoadPastMatches(seasons);
                UpdateTodayUpcomingMatches();
                ReloadPostponedMatches();
                db.SaveChanges();
            }
        }

        public void ReloadPostponedMatches()
        {
            using (ContextApp db = new ContextApp())
            {
                var postponedMatches = db.Matches.Where(x => x.Date.Value < DateTime.Today && (x.Status == "Ожидается" || x.Status == "Перенесен"));
                var matchIdToSeasonId = new Dictionary<string, int>();
                foreach (var postponedMatch in postponedMatches)
                {
                    matchIdToSeasonId[postponedMatch.Id] = postponedMatch.SeasonId;
                }
                LoadMatchesAsync(postponedMatches.Select(x => x.Id).ToArray(), matchIdToSeasonId);
                db.SaveChanges();
            }
        }

        public void LoadPlayersAndClubsInfo(List<Season> seasons)
        {
            Console.WriteLine($"Загрузка всех игроков и клубов с {seasons.Count} сезонов...");
            var tuple = dataFetcher.GetAllPlayersAndClubsIdAsync(seasons).GetAwaiter().GetResult();
            Console.WriteLine("Фильтрация клубов...");
            var unloadedClubsId = FilterClubsId(tuple.clubsId);
            Console.WriteLine("Фильтрация игроков...");
            var unloadedPlayersId = FilterPlayersId(tuple.playersId);
            Console.WriteLine($"Количество незагруженных клубов: {unloadedClubsId.Length}");
            Console.WriteLine($"Количество незагруженных игроков: {unloadedPlayersId.Length}");
            Console.WriteLine($"Сохранение {unloadedClubsId.Length} клубов...");
            LoadClubsAsync(unloadedClubsId.Where(x => x != null).ToArray()).GetAwaiter().GetResult();
            Console.WriteLine($"Сохранение {unloadedPlayersId.Length} игроков...");
            LoadPlayersAsync(unloadedPlayersId.Where(x => x != null).ToArray()).GetAwaiter().GetResult();
            Console.WriteLine("Привязка клубов к сезонам...");
            LoadSeasonsClubsIdAsync(seasons).GetAwaiter().GetResult();
        }

        public void LoadPastMatches(List<Season> seasons)
        {
            var pastMatches = dataFetcher.GetAllPastMatchesIdBySeasonsAsync(seasons).GetAwaiter().GetResult();
            var unloadedMatchesId = FilterPastMatchesId(pastMatches.matchesId);
            Console.WriteLine($"Сохранение {unloadedMatchesId.Length} прошедших матчей");
            LoadMatchesAsync(unloadedMatchesId.ToArray(), pastMatches.matchIdToSeasonId).GetAwaiter().GetResult();
        }

        public void LoadUpcomingMatches()
        {
            var currentSeasons = dbManager.GetCurrentSeasons();
            var upcomingMatches = dataFetcher.GetAllUpcomingMatchesIdBySeasonsAsync(currentSeasons).GetAwaiter().GetResult();
            Console.WriteLine($"Сохранение {upcomingMatches.matchesId.Count} предстоящих матчей");
            LoadMatchesAsync(upcomingMatches.matchesId.ToArray(), upcomingMatches.matchIdToSeasonId).GetAwaiter().GetResult();
        }

        public void UpdateTodayUpcomingMatches()
        {
            var currentSeasons = dbManager.GetCurrentSeasons();
            var upcomingMatches = dataFetcher.GetAllUpcomingMatchesIdBySeasonsAsync(currentSeasons).GetAwaiter().GetResult();
            var unloadedMatchesId = FilterUpcomingMatchesId(upcomingMatches.matchesId);
            Console.WriteLine($"Сохранение {unloadedMatchesId.Length} предстоящих сегодняшних матчей");
            LoadMatchesAsync(unloadedMatchesId, upcomingMatches.matchIdToSeasonId).GetAwaiter().GetResult();
        }

        public async Task LoadCompetitionsInfoAsync(List<string> compsId)
        {
            Task[] tasks = new Task[compsId.Count];
            int i = 0;
            foreach (var compId in compsId)
            {
                tasks[i] = LoadOneCompetitionInfoAsync(compId);
                i++;
            }
            await Task.WhenAll(tasks);
        }

        public void LoadCompetitionInfo(List<string> compsId)
        {
            Console.WriteLine("Загрузка турниров");
            LoadCompetitionsInfoAsync(compsId).GetAwaiter().GetResult();
        }

        public async Task LoadOneCompetitionInfoAsync(string compId)
        {
            await Task.Run(() => LoadOneCompetitionInfo(compId));
        }

        public async Task LoadClubsAsync(string[] clubsId)
        {
            Task[] tasks = new Task[clubsId.Length];
            int i = 0;
            foreach (var clubId in clubsId)
            {
                tasks[i] = LoadOneClubAsync(clubId);
                i++;
                await Task.Delay(50);
            }
            await Task.WhenAll(tasks);
        }

        public async Task LoadOneClubAsync(string clubId)
        {
            await Task.Run(() => LoadOneClub(clubId));
        }

        public async Task LoadPlayersAsync(string[] playersId)
        {
            Task[] tasks = new Task[playersId.Length];
            int i = 0;
            foreach (var playerId in playersId)
            {
                tasks[i] = LoadOnePlayerAsync(playerId);
                i++;
                await Task.Delay(50);
            }
            await Task.WhenAll(tasks);
        }

        public async Task LoadOnePlayerAsync(string playerId)
        {
            await Task.Run(() => LoadOnePlayer(playerId));
        }

        public async Task LoadSeasonsClubsIdAsync(List<Season> seasons)
        {
            Task[] tasks = new Task[seasons.Count];
            int i = 0;
            foreach (var season in seasons)
            {
                tasks[i] = LoadOneSeasonClubsIdAsync(season);
                i++;
                await Task.Delay(50);
            }
            await Task.WhenAll(tasks);
        }

        public async Task LoadOneSeasonClubsIdAsync(Season season)
        {
            await Task.Run(() => LoadOneSeasonClubsId(season));
        }

        public async Task LoadSeasonsPlayerStatisticsAsync(List<Season> seasons)
        {
            Task[] tasks = new Task[seasons.Count];
            int i = 0;
            foreach (var season in seasons)
            {
                tasks[i] = LoadOneSeasonPlayerStatisticsAsync(season);
                i++;
                await Task.Delay(150);
            }
            await Task.WhenAll(tasks);

        }

        public async Task LoadOneSeasonPlayerStatisticsAsync(Season season)
        {
            await Task.Run(() => LoadOneSeasonPlayerStatistics(season));
        }

        public async Task LoadSeasonsCompetitionTableAsync(List<Season> seasons)
        {
            Task[] tasks = new Task[seasons.Count];
            int i = 0;
            foreach (var season in seasons)
            {
                tasks[i] = LoadOneSeasonCompetitionTableAsync(season);
                i++;
                await Task.Delay(150);
            }
            await Task.WhenAll(tasks);
        }

        public async Task LoadOneSeasonCompetitionTableAsync(Season season)
        {
            await Task.Run(() => LoadOneSeasonCompetitionTable(season));
        }

        public async Task LoadOneMatchAsync(string matchId, int seasonId)
        {
            await Task.Run(() => LoadOneMatch(matchId, seasonId));
        }

        public async Task LoadMatchesAsync(string[] matchesId, Dictionary<string, int> matchIdToSeasonId)
        {
            Task[] tasks = new Task[matchesId.Length];
            int i = 0;
            foreach (var matchId in matchesId)
            {
                tasks[i] = LoadOneMatchAsync(matchId, matchIdToSeasonId[matchId]);
                i++;
                await Task.Delay(50);
            }
            await Task.WhenAll(tasks);
        }

        public void LoadOneMatch(string matchId, int seasonId)
        {
            using (ContextApp db = new ContextApp())
            {
                var c = new SqlCommand();
                c.CommandTimeout = 0;
                string htmlCode = dataFetcher.GetHTMLInfo(matchId, SearchScope.games);
                var match = dataFetcher.GetMatchByHtml(htmlCode, matchId);
                var squad = dataFetcher.GetMatchSquadByHtml(htmlCode);
                var events = dataFetcher.GetMatchEventsByHtml(htmlCode, squad);
                for (int i = 0; i < match.Statistics.Count; i++)
                {
                    match.Statistics[i].Squad = squad[i];
                    match.Statistics[i].Events = events[i];
                    var players = squad[i].Select(x => new { x.PlayerId, x.Label }).Union(events[i].Select(x => new { x.PlayerId, x.Label }));
                    var unloadedPlayersId = players.Select(x => x.PlayerId).Except(db.Players.Select(x => x.Id)).ToList();
                    playersExpectedLength = db.Players.Count() + unloadedPlayersId.Count;
                    foreach (var playerId in unloadedPlayersId)
                    {
                        if (playerId is null)
                            continue;
                        LoadOnePlayer(playerId);
                    }
                    foreach(var player in players)
                    {
                        if(!db.PlayerStatistics.Any(x => x.PlayerId == player.PlayerId && x.SeasonId == seasonId))
                        {
                            var ps = new PlayerStatistics()
                            {
                                IsZero = true,
                                SeasonId = seasonId,
                                PlayerId = player.PlayerId,
                                ClubId = match.Statistics[i].ClubId
                            };
                            if (players.Any(x => x.PlayerId == player.PlayerId))
                            {
                                ps.Label = players.First(x => x.PlayerId == player.PlayerId).Label;
                                if (squad[i].FirstOrDefault(x => x.PlayerId == player.PlayerId) is MatchSquadPlayers msp)
                                {
                                    ps.Number = msp.Number;
                                }
                            }
                            db.PlayerStatistics.Add(ps);
                            Console.WriteLine($"Строка статистики игрока {ps.Label} успешно загружена.");
                        }
                    }
                }
                match.SeasonId = seasonId;
                var result = db.Matches.Include(x => x.Statistics).SingleOrDefault(x => x.Id == match.Id);
                if (result is null)
                {
                    db.Matches.Add(match);
                    Console.WriteLine($"Матч {match.Id} успешно загружен. Всего: {db.Matches.Count()}/{matchesExpectedLength}");
                }
                else
                {
                    match.Id = result.Id;
                    db.Entry(result).CurrentValues.SetValues(match);
                    result.Statistics[0] = match.Statistics[0];
                    result.Statistics[1] = match.Statistics[1];
                }
                try
                {
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                    Console.WriteLine($"Матч {match.Id} не удалось загрузить в БД");
                }
            }
        }

        public void LoadOneClub(string clubId)
        {
            using (ContextApp db = new ContextApp())
            {
                if (!db.Clubs.Any(x => x.Id == clubId))
                {
                    var clubInfo = dataFetcher.GetClubInfoById(clubId);
                    db.Clubs.Add(clubInfo);
                    try
                    {
                        db.SaveChanges();
                        Console.WriteLine($"Клуб {clubInfo.Name} успешно загружен. Всего: {db.Clubs.Count()}/{clubsExpectedLength}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.InnerException);
                        Console.WriteLine($"Клуб {clubId} не удалось добавить в БД");
                    }
                }
            }
        }

        public void LoadOnePlayer(string playerId)
        {
            using (ContextApp db = new ContextApp())
            {
                if (playerId != null && !db.Players.Any(x => x.Id == playerId))
                {
                    var player = dataFetcher.GetPlayerInfoById(playerId);
                    db.Players.Add(player);
                    try
                    {
                        db.SaveChanges();
                        Console.WriteLine($"Игрок {player.OriginalName} успешно загружен. Всего: {db.Players.Count()}/{playersExpectedLength}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.InnerException);
                        Console.WriteLine($"Игрока {playerId} не удалось добавить в БД");
                    }
                }
            }
        }

        public void LoadOneCompetitionInfo(string compId)
        {
            using (ContextApp db = new ContextApp())
            {
                var tuple = dataFetcher.GetCompetitionInfo(compId);
                if (!db.Competitions.Any(x => x.Id == compId))
                {
                    db.Competitions.Add(tuple.comp);
                    db.SaveChanges();
                    Console.WriteLine($"Турнир {tuple.comp.Name} {tuple.comp.Country} успешно загружен. Всего: {db.Competitions.Count()}");
                }

                foreach (var seasonYear in tuple.seasonsYear)
                {
                    if (!db.Seasons.Any(x => x.CompetitionId == compId && x.Year == seasonYear))
                    {
                        Season season = new Season
                        {
                            Year = seasonYear,
                            CompetitionId = compId
                        };
                        db.Seasons.Add(season);
                        db.SaveChanges();
                        Console.WriteLine($"Cезон {season.Year} в турнир {tuple.comp.Name} {tuple.comp.Country} успешно загружен. Всего: {db.Seasons.Count()}");
                    }
                }
            }
        }

        public void LoadOneSeasonPlayerStatistics(Season season)
        {
            using (ContextApp db = new ContextApp())
            {
                var c = new SqlCommand();
                c.CommandTimeout = 0;
                db.PlayerStatistics.RemoveRange(db.PlayerStatistics.Where(x => x.SeasonId == season.Id && x.IsZero == false));
                Console.WriteLine($"Старая таблица статистики игроков сезона {season.Id} {season.Year} успешно удалена. Всего: {db.PlayerStatistics.Count()}");
                var playerStatistics = dataFetcher.GetSeasonPlayerStatisticsById(season.CompetitionId, season.Id, season.Year);
                foreach (var row in playerStatistics)
                {
                    PlayerStatistics result;
                    try
                    {
                        result = db.PlayerStatistics.SingleOrDefault(x => x.IsZero == true && x.SeasonId == row.SeasonId && x.Label == row.Label && x.ClubId == row.ClubId);
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine($"Встречено несколько строк статистики для игрока [{row.Id}]{row.Label}");
                        db.PlayerStatistics.RemoveRange(db.PlayerStatistics.Where(x => x.SeasonId == row.SeasonId && x.Label == row.Label && x.ClubId == row.ClubId));
                        result = null;
                    }
                    if (result is null)
                    {
                        db.PlayerStatistics.Add(row);
                    }
                    else
                    {
                        row.Id = result.Id;
                        db.Entry(result).CurrentValues.SetValues(row);
                        Console.WriteLine($"Таблица статистики игрока {row.Label} в сезоне {season.Id} {season.Year} успешно обновлена");
                    }
                }
                db.SaveChanges();
                Console.WriteLine($"Таблица статистики игроков сезона {season.Id} {season.Year} успешно загружена. Всего: {db.PlayerStatistics.Count()}");
            }
        }

        public void LoadOneSeasonCompetitionTable(Season season)
        {
            using (ContextApp db = new ContextApp())
            {
                var c = new SqlCommand();
                c.CommandTimeout = 0;
                var table = dataFetcher.GetCompetitionTableById(season.CompetitionId, season.Id, season.Year);
                foreach (var row in table)
                {
                    var result = db.CompetitionTable.SingleOrDefault(x => x.ClubId == row.ClubId && x.SeasonId == row.SeasonId && x.GroupName == row.GroupName);
                    if (result is null)
                    {
                        if (!db.Clubs.Any(x => x.Id == row.ClubId || x.Id == null))
                        {
                            Console.WriteLine($"Клуба {row.ClubId} нет в БД. Исправляем...");
                            var clubInfo = dataFetcher.GetClubInfoById(row.ClubId);
                            db.Clubs.Add(clubInfo);
                            Console.WriteLine($"Клуб {row.ClubId} успешно загружен. Всего: {db.Clubs.Count()}");
                        }
                        db.CompetitionTable.Add(row);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception) { }
                        Console.WriteLine($"Строка турнирной таблицы клуба {row.ClubId} успешно загружена. Всего: {db.CompetitionTable.Count()}");
                    }
                    else
                    {
                        row.Id = result.Id;
                        db.Entry(result).CurrentValues.SetValues(row);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception) { }
                        Console.WriteLine($"Строка турнирной таблицы клуба {row.ClubId} успешно обновлена.");
                    }
                }
            }
        }

        public void LoadOneSeasonClubsId(Season season)
        {
            using (ContextApp db = new ContextApp())
            {
                var clubsId = dataFetcher.GetClubsIdInLeague(season.CompetitionId, season.Year).Distinct();
                foreach (var clubId in clubsId)
                {
                    if (!db.ClubsSeasons.Any(x => x.ClubId == clubId && x.SeasonId == season.Id))
                    {
                        if (!db.Clubs.Any(x => x.Id == clubId))
                        {
                            Console.WriteLine($"Клуба {clubId} нет в БД. Исправляем...");
                            var clubInfo = dataFetcher.GetClubInfoById(clubId);
                            db.Clubs.Add(clubInfo);
                            Console.WriteLine($"Клуб {clubId} успешно загружен. Всего: {db.Clubs.Count()}");
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception) { }
                        }
                        var clubSeason = new FootballClubSeason() { ClubId = clubId, SeasonId = season.Id };
                        db.ClubsSeasons.Add(clubSeason);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception) { }
                        Console.WriteLine($"Клуб {clubId} успешно привязан к сезону {season.Id}");
                    }
                }
            }
        }

        private DatabaseLoader()
        {
            dataFetcher = DataFetcher.GetInstance();
            dbManager = DatabaseManager.GetInstance();
        }
    }
}
