namespace DL.PrettyText
{
    using System.Text.RegularExpressions;
    using System.Web.Script.Serialization;

    public static class JsonFormatter
    {
        public static string Format(object obj, ushort indent = 4)
        {
            return Format(new JavaScriptSerializer().Serialize(obj), indent);
        }

        public static string Format(string json, ushort indent = 4)
        {
            if (string.IsNullOrEmpty(json))
            {
                return string.Empty;
            }

            return new JsonFormatterInternals.JsonFormatter(indent).Format(json);
        }

        public static string Minify(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return string.Empty;
            }

            return Regex.Replace(json, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
        }
    }
}
