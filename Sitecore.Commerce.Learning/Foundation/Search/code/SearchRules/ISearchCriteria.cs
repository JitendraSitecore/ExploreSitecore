using System;
using System.Linq.Expressions;
using Himalaya.DXP.Foundation.Search.Model;


namespace Himalaya.DXP.Foundation.Search.SearchRules
{
    /// <summary>
    /// Search criteria 
    /// </summary>
    public interface ISearchCriteria
    {
        /// <summary>
        /// Get expression for search criteria
        /// </summary>
        /// <returns></returns>
        Expression<Func<SearchResult, bool>> GetExpression();

    }
}
