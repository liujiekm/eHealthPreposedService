using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eHPS.API.Controllers
{
    [RoutePrefix("Test")]
    [Authorize]
    public class TestfulController : ApiController
    {

        [Route("Get")]
        
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Testful/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Testful
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Testful/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Testful/5
        public void Delete(int id)
        {
        }
    }
}
