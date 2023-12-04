using Microsoft.AspNetCore.Mvc.ModelBinding;
using Vives.Services.Model;

namespace PeopleManager.Ui.Mvc.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, ServiceResult serviceResult)
        {
            modelState.AddModelErrors(serviceResult.Messages);
        }
        public static void AddModelErrors(this ModelStateDictionary modelState, IList<ServiceMessage> serviceMessages)
        {
            foreach (var message in serviceMessages)
            {
                if (!string.IsNullOrWhiteSpace(message.PropertyName))
                {
                    modelState.AddModelError(message.PropertyName, message.Title);
                }
                else
                {
                    modelState.AddModelError("", message.Title);
                }
            }
        }
    }
}
