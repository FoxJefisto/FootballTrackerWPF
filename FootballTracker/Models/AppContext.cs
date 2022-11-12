using lesson1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballTracker.Models
{
    public class AppContext : DbContext
    {
        public DbSet<Competition> Competitions { get; set; }

        public DbSet<Season> Seasons { get; set; }

        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }

        public DbSet<CompetitionTable> CompetitionTable { get; set; }

        public DbSet<FootballClubSeason> ClubsSeasons { get; set; }

        public DbSet<FootballClub> Clubs { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<FootballClubPlayer> ClubsPlayers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; DATABASE=FootballDB; Trusted_Connection=True; TrustServerCertificate=Yes;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FootballClubPlayer>()
                .HasKey(t => new { t.ClubId, t.PlayerId });
            modelBuilder.Entity<FootballClubPlayer>()
                .HasOne(p => p.Player).WithMany(p => p.ClubPlayer).HasForeignKey(p => p.PlayerId);
            modelBuilder.Entity<FootballClubPlayer>()
                .HasOne(c => c.Club).WithMany(c => c.ClubPlayer).HasForeignKey(c => c.ClubId);

            modelBuilder.Entity<FootballClubSeason>()
                .HasKey(t => new { t.ClubId, t.SeasonId });
            modelBuilder.Entity<FootballClubSeason>()
                .HasOne(c => c.Club).WithMany(c => c.ClubsSeasons).HasForeignKey(c => c.ClubId);
            modelBuilder.Entity<FootballClubSeason>()
                .HasOne(s => s.Season).WithMany(s => s.ClubsSeasons).HasForeignKey(s => s.SeasonId);

        }
    }
}
