using Microsoft.AspNetCore.Mvc;

namespace ProductsApi.Modules.Products;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "Products module is running" });
}