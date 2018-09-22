using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Himalaya.DXP.Foundation.Search.Model
{
    public class SearchResult : SearchResultItem
    {
        [IndexField("__display_name_t"), XmlIgnore]
        public string DisplayName { get; set; }

        [IndexField("_fullpath"), XmlIgnore]
        public string FullPath { get; set; }

        [IndexField("name_t"), XmlIgnore]
        public string ProductName { get; set; }

        [IndexField("images_sm"), XmlIgnore]
        public string ProductImages { get; set; }

        [IndexField("images_sm"), XmlIgnore]
        public ID ProductImageIDs { get; set; }

        [IndexField("productvarients_s"), XmlIgnore]
        public string ProductVarients { get; set; }

        [IndexField("parentcategorylist_sm"),XmlIgnore]
        public string ProductCategoryList { get; set; }

    }

    public class ProductSearchResult
    {
        public string DisplayName { get; set; }

        public string FullPath { get; set; }

        public string ProductName { get; set; }

        public string[] ProductImages { get; set; }

        public ID ProductImageIDs { get; set; }

        public string ProductId { get; set; }

        public Varient[] ProductVarients { get; set; }
    }


    public class Varient
    {
        public string VarientId { get; set; }

        public string DisplayName { get; set; }
    }

    public class SearchCriteriaInput
    {
        public string TemplateName { get; set; }
        public string TemplateID { get; set; }
        public string Language { get; set; }
        public string ParentCategory { get; set; }

        public bool IsAllProduct { get; set; }
    }
}