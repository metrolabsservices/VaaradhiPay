using System.Linq;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services
{
    public class PaginationService<T> : IPaginationService<T>
    {
        public IQueryable<T> ApplyPagination(IQueryable<T> query, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
