namespace LazyWebBot.Services.Search
{
    using System;
    using System.Net;

    public class SearchService : ISearchService
    {
        public Uri GetUriForTerm(string term)
        {
            var encodedTerm = Uri.EscapeDataString(term);
            var url = string.Format("http://www.google.co.uk/search?output=search&sclient=psy-ab&q={0}&btnI=", encodedTerm);
            var request = WebRequest.Create(url);
            {
                request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0");
                request.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                request.Headers.Add("accept-language", "en-US,en;q=0.5");
                request.Headers.Add("accept-encoding", "gzip, deflate");
                request.Headers.Add("referrer", "http://www.google.co.uk");

                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.Moved)
                {
                    return request.RequestUri;
                }
            }

            return null;
        }
    }
}