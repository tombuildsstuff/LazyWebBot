namespace LazyWebBot.Playground
{
    using System;

    using LazyWebBot.Services.Parser;
    using LazyWebBot.Services.Parser.Calais;
    using LazyWebBot.Services.Parser.Wolfram;
    using LazyWebBot.Services.Search;

    public class Program
    {
        public static void Main(string[] args)
        {
            var questions = new[]
            {
                "What colour is the sky?",
                "Where does one buy Parker Pen refills?"
            };
            const string OpenCalaisLicenceKey = "kfeda958p452a8dekg7rvn3j";
            const string WolframAlphaKey = "74V7RV-W4WAKAA2K6";

            var parser = new TextParser(new OpenCalaisService { LicenseKey = OpenCalaisLicenceKey }, new WolframAlphaService { LicenseKey = WolframAlphaKey });

            foreach (var question in questions)
            {
                var term = parser.GetValuableWords(question);
                Console.WriteLine("{0}: {1}", question, !string.IsNullOrWhiteSpace(term) ? term : "(unknown)");
            }

            Console.ReadLine();
        }

        private static void TestSearch()
        {
            var service = new SearchService();
            var terms = new[]
                            {
                                "Parker Pen refills", "online invoice generators",
                                "batch-convert .m4a-&gt;.mp3, regardless of folder structure",
                            };

            foreach (var term in terms)
            {
                var uri = service.GetUriForTerm(term);
                Console.WriteLine(uri != null ? uri.ToString() : "(not found)");
            }
        }
    }
}