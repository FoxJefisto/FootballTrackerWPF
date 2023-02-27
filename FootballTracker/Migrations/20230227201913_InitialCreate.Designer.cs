﻿// <auto-generated />
using System;
using FootballTracker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FootballTracker.Migrations
{
    [DbContext(typeof(ContextApp))]
    [Migration("20230227201913_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FootballTracker.Model.Competition", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Competitions");
                });

            modelBuilder.Entity("FootballTracker.Model.CompetitionTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClubId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("GamesDrawn")
                        .HasColumnType("int");

                    b.Property<int>("GamesLost")
                        .HasColumnType("int");

                    b.Property<int>("GamesPlayed")
                        .HasColumnType("int");

                    b.Property<int>("GamesWon")
                        .HasColumnType("int");

                    b.Property<int>("GoalsDifference")
                        .HasColumnType("int");

                    b.Property<int>("GoalsMissed")
                        .HasColumnType("int");

                    b.Property<int>("GoalsScored")
                        .HasColumnType("int");

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("SeasonId");

                    b.ToTable("CompetitionTable");
                });

            modelBuilder.Entity("FootballTracker.Model.FootballClub", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FoundationDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainCoach")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ClubName");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Stadium")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("FootballTracker.Model.FootballClubSeason", b =>
                {
                    b.Property<string>("ClubId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.HasKey("ClubId", "SeasonId");

                    b.HasIndex("SeasonId");

                    b.ToTable("ClubsSeasons");
                });

            modelBuilder.Entity("FootballTracker.Model.FootballMatch", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<string>("Stage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("FootballTracker.Model.MatchEvent", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Minute")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("StatisticsId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("StatisticsId");

                    b.ToTable("MatchEvents");
                });

            modelBuilder.Entity("FootballTracker.Model.MatchSquadPlayers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsCaptain")
                        .HasColumnType("bit");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Number")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("StatisticsId")
                        .HasColumnType("int");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("StatisticsId");

                    b.ToTable("MatchSquad");
                });

            modelBuilder.Entity("FootballTracker.Model.MatchStatistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("AccPasses")
                        .HasColumnType("float");

                    b.Property<int?>("Attacks")
                        .HasColumnType("int");

                    b.Property<int?>("AttacksDangerous")
                        .HasColumnType("int");

                    b.Property<int?>("BallPossession")
                        .HasColumnType("int");

                    b.Property<string>("ClubId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Corners")
                        .HasColumnType("int");

                    b.Property<int?>("Crosses")
                        .HasColumnType("int");

                    b.Property<int?>("Fouls")
                        .HasColumnType("int");

                    b.Property<int?>("FreeKicks")
                        .HasColumnType("int");

                    b.Property<int?>("Goals")
                        .HasColumnType("int");

                    b.Property<byte>("HomeAway")
                        .HasColumnType("tinyint");

                    b.Property<string>("MatchId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Offsides")
                        .HasColumnType("int");

                    b.Property<int?>("Passes")
                        .HasColumnType("int");

                    b.Property<int?>("Prowing")
                        .HasColumnType("int");

                    b.Property<int?>("RCards")
                        .HasColumnType("int");

                    b.Property<int?>("Saves")
                        .HasColumnType("int");

                    b.Property<int?>("Shots")
                        .HasColumnType("int");

                    b.Property<int?>("ShotsBlocked")
                        .HasColumnType("int");

                    b.Property<int?>("ShotsOnTarget")
                        .HasColumnType("int");

                    b.Property<int?>("Tackles")
                        .HasColumnType("int");

                    b.Property<double?>("Xg")
                        .HasColumnType("float");

                    b.Property<int?>("YCards")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("MatchId");

                    b.ToTable("MatchStatistics");
                });

            modelBuilder.Entity("FootballTracker.Model.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Citizenship")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Height")
                        .HasColumnType("int");

                    b.Property<string>("ImgSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginalName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlaceOfBirth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Weight")
                        .HasColumnType("int");

                    b.Property<string>("WorkingLeg")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("FootballTracker.Model.PlayerStatistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Assists")
                        .HasColumnType("int");

                    b.Property<string>("ClubId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DoubleGoals")
                        .HasColumnType("int");

                    b.Property<int>("FairPlayScore")
                        .HasColumnType("int");

                    b.Property<int>("GoalPlusPass")
                        .HasColumnType("int");

                    b.Property<int>("Goals")
                        .HasColumnType("int");

                    b.Property<int>("HatTricks")
                        .HasColumnType("int");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Matches")
                        .HasColumnType("int");

                    b.Property<int>("Minutes")
                        .HasColumnType("int");

                    b.Property<int?>("Number")
                        .HasColumnType("int");

                    b.Property<int>("OwnGoals")
                        .HasColumnType("int");

                    b.Property<int>("PenGoals")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RedCards")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("YellowCards")
                        .HasColumnType("int");

                    b.Property<int>("YellowRedCards")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("SeasonId");

                    b.ToTable("PlayerStatistics");
                });

            modelBuilder.Entity("FootballTracker.Model.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompetitionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("FootballTracker.Model.CompetitionTable", b =>
                {
                    b.HasOne("FootballTracker.Model.FootballClub", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId");

                    b.HasOne("FootballTracker.Model.Season", "Season")
                        .WithMany("Table")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("FootballTracker.Model.FootballClubSeason", b =>
                {
                    b.HasOne("FootballTracker.Model.FootballClub", "Club")
                        .WithMany("ClubsSeasons")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FootballTracker.Model.Season", "Season")
                        .WithMany("ClubsSeasons")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("FootballTracker.Model.FootballMatch", b =>
                {
                    b.HasOne("FootballTracker.Model.Season", "Season")
                        .WithMany("Matches")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("FootballTracker.Model.MatchEvent", b =>
                {
                    b.HasOne("FootballTracker.Model.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("FootballTracker.Model.MatchStatistics", "Statistics")
                        .WithMany("Events")
                        .HasForeignKey("StatisticsId");

                    b.Navigation("Player");

                    b.Navigation("Statistics");
                });

            modelBuilder.Entity("FootballTracker.Model.MatchSquadPlayers", b =>
                {
                    b.HasOne("FootballTracker.Model.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("FootballTracker.Model.MatchStatistics", "Statistics")
                        .WithMany("Squad")
                        .HasForeignKey("StatisticsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Statistics");
                });

            modelBuilder.Entity("FootballTracker.Model.MatchStatistics", b =>
                {
                    b.HasOne("FootballTracker.Model.FootballClub", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId");

                    b.HasOne("FootballTracker.Model.FootballMatch", "Match")
                        .WithMany("Statistics")
                        .HasForeignKey("MatchId");

                    b.Navigation("Club");

                    b.Navigation("Match");
                });

            modelBuilder.Entity("FootballTracker.Model.PlayerStatistics", b =>
                {
                    b.HasOne("FootballTracker.Model.FootballClub", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId");

                    b.HasOne("FootballTracker.Model.Player", "PlayerName")
                        .WithMany("PlayerStatistics")
                        .HasForeignKey("PlayerId");

                    b.HasOne("FootballTracker.Model.Season", "Season")
                        .WithMany("PlayerStatistics")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("PlayerName");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("FootballTracker.Model.Season", b =>
                {
                    b.HasOne("FootballTracker.Model.Competition", "Competition")
                        .WithMany("Seasons")
                        .HasForeignKey("CompetitionId");

                    b.Navigation("Competition");
                });

            modelBuilder.Entity("FootballTracker.Model.Competition", b =>
                {
                    b.Navigation("Seasons");
                });

            modelBuilder.Entity("FootballTracker.Model.FootballClub", b =>
                {
                    b.Navigation("ClubsSeasons");
                });

            modelBuilder.Entity("FootballTracker.Model.FootballMatch", b =>
                {
                    b.Navigation("Statistics");
                });

            modelBuilder.Entity("FootballTracker.Model.MatchStatistics", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Squad");
                });

            modelBuilder.Entity("FootballTracker.Model.Player", b =>
                {
                    b.Navigation("PlayerStatistics");
                });

            modelBuilder.Entity("FootballTracker.Model.Season", b =>
                {
                    b.Navigation("ClubsSeasons");

                    b.Navigation("Matches");

                    b.Navigation("PlayerStatistics");

                    b.Navigation("Table");
                });
#pragma warning restore 612, 618
        }
    }
}
