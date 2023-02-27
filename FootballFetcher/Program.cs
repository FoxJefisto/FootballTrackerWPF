using FootballFetcher.Database;
using System.Diagnostics;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FootballFetcher
{
    internal class Program
    {
        static DatabaseLoader dbLoader = DatabaseLoader.GetInstance();
        static int count = 0;
        static void Main(string[] args)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            Console.Write("Меню:\n1 - загрузить данные турниров(Используется для загрузки основных данных. Выбрать только при первом запуске)\n" +
                "2 - режим обновления данных в реальном времени(Используется для обновления событий матчей и статистики)\nВыбор: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    dbLoader.LoadNewDataByCompetitionId(new List<string>() { "15", "16", "17", "18", "12", "19", "20", "742", "13" }, 22);
                    break;
                case "2":
                    dbLoader.UpdateCurrentMatches();
                    Timer aTimer = new Timer();
                    aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    aTimer.Interval = 60000;
                    aTimer.Enabled = true;
                    Console.ReadLine();
                    break;
                default:
                    break;
            }
            //stopwatch.Stop();
            //TimeSpan ts = stopwatch.Elapsed;
            //string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts.Hours, ts.Minutes, ts.Seconds,
            //    ts.Milliseconds / 10);
            //Console.WriteLine($"Время работы: {elapsedTime}");
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now:T}] Событие таймера[{count}]");
            dbLoader.UpdateCurrentMatches();
            if (count % 10 == 0)
            {
                count = 0;
                dbLoader.UpdateStatistics();
            }
            count++;
        }
    }
}