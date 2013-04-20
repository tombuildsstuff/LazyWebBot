namespace LazyWebBot.Services.Search
{
    using System;

    public interface ISearchService
    {
        Uri GetUriForTerm(string term);
    }
}