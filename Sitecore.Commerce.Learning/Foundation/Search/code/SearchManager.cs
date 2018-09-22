using Sitecore.ContentSearch.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Himalaya.DXP.Foundation.Search.Model;
using System.Linq.Expressions;
using Himalaya.DXP.Foundation.Search.SearchRules;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;

namespace Himalaya.DXP.Foundation.Search
{
    /// <summary>
    /// Search Manager
    /// </summary>
    public class SearchManager
    {
        Expression<Func<SearchResult, bool>> QueryBuilder;
        List<ISearchCriteria> SearchCriterias;

        string ParentCatalogPath = "/sitecore/content/sitecore/storefront/home/catalogs/habitat_master/habitat_master-departments/";

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchManager()
        {
            QueryBuilder = PredicateBuilder.True<SearchResult>();
            SearchCriterias = new List<ISearchCriteria>();
        }

        /// <summary>
        /// Register criterias
        /// </summary>
        /// <param name="searchCriteria"></param>
        public void EnrollSearchCriteria(ISearchCriteria searchCriteria)
        {
            SearchCriterias.Add(searchCriteria);
        }

        /// <summary>
        /// Execute query inside the solr
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public SearchResultsResponse<SearchResult> ExecuteQuery(string indexName, Expression<Func<SearchResult, string>> orderBy, int pageNo, int pageSize)
        {
            SearchResultsResponse<SearchResult> searchResults = null;
            
            ISearchIndex index = ContentSearchManager.GetIndex(indexName);
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                IQueryable<SearchResult> query = context.GetQueryable<SearchResult>();

                BuildPredicateBuilder();

                SearchResults<SearchResult> queryResults = query.Where(QueryBuilder).OrderBy(orderBy).Page(pageNo, pageSize).GetResults();
                searchResults = FillSearchResponse(queryResults.Hits.Select(x => x.Document), 100);
            }

            return searchResults;
        }


        /// <summary>
        /// Execute query inside the solr
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public SearchResultsResponse<SearchResult> ExecuteQuery(string indexName, SearchCriteriaInput searchCriteriaInput, int pageNo, int pageSize)
        {
            SearchResultsResponse<SearchResult> searchResults = null;

            ISearchIndex index = ContentSearchManager.GetIndex(indexName);
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                IQueryable<SearchResult> query = context.GetQueryable<SearchResult>();

                BuildPredicateBuilder();

                query = query.Where(x => x.TemplateName == searchCriteriaInput.TemplateName);
                query = query.Where(x => x.Language == searchCriteriaInput.Language);

                if (!searchCriteriaInput.IsAllProduct)
                {
                    query = query.Where(x => x.ProductCategoryList.Contains(searchCriteriaInput.ParentCategory));
                }

                SearchResults <SearchResult> queryResults = query.Page(pageNo, pageSize).GetResults();
                searchResults = FillSearchResponse(queryResults.Hits.Select(x => x.Document), 100);
            }

            return searchResults;
        }


        /// <summary>
        /// Build Prediate builders
        /// </summary>
        private void BuildPredicateBuilder()
        {
            foreach(ISearchCriteria searchCriteria in SearchCriterias)
            {
                QueryBuilder = QueryBuilder.And(searchCriteria.GetExpression());
            }
        }

        /// <summary>
        /// Fill response
        /// </summary>
        /// <param name="searchResults"></param>
        /// <param name="totalResults"></param>
        /// <returns></returns>
        private SearchResultsResponse<SearchResult> FillSearchResponse(IEnumerable<SearchResult> searchResults, int totalResults)
        {
            SearchResultsResponse<SearchResult> searchResponseResults = null;

            if(searchResults.Any())
            {
                searchResponseResults = new SearchResultsResponse<SearchResult>();

                searchResponseResults.SearchResults = searchResults;
                searchResponseResults.TotalRecords = totalResults;
            }

            return searchResponseResults;
        }

    }
}