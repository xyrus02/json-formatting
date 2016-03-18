namespace Demo
{
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;

    public class Program
    {
        public static void Main(string[] args)
        {
            var obj = GetData();

            System.Console.WriteLine("== System.Web.Script.Serialization ==\n");
            System.Console.WriteLine(new JavaScriptSerializer().Serialize(obj));

            System.Console.WriteLine("\n\n== JSON.Net format ==\n");
            System.Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));

            System.Console.WriteLine("\n\n== Pretty Text format indent with 4 spaces ==\n");
            System.Console.WriteLine(DL.PrettyText.Formatter.Format(obj, 4));

            System.Console.WriteLine("\n\n== Pretty Text custom colors ==\n");
            DL.PrettyText.Console.Write(obj, 3, true);

            System.Console.ReadLine();
        }

        private static object GetData()
        {
            return new
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
                        Nothing = (object)null,
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
        }
    }
}
