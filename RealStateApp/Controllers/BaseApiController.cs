using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RealStateApp.Presentation.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
