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

        public static TSort GetSortType<TSort>(this HttpRequest hrt) where TSort : Enum
        {
            string sortType = hrt.Headers["SortType"].ToString();

            try
            {
                return (TSort)Enum.Parse(typeof(TSort), sortType, false);
            }
            catch
            {
                return default;
            }
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
    }
}