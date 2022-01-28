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

        public static T GetSortBy<T>(this HttpRequest hrt) where T : Enum
        {
            string sortBy = hrt.Headers["SortBy"].ToString();

            try
            {
                return (T)Enum.Parse(typeof(T), sortBy, false);
            }
            catch
            {
                return default;
            }
        }
    }
}