namespace FootballTracker.Model
{
    public class RowInMatchEvents
    {
        public Player? HomePlayer { get; set; }
        public string? HomeLabel { get; set; }
        public string? Minute { get; set; }
        public string? Type { get; set; }
        public string? Label { get; set; }
        public Player? AwayPlayer { get; set; }
        public string? AwayLabel { get; set; }

        public RowInMatchEvents(Player? homePlayer, string? homeLabel, string? type, string? minute, Player? awayPlayer, string? awayLabel)
        {
            this.HomePlayer = homePlayer;
            this.HomeLabel = homeLabel;
            this.Minute = minute;
            this.Type = type;
            this.Label = $"{minute}, {type}";
            this.AwayPlayer = awayPlayer;
            this.AwayLabel = awayLabel;
        }
    }
}
