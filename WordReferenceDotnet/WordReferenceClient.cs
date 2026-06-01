using System.Runtime.InteropServices.JavaScript;
using AngleSharp;
using AngleSharp.Dom;

namespace WordReferenceDotnet;

public class WordReferenceClient
{
    /*private readonly HttpClient _client;

    public WordReferenceClient()
    {
        _client = new HttpClient() { BaseAddress =  new Uri("https://wordreference.com/") };
        _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible)");
    }*/
    
    public async Task<TranslationResult> TranslateAsync(string word, string from, string to, bool includeAdditional = false)
    {
        if (string.IsNullOrWhiteSpace(word))
            return new TranslationResult { Error = "A word to translate is required." };
        
        if (!ValidateLanguage(from))
            return new TranslationResult { Error = "From language is invalid. Must be an ISO 639-1 alpha-2 code." };

        if (!ValidateLanguage(to))
            return new TranslationResult { Error = "From language is invalid. Must be an ISO 639-1 alpha-2 code." };

        word = word.Trim();
        var address = $"https://www.wordreference.com/{from}{to}/{word}";
        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var document = await context.OpenAsync(address);

        var tables = document.QuerySelectorAll("table.WRD");
        var translations = new List<TranslatedWord>();
        foreach (var table in tables)
        {
            if (table.Id == "compound_forms" && !includeAdditional)
                continue;

            var translationRows = table.QuerySelectorAll(".odd, .even");
            foreach (var translationRow in translationRows)
            {
                if (translationRow.Children.Count < 3)
                    continue;
                
                var translatedWord = translationRow.Children[2].ChildNodes.First(x => x.NodeType == NodeType.Text).TextContent.Trim();
                var translatedContext = translationRow.Children[1].ChildNodes.FirstOrDefault(x => x.NodeType == NodeType.Text)?.TextContent.Trim();
                var translationType = translationRow.Children[2].QuerySelector("em.POS2")?.TextContent.Trim();
                
                translations.Add(new TranslatedWord(translatedWord, translatedContext, translationType));
            }
        }

        return new TranslationResult
        {
            Result = new Translation(from, to, word, translations),
        };
    }

    private bool ValidateLanguage(string lang)
    {
        return lang switch
        {
            "en" => true,
            "es" => true,
            "fr" => true,
            "it" => true,
            "de" => true,
            "nl" => true,
            "pt" => true,
            "sv" => true,
            "ru" => true,
            "pl" => true,
            "ro" => true,
            "cz" => true,
            "gr" => true,
            "tr" => true,
            "zh" => true,
            "ja" => true,
            "ko" => true,
            "ar" => true,
            _ => false
        };
    }
}