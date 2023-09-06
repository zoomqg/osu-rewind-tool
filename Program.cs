using Rewind_Scraper;
using System.Text.Json;

string json = File.ReadAllText(System.Environment.CurrentDirectory + "\\secrets.json");

var data = JsonSerializer.Deserialize<JsonSecrets>(json)!;

Console.Write("How many posts do you need sir? (default: 10): ");
string? input = Console.ReadLine();
int postLimit = string.IsNullOrWhiteSpace(input) ? 10 : Convert.ToInt32(input);

var redditWrapper = new RedditWrapper(appId: data.AppId, appSecret: data.AppSecret, refreshToken: data.RefreshToken);

var result = redditWrapper.ProcessPosts(limit: postLimit);

var csvGuy = new CsvGenerator(result);

csvGuy.MakeFile();

Console.WriteLine("Done!");