using System.Linq;

namespace VaaradhiPay.Services.Interfaces
{
    public interface IPaginationService<T>
    {
        IQueryable<T> ApplyPagination(IQueryable<T> query, int page, int pageSize);
    }
}
