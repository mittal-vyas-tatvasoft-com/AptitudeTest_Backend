using AptitudeTest.Core.ViewModels;

namespace AptitudeTest.Data.Common
{
    public class Pagination<T>
    {
        public static PaginationVM<T> Paginate(List<T> entityList, int? pageSize, int? currentPageIndex)
        {
            int totalCount = entityList.Count();
            pageSize = pageSize ?? 10;
            if (pageSize == 0)
            {
                pageSize = 10;
            }
            if (pageSize >= totalCount)
            {
                pageSize = totalCount;
            }
            int totalPages = 0;
            if (totalCount != 0)
            {
                totalPages = (int)Math.Ceiling((totalCount / (double)pageSize));
            }
            currentPageIndex ??= 0;
            int pageIndex = 0;
            if (currentPageIndex < totalPages)
            {
                pageIndex = (int)currentPageIndex;
            }
            int skip = (int)pageSize * pageIndex;
            entityList = entityList.Skip(skip).Take((int)pageSize).ToList();
            bool isPreviousPage = true, isNextPage = true;

            if (pageIndex == 0 || totalCount == 0)
            {
                isPreviousPage = false;
            }

            if (pageIndex == totalPages - 1 || totalCount == 0)
            {
                isNextPage = false;
            }
            return new PaginationVM<T>()
            {
                EntityList = entityList,
                CurrentPageIndex = pageIndex,
                IsNextPage = isNextPage,
                IsPreviousPage = isPreviousPage,
                PageCount = totalPages,
                PageSize = (int)pageSize,
                TotalItemsCount = totalCount
            };
        }
    }
}
