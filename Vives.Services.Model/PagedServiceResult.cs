using Vives.Services.Model.Abstractions;

namespace Vives.Services.Model
{
    public class PagedServiceResult<TEntity, TFilter>: ServiceResult, IPagedServiceResult
    {
        public IList<TEntity> Data { get; set; } = new List<TEntity>();
        public required Paging Paging { get; set; }
        public int TotalCount { get; set; }
        public TFilter? Filter { get; set; }
    }
}
