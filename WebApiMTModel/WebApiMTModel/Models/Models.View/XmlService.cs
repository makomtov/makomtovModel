using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Hosting;

namespace WebApiMTModel.Models.Models.View
{
    public class XmlService
    {
        public bool CheckCity(string City)
        {
            string xmlFile = HostingEnvironment.MapPath("~/App_Data/cities.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlFile);
            //   ds.Tables[0].PrimaryKey = new DataColumn[1] { ds.Tables[0].Columns["Heb"] };
            string filter = "Heb='" + City + "'";
            DataRow[] data = ds.Tables["City"].Select(filter);
            return data.Length == 0;
        }
    }
}