using Microsoft.AspNetCore.Mvc;

namespace RestaurantFrontend.Controllers
{/ Använd policyn i en controller
[Authorize(Policy = "Over18Only")]
[ApiController]
[Route("api/[controller]")]
public class RestrictedController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAdultContent()
    {
        return Ok("Detta är innehåll endast tillgängligt för användare över 18 år.");
    }
}
