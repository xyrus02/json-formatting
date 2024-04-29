using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using Newtonsoft.Json;

namespace XyrusWorx.Json;

public class ColorJsonWriter(TextWriter writer, ColorJsonConfiguration? configuration = default) : JsonTextWriter(writer)
{
    private static readonly Dictionary<ConsoleColor, string> _prefixes = new();
    private static readonly string _suffix;
    
    private const string _internalWritePropertyName = "InternalWritePropertyName";
    private const string _internalWriteValue = "InternalWriteValue";
    private const string _writeValueInternal = "WriteValueInternal";
    private const string _writeValueToBuffer = "WriteValueToBuffer";
    private const string _writeEscapedString = "WriteEscapedString";
    
    private static readonly Dictionary<string, MethodInfo> _methodCache = new();
    private static readonly FieldInfo? _writeBuffer;
    
    // ReSharper disable once ReplaceWithPrimaryConstructorParameter
    private readonly TextWriter _writer = writer;
    private readonly ColorJsonConfiguration _configuration = configuration ?? new ColorJsonConfiguration();

    static ColorJsonWriter()
    {
        const char esc = (char)27;
        var colorMap = new Dictionary<ConsoleColor, Color>
        {
            [ConsoleColor.Black] = Color.FromArgb(0),
            [ConsoleColor.DarkBlue] = Color.FromArgb(139),
            [ConsoleColor.DarkGreen] = Color.FromArgb(25600),
            [ConsoleColor.DarkCyan] = Color.FromArgb(35723),
            [ConsoleColor.DarkRed] = Color.FromArgb(9109504),
            [ConsoleColor.DarkMagenta] = Color.FromArgb(9109643),
            [ConsoleColor.DarkYellow] = Color.FromArgb(8421376),
            [ConsoleColor.Gray] = Color.FromArgb(8421504),
            [ConsoleColor.DarkGray] = Color.FromArgb(11119017),
            [ConsoleColor.Blue] = Color.FromArgb(255),
            [ConsoleColor.Green] = Color.FromArgb(32768),
            [ConsoleColor.Cyan] = Color.FromArgb(65535),
            [ConsoleColor.Red] = Color.FromArgb(16711680),
            [ConsoleColor.Magenta] = Color.FromArgb(16711935),
            [ConsoleColor.Yellow] = Color.FromArgb(16776960),
            [ConsoleColor.White] = Color.FromArgb(16777215)
        };
        
        _suffix = esc + "[0m";
        
        foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
        {
            var colorRgb = colorMap[color];
            _prefixes.Add(color, $"{esc}[38;2;{colorRgb.R};{colorRgb.G};{colorRgb.B}m");
        }
        
        _writeBuffer = typeof(JsonTextWriter).GetField("_writeBuffer", BindingFlags.Instance | BindingFlags.NonPublic)!;
        if (_writeBuffer == null)
            throw new MissingFieldException(
                "Incompatible version of Newtonsoft.Json referenced. Please contact the authors and ask them to support the newest version.");
    }

    public override void WriteStartObject() => WithColor(base.WriteStartObject, _configuration.DelimitersColor);
    public override void WriteStartArray() => WithColor(base.WriteStartArray, _configuration.DelimitersColor);
    public override void WriteStartConstructor(string name) => WithColor(base.WriteStartConstructor, name, _configuration.DelimitersColor);

    public override void WritePropertyName(string name)
    {
        Call(_internalWritePropertyName, name);
        
        if (QuoteName) WriteQuote();
        WithColor(() => Call(_writeEscapedString, name, false), _configuration.HashKeysColor);
        if (QuoteName) WriteQuote();
        
        WithColor(() => _writer.Write(":"), _configuration.DelimitersColor);
    }
    public override void WritePropertyName(string name, bool escape)
    {
        Call(_internalWritePropertyName, name);
        
        if (QuoteName) WriteQuote();
        if (escape)
        {
            WithColor(() => Call(_writeEscapedString, name, false), _configuration.HashKeysColor);
        }
        else
        {
            WithColor(() => _writer.Write(name), _configuration.HashKeysColor);
        }
        if (QuoteName) WriteQuote();
        
        WithColor(() => _writer.Write(":"), _configuration.DelimitersColor);
    }

