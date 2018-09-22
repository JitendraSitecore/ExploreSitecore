using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace Himalaya.DXP.Foundation.Search.ComputedFields
{
    public class ProductSubCategoryComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var scIndexible = indexable as SitecoreIndexableItem;
            string productCategory = string.Empty;
            string commerceProductTemplateId = Sitecore.Configuration.Settings.GetSetting("CommerceProductTemplateId").ToString();
            string rootCategoryName = Sitecore.Configuration.Settings.GetSetting("RootCategoryName").ToString();
            string catalogName = Sitecore.Configuration.Settings.GetSetting("CatalogName").ToString();
            Item contextItem = scIndexible.Item;
            try
            {
                if (contextItem != null)
                {
                    Sitecore.Diagnostics.Log.Info("Inside Product Sub Category", this);
                    if (Convert.ToString(contextItem.TemplateID) == commerceProductTemplateId)
                    {
                        // Condition for 3 level category product
                        // Condition for 3 level category product
                        if (contextItem.Parent != null && contextItem.Parent.Parent != null
                            && contextItem.Parent.Parent.Parent != null && contextItem.Parent.Parent.Parent.Parent != null)
                        {
                            if (contextItem.Parent.Parent.Parent.Parent.DisplayName.ToLower() == rootCategoryName)
                            {
                                Sitecore.Diagnostics.Log.Info("3 Level Product", this);
                                return contextItem.Parent.Parent.DisplayName.ToLower();
                            }
                            else if (contextItem.Parent.Parent.Parent.Parent.DisplayName.ToLower() == catalogName)
                            {
                                Sitecore.Diagnostics.Log.Info("2 Level Product", this);
                                return contextItem.Parent.DisplayName.ToLower();
                            }
                        }
                    }
                    //else return null;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("ProductSubCategoryComputedField", ex, "ComputeFieldValue");
            }
            return productCategory;
        }
    }
}