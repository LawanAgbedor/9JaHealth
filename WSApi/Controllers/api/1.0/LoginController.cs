using BIZ.managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WSApi.Controllers.api._1._0
{
    [RoutePrefix("api/1.0/login")]
    public class LoginController : ApiController
    {
        [Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("{username}/{password}/")]
        [HttpGet]
        public IHttpActionResult Login1(string username, string password)
        {
            var userId = LoginManager.AuthenticateUser(username, password, true);
            return Ok(userId);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult PostLogin1([FromBody]LoginObject loginObject)
        {
            var userId = LoginManager.AuthenticateUser(loginObject.Username, loginObject.Password, true);
            return Ok(userId);
        }

        // PUT: api/Login/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Login/5
        public void Delete(int id)
        {
        }
    }

    public class LoginObject
    {
        public LoginObject()
        {
            this.KeepAlive = true;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public bool KeepAlive { get; set; }
        public string AuthKey { get; set; }
    }
}
