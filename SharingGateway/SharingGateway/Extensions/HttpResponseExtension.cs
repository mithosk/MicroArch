using Microsoft.AspNetCore.Http;
using System;

namespace SharingGateway.Extensions
{
    public static class HttpResponseExtension
    {
        public static void SetPageIndex(this HttpResponse hrt, uint pageIndex)
        {
            hrt.Headers.Add("PageIndex", pageIndex.ToString());
        }

        public static void SetPageSize(this HttpResponse hrt, ushort pageSize)
        {
            hrt.Headers.Add("PageSize", pageSize.ToString());
        }

        public static void SetPageCount(this HttpResponse hrt, uint pageCount)
        {
            hrt.Headers.Add("PageCount", pageCount.ToString());
        }

        public static void SetTotalItemCount(this HttpResponse hrt, uint totalItemCount)
        {
            hrt.Headers.Add("TotalItemCount", totalItemCount.ToString());
        }

        public static void SetSortBy<TEnum>(this HttpResponse hrt, TEnum sortBy) where TEnum : Enum
        {
            hrt.Headers.Add("SortBy", sortBy.ToString());
        }
    }
}