namespace Np.Common
{
    public class PageHelper
    {
        public static int GetSkipCount(int page, int pageSize)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 100;
            }
            var skip = (page - 1) * pageSize;
            return skip;
        }
    }
}
