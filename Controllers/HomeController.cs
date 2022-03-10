using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reconocimientos.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : Controller
    {
        // GET: /
        [HttpGet]
        public string home()
        {
            //return "{ mensaje: es necesario un token para utilizar la aplicacion }";
            return "favor de iniciar sesion";
        }
    }
}
