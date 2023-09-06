using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_MilkAndCookies.Opgaver.Opgave2.Extenders;
using WebApplication_MilkAndCookies.Opgaver.Opgave2.Models;

namespace WebApplication_MilkAndCookies.Opgaver.Opgave2
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartClearerController : ControllerBase
    {
        string basketKey = "basket";

        [HttpDelete("DelP")]
        public IEnumerable<Product> DeleteProduct([FromQuery] string productName)
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

                products.RemoveAll(p => p.Name == productName);

                HttpContext.Session.SetObjectAsJson(basketKey, products);
            }

            return products;
        }
    }
}
