using Microsoft.EntityFrameworkCore;

namespace FootballFetcher.Model
{
    class ContextApp : DbContext
    {
        public DbSet<Competition> Competitions { get; set; }

        public DbSet<Season> Seasons { get; set; }

        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }

        public DbSet<CompetitionTable> CompetitionTable { get; set; }

        public DbSet<FootballClubSeason> ClubsSeasons { get; set; }

        public DbSet<FootballClub> Clubs { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<FootballMatch> Matches { get; set; }

        public DbSet<MatchStatistics> MatchStatistics { get; set; }

        public DbSet<MatchEvent> MatchEvents { get; set; }

        public DbSet<MatchSquadPlayers> MatchSquad { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; DATABASE=FootballDB; Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FootballClubSeason>()
                .HasKey(t => new { t.ClubId, t.SeasonId });
            modelBuilder.Entity<FootballClubSeason>()
                .HasOne(c => c.Club).WithMany(c => c.ClubsSeasons).HasForeignKey(c => c.ClubId);
            modelBuilder.Entity<FootballClubSeason>()
                .HasOne(s => s.Season).WithMany(s => s.ClubsSeasons).HasForeignKey(s => s.SeasonId);

        }
    }
}
