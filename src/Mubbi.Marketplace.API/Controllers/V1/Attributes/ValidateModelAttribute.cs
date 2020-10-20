using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mubbi.Marketplace.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mubbi.Marketplace.API.Controllers.V1.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                var response = new ApiResponse<List<string>>(false, "The request model is not valid.", errors);
                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}
