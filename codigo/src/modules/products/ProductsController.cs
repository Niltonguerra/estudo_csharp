using Microsoft.AspNetCore.Mvc;

namespace ProductsApi.Modules.Products;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "Products module is running" });
}