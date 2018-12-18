using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppWebApi.Controllers.api._1._0
{
    [RoutePrefix("api/1.0/account")]
    public class AccountController : ApiController
    {
        [Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok(new { id = 1, name = "lawan" });
        }

        [Route("ama/{id1}/alla/{id2}")]
        [HttpGet]
        public string GetLawanAll(int id1, int id2)
        {
            return $"An AMA ALL ID1=${id1} ID2=${id2}";
        }

        [Route("osi/{id1}/alla/{id2}")]
        [HttpGet]
        public IHttpActionResult GetLawanAll2(int id1, int id2)
        {
            return Ok($"An OSI ALL ID1=${id1} ID2=${id2}");
        }


        // POST: api/1.0/Account
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok(new { id = 1, name = "lawan" });
        }

        // PUT: api/1.0/Account/5 --Where 5 is id of account to update
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return Ok(new { id = 1, name = "lawan2" });
        }

        // DELETE: api/Account/5
        public void Delete(int id)
        {
        }
    }
}
