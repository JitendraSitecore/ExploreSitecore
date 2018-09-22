using Himalaya.DXP.Foundation.Search;
using Himalaya.DXP.Foundation.Search.Model;
using Himalaya.DXP.Foundation.Search.SearchRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Commerce.Learning.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult GetProductForCateogory()
        {

            SearchManager searchManager = new SearchManager();

            SearchCriteriaInput searchCriteriaInput = new SearchCriteriaInput();
            searchCriteriaInput.TemplateID = "225F8638261148419B8919A5440A1DA1";
            searchCriteriaInput.Language = "en";
            searchCriteriaInput.TemplateName = "Commerce Product";
            searchCriteriaInput.ParentCategory = "Habitat_Master-Appliances";

            ProductTemplateCriteria productTemplateCriteria = new ProductTemplateCriteria(searchCriteriaInput);
            LanguageCriteria languageCriteria = new LanguageCriteria(searchCriteriaInput);

            searchManager.EnrollSearchCriteria(productTemplateCriteria);
            searchManager.EnrollSearchCriteria(languageCriteria);

            SearchResultsResponse<SearchResult> searchResultsResponse =
                searchManager.ExecuteQuery("sitecore_master_index", searchCriteriaInput, 1, 100);

            List<SearchResult> lstSearchResult = searchResultsResponse.SearchResults.ToList();

            return View("~/Views/Search/SearchResult.cshtml", lstSearchResult);

        }

        [HttpGet]
        public JsonResult GetProductForCategoryJSON(Int16 pageNo)
        {
            SearchManager searchManager = new SearchManager();

            SearchCriteriaInput searchCriteriaInput = new SearchCriteriaInput();
            searchCriteriaInput.TemplateID = "225F8638261148419B8919A5440A1DA1";
            searchCriteriaInput.Language = "en";
            searchCriteriaInput.TemplateName = "Commerce Product";
            searchCriteriaInput.ParentCategory = "Habitat_Master-Appliances";

            ProductTemplateCriteria productTemplateCriteria = new ProductTemplateCriteria(searchCriteriaInput);
            LanguageCriteria languageCriteria = new LanguageCriteria(searchCriteriaInput);

            searchManager.EnrollSearchCriteria(productTemplateCriteria);
            searchManager.EnrollSearchCriteria(languageCriteria);

            SearchResultsResponse<SearchResult> searchResultsResponse =
                searchManager.ExecuteQuery("sitecore_master_index", searchCriteriaInput, pageNo, 20);

            List<SearchResult> lstSearchResult = searchResultsResponse.SearchResults.ToList();

            return this.Json(lstSearchResult);
        }
    }
}