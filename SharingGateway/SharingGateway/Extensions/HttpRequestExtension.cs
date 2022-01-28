using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SharingGateway.Extensions
{
    public static class HttpRequestExtension
    {
        public static Guid GetUserId(this HttpRequest hrt)
        {
            string token = hrt.Headers["Authorization"].ToString().Replace("Bearer ", "");
            JwtSecurityToken jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return jwtSecurityToken.Claims
                .Where(cla => cla.Type == "UserId")
                .Select(cla => Guid.Parse(cla.Value))
                .Single();
        }

        public static uint? GetPageIndex(this HttpRequest hrt)
        {
            string pageIndex = hrt.Headers["PageIndex"].ToString();

            return string.IsNullOrEmpty(pageIndex) ? null : uint.Parse(pageIndex);
        }

        public static ushort? GetPageSize(this HttpRequest hrt)
        {
            string pageSize = hrt.Headers["PageSize"].ToString();

            return string.IsNullOrEmpty(pageSize) ? null : ushort.Parse(pageSize);
        }

        public static bool CheckSortBy<TEnum>(this HttpRequest hrt) where TEnum : Enum
        {
            string sortBy = hrt.Headers["SortBy"].ToString();

            try
            {
                Enum.Parse(typeof(TEnum), sortBy, false);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static TEnum GetSortBy<TEnum>(this HttpRequest hrt) where TEnum : Enum
        {
            string sortBy = hrt.Headers["SortBy"].ToString();

            try
            {
                return (TEnum)Enum.Parse(typeof(TEnum), sortBy, false);
            }
            catch
            {
                return default;
            }
        }
    }
}