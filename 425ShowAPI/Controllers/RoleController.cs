using Microsoft.AspNetCore.Mvc;

namespace Four25ShowAPI.Controllers
{
    [ApiController]
    [Route("roles")]
    public class RoleController : ControllerBase
    {
        public string[] Index()
        {
            return new[] { "role1", "role2", "role3", "role4" };
        }
    }
}
