using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("products")]
public class ProdutctController : Controller
{
    
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
    {

        var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
        return Ok(products);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
    {
         var product = await context.Products.Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return Ok(product);
    }


    
    [HttpGet]
    [Route("categories/{id:int}")]
    public async Task<ActionResult<Product>> GetByCateory(int id, [FromServices] DataContext context)
    {
         var product = await context.Products.Include(x => x.Category)
         .Where(x => x.CategoryId == id)
         .AsNoTracking()
         .ToListAsync();
        return Ok(product);
    }



    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Product>> Post([FromBody] Product model, [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Products.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível cadastrar o produto" });

        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Product>> Put(int id, [FromBody] Product model, [FromServices] DataContext context)
    {
        if (model.Id != id)
            return NotFound(new { message = "Produto não encontrado" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            context.Entry<Product>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível atualizar o produto" });

        }
        
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult> Delete(int id, [FromServices] DataContext context)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if(product == null)
            return NotFound(new { message = "Produto não encontrado" });

        try
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return Ok(new {message = "Produto removido com sucesso"});
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível remover o produto" });

        }
        
    }
}