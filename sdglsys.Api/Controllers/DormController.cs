using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdglsys.DbHelper;

namespace sdglsys.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Dorm")]
    public class DormController : Controller
    {
        // GET: api/Dorm
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Dorm/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            var Db = new Dorms().Db;
            return Db.Queryable<Entity.TDorm>().Where(d => d.Id == id).ToJson();
        }
        
        // POST: api/Dorm
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Dorm/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
