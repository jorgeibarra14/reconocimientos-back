using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;

namespace Reconocimientos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PedidosController : Controller
    {
        private readonly IPedidosService _pedidosService;
        private readonly IOdsService _odsService;
        private readonly IReconocimientoService _reconocimientoService;
        private readonly IProductosService _productoservice;
        private readonly IPuntosService _puntosService;

        public PedidosController(IPedidosService pedidosService,
             IOdsService odsService,
             IProductosService productoservice,
             IReconocimientoService reconocimientoService, IPuntosService puntosService)
        {
            _pedidosService = pedidosService;
            _odsService = odsService;
            _productoservice = productoservice;
            _reconocimientoService = reconocimientoService;
            _puntosService = puntosService;
        }

        // GET: api/<PedidosController>/ObtenerPedidos
        [HttpGet("ObtenerPedidos")]
        public ActionResult<IEnumerable<Pedidos>> ObtenerPedidos()
        {
            List<Pedidos> pedidos = (List<Pedidos>)_pedidosService.ObtenerAllPedidos();
            return Ok(pedidos);
        }

        // GET api/<PedidosController>/ObtenerPedidosId
        [HttpGet("ObtenerPedidosId")]
        public ActionResult<IEnumerable<Pedidos>> ObtenerPedidosId(int id)
        {
            List<Pedidos> pedidosId = (List<Pedidos>)_pedidosService.ObtenerPedidosId(id);
            return Ok(pedidosId);
        }

        // GET api/<PedidosController>/ObtenerPedidosSolicitante
        [HttpGet("ObtenerPedidosSolicitante")]
        public ActionResult<IEnumerable<Pedidos>> ObtenerPedidosSolicitante(int id_solicitante)
        {
            List<Pedidos> pedidosSolicitante = (List<Pedidos>)_pedidosService.ObtenerPedidosSolicitante(id_solicitante);
            return Ok(pedidosSolicitante);
        }

        // POST api/<PedidosController>/AgregarPedidos
        [HttpPost("AgregarPedidos")]
        public IActionResult AgregarPedidos([FromBody] Pedidos pedido)
        {

            //Validar stock 
            int totalPuntos = 0;
            foreach (ProductosPedido producto in pedido.productos)
            {
                List<Productos> productoResult = (List<Productos>)_productoservice.getProductsById(producto.producto_id);
                var stock = productoResult[0].stock;
                if (stock < producto.cantidad)
                {
                    return BadRequest("No hay stock suficiente para realizar el pedido");
                }
                totalPuntos += producto.producto_costo;
            }

            //Traer datos del colaborador que solicita
            List<InformacionOdsDetalle> empleado = (List<InformacionOdsDetalle>)_odsService.ObtenerInformacionODSporId(pedido.id_solicitante);

            pedido.nombre_solicitante = empleado[0].NombreCompleto;
            pedido.id_autorizador = empleado[0].Id;
            pedido.nombre_autorizador = empleado[0].Nombre;

            //Hacer el pedido
            var resultado = _pedidosService.InsertarPedidos(pedido);

            var pedidoCelular = new PedidosCelular { Celular = pedido.celularEmpleado, IdPedido = resultado };
            var respuestaCelular = _pedidosService.InsertarPedidoCelular(pedidoCelular);

            var usuarioPuntos = new UsuariosPuntos { IdEmpleado = pedido.id_solicitante, Valor = totalPuntos * -1, Tipo = "Gasto", IdPedido = resultado };
            var res = _puntosService.InsertarPuntosTienda(usuarioPuntos);
            //Descontar puntos


            return Ok(resultado);
        }

        // POST api/<PedidosController>/ModificarPedidos
        [HttpPost("ModificarPedidos")]
        public IActionResult ModificarPedidos([FromBody] Pedidos pedidos)
        {
            return Ok(_pedidosService.ActulizarPedidos(pedidos));
        }

        // POST api/<PedidosController>/EliminarPedidos
        [HttpGet("EliminarPedidos")]
        public IActionResult EliminarPedidos(int id)
        {
            return Ok(_pedidosService.EliminarPedidos(id));
        }


    }
}