    protected override void WriteEnd(JsonToken token) => WithColor(base.WriteEnd, token, _configuration.DelimitersColor);
    protected override void WriteValueDelimiter() => WithColor(base.WriteValueDelimiter, _configuration.DelimitersColor);
    
    public override void WriteValue(int value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(uint value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(long value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(ulong value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(float value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(double value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(bool value) => WithColor(base.WriteValue, value, _configuration.KeywordsColor);
    public override void WriteValue(short value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(ushort value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(char value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(byte value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(sbyte value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    public override void WriteValue(decimal value) => WithColor(base.WriteValue, value, _configuration.NumbersColor);
    
    public override void WriteValue(object? value)
    {
        if (value == null)
        {
            WriteNull();
            return;
        }
        
        var color = value switch
        {
            decimal or double or float or ulong or long 
                or uint or int or ushort or short or byte 
                or sbyte or BigInteger
                => _configuration.NumbersColor,
            string => _configuration.StringsColor,
            bool => _configuration.KeywordsColor,
            _ => _configuration.DefaultColor 
        };

        if (color == _configuration.DefaultColor)
        {
            if (value.GetType().IsValueType) 
                base.WriteValue(value);
            else WriteRawEntity(value);
        }

        WithColor(() => base.WriteValue(value), color);
    }
    public override void WriteValue(string? value)
    {
        Call(_internalWriteValue, JsonToken.String);

        if (value == null)
        {
            WithColor(() => Call(_writeValueInternal, JsonToken.Null), _configuration.KeywordsColor);
        }
        else
        {
            if (QuoteName) WriteQuote();
            WithColor(() => Call(_writeEscapedString, value, false), _configuration.StringsColor);
            if (QuoteName) WriteQuote();
        }
    }
    public override void WriteValue(DateTime value)
    {
        Call(_internalWriteValue, JsonToken.Date);
        value = EnsureDateTime(value, DateTimeZoneHandling);
        
        WriteQuote();
        
        if (string.IsNullOrEmpty(DateFormatString))
        {
            var length = (int)Call(_writeValueToBuffer, value)!;
            var buffer = WriteBuffer()!;
            
            Debug.Assert(buffer != null, "Write buffer is null!!");

            var serialized = new string(buffer[..(length-1)]).Trim(QuoteChar);
            WithColor(() => _writer.Write(serialized), _configuration.StringsColor);
        }
        else
        {
            WithColor(() => _writer.Write(value.ToString(DateFormatString, Culture)), _configuration.StringsColor);
        }
        WriteQuote();
    }
    public override void WriteValue(DateTimeOffset value)
    {
        Call(_internalWriteValue, JsonToken.Date);
        
        WriteQuote();
        
        if (string.IsNullOrEmpty(DateFormatString))
        {
            var length = (int)Call(_writeValueToBuffer, value)!;
            var buffer = WriteBuffer()!;
            
            Debug.Assert(buffer != null, "Write buffer is null!!");

            var serialized = new string(buffer[..(length-1)]).Trim(QuoteChar);
            WithColor(() => _writer.Write(serialized), _configuration.StringsColor);
        }
        else
        {
            WithColor(() => _writer.Write(value.ToString(DateFormatString, Culture)), _configuration.StringsColor);
        }
        WriteQuote();
    }
    public override void WriteValue(byte[]? value)
    {
        if (value == null)
        {
            WriteNull();
        }
        else
        {
            Call(_internalWriteValue, JsonToken.Bytes);
            WriteQuote();
            WithColor(() => _writer.Write(Convert.ToBase64String(value)), _configuration.StringsColor);
            WriteQuote();
        }
    }
    public override void WriteValue(float? value)
    {
        if (value == null)
        {
            WriteNull();
        }
        else
        {
            WriteValue(value.Value);
        }
    }
    public override void WriteValue(double? value)
    {
        if (value == null)
        {
            WriteNull();
        }
        else
        {
            WriteValue(value.Value);
        }
    }
    public override void WriteValue(Guid value)
    {
        Call(_internalWriteValue, JsonToken.String);

        var text = value.ToString("D", CultureInfo.InvariantCulture);
        
        WriteQuote();
        WithColor(() => _writer.Write(text), _configuration.StringsColor);
        WriteQuote();
    }
    public override void WriteValue(TimeSpan value)
    {
        Call(_internalWriteValue, JsonToken.String);

        var text = value.ToString(null, CultureInfo.InvariantCulture);
        
        WriteQuote();
        WithColor(() => _writer.Write(text), _configuration.StringsColor);
        WriteQuote();
    }
    public override void WriteValue(Uri? value)
    {
        if (value == null)
        {
            WriteNull();
        }
        else
        {
            Call(_internalWriteValue, JsonToken.String);
            WriteQuote();
            WithColor(() => Call(_writeEscapedString, value.OriginalString, false), _configuration.StringsColor);
            WriteQuote();
        }
    }

    public override void WriteComment(string? text) => WithColor(base.WriteComment, text, _configuration.CommentColor);

    public override void WriteNull() => WithColor(base.WriteNull, _configuration.KeywordsColor);
    public override void WriteUndefined() => WithColor(base.WriteUndefined, _configuration.KeywordsColor);

    public override void WriteRaw(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            base.WriteRaw(json);
            return;
        }
        
        var deserialized = JsonConvert.DeserializeObject(json);
        WriteRawEntity(deserialized);
    }

    private void WriteRawEntity(object? entity)
    {
        if (entity == null)
        {
            WriteNull();
            return;
        }
        
        using var stringWriter = new StringWriter();
        using var nestedWriter = new ColorJsonWriter(stringWriter, _configuration);
        
        new JsonSerializer().Serialize(nestedWriter, entity);
        
        nestedWriter.Flush();
        stringWriter.Flush();
        
        base.WriteRaw(stringWriter.ToString());
    }
    private void WriteQuote()
    {
        var quote = new string(QuoteChar, 1);
        WithColor(() => _writer.Write(quote), _configuration.DelimitersColor);
    }

    private DateTime EnsureDateTime(DateTime value, DateTimeZoneHandling timeZone)
    {
        switch (timeZone)
        {
            case DateTimeZoneHandling.Local:
                switch (value.Kind)
                {
                    case DateTimeKind.Unspecified:
                        value = new DateTime(value.Ticks, DateTimeKind.Local);
                        break;
                    case DateTimeKind.Utc:
                        value = value.ToLocalTime();
                        break;
                }
                break;
            case DateTimeZoneHandling.Utc:
                switch (value.Kind)
                {
                    case DateTimeKind.Unspecified:
                        value = new DateTime(value.Ticks, DateTimeKind.Utc);
                        break;
                    case DateTimeKind.Local:
                        value = value.ToUniversalTime();
                        break;
                }
                break;
            case DateTimeZoneHandling.Unspecified:
                value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
                break;
            case DateTimeZoneHandling.RoundtripKind:
                break;
            default:
                throw new ArgumentException("Invalid date time handling value.");
        }

        return value;
    }
    
    private void WithColor(Action action, ConsoleColor color)
    {
        try
        {
            _writer.Write(_prefixes[color]);
            action.Invoke();
        }
        finally
        {
            _writer.Write(_suffix);
        }
    }
    private void WithColor<T>(Action<T> action, T value, ConsoleColor color)
    {
        WithColor(() => action.Invoke(value), color);
    }
    
    private object? Call(string method, params object?[] parameters)
    {
        MethodInfo methodInfo;
        
        if (_methodCache.TryGetValue(method, out var maybeFoundMethod))
        {
            methodInfo = maybeFoundMethod;
        }
        else
        {
            methodInfo = typeof(JsonTextWriter).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic) ?? 
                         throw new MissingMethodException("Method not found: " + method);
            
            if (methodInfo == null)
                throw new MissingFieldException(
                    "Incompatible version of Newtonsoft.Json referenced. Please contact the authors and ask them to support the newest version.");
            
            _methodCache.Add(method, methodInfo);
        }

        return methodInfo.Invoke(this, parameters.ToArray());
    }
    private char[]? WriteBuffer() => _writeBuffer?.GetValue(this) as char[];
}