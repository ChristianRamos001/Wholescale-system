using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema001.Datos;
using Sistema001.Entidades.Ventas;
using Sistema001.Web.Models.Ventas;
using Sistema001.Web.Models.Ventas.Venta;

namespace Sistema001.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public VentasController(DbContextSistema context)
        {
            _context = context;
        }

        [Authorize(Roles = "Vendedor,Administrador")]
        // GET: api/Ventas/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> Listar()
        {

            var ventas = await _context.Ventas
                .Include(i => i.usuario)
                .Include(i => i.persona)
                .OrderByDescending(i => i.idventa)
                .Take(100)
                .ToListAsync();


            return ventas.Select(a => new VentaViewModel
            {
                idventa = a.idventa,
                idcliente = a.idcliente,

                cliente = a.persona.nombre,
                direccion = a.persona.direccion,
                email = a.persona.email,
                num_documento = a.persona.num_documento,
                telefono = a.persona.telefono,

                idusuario = a.idusuario,
                usuario = a.usuario.nombre,
                estado = a.estado,
                fecha_hora = a.fecha_hora,
                impuesto = a.impuesto,
                num_comprobante = a.num_comprobante,
                serie_comprobante = a.serie_comprobante,
                tipo_comprobante = a.tipo_comprobante,
                total = a.total

            });

        }

        [Authorize(Roles = "Vendedor,Administrador,Almacenero")]
        // GET: api/Ventas/VentasMes12
        [HttpGet("[action]")]
        public async Task<IEnumerable<ConsultaViewModel>> VentasMes12()
        {

            var consulta = await _context.Ventas
                .GroupBy(v=>v.fecha_hora.Month)
                .Select(x=> new { etiqueta= x.Key, valor=x.Sum(v=>v.total)})
                .OrderByDescending(x=>x.etiqueta)
                .Take(12)
                .ToListAsync();


            return consulta.Select(a => new ConsultaViewModel
            {
                etiqueta = a.etiqueta.ToString(),
                valor = a.valor
            });

        }

        [Authorize(Roles = "Vendedor,Administrador")]
        // GET: api/Ventas/ListarDetalles/id
        [HttpGet("[action]/{id}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int id)
        {

            var detalles = await _context.DetalleVentas
                .Where(d => d.idventa == id)
                .Include(d => d.articulo)
                .ToListAsync();


            return detalles.Select(a => new DetalleViewModel
            {
                cantidad = a.cantidad,
                idarticulo = a.idarticulo,
                articulo = a.articulo.nombre,
                precio = a.precio,
                descuento = a.descuento
            
            });

        }

        [Authorize(Roles = "Vendedor,Administrador")]
        // GET: api/Ventas/ConsultarFecha/FechaInicio/FechaFin
        [HttpGet("[action]/{FechaInicio}/{FechaFin}")]
        public async Task<IEnumerable<VentaViewModel>> ConsultarFecha([FromRoute] DateTime FechaInicio, DateTime FechaFin)
        {

            var ventas = await _context.Ventas
                .Include(i => i.usuario)
                .Include(i => i.persona)
                .Where(i=> i.fecha_hora>=FechaInicio)
                .Where(i=>i.fecha_hora <= FechaFin)
                .OrderByDescending(i => i.idventa)
                .Take(100)
                .ToListAsync();


            return ventas.Select(a => new VentaViewModel
            {
                idventa = a.idventa,
                idcliente = a.idcliente,

                cliente = a.persona.nombre,
                direccion = a.persona.direccion,
                email = a.persona.email,
                num_documento = a.persona.num_documento,
                telefono = a.persona.telefono,

                idusuario = a.idusuario,
                usuario = a.usuario.nombre,
                estado = a.estado,
                fecha_hora = a.fecha_hora,
                impuesto = a.impuesto,
                num_comprobante = a.num_comprobante,
                serie_comprobante = a.serie_comprobante,
                tipo_comprobante = a.tipo_comprobante,
                total = a.total

            });

        }

        [Authorize(Roles = "Vendedor,Administrador")]
        // GET: api/Ventas/ListarFiltro/texto
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<VentaViewModel>> ListarFiltro([FromRoute] string texto)
        {

            var ingresos = await _context.Ventas
                .Include(v => v.usuario)
                .Include(v => v.persona)
                .Where(v => v.num_comprobante.Contains(texto))
                .OrderByDescending(v => v.idventa)
                .ToListAsync();


            return ingresos.Select(a => new VentaViewModel
            {
                idventa = a.idventa,
                idcliente = a.idcliente,
                cliente = a.persona.nombre,
                direccion = a.persona.direccion,
                email = a.persona.email,
                num_documento = a.persona.num_documento,
                telefono = a.persona.telefono,
                idusuario = a.idusuario,
                usuario = a.usuario.nombre,
                estado = a.estado,
                fecha_hora = a.fecha_hora,
                impuesto = a.impuesto,
                num_comprobante = a.num_comprobante,
                serie_comprobante = a.serie_comprobante,
                tipo_comprobante = a.tipo_comprobante,
                total = a.total

            });


        }

        // POST: api/Ventas/Crear
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPost("[action]")]
        public async Task<ActionResult<Venta>> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid) // si los data anotation no se cumplen esto valida que se cumplan sino el request sera detenido
            {
                return BadRequest(ModelState);
            }

            var fechaHora = DateTime.Now;

            Venta venta = new Venta
            {
                idcliente = model.idcliente,
                idusuario = model.idusuario,
                tipo_comprobante = model.tipo_comprobante,
                serie_comprobante = model.serie_comprobante,
                num_comprobante = model.num_comprobante,
                fecha_hora = fechaHora,
                impuesto = model.impuesto,
                total = model.total,
                estado = "Aceptado"
            };

            try
            {
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                var id = venta.idventa;
                foreach (var det in model.detalles)
                {
                    DetalleVenta detalle = new DetalleVenta
                    {
                        idventa= id,
                        idarticulo = det.idarticulo,
                        cantidad = det.cantidad,
                        precio = det.precio,
                        descuento = det.descuento
                    };

                    _context.DetalleVentas.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize(Roles = "Vendedor,Administrador")]
        // PUT: api/Ventas/Anular/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var venta = await _context.Ventas
                .FirstOrDefaultAsync(v => v.idventa== id); // expresion lambda para validar con lo que esta en la web vs la base de datos

            if (venta == null)
            {
                return NotFound();
            }

            venta.estado = "Anulado";

            try
            {
                await _context.SaveChangesAsync();

                var detalle = await _context.DetalleVentas
                    .Include(d => d.articulo)
                    .Where(d => d.idventa == id)
                    .ToListAsync();

                foreach (var det in detalle)
                {
                    var articulo = await _context.Articulos
                        .FirstOrDefaultAsync(a => a.idarticulo == det.articulo.idarticulo);

                    articulo.stock = articulo.stock + det.cantidad;

                    await _context.SaveChangesAsync();
                }



            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.idventa == id);
        }
    }
}
