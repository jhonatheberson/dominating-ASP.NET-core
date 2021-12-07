# criando API Dot.Net Core webapi

o comando para criar aplicações é:

~~~bash
dotnet new
~~~

usando esse comando, mostra todas aplicações e templates possiveis de criação em nosso caso usaremos **webapi**

~~~bash
dotnet new webapi -n [nomeDoProjeto]
~~~


para rodar a aplicação basta utilizar

~~~bash
dotnet run
~~~

usando [swager](https://localhost:5001/swagger/index.html)

*https://localhost:5001/swagger/index.html*

para fazer mudanças na aplicação enquanto desenvolve e api, fazer as alterações, utilizamos modo **watch**

~~~bash
dotnet watch run
~~~

- *Properties/launchSettings.json* --  está as informações de conexão e configurações do projeto

- *Controllers/* -- fica os controller que vai manipular os dados para responder

  - FromBody -- a requisição vai no corpo


# Controllers

  A controler do é coluna servical da API, e herda da **ControllerBase** no MVC.
  também importamos o atributo **ApiController** que nos fornece mais metodos necessarios para o desenvolvimento.

  ## Rotas

  rotas podem ser realizadas no controllers inicialmente da seguinte forma:

  ~~~c#
  [HttpGet("get-id/{id:int}")]
  public ActionResult<string> Get(int id)
  {
    return Summaries[0];
  }
  ~~~

  ou de forma mais complexa


  ~~~c#
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
  ~~~

  ## Action Results e Formatadores de dados de resposta

  o Action Results é o resultado de uma Action, e existe varios tipos de action Results

  quando usamos o **ActionResults**, temos varios metodos de Https:

  - **BadRequest();** que retorna o erro http como statuscode 400.
  - **Ok** que retorna status 200 do http.
  - **FromHeader** é quando passo o parametro está sendo enviado via header
  - **FromQuery** é quando não estou passando pela rota o parametros, 
  - **FromBody** é quando estou passando os dados pelo body
  - **FromRoute** é quando o parametro está vindo do rota da requisição

  ~~~c#
  [HttpGet("getall")]
  public ActionResult<IEnumerable<string>> GetAll()
  {
    var valores = new string[] {"value1", "value2"};

    if (valores.Length < 5000)
      return BadRequest();

    return Ok(valores);
  }
  ~~~

  que quando não usamos, por exemplo **IEnumerable**, não temos essas funcionalidades

  ~~~c#
  [HttpGet("getall2")]
  public IEnumerable<string> getAll2()
  {
    var valores = new string[] {"value1", "value2"};

    if (valores.Length < 5000)
      return null;

    return valores;
  }
  ~~~

  ### usando Post

  **ProducesResponseType** é muito importante para documentação,e retornar dados de forma mais detalhada

  ~~~c#
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
  ~~~

  ### usando Put

  ~~~c#
  [HttpPut("{id}")]
  //aqui especifico que está o id está vindo do rota da requisição queryparams
  //não preciso marcar o FromRoute, porque o "id" está no rota, e que vou receber é mesmo nome 
  // ele percebe que tem um tipo complexo, e já cria uma instancia para você da class Product
  // "FromHeader" é quando passo o parametro está sendo enviado via header
  // "FromQuery" é quando não estou passando pela rota o parametros, 
  //mas pela query, é importante que nome da variavel seja o mesmo
  //"FromServices" é injetar uma interface, uma classe, vai resolver, não faz parte do http, mas sim do ASP.Net Core
  //aqui especifico que está o value está vindo do Body da requisição usando "FromBody"
  public ActionResult Put([FromRoute] int id,[FromBody] Product product)
  {
    if(product.id  == 0) return BadRequest();

    // atualizar no database

    return CreatedAtAction(actionName: "Update", id);
  }
  ~~~

  ### usando Delete
  ~~~c#
  [HttpDelete]
  public void Delete([FromQuery] int id)
  {

  }
  ~~~

  foi criado um tipo complexo chamado **Product**, que pode ser usado como entrada de parametros, ou seja esse tipo será solicitado no Post, se assim for solicitado

  ~~~c#
    public class Product
    {
        public int id { get; set; }

        
        public string Name {get; set;}

        
        public string Descritption {get; set;}

    } 
  ~~~