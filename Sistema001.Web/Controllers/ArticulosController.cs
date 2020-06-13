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
using Sistema001.Web.Models.Almacen.Articulo;

namespace Sistema001.Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public ArticulosController(DbContextSistema context)
        {
            _context = context;
        }

        [Authorize(Roles = "Almacenero,Administrador")]
        // GET: api/Articulos/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<ArticuloViewModel>> Listar()
        {

            var articulo = await _context.Articulos
                .Include(a => a.categoria)
                .ToListAsync();

            return articulo.Select(a => new ArticuloViewModel
            {
                idarticulo = a.idarticulo,
                idcategoria = a.idcategoria,
                categoria = a.categoria.nombre,
                codigo = a.codigo,
                nombre = a.nombre,
                stock = a.stock,
                precio_venta = a.precio_venta,
                descripcion = a.descripcion,
                condicion = a.condicion

            });

        }

        [Authorize(Roles = "Vendedor,Administrador,Almacenero")]
        // GET: api/Articulos/ArticulosMasVendidos
        [HttpGet("[action]/{fecha}")]
        public async Task<IEnumerable<ConsultaViewModel>> ArticulosMasVendidos([FromRoute] DateTime fecha)
        {

            var consulta = await _context.DetalleVentas
                .Include(v=>v.venta)
                .Include(d=>d.articulo)
                .Where(v=>v.venta.estado=="Aceptado")
                .Where(v=>v.venta.fecha_hora<= DateTime.Now)
                .Where(v=>v.venta.fecha_hora>= fecha)
                .GroupBy(v => new { v.idarticulo,v.articulo.nombre })
                .Select(x => new { etiqueta = x.Key.idarticulo, nombreEtiqueta = x.Key.nombre,valor = x.Sum(v=>v.cantidad) })
                .OrderByDescending(x => x.etiqueta)
                .ToListAsync();


            //var consulta = await _context.Ventas
            //  .Include(i => i.usuario)
            //  .Include(i => i.persona)
            //  .OrderByDescending(i => i.idventa)
            //  .Take(100)
            //  .ToListAsync();

         

            return consulta.Select(a => new ConsultaViewModel
            {
                etiqueta = a.etiqueta.ToString(),
                nombreEtiqueta= a.nombreEtiqueta ,
                valor = a.valor
            });

        }

        [Authorize(Roles = "Almacenero,Administrador")]
        // GET: api/Articulos/ListarIngreso/texto
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarIngreso([FromRoute] string texto)
        {

            var articulo = await _context.Articulos
                .Include(a => a.categoria)
                .Where(a=>a.condicion==true)
                .Where(a=>a.nombre.Contains(texto))
                .ToListAsync();

            return articulo.Select(a => new ArticuloViewModel
            {
                idarticulo = a.idarticulo,
                idcategoria = a.idcategoria,
                categoria = a.categoria.nombre,
                codigo = a.codigo,
                nombre = a.nombre,
                stock = a.stock,
                precio_venta = a.precio_venta,
                descripcion = a.descripcion,
                condicion = a.condicion

            });

        }

        [Authorize(Roles = "Vendedor,Administrador")]
        // GET: api/Articulos/ListarVenta/texto
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarVenta([FromRoute] string texto)
        {

            var articulo = await _context.Articulos
                .Include(a => a.categoria)
                .Where(a => a.condicion == true)
                .Where(a => a.stock > 0)
                .Where(a => a.nombre.Contains(texto))
                .ToListAsync();

            return articulo.Select(a => new ArticuloViewModel
            {
                idarticulo = a.idarticulo,
                idcategoria = a.idcategoria,
                categoria = a.categoria.nombre,
                codigo = a.codigo,
                nombre = a.nombre,
                stock = a.stock,
                precio_venta = a.precio_venta,
                descripcion = a.descripcion,
                condicion = a.condicion

            });

        }

        [Authorize(Roles = "Almacenero,Administrador")]
        // GET: api/Articulos/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ArticuloViewModel>> Mostrar([FromRoute]int id)
        {
            var articulo = await _context.Articulos
                .Include(a => a.categoria)
                .FirstOrDefaultAsync(a => a.idarticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre,
                stock = articulo.stock,
                precio_venta = articulo.precio_venta,
                descripcion = articulo.descripcion,
                condicion = articulo.condicion

            });
        }

        [Authorize(Roles = "Almacenero,Administrador")]
        // GET: api/Articulos/BuscarCodigo/1
        [HttpGet("[action]/{codigo}")]
        public async Task<ActionResult<ArticuloViewModel>> BuscarCodigo([FromRoute] string codigo)
        {
            var articulo = await _context.Articulos
                .Include(a => a.categoria)
                .Where(a=>a.condicion==true)
                .SingleOrDefaultAsync(a => a.codigo== codigo);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre,
                stock = articulo.stock,
                precio_venta = articulo.precio_venta,
                descripcion = articulo.descripcion,
                condicion = articulo.condicion

            });
        }

        [Authorize(Roles = "Vendedor,Administrador")]
        // GET: api/Articulos/BuscarCodigo/1
        [HttpGet("[action]/{codigo}")]
        public async Task<ActionResult<ArticuloViewModel>> BuscarCodigoVenta([FromRoute] string codigo)
        {
            var articulo = await _context.Articulos
                .Include(a => a.categoria)
                .Where(a=>a.stock > 0)
                .Where(a => a.condicion == true)
                .SingleOrDefaultAsync(a => a.codigo == codigo);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre,
                stock = articulo.stock,
                precio_venta = articulo.precio_venta,
                descripcion = articulo.descripcion,
                condicion = articulo.condicion

            });
        }



        // PUT: api/Articulos/Actualizar
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            //from body nos permite igual el objeto JSON al objeto que se esta instanciando
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idarticulo <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos
                .FirstOrDefaultAsync(a => a.idarticulo == model.idarticulo);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.nombre = model.nombre;
            articulo.idcategoria= model.idcategoria;
            articulo.codigo = model.codigo;
            articulo.precio_venta = model.precio_venta;
            articulo.stock = model.stock;
            articulo.descripcion = model.descripcion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Articulos/Crear
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpPost("[action]")]
        public async Task<ActionResult<Articulo>> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid) // si los data anotation no se cumplen esto valida que se cumplan sino el request sera detenido
            {
                return BadRequest(ModelState);
            }

            Articulo articulo = new Articulo
            {

                idcategoria = model.idcategoria,
                codigo = model.codigo,
                nombre = model.nombre,
                precio_venta = model.precio_venta,
                stock = model.stock,
                descripcion = model.descripcion,
                condicion = true

            };

            _context.Articulos.Add(articulo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }

        // PUT: api/Articulos/Desactivar/1
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos
                .FirstOrDefaultAsync(a => a.idarticulo== id); // expresion lambda para validar con lo que esta en la web vs la base de datos

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.condicion = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Articulos/Activar/1
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos
                 .FirstOrDefaultAsync(a => a.idarticulo == id); // expresion lambda para validar con lo que esta en la web vs la base de datos

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.condicion = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        private bool ArticuloExists(int id)
        {
            return _context.Articulos.Any(e => e.idarticulo == id);
        }
    }
}
