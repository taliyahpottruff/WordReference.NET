namespace WordReferenceDotnet;

public class TranslatedWord(string word, string? context = null, string? type = null)
{
    public string Word { get; set; } = word;
    public string? Type { get; set; } = type;
    public string? Context { get; set; } = context;
}