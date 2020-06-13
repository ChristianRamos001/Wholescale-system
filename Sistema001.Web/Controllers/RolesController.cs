using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema001.Datos;
using Sistema001.Entidades.Usuarios;
using Sistema001.Web.Models.Usuarios.Rol;

namespace Sistema001.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public RolesController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Roles/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolViewModel>> Listar()
        {
            var roles = await _context.Roles.ToListAsync();

            return roles.Select(c => new RolViewModel
            {
                idrol = c.idrol,
                condicion = c.condicion,
                descripcion = c.descripcion,
                nombre = c.nombre,
            });

        }

        // GET: api/Roles/Select
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> Select()
        {
            var roles = await _context.Roles
                .Where(r => r.condicion == true)
                .ToListAsync();

            return roles.Select(r => new SelectViewModel
            {
                idrol = r.idrol,
                nombre = r.nombre,
            });

        }
        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.idrol == id);
        }
    }
}
