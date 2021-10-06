using StoryService.Utilities.Logic;
using System;
using Xunit;

namespace StoryService.Test.Utilities
{
    public class GeolocationUtilityTest : TestBase
    {
        [Fact]
        public void MinLatCalculation()
        {
            //parameters
            float refLat = (float)43.97549;
            ushort radius = 10;
            float threshold = (float)0.09;

            //execution
            float minLat = new GeolocationUtility().MinLat(refLat, radius);

            //check
            float diff = Math.Abs(refLat - minLat);
            Assert.True(diff > 0 && diff <= threshold);
        }

        [Fact]
        public void MaxLatCalculation()
        {
            //parameters
            float refLat = (float)43.97549;
            ushort radius = 10;
            float threshold = (float)0.09;

            //execution
            float maxLat = new GeolocationUtility().MaxLat(refLat, radius);

            //check
            float diff = Math.Abs(refLat - maxLat);
            Assert.True(diff > 0 && diff <= threshold);
        }

        [Fact]
        public void MinLonCalculation()
        {
            //parameters
            float refLat = (float)43.97549;
            float refLon = (float)12.69761;
            ushort radius = 10;
            float threshold = (float)0.13;

            //execution
            float minLon = new GeolocationUtility().MinLon(refLat, refLon, radius);

            //check
            float diff = Math.Abs(refLon - minLon);
            Assert.True(diff > 0 && diff <= threshold);
        }

        [Fact]
        public void MaxLonCalculation()
        {
            //parameters
            float refLat = (float)43.97549;
            float refLon = (float)12.69761;
            ushort radius = 10;
            float threshold = (float)0.13;

            //execution
            float maxLon = new GeolocationUtility().MaxLon(refLat, refLon, radius);

            //check
            float diff = Math.Abs(refLon - maxLon);
            Assert.True(diff > 0 && diff <= threshold);
        }

        [Fact]
        public void DistanceCalculation()
        {
            //parameters
            float lat1 = (float)43.97549;
            float lon1 = (float)12.69761;
            float lat2 = (float)43.99242;
            float lon2 = (float)12.65841;
            uint realDistance = 4;

            //execution
            uint calculatedDistance = new GeolocationUtility().Distance(lat1, lon1, lat2, lon2);

            //check
            Assert.Equal(realDistance, calculatedDistance);
        }
    }
}