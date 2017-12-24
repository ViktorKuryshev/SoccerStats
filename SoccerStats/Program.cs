using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
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
			var topTenPlayers = GetTopTenPlayers(players);

			foreach (var player in topTenPlayers)
			{
				List<NewsResult> newsResults = GetNewsForPlayer(String.Format("{0} {1}", player.FirstName, player.SecondName));
				foreach (var result in newsResults)
				{
					Console.WriteLine(String.Format("Name: {0} Url: {1} \r\n", result.name, result.url));
					Console.ReadKey();
				}
			}
			fileName = Path.Combine(directory.FullName, "topten.json");
			SerializePlayersToFile(topTenPlayers, fileName);
			
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

	    public static List<Player> GetTopTenPlayers(List<Player> players)
	    {
		    var topTenPlayers = new List<Player>();
			players.Sort(new PlayerComparer());
		    int counter = 0;
		    foreach (var player in players)
		    {
			    topTenPlayers.Add(player);
			    counter++;
			    if (counter >= 10) break;
		    }
		    return topTenPlayers;
	    }

	    public static void SerializePlayersToFile(List<Player> players, string fileName)
	    {
			var serializer = new JsonSerializer();
		    using (var writer = new StreamWriter(fileName))
		    using (var jasonWriter = new JsonTextWriter(writer))
		    {
			    serializer.Serialize(jasonWriter, players);
		    }
		   
		}

	    public static string GetHomeGooglePage()
	    {
		    var webClient = new WebClient();
		    byte[] googleHome = webClient.DownloadData("https://www.google.com");

			using (var stream = new MemoryStream(googleHome))
			using(var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
	    }

	    public static List<NewsResult> GetNewsForPlayer(string playerName)
	    {
		    var results = new List<NewsResult>();
			var webClient = new WebClient();
			webClient.Headers.Add("Ocp-Apim-Subscription-Key", "43b127415c9242baa9325d79bdd65267");
		    byte[] searchResults = webClient.DownloadData(string.Format("https://api.cognitive.microsoft.com/bing/v7.0/search?q={0}&mkt=en-us", Uri.EscapeDataString(playerName)));
			var serializer = new JsonSerializer();

			using (var stream = new MemoryStream(searchResults))
			using (var reader = new StreamReader(stream))
				using(var jsonReader = new JsonTextReader(reader))
				{
					results = serializer.Deserialize<NewsSearch>(jsonReader).webPages.NewsResults;
				}

		    return results;
	    }
	}
}
