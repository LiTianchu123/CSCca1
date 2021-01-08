
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductStore.Controllers
{
    public class ProductController : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();

        [HttpGet]
        [Route("api/v3/products")]
        public IEnumerable<Product> GetAllProductsFromRepository()
        {
            return repository.GetAll();

        }
     
        [HttpGet]
        [Route("api/v3/products/{id:int}", Name = "getProductByIdv3")]
        public Product retrieveProductfromRepository(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        [HttpPost]
        [Route("api/v3/products")]
        public HttpResponseMessage PostProduct(Product item)
        {
            if (ModelState.IsValid)
            {
                item = repository.Add(item);
                var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

                // Generate a link to the new product and set the Location header in the response.

                string uri = Url.Link("getProductByIdv3", new { id = item.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }


        [HttpPut]
        [Route("api/v3/products/{id:int}")]
        public HttpResponseMessage PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                var response = Request.CreateResponse<Product>(HttpStatusCode.OK, product);
                return response;
            }
        }

        [HttpDelete]
        [Route("api/v3/products/{id:int}")]
        public HttpResponseMessage DeleteProduct(int id)
        {
            try
            {
                repository.Remove(id);
                var response = Request.CreateResponse<IEnumerable<Product>>(HttpStatusCode.OK, repository.GetAll());
                return response;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

    }
}