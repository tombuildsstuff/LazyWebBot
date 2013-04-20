namespace LazyWebBot.Playground
{
    using System;

    using LazyWebBot.Services.Search;

    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new SearchService();
            var terms = new[]
            {
                "Parker Pen refills",
                "online invoice generators",
                "batch-convert .m4a-&gt;.mp3, regardless of folder structure",
            };
            
            foreach (var term in terms)
            {
                var uri = service.GetUriForTerm(term);
                Console.WriteLine(uri != null ? uri.ToString() : "(not found)");
            }

            Console.ReadLine();
        }
    }
}