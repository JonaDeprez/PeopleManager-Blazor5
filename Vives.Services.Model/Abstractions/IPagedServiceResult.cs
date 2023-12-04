namespace Vives.Services.Model.Abstractions
{
    public interface IPagedServiceResult
    {
        Paging Paging { get; set; }
        int TotalCount { get; set; }
    }
}
