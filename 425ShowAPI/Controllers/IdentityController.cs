using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Four25ShowAPI.Controllers
{
    [ApiController]
    [Route("identity")]
    public class IdentityController : ControllerBase
    {
        public IActionResult Index()
        {
            var userClaims = base.User.Claims;
            var data = userClaims.Select(c => new { Type = c.Type, Name = c.Value }).ToArray();

            return new JsonResult(data);
        }
    }
}
