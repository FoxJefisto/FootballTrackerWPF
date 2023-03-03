<p align="center">
      <img src="https://i.imgur.com/ZipTFPw.jpg" alt="Project Logo" width="726">
</p>

<p align="center">
    <img src="https://img.shields.io/github/watchers/FoxJefisto/FootballTrackerWPF?style=social" alt="watchers">
    <img src="https://img.shields.io/github/last-commit/FoxJefisto/FootballTrackerWPF" alt="last commit">
    <img src="https://img.shields.io/github/repo-size/FoxJefisto/FootballTrackerWPF" alt=repo-size">
</p>

## About
This project demonstrates an alternative way of working with databases using the ORM Entity Framework tools. The database for storing soccer statistics consists of 9 tables and contains more than 2 million rows of data. With the help of WPF technology, an application for soccer fans has been developed. It contains all the results of sport events, also it provides the opportunity to follow the matches live.
### ...
В данном проекте был продемонстрирован альтернативный способ работы с базами данных при помощи средств ORM Entity Framework. База данных для хранения футбольной статистики состоит из 9 таблиц и содержит более 2 миллионов строк данных. С помощью технологии WPF было разработано приложение для футбольных фанатов, которое содержит в себе все результаты спортивных событий, а также предоставляет возможность следить за матчами в прямом эфире.

## Database Diagram
<table>
    <tr>
         <img src="https://i.imgur.com/93JnnuQ.jpg" alt="diagram">
    </tr>
</table>

## Usage
1. Specify the connection string in FootballTracker/Model/AppContext.cs and FootballFetcher/Model/AppContext.cs
2. Open a command prompt and enter the following commands:
- dotnet tool install --global dotnet-ef
- cd FootballTracker
- dotnet ef migrations add InitialCreate
- dotnet ef database update
3. After these commands the database will be created
4. Run FootballFetcher app
5. Enter 1 in the console that opens. The download process takes about 30-60 minutes
6. Run FootballTracker app
7. P.S. If you want to update the match data in real time, start the FootballFetcher app and enter 2. Match data and statistics will be updated every minute
## Screenshots

<table>
    <tr>
            <img src="https://i.imgur.com/4wIYlRL.png" alt="Screenshot">
    </tr>
    <tr>
            <img src="https://i.imgur.com/iKVXzvO.png" alt="Screenshot">
    </tr>
    <tr>
            <img src="https://i.imgur.com/RCBQKpI.png" alt="Screenshot">
    </tr>
    <tr>
            <img src="https://i.imgur.com/4YNbfpv.png" alt="Screenshot">
    </tr>
    <tr>
            <img src="https://i.imgur.com/DwjGl9r.png" alt="Screenshot">
    </tr>
    <tr>
            <img src="https://i.imgur.com/BydeDos.png" alt="Screenshot">
    </tr>
</table>

## Developers

- [FoxJefisto](https://github.com/FoxJefisto)
