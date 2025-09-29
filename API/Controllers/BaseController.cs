using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }
}