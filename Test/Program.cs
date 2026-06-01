// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using WordReferenceDotnet;

var wr = new WordReferenceClient();

var translation = await wr.TranslateAsync("car", "en", "fr");
var json = JsonSerializer.Serialize(translation, new JsonSerializerOptions { WriteIndented = true });
Console.WriteLine(json);