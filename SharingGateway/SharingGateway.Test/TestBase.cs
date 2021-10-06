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
    }
}