# XyrusWorx.Json.ColorJsonWriter
I was in need of a JSON serializer, which can write colorized JSON to the console. I found [Dennis Lucato's PrettyText package](https://github.com/dluc/DL.PrettyText.NET)
but quickly found out that it doesn't work on .NET 8 because it relies on a very old .NET Framework. Besides that, it looks like the package has been abandoned by now.

So I re-implemented it basically from scratch (but made a compatible pull request to the original repo first). Same license, same conditions. Thanks Dennis for your work. I hope I could improve on it.

## Download

You can download this package on [Nuget.org](https://www.nuget.org/packages/XyrusWorx.Json.ColorJsonWriter).

## Example usage

```csharp
var configuration = new ColorJsonConfiguration {
    // you can configure the colors here
};

// Create a the writer (same as Newtonsoft.Json.JsonTextWriter but with colors)
using var writer = new ColorJsonWriter(Console.Out);

// Create a new serializer with formatting
var serializer = new JsonSerializer { 
    Formatting = Formatting.Indented 
};
    
// Serialize using the writer
serializer.Serialize(writer, new {
    myString = "string",
    myNumber = 123,
    myObject = {
        childProperty = "something"
    },
    myArray = new string[] { "one", "two", "three" }
});
```