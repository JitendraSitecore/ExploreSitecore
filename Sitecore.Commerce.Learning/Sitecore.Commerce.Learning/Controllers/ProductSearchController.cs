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
    public class ProductSearchController : Controller
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
        public JsonResult GetProductForCategoryJSON(string category, Int16 pageNo)
        {
            SearchManager searchManager = new SearchManager();

            SearchCriteriaInput searchCriteriaInput = new SearchCriteriaInput();
            searchCriteriaInput.TemplateID = "225F8638261148419B8919A5440A1DA1";
            searchCriteriaInput.Language = "en";
            searchCriteriaInput.TemplateName = "Commerce Product";

            if(category.ToUpper().Equals("ALL"))
            {
                searchCriteriaInput.IsAllProduct = true;
            }
            else
            {
                searchCriteriaInput.IsAllProduct = false;
                switch (category.ToUpper())
                {
                    case "HABITAT_MASTER-APPLIANCES":
                        searchCriteriaInput.ParentCategory = "06f5147b-9e24-fa33-20fb-d4b7d8d21392";
                        break;
                    case "HABITAT_MASTER-AUDIO":
                        searchCriteriaInput.ParentCategory = "a0860c27-2a32-841f-7014-75b163b9471e";
                        break;
                }
            }
            

            ProductTemplateCriteria productTemplateCriteria = new ProductTemplateCriteria(searchCriteriaInput);
            LanguageCriteria languageCriteria = new LanguageCriteria(searchCriteriaInput);

            searchManager.EnrollSearchCriteria(productTemplateCriteria);
            searchManager.EnrollSearchCriteria(languageCriteria);

            SearchResultsResponse<SearchResult> searchResultsResponse =
                searchManager.ExecuteQuery("sitecore_master_index", searchCriteriaInput, pageNo, 20);

            List<SearchResult> lstSearchResult = null;
            if(searchResultsResponse != null && 
                searchResultsResponse.SearchResults != null && 
                searchResultsResponse.SearchResults.Any())
            {
                lstSearchResult = searchResultsResponse.SearchResults.ToList<SearchResult>();
            }

            List<ProductSearchResult> productSearchResults = new List<ProductSearchResult>();
            if(lstSearchResult != null && lstSearchResult.Any())
            {
                foreach(SearchResult searchResult in lstSearchResult)
                {
                    ProductSearchResult productSearchResult = new ProductSearchResult();
                    productSearchResult.DisplayName = searchResult.DisplayName;
                    productSearchResult.FullPath = searchResult.FullPath;
                    
                    string[] images;
                    if (searchResult.Fields.ContainsKey("images"))
                        images = (searchResult.Fields["images"] as string).Split('|');
                    else
                        images = new string[] { };

                    productSearchResult.ProductImages = images;
                    productSearchResult.ProductName = searchResult.ProductName;

                    List<Varient> varients = new List<Varient>();
                    if(!string.IsNullOrEmpty(searchResult.ProductVarients))
                    {
                        string[] productVarients = searchResult.ProductVarients.Split('|');
                        foreach(string productVarient in productVarients)
                        {
                            string[] arrVarient = productVarient.Split('=');
                            
                            if (arrVarient.Length>=2)
                            {
                                varients.Add(new Varient() { VarientId = arrVarient[0], DisplayName = arrVarient[1] });
                            }
                            
                        }
                    }

                    productSearchResult.ProductVarients = varients.ToArray<Varient>();

                    productSearchResults.Add(productSearchResult);
                }
            }

            return this.Json(productSearchResults, JsonRequestBehavior.AllowGet);
        }
    }
}