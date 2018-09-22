using Sitecore.Commerce.CustomModels.Models;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Services.Carts;
using Sitecore.Commerce.Services.Prices;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Common.Models;
using Sitecore.Commerce.XA.Foundation.Common.Models.JsonResults;
using Sitecore.Commerce.XA.Foundation.Connect;
using Sitecore.Commerce.XA.Foundation.Connect.Arguments;
using Sitecore.Commerce.XA.Foundation.Connect.Entities;
using Sitecore.Commerce.XA.Foundation.Connect.Managers;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static Sitecore.Commerce.XA.Foundation.Connect.ConnectConstants;

namespace Sitecore.Commerce.Learning.Controllers
{
    public class ShopUserController : Controller
    {

        private readonly IVisitorContext VisitorContext;
        private readonly IStorefrontContext StorefrontContext;
        private readonly ICartManager CartManager;
        private readonly IContext Context;
        private readonly IPricingManager PricingManager;
        private readonly IModelProvider ModelProvider;
        private readonly ISiteContext SiteContext;
        private readonly ICatalogManager CatalogManager;

        public ShopUserController()
        {
            this.VisitorContext = (VisitorContext)ServiceLocator.ServiceProvider.GetService(typeof(IVisitorContext));

            VisitorContext = (VisitorContext)ServiceLocator.ServiceProvider.GetService(typeof(IVisitorContext));
            this.StorefrontContext = ((AccountManager)((VisitorContext)VisitorContext).AccountManager).StorefrontContext;
            this.CartManager = ((AccountManager)((VisitorContext)VisitorContext).AccountManager).CartManager;
            this.Context = ((StorefrontContext)StorefrontContext).Context;


            //this.StorefrontContext = (StorefrontContext)ServiceLocator.ServiceProvider.GetService(typeof(IStorefrontContext));
            //this.CartManager = (CartManager)ServiceLocator.ServiceProvider.GetService(typeof(ICartManager));
            //this.Context = (IContext)ServiceLocator.ServiceProvider.GetService(typeof(IContext));

            this.PricingManager = (PricingManager)ServiceLocator.ServiceProvider.GetService(typeof(IPricingManager));

            ModelProvider = (ModelProvider)ServiceLocator.ServiceProvider.GetService(typeof(IModelProvider));
            SiteContext = (SiteContext)ServiceLocator.ServiceProvider.GetService(typeof(ISiteContext));
            CatalogManager = (CatalogManager)ServiceLocator.ServiceProvider.GetService(typeof(ICatalogManager));
        }

        public string Index()
        {

            BaseJsonResult model = new BaseJsonResult(Context, StorefrontContext);
            CommerceStorefront currentStorefront = StorefrontContext.CurrentStorefront;
            ManagerResponse<CartResult, Entities.Carts.Cart> currentCart = CartManager.GetCurrentCart(VisitorContext, StorefrontContext, false);

            List<CartLineArgument> list = new List<CartLineArgument>();
            list.Add(new CartLineArgument
            {
                CatalogName = StorefrontContext.CurrentStorefront.Catalog,
                ProductId = "6042305",
                VariantId = "56042305",
                Quantity = decimal.Parse("2")
            });
            ManagerResponse<CartResult, Entities.Carts.Cart> managerResponse = CartManager.AddLineItemsToCart(currentStorefront, VisitorContext, currentCart.Result, list);
            if (!managerResponse.ServiceProviderResult.Success)
            {
                model.SetErrors(managerResponse.ServiceProviderResult);
                return managerResponse.ServiceProviderResult.ToString();
            }
            model.Success = true;

            return "Welcome in Sitecore Commerce";
        }

        public string ShowPrice()
        {

            Item itemProduct = Sitecore.Context.Database.GetItem(Sitecore.Configuration.Settings.GetSetting("Product_Path"));

            //List<ProductEntity> productEntityList = new List<ProductEntity>();

            StringBuilder price = new StringBuilder();

            ProductEntity productEntity = ModelProvider.GetModel<ProductEntity>();
            productEntity.Initialize(StorefrontContext.CurrentStorefront, itemProduct, null);

            List<VariantEntity> variantEntities = new List<VariantEntity>();

            if (itemProduct.HasChildren)
            {
                foreach(Item varientItem in itemProduct.Children)
                {
                    VariantEntity variantEntity = ModelProvider.GetModel<VariantEntity>();
                    variantEntity.Initialize(varientItem);

                    if(variantEntity.ListPrice != null)
                    {
                        price.AppendFormat("{0}={1}|", variantEntity.Item.DisplayName, variantEntity.ToJson());
                    }

                    variantEntities.Add(variantEntity);
                }
            }

            


            //var pricingServiceProvider = new PricingServiceProvider();
            //// Create a GetProductPricesRequest object, specify the product's ID and do not
            //// specify any price types. Default price type is ListPrice

            //GetProductPricesRequest request = new GetProductPricesRequest("6042567", PriceTypes.List);
            //// Call the service provider and receive the result.
            //request.CurrencyCode = StorefrontContext.CurrentStorefront.CurrencyProvider.GetSelectedCurrency();
            //request.Quantity = 1;
            //request.UserId = VisitorContext.UserId;

            //GetProductPricesResult result = pricingServiceProvider.GetProductPrices(request);
            //// Result prices contains the list price only.

            //decimal price = result.Prices.First().Value.Amount;

            ManagerResponse<GetProductPricesResult, IDictionary<string, Entities.Prices.Price>> managerResponse = 
                PricingManager.GetProductPrices(StorefrontContext.CurrentStorefront, VisitorContext, StorefrontContext.CurrentStorefront.Catalog, "6042305", true);


            managerResponse.ServiceProviderResult.Prices.ToArray();



            return price.ToString();

            
        }

    }
}