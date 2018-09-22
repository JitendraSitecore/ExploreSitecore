using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Himalaya.DXP.Foundation.Search.Model
{
    [Serializable]
    public class SearchResultsResponse<T> where T : new()
    {
        public IEnumerable<T> SearchResults { get; set; }
        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public double TotalPagesCount { get; set; }
        public string SearchedKeyword { get; set; }

        public string NoResultfoundText { get; set; }
        public string SearchResultSuccessText { get; set; }
    }
}