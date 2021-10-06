using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UserService.Data;

namespace UserService.Test
{
    public class TestBase
    {
        protected DbContextOptions<DataContext> _dataOptions;

        public TestBase()
        {
            _dataOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseLazyLoadingProxies()
                .Options;
        }

        protected static List<object[]> LoadJson<TItem>(string fileName)
        {
            return JsonConvert.DeserializeObject<List<TItem>>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Json\" + fileName))
                .Select(ite => new object[] { ite })
                .ToList();
        }
    }
}