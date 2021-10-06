using AgileServiceBus.Interfaces;
using FakeItEasy;
using FakeItEasy.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoryService.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StoryService.Test
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

        protected TEvent GetNotifyMessage<TEvent>(IMicroserviceBus bus) where TEvent : class
        {
            ICompletedFakeObjectCall call = Fake.GetCalls(bus)
                .Where(cfo =>
                    cfo.Method.Name == "NotifyAsync" &&
                    cfo.Arguments[0] is TEvent
                )
                .LastOrDefault();

            return (TEvent)call?.Arguments[0];
        }

        protected static List<object[]> LoadJson<TItem>(string fileName)
        {
            return JsonConvert.DeserializeObject<List<TItem>>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Json\" + fileName))
                .Select(ite => new object[] { ite })
                .ToList();
        }
    }
}