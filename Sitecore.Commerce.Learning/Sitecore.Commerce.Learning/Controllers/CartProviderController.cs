using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Entities.Prices;
using Sitecore.Commerce.Services;
using Sitecore.Commerce.Services.Carts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Commerce.Learning.Controllers
{
    public class CartProviderController : Controller
    {
        // GET: CartProvider
        public ActionResult Index()
        {
            CartServiceProvider cartServiceProvider = new CartServiceProvider();

            CreateOrResumeCartRequest createOrResumeCartRequest = new CreateOrResumeCartRequest("Himalaya Wellness", "jitendra.kumar@edynamic.net");
            CartResult cartResult = cartServiceProvider.CreateOrResumeCart(createOrResumeCartRequest);

            CartLine cartLine = new CartLine
            {
                Quantity = 1,
                Product = new CartProduct
                {
                    ProductId = "7042112",
                    Price = new Price(55, "USD")
                    
                }
            };

            //cartResult.Cart.Lines.Count

            Collection<CartLine> cartLines = new Collection<CartLine>() { cartLine };

            AddCartLinesRequest addCartLinesRequest = new AddCartLinesRequest(cartResult.Cart, cartLines);
            cartResult.Cart = cartServiceProvider.AddCartLines(addCartLinesRequest).Cart;
            SaveCartRequest saveCartRequest = new SaveCartRequest(cartResult.Cart);
            ServiceProviderResult result = cartServiceProvider.SaveCart(saveCartRequest);

            GetCartsRequest request = new GetCartsRequest("Himalaya Wellness")
            {
                UserIds = new Collection<string> { "jitendra.kumar@edynamic.net" },
                CustomerIds = new Collection<string> { "jitendra.kumar@edynamic.net" },
                Names = new Collection<string> { "Jitendra Kumar" },
                Statuses = new Collection<string> { "InProcess" },
                IsLocked = false
            };

            // Call service provider and receive the result.
            GetCartsResult getCartsresult = cartServiceProvider.GetCarts(request);


            return View();
        }
    }
}