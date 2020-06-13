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
using Sistema001.Web.Models.Ventas.Persona;

namespace Sistema001.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public PersonasController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Personas/Listar
        [Authorize(Roles = "Vendedor,Administrador,Almacenero")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonaViewModel>> Listar()
        {
            var personas = await _context.Personas.ToListAsync();

            return personas.Select(p => new PersonaViewModel
            {
                idpersona = p.idpersona,
                direccion = p.direccion,
                email = p.email,
                nombre = p.nombre,
                num_documento = p.num_documento,
                telefono = p.telefono,
                tipo_documento = p.tipo_documento,
                tipo_persona = p.tipo_persona,
                
            });

        }

        // GET: api/Personas/ListarClientes
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonaViewModel>> ListarClientes()
        {
            var personas = await _context.Personas.Where(p => p.tipo_persona == "Cliente").ToListAsync();

            return personas.Select(p => new PersonaViewModel
            {
                idpersona = p.idpersona,
                direccion = p.direccion,
                email = p.email,
                nombre = p.nombre,
                num_documento = p.num_documento,
                telefono = p.telefono,
                tipo_documento = p.tipo_documento,
                tipo_persona = p.tipo_persona,

            });

        }
        // GET: api/Personas/ListarProovedores
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonaViewModel>> ListarProovedores()
        {
            var personas = await _context.Personas.Where(p => p.tipo_persona == "Proovedor").ToListAsync();

            return personas.Select(p => new PersonaViewModel
            {
                idpersona = p.idpersona,
                direccion = p.direccion,
                email = p.email.ToLower(),
                nombre = p.nombre,
                num_documento = p.num_documento,
                telefono = p.telefono,
                tipo_documento = p.tipo_documento,
                tipo_persona = p.tipo_persona,

            });

        }

        // GET: api/Personas/SelectProveedores
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> SelectProveedores()
        {
            var personas = await _context.Personas.Where(p => p.tipo_persona == "Proovedor").ToListAsync();

            return personas.Select(p => new SelectViewModel
            {
                idpersona = p.idpersona,
                nombre = p.nombre,
            
            });

        }

        // GET: api/Personas/SelectProveedores
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> SelectClientes()
        {
            var personas = await _context.Personas.Where(p => p.tipo_persona == "Cliente").ToListAsync();

            return personas.Select(p => new SelectViewModel
            {
                idpersona = p.idpersona,
                nombre = p.nombre,

            });

        }


        // POST: api/Personas/Crear
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Vendedor,Administrador,Almacenero")]
        [HttpPost("[action]")]
        public async Task<ActionResult<Persona>> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid) // si los data anotation no se cumplen esto valida que se cumplan sino el request sera detenido
            {
                return BadRequest("Error fatal");
            }

            var email = model.email.ToLower();
            if (await _context.Personas.AnyAsync(u => u.email == email))
            {
                return BadRequest("El email ya existe");
            }

            Persona persona = new Persona
            {
                tipo_persona = model.tipo_persona,
                tipo_documento = model.tipo_documento,
                telefono = model.telefono,
                num_documento = model.num_documento,
                nombre = model.nombre,
                email = model.email.ToLower(),
                direccion = model.direccion,
      
            };

            _context.Personas.Add(persona);

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

        // PUT: api/Personas/Actualizar
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Vendedor,Administrador,Almacenero")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            //from body nos permite igual el objeto JSON al objeto que se esta instanciando
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idpersona <= 0)
            {
                return BadRequest();
            }

            var persona = await _context.Personas.FirstOrDefaultAsync(p => p.idpersona== model.idpersona);

            if (persona == null)
            {
                return NotFound();
            }

            persona.tipo_persona = model.tipo_persona;
            persona.tipo_documento = model.tipo_documento;
            persona.telefono = model.telefono;
            persona.num_documento = model.num_documento;
            persona.nombre = model.nombre;
            persona.email = model.email;
            persona.direccion = model.direccion;
            

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


        private bool PersonaExists(int id)
        {
            return _context.Personas.Any(e => e.idpersona == id);
        }
    }
}
