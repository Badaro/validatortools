using Newtonsoft.Json;
using ValidatorTools;

string formatDataFolder = "Formats";
string outputFolder = "Output";
string setDataUrl = "https://mtgjson.com/api/v5/{set}.json";

HttpClient client = new HttpClient();
Dictionary<string, string[]> loadedSets = new Dictionary<string, string[]>();

if(!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

foreach (var inputFile in Directory.GetFiles(formatDataFolder, "*.json"))
{
    string outputFile = Path.Combine(outputFolder, Path.GetFileName(inputFile));

    HashSet<string> formatCards = new System.Collections.Generic.HashSet<string>();
    var formatData = JsonConvert.DeserializeObject <FormatData>(File.ReadAllText(inputFile));
    foreach (var set in formatData.Sets)
    {
        if(!loadedSets.ContainsKey(set))
        {
            string setUrl = setDataUrl.Replace("{set}", set);

            Console.WriteLine($"Loading data for {set} using {setUrl}");
            string setJson = await client.GetStringAsync(setUrl);
            dynamic setData = JsonConvert.DeserializeObject<dynamic>(setJson);

            HashSet<string> setCards = new HashSet<string>();
            foreach (var card in setData.data.cards)
            {
                string cardName = card.name;
                if (!setCards.Contains(cardName)) setCards.Add(cardName);
            }

            loadedSets.Add(set, setCards.ToArray());
        }

        Console.WriteLine($"Adding to format cards from {set}");
        foreach (var card in loadedSets[set]) if (!formatCards.Contains(card)) formatCards.Add(card);
    }

    foreach (string banned in formatData.Banned) formatCards.Remove(banned);

    formatData.Cards = formatCards.Order().ToArray();
    File.WriteAllText(outputFile, JsonConvert.SerializeObject(formatData, Newtonsoft.Json.Formatting.Indented));
}






