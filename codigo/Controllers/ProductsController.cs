using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.DTOs;
using ProductsApi.Models;

namespace ProductsApi.Controllers;

[ApiController]
[Route("api/products")]
[Authorize] // todos os endpoints exigem JWT, exceto os marcados com [AllowAnonymous]
public class ProductsController(AppDbContext db) : ControllerBase
{
    // GET api/products — lista todos
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var products = await db.Products.ToListAsync();
        return Ok(products);
    }

    // GET api/products/5 — busca por id
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await db.Products.FindAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    // POST api/products — cria novo
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

        db.Products.Add(product);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT api/products/5 — atualiza
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();

        if (dto.Name is not null) product.Name = dto.Name;
        if (dto.Description is not null) product.Description = dto.Description;
        if (dto.Price is not null) product.Price = dto.Price.Value;
        if (dto.Stock is not null) product.Stock = dto.Stock.Value;

        await db.SaveChangesAsync();
        return Ok(product);
    }

    // DELETE api/products/5 — remove
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();

        db.Products.Remove(product);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
