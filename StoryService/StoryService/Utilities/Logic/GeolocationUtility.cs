using System;

namespace StoryService.Utilities.Logic
{
    public class GeolocationUtility
    {
        private const ushort EARTH_RADIUS = 6371;

        public virtual float MinLat(float refLat, ushort radius)
        {
            float refLatRad = ToRadians(refLat);
            float minLatRad = refLatRad - ToAngularRadius(radius);
            float minLatDeg = ToDegrees(minLatRad);

            return minLatDeg;
        }

        public virtual float MaxLat(float refLat, ushort radius)
        {
            float refLatRad = ToRadians(refLat);
            float minLatRad = refLatRad + ToAngularRadius(radius);
            float maxLatDeg = ToDegrees(minLatRad);

            return maxLatDeg;
        }

        public virtual float MinLon(float refLat, float refLon, ushort radius)
        {
            float refLatRad = ToRadians(refLat);
            float refLonRad = ToRadians(refLon);
            float minLonRad = (float)(refLonRad - Math.Asin(Math.Sin(ToAngularRadius(radius)) / Math.Cos(refLatRad)));
            float minLonDeg = ToDegrees(minLonRad);

            return minLonDeg;
        }

        public virtual float MaxLon(float refLat, float refLon, ushort radius)
        {
            float refLatRad = ToRadians(refLat);
            float refLonRad = ToRadians(refLon);
            float minLonRad = (float)(refLonRad + Math.Asin(Math.Sin(ToAngularRadius(radius)) / Math.Cos(refLatRad)));
            float maxLonDeg = ToDegrees(minLonRad);

            return maxLonDeg;
        }

        public virtual uint Distance(float lat1, float lon1, float lat2, float lon2)
        {
            float lat1Rad = ToRadians(lat1);
            float lon1Rad = ToRadians(lon1);
            float lat2Rad = ToRadians(lat2);
            float lon2Rad = ToRadians(lon2);
            float distance = (float)(Math.Acos(Math.Sin(lat1Rad) * Math.Sin(lat2Rad) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(lon1Rad - lon2Rad)) * EARTH_RADIUS);

            return (uint)(distance + 0.5);
        }

        private static float ToRadians(float degrees)
        {
            return (float)(degrees * Math.PI / (float)180);
        }

        private static float ToDegrees(float radians)
        {
            return (float)(radians * 180 / Math.PI);
        }

        private static float ToAngularRadius(ushort radius)
        {
            return radius / (float)EARTH_RADIUS;
        }
    }
}