using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Data;

namespace WebApiMTModel.Controllers
{
    [RoutePrefix("api/XMLData")]
    public class XMLController : ApiController
    {
        // GET:
        // /api/XMLData/cities
        [System.Web.Http.Route("{filename}")]
        public DataSet  Get(string filename)
        {
           // XmlReader xmlFile;
          string  xmlFile = HostingEnvironment.MapPath("~/App_Data/" + filename + ".xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlFile);
            return ds;
        }

        // GET: api/XML/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/XML
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/XML/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/XML/5
        public void Delete(int id)
        {
        }
    }
}
