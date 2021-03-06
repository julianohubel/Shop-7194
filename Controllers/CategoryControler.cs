using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("categories")]
public class CategoryCotroler : Controller
{
    
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {

        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
    {
         var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return Ok(category);
    }


    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> Post([FromBody] Category model, [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Categories.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível cadastrar a categoria" });

        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model, [FromServices] DataContext context)
    {
        if (model.Id != id)
            return NotFound(new { message = "Categoria não encontrada" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível atualizar a categoria" });

        }
        
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult> Delete(int id, [FromServices] DataContext context)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if(category == null)
            return NotFound(new { message = "Categoria não encontrada" });

        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new {message = "Categoria removida com sucesso"});
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível remover a categoria" });

        }
        
    }
}