using lesson1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FootballTracker.Models
{
    public class MatchStatisticsTable
    {
        public List<RowInMatchStatisticsTable> result;

        public MatchStatisticsTable(MatchStatistics home, MatchStatistics away)
        {
            result = new List<RowInMatchStatisticsTable>();
            var type = typeof(MatchStatistics);
            var props = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(StatisticAttribute)));
            foreach(var prop in props)
            {
                var valueHome = prop.GetValue(home);
                var valueAway = prop.GetValue(away);
                if(valueHome != null && valueAway != null)
                {
                    string propName = prop.Name switch
                    {
                        "Goals" => "Голы",
                        "Xg" => "Xg",
                        "Shots" => "Удары",
                        "ShotsOnTarget" => "Удары в створ",
                        "ShotsBlocked" => "Заблокированные удары",
                        "Saves" => "Сейвы",
                        "BallPossession" => "Владение, %",
                        "Corners" => "Угловые",
                        "Fouls" => "Нарушения",
                        "Offsides" => "Офсайды",
                        "YCards" => "Желтые карточки",
                        "RCards" => "Красные карточки",
                        "Attacks" => "Атаки",
                        "AttacksDangerous" => "Опасные атаки",
                        "Passes" => "Передачи",
                        "AccPasses" => "Точность передач, %",
                        "FreeKicks" => "Штрафные удары",
                        "Prowing" => "Вбрасывания",
                        "Crosses" => "Навесы",
                        "Tackles" => "Отборы",
                        _ => ""
                    };
                    result.Add(new RowInMatchStatisticsTable(valueHome.ToString(), propName, valueAway.ToString()));
                }
            }
        }
    }
}
