using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Configuration;


namespace Himalaya.DXP.Foundation.Search.ComputedFields
{
    public class ProductPathComputedFields : IComputedIndexField
    {
        private ID VarientTemplateId { get { return new ID("{C92E6CD7-7F14-46E7-BBF5-29CE31262EF4}"); } }
        private ID CommerceProductId { get { return new ID("{225F8638-2611-4841-9B89-19A5440A1DA1}"); } }

        public string FieldName { get; set ; }
        public string ReturnType { get; set ; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            string indexString = string.Empty;
            try
            {
                SitecoreIndexableItem sitecoreIndexableItem = indexable as SitecoreIndexableItem;

                Item currentItem = sitecoreIndexableItem.Item;

                if (currentItem != null && currentItem.TemplateID == CommerceProductId)
                {
                    if (currentItem.Paths != null && !string.IsNullOrEmpty(currentItem.Paths.Path))
                        indexString = currentItem.Paths.FullPath.Replace("/", Settings.GetSetting("Product_Path_Seperator"));
                }
            }
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("ProductPathComputedFields -> Error", ex, this);
            }
            return indexString;
        }
    }
}