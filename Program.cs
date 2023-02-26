string inputFilePath = "in.txt";
string outputFilePath = "out.txt";
var Players = new List<Player>();

SetOptions(args, ref inputFilePath, ref outputFilePath);

foreach (var line in File.ReadAllLines(inputFilePath))
{
    SetPlayers(line);
}

foreach (var player in Players)
{
    Console.WriteLine(player);
}

CalculateWinner(Players);

/***************************** Helper methods and function */
static void SetOptions(string[] args, ref string @in, ref string @out)
{
    for (int i = 0; i < args.Length; i++)
    {
        switch (args[i])
        {
            case "--in":
                @in = args[++i];
                break;
            case "--out":
                @out = args[++i];
                break;
            default:
                Console.WriteLine($"Unknown option: {args[i]}");
                return;
        }
    }
}

void CalculateWinner(List<Player> Players)
{
    int maxScore = Players.Max(p => p.Score);

    IEnumerable<Player> winners = Players.Where(p => p.Score == maxScore);

    string winnerNames = string.Join(",", winners.Select(p => p.Name));

    Console.WriteLine("{0} has the highest score of {1}", winnerNames, maxScore);
    Console.WriteLine("The result is saved in {0}", outputFilePath);

    File.WriteAllText(outputFilePath, $"{winnerNames}: {maxScore}");
}

void SetPlayers(string line)
{
    var l = line.Split(":");
    var name = l[0];
    var cards = l[1].Split(",");

    var player = new Player
    {
        Name = name,
        Cards = new List<Card>()
    };

    foreach (var card in cards)
    {
        var c = new Card(card);
        player.Cards.Add(c);
    }

    Players.Add(player);
}

class Player
{
    public string Name { get; set; }
    public List<Card> Cards { get; set; }

    public int Score => Cards.Sum(card => card.FaceValue + card.SuitValue);

    public override string ToString()
    {
        return $"{Name}: {string.Join(",", Cards)}:{Score}";
    }
}

class Card
{
    public int FaceValue { get { return CalculateFaceValue(); } }
    public int SuitValue { get { return CalculateSuitValue(); } }
    public string Face { get; set; }
    public char Suit { get; set; }

    public Card(string card)
    {
        if (!string.IsNullOrEmpty(card))
        {
            if (card.Length == 2)
            {
                Face = card[0].ToString();
                Suit = card[1];
            }
            else if (card.Length == 3)
            {
                Face = card[0].ToString() + card[1].ToString();
                Suit = card[2];
            }
        }
    }

    public override string ToString()
    {
        return $"{Face}{Suit}";
    }

    int CalculateFaceValue()
    {
        if (int.TryParse(Face, out int value))
        {
            return value;
        }

        Dictionary<string, int> FaceValues = new()
        {
            { "J", 11 },
            { "Q", 12 },
            { "K", 13 },
            { "A", 11 }
        };

        return FaceValues[Face.ToString().ToUpper()];
    }

    int CalculateSuitValue()
    {
        if (char.IsDigit(Suit))
        {
            _ = int.TryParse(Suit.ToString(), out int value);
            return value;
        }

        Dictionary<string, int> SuitValues = new()
        {
            { "H", 1 },
            { "S", 2 },
            { "C", 3 },
            { "D", 4 }
        };

        return SuitValues[Suit.ToString().ToUpper()];
    }
}