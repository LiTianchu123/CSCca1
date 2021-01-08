using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductStore.Models;
using System.Web.Http.Cors;

namespace TalentApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductsController : ApiController
    {
        static readonly TalentRepository repository = new TalentRepository();

       

        [Route("api/talents")]
        [System.Web.Mvc.RequireHttps]
        [HttpGet]
        public IEnumerable<Talent> GetAllTalents()
        {
            return repository.GetAll();
        }

        [Route("api/talents/{id:int}")]
        [System.Web.Mvc.RequireHttps]
        [HttpGet]
        public Talent GetTalent(int id)
        {
            Talent item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        [Route("api/talents")]
        [System.Web.Mvc.RequireHttps]
        [HttpPost]
        public HttpResponseMessage PostTalent(Talent item)
        {
            if (ModelState.IsValid)
            {
                item = repository.Add(item);
                var response = Request.CreateResponse(HttpStatusCode.Created, item);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }


        [Route("api/talents/{id:int}")]
        [System.Web.Mvc.RequireHttps]
        [HttpPut]
        public HttpResponseMessage PutTalent(int id, Talent item)
        {
            item.Id = id;
            if (!repository.Update(item))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, item);
                return response;
            }
            
        }


        [Route("api/talents/{id:int}")]
        [System.Web.Mvc.RequireHttps]
        [HttpDelete]
        public HttpResponseMessage DeleteTalent(int id)
        {
            try
            {
                repository.Remove(id);
                var response = Request.CreateResponse(HttpStatusCode.OK, repository.GetAll());
                return response;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }
    }
}
