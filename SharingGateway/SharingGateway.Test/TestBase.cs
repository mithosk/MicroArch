using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharingGateway.Test
{
    public class TestBase
    {
        protected static List<object[]> LoadJson<TItem>(string fileName)
        {
            return JsonConvert.DeserializeObject<List<TItem>>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Json\" + fileName))
                .Select(ite => new object[] { ite })
                .ToList();
        }

        protected static void AddRequestHeader(Controller controller, string key, string value)
        {
            HttpContext httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add(key, value);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }
    }
}