using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;


namespace Himalaya.DXP.Foundation.Search.ComputedFields
{
    public class ProductVarientsComputedFields : IComputedIndexField
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
                    if (currentItem.HasChildren)
                    {

                        IEnumerable<Item> items = currentItem.Children.Where(x => x.TemplateID == VarientTemplateId);
                        foreach (Item item in items)
                        {
                            if (string.IsNullOrEmpty(indexString))
                            {
                                indexString += string.Format("{0}={1}", item.Name, item.DisplayName);
                            }
                            else
                            {
                                indexString += string.Format("|{0}={1}", item.Name, item.DisplayName);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("ProductVarientsComputedFields -> Error", ex, this);
            }

            return indexString;
        }
    }
}