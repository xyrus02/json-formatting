using Newtonsoft.Json;

namespace XyrusWorx.Json;

public static class ColorJsonConvert
{
    public static string Serialize(object? obj, Formatting formatting = Formatting.Indented, ColorJsonConfiguration? configuration = null)
    {
        var serializer = new JsonSerializer();
        
        using var stringWriter = new StringWriter();
        using var writer = new ColorJsonWriter(stringWriter, configuration);

        serializer.Formatting = formatting;
        serializer.Serialize(writer, obj);
        
        writer.Flush();
        return stringWriter.ToString();
    }
}