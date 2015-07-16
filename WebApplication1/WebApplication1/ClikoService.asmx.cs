using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace WebApplication1
{
    /// <summary>
    /// Сводное описание для ClikoService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ClikoService : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetData(string Name)
        {

            return "Hello World";
        }
    }
}
