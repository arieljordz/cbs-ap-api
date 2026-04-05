using CbsAp.Application.Shared.ResultPatten;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace CbsAp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        protected string CurrentUser => HttpContext.User.Identity.Name;
        protected string? CurrentRole => HttpContext.User.FindFirstValue(ClaimTypes.Role);

        protected ActionResult CreateResponse<T>(ResponseResult<T> result)
        {

            if (result.IsSuccess)
            {
                return base.Ok(result);
            }

            ActionResult response = result.StatusCode switch
            {
                (int)HttpStatusCode.NotFound => NotFound(result),
                (int)HttpStatusCode.Conflict => Conflict(result),
                (int)HttpStatusCode.Unauthorized => Unauthorized(result),
                (int)HttpStatusCode.BadRequest => BadRequest(result),
                _ => Ok(result)
            };

            return response;

       
        }
    }
}
