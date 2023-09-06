using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_MilkAndCookies.Opgaver.Opgave2.Extenders;
using WebApplication_MilkAndCookies.Opgaver.Opgave2.Models;

namespace WebApplication_MilkAndCookies.Opgaver.Opgave2
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingcartController : ControllerBase
    {
        string basketKey = "basket";

        [HttpGet]
        public IEnumerable<Product> GetCart([FromQuery] Product product)
        {
            List<Product>? products = new List<Product>();

            if (HttpContext.Session.IsAvailable)
            {
                try
                {
                    products = HttpContext.Session.GetObjectFromJson<List<Product>>(basketKey);
                }
                catch { }

                if (products is null) products = new List<Product>();

                products.Add(product);

                HttpContext.Session.SetObjectAsJson(basketKey, products);
            }
            else
            {
                products.Add(product);
            }

            return products;
        }
    }
}
