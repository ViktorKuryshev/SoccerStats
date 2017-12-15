﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace SoccerStats
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "SoccerGameResults.csv");
            var fileContents = ReadSoccerResults(fileName);
			fileName = Path.Combine(directory.FullName, "players.json");
			var players = DeserializePlayers(fileName);

			foreach (var player in players)
			{
				Console.WriteLine(player.FirstName);
			}

		}

        public static string ReadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }

        public static List<GameResult> ReadSoccerResults(string fileName)
        {
            var soccerResults = new List<GameResult>();
            using (var reader = new StreamReader(fileName))
            {
                string line = "";
                reader.ReadLine(); //чтобы переместить позицию ридера на вторую строку с которой и начинаются данные
                while ((line = reader.ReadLine())!=null)
                {
                    var gameResult = new GameResult();
                    string[] values = line.Split(',');
                    
                    DateTime gameDate;
                    if(DateTime.TryParse(values[0], out gameDate)) //Проверяем правильные ли данные TryParse возвращает True/False
                    {
                        gameResult.GameDate = gameDate;
                    }
					gameResult.TeamName = values[1];

					HomeOrAway homeOrAway;
					if(Enum.TryParse(values[2], out homeOrAway))
					{
						gameResult.HomeOrAway = homeOrAway;
					}

					int parseInt;
					if(int.TryParse(values[3], out parseInt))
					{
						gameResult.Goals = parseInt;
					}

					if (int.TryParse(values[4], out parseInt))
					{
						gameResult.GoalAttempts = parseInt;
					}

					if (int.TryParse(values[5], out parseInt))
					{
						gameResult.ShotsOnGoal = parseInt;
					}

					if (int.TryParse(values[6], out parseInt))
					{
						gameResult.ShotsOffGoal = parseInt;
					}

					double posessionPercent;
					if(double.TryParse(values[7], out posessionPercent))
					{
						gameResult.PosessionPercent = posessionPercent;
					}

					
					soccerResults.Add(gameResult);
                }
            }
            return soccerResults;
        }

		public static List<Player> DeserializePlayers(string fileName)
		{
			var players = new List<Player>();
			var serializer = new JsonSerializer();
			using (var reader = new StreamReader(fileName))
			using (var jasonReader = new JsonTextReader(reader))
			{
				players = serializer.Deserialize<List<Player>>(jasonReader);
			}
				return players;
		}
    }
}
