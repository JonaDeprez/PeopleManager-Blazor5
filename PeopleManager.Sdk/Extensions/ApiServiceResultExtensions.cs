using Vives.Services.Model;

namespace PeopleManager.Sdk.Extensions
{
    public static class ApiServiceResultExtensions
    {
        public static ServiceResult<T> ApiError<T>(this ServiceResult<T> result)
        {
            result.AddApiError();
            return result;
        }

        public static ServiceResult ApiError(this ServiceResult result)
        {
            result.AddApiError();
            return result;
        }

        private static void AddApiError(this ServiceResult result)
        {
            result.Messages.Add(new ServiceMessage
            {
                Code = "ApiError",
                Title = "Api returned a null result",
                Type = ServiceMessageType.Error
            });
        }
    }
}
