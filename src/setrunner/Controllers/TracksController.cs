using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using setrunner.Models;

namespace setrunner.Controllers
{
    [Route("api/[controller]")]
    public class TracksController : Controller
    {
        // GET api/tracks
        [HttpGet]
        public JsonResult Get(string artistName)
        {
            return new JsonResult(new string[] { "track 1", "track 2" });
        }

        // GET api/tracks/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"track {id}";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
