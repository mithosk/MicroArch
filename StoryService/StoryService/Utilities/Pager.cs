namespace StoryService.Utilities
{
    public class Pager
    {
        public static uint RecordSkip(uint pageIndex, ushort pageSize)
        {
            return (pageIndex - 1) * pageSize;
        }

        public static uint PageCount(uint totalItemCount, ushort pageSize)
        {
            uint result = totalItemCount / pageSize;
            if (totalItemCount % pageSize > 0)
                result++;

            return result;
        }
    }
}