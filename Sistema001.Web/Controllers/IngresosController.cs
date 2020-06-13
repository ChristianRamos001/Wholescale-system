using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema001.Datos;
using Sistema001.Entidades.Almacen;
using Sistema001.Web.Models.Almacen.Ingreso;

namespace Sistema001.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresosController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public IngresosController(DbContextSistema context)
        {
            _context = context;
        }

        [Authorize(Roles = "Almacenero,Administrador")]
        // GET: api/Ingresos/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<IngresoViewModel>> Listar()
        {

            var ingresos = await _context.Ingresos
                .Include(i => i.usuario)
                .Include(i => i.persona)
                .OrderByDescending(i => i.idingreso)
                .Take(100)
                .ToListAsync();
                

            return ingresos.Select(a => new IngresoViewModel
            {
                idingreso = a.idingreso,
                idproveedor = a.idproveedor,
                proveedor = a.persona.nombre,
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

        [Authorize(Roles = "Almacenero,Administrador")]
        // GET: api/Ingresos/ConsultarFecha/FechaInicio/FechaFin
        [HttpGet("[action]/{FechaInicio}/{FechaFin}")]
        public async Task<IEnumerable<IngresoViewModel>> ConsultarFecha([FromRoute] DateTime FechaInicio, DateTime FechaFin)
        {

            var ingresos = await _context.Ingresos
                .Include(i => i.usuario)
                .Include(i => i.persona)
                .Where(i => i.fecha_hora >= FechaInicio)
                .Where(i => i.fecha_hora <= FechaFin)
                .OrderByDescending(i => i.idingreso)
                .Take(100)
                .ToListAsync();


            return ingresos.Select(a => new IngresoViewModel
            {
                idingreso = a.idingreso,
                idproveedor = a.idproveedor,
                proveedor = a.persona.nombre,
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

        [Authorize(Roles = "Almacenero,Administrador")]
        // GET: api/Ingresos/ListarDetalles/id
        [HttpGet("[action]/{id}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int id)
        {

            var detalles = await _context.DetalleIngresos
                .Where(d=>d.idingreso==id)
                .Include(d=>d.articulo)
                .ToListAsync();


            return detalles.Select(a => new DetalleViewModel
            {
               cantidad = a.cantidad,
               idarticulo =a.idarticulo,
               articulo = a.articulo.nombre,
               precio = a.precio
            });

        }

        [Authorize(Roles = "Almacenero,Administrador")]
        // GET: api/Ingresos/ListarFiltro/texto
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<IngresoViewModel>> ListarFiltro([FromRoute] string texto)
        {

            var ingresos = await _context.Ingresos
                .Include(i => i.usuario)
                .Include(i => i.persona)
                .Where(i=>i.num_comprobante.Contains(texto))
                .OrderByDescending(i => i.idingreso)
                .ToListAsync();


            return ingresos.Select(a => new IngresoViewModel
            {
                idingreso = a.idingreso,
                idproveedor = a.idproveedor,
                proveedor = a.persona.nombre,
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

        // POST: api/Ingresos/Crear
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpPost("[action]")]
        public async Task<ActionResult<Ingreso>> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid) // si los data anotation no se cumplen esto valida que se cumplan sino el request sera detenido
            {
                return BadRequest(ModelState);
            }

            var fechaHora = DateTime.Now;

            Ingreso ingreso = new Ingreso
            {
                idproveedor = model.idproveedor,
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
                _context.Ingresos.Add(ingreso);
                await _context.SaveChangesAsync();

                var id = ingreso.idingreso;
                foreach (var det in model.detalles)
                {
                    DetalleIngreso detalle = new DetalleIngreso
                    {
                        idingreso = id,
                        idarticulo = det.idarticulo,
                        cantidad = det.cantidad,
                        precio = det.precio
                    };

                    _context.DetalleIngresos.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Authorize(Roles = "Almacenero,Administrador")]
        // PUT: api/Ingresos/Anular/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var ingreso = await _context.Ingresos
                .FirstOrDefaultAsync(i => i.idingreso== id); // expresion lambda para validar con lo que esta en la web vs la base de datos

            if (ingreso == null)
            {
                return NotFound();
            }

            ingreso.estado="Anulado";

            try
            {
                await _context.SaveChangesAsync();

                var detalle = await _context.DetalleIngresos
                    .Include(d => d.articulo)
                    .Where(d => d.idingreso == id)
                    .ToListAsync();

                foreach(var det in detalle)
                {
                    var articulo = await _context.Articulos
                        .FirstOrDefaultAsync(a => a.idarticulo == det.articulo.idarticulo);

                    articulo.stock = articulo.stock - det.cantidad;

                    await _context.SaveChangesAsync();
                }



            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }



        private bool IngresoExists(int id)
        {
            return _context.Ingresos.Any(e => e.idingreso == id);
        }
    }
}
