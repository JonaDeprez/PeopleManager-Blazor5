namespace Vives.Services.Model
{
    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult()
        {
            
        }
        public ServiceResult(T result)
        {
            Result = result;
        }
        public T? Result { get; set; }
    }
}
