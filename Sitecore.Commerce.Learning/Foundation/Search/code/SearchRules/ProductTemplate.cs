using Himalaya.DXP.Foundation.Search.Model;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using System;
using System.Linq.Expressions;

namespace Himalaya.DXP.Foundation.Search.SearchRules
{
    /// <summary>
    /// Product template criteria
    /// </summary>
    public class ProductTemplateCriteria : ISearchCriteria
    {
        private SearchCriteriaInput SearchCriteriaParameter { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchCriteriaInput"></param>
        public ProductTemplateCriteria(SearchCriteriaInput searchCriteriaInput)
        {
            SearchCriteriaParameter = searchCriteriaInput;
        }

        /// <summary>
        /// Get expression
        /// </summary>
        /// <returns></returns>
        public Expression<Func<SearchResult, bool>> GetExpression()
        {
            Expression<Func<SearchResult, bool>> query = PredicateBuilder.False<SearchResult>();
            query = query.Or(x => x.TemplateId == new ID(SearchCriteriaParameter.TemplateID));

            return query;
        }
    }
}