namespace WordReferenceDotnet;

public class Translation(string fromLang, string toLang, string fromWord, ICollection<TranslatedWord> toWords)
{
    public string FromLang { get; set; } = fromLang;
    public string ToLang { get; set; } = toLang;
    public string Original { get; set; } = fromWord;
    public ICollection<TranslatedWord> Translations { get; set; } = toWords;
}