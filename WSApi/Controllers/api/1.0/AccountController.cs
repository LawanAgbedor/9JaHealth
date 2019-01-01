using BIZ.managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UTIL.exceptions;

namespace WSApi.Controllers.api._1._0
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
        public IHttpActionResult Get(Guid? id)
        {
            if (!id.HasValue)
                throw new AppWebException(AppWebExceptionType.InvalidParameter, "Invalid NULL value for 'id'.", null);

            var data = UserManager.GetByID(id.Value);
            return Ok(data);
        }

        [Route("username/{username}")]
        public IHttpActionResult GetByUsername(string username)
        {
            var data = UserManager.GetByUserName(username);
            return Ok(data);
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
