using System.Text.Json;
using Newtonsoft.Json;
using XyrusWorx.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

var obj = new
{
    someObject = new
    {
        name = "example  " + '\\' + " " + '"' + "quoted" + '"' + " / 'text'",
        data = new
        {
            shortString = "S",
            nested = new
            {
                nested = new
                {
                    ID = "SGML",
                    Code = ".. unícødè ☁ ☀ chârs ..",
                    GlossTerm = "Standard Generalized Markup Language",
                    Acronym = "SGML",
                    Abbrev = "ISO 8879:1986",
                    definition = new
                    {
                        desc = "A meta-markup null, language, used to create markup languages such as DocBook.",
                        See_Also_This = new[] { "GML", "XML" }
                    },
                    GlossSee = "markup"
                }
            },
            value1 = 0,
            value2 = 1.2346,
            value3 = -2340,
            Yes = true,
            No = false,
            Nothing = (object)null!,
            list_of_doubles = new[] { 0, 1d / 4, 2d / 3, -3 },
            hashOfIntegers = new
            {
                a0 = 0,
                a1 = 1,
                a2 = 2
            }
        }
    }
};

Console.WriteLine("== System.Text.Json ==\n");
Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions{WriteIndented = true}));
            
Console.WriteLine("== Newtonsoft JSON ==\n");
Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
            
Console.WriteLine("\n\n== Pretty Text format indent with 4 spaces ==\n");
Console.WriteLine(ColorJsonConvert.Serialize(obj, Formatting.Indented, new ColorJsonConfiguration()));

Console.WriteLine("\n\n== Pretty Text custom colors ==\n");
Console.WriteLine(ColorJsonConvert.Serialize(obj, Formatting.Indented, new ColorJsonConfiguration
{
    StringsColor = ConsoleColor.DarkRed,
    KeywordsColor = ConsoleColor.Cyan,
    HashKeysColor = ConsoleColor.DarkYellow
}));
