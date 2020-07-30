using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1")]
    public class HomeController : Controller
    {   

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context )
        {
            var manager  =  new User { Id= 1, Role = "Manager", UserName = "Juliano", Password= "123mudar"};
            var employee = new User{ Id=2 , Role="Employee", UserName="Funcionario", Password="123mudar"};
            var category = new Category{ Id = 1 , Title="Hardware"};
            var product = new Product{ Id=1, Title="Mouse", Category=category, Description="Mouse Logitech", Price=99};

            context.Users.Add(manager);
            context.Users.Add(employee);
            context.Categories.Add(category);
            context.Products.Add(product);

            await context.SaveChangesAsync();

            return Ok(
                new {message="Dados Configurados"}
            );

            
        }

    }
}