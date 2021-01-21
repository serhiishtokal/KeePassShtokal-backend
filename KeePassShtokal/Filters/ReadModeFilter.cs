using KeePassShtokal.AppCore.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KeePassShtokal.Filters
{
    public class OnlyWriteModeAttribute : ActionFilterAttribute
    {
        private readonly string _errorMessage;

        public OnlyWriteModeAttribute(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool.TryParse(context.HttpContext.User.FindFirst(type: "IsReadMode")?.Value, out var isReadMode);

            if (isReadMode) context.Result = new BadRequestObjectResult(new Status(false, _errorMessage));
        }
    }
}
