using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using TheSwitch.Interfaces;

namespace TheSwitch.Server
{
    public class RestServerHandler
    {
        private Type serviceType;
        private IRestService service;
        private JavaScriptSerializer serializer;

        public RestServerHandler(IRestService srv)
        {
            serviceType = srv.GetType();
            service = srv;
            serializer = new JavaScriptSerializer();
        }

        private object[] MatchParameters(ParameterInfo[] prms, NameValueCollection qs)
        {
            List<object> ret = new List<object>();
            foreach (var prm in prms)
            {
                var pval = qs[prm.Name.ToLower()];
                
                ret.Add(Convert.ChangeType(pval,prm.ParameterType));
            }
            return ret.ToArray();
        }

        public void ServeUrl(HttpListenerContext ctx)
        {
            var url = ctx.Request.Url;//, ctx.Response
            var func = url.AbsolutePath.Substring(6);
            var mtd = serviceType.GetMethod(func.UppercaseFirst());
            var prms = mtd.GetParameters();
            var qs = HttpUtility.ParseQueryString(url.Query);
            var ret = mtd.Invoke(service,MatchParameters(prms,qs));
            var callbackFunction = qs["cb"];
            var hasCb = !string.IsNullOrEmpty(callbackFunction);
            var data = Encoding.UTF8.GetBytes((hasCb?callbackFunction+"(":"")+serializer.Serialize(ret)+(hasCb?");":""));

            ctx.Response.OutputStream.Write(data,0,data.Length);
            
        }
    }
}
