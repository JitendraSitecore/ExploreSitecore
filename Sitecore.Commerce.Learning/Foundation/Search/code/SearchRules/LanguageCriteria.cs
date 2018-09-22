using Himalaya.DXP.Foundation.Search.Model;
using Sitecore.ContentSearch.Linq.Utilities;
using System;
using System.Linq.Expressions;

namespace Himalaya.DXP.Foundation.Search.SearchRules
{
    public class LanguageCriteria : ISearchCriteria
    {
        private SearchCriteriaInput SearchCriteriaParameter { get; set; }
        public LanguageCriteria(SearchCriteriaInput searchCriteriaInput)
        {
            SearchCriteriaParameter = searchCriteriaInput;
        }

        public Expression<Func<SearchResult, bool>> GetExpression()
        {
            Expression<Func<SearchResult, bool>> query = PredicateBuilder.False<SearchResult>();
            query = query.Or(x => x.Language == SearchCriteriaParameter.Language);

            return query;
        }
    }
}