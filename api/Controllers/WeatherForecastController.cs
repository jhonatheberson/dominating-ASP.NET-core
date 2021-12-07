using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("get-id/{id:int}")]
        public ActionResult<string> Get(int id)
        {
          return Summaries[0];
        }

        [HttpGet("getall")]
        public ActionResult<IEnumerable<string>> GetAll()
        {
          var valores = new string[] {"value1", "value2"};

          if (valores.Length < 5000)
            return BadRequest();

          return Ok(valores);
        }

        [HttpGet("getall2")]
        public IEnumerable<string> getAll2()
        {
          var valores = new string[] {"value1", "value2"};

          if (valores.Length < 5000)
            return null;

          return valores;
        }


        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)] //usado para criação, insert
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // erro caso de errado
        
        public ActionResult Post(Product product)
        {
          if(product.id == 0) return BadRequest();

          //add no database
          // return Ok(product);
          return CreatedAtAction(actionName: "Post", product);
        }
      

        [HttpPut("{id}")]
        //aqui especifico que está o id está vindo do rota da requisição queryparams
        //não preciso marcar o FromRoute, porque o "id" está no rota, e que vou receber é mesmo nome 
        // ele percebe que tem um tipo complexo, e já cria uma instancia para você da class Product
        // "FromHeader" é quando passo o parametro está sendo enviado via header
        // "FromQuery" é quando não estou passando pela rota o parametros, 
        //mas pela query, é importante que nome da variavel seja o mesmo
        //"FromServices" é injetar uma interface, uma classe, vai resolver, não faz parte do http, mas sim do ASP.Net Core
        //aqui especifico que está o value está vindo do Body da requisição usando "FromBody"
        [ProducesResponseType(typeof(Product), StatusCodes.Status202Accepted)] //usado para criação, Update
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // erro caso de errado
        public ActionResult Put([FromRoute] int id,[FromBody] Product product)
        {
          if(product.id  == 0) return BadRequest();

          // atualizar no database

          return CreatedAtAction(actionName: "Update", id);
        }

        [HttpDelete]
        public ActionResult Delete([FromQuery] int id)
        {
          // delete database
          return Ok();
        }
    }
    
    public class Product
    {
        public int id { get; set; }

        
        public string Name {get; set;}

        
        public string Descritption {get; set;}

    }

}

