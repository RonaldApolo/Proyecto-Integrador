using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecuaRefills2.Data;
using ecuaRefills2.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ecuaRefills2.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes

        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = _context.Cliente.Include(c => c.Ciudad).OrderBy(x => x.Nombre);

            var clientes = from s in _context.Cliente
                           select s;
            //Si el string no es nulo o no esta vacio se realiza la consulta.
            if (!String.IsNullOrEmpty(searchString))
            {
                clientes = clientes.Include(c => c.Ciudad).Where(s => s.Nombre.Contains(searchString) || s.Cedula.Contains(searchString)
                || s.CorreoElectronico.Contains(searchString) || s.Ciudad.Nombre.Contains(searchString)).OrderBy(x => x.Nombre);
                return View(await clientes.ToListAsync());
            }

            return View(await applicationDbContext.ToListAsync());

        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .Include(c => c.Ciudad)
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Provincias = new SelectList(_context.Provincia.OrderBy(x => x.Nombre), "ProvinciaId", "Nombre");
            return View();
        }


        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,Cedula,Nombre,CorreoElectronico,Telefono,CiudadId")] Cliente cliente, Cliente model)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(cliente);
            //    await _context.SaveChangesAsync();
            //    TempData["mensaje"] = "El cliente se ha creado correctamente.";
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CiudadId"] = new SelectList(_context.Ciudad, "CiudadId", "Nombre", cliente.CiudadId);
            //return View(cliente);
            try
            {
                var cedulaExiste = _context.Cliente.FirstOrDefault(m => m.Cedula == model.Cedula);

                if (cedulaExiste == null)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(cliente);
                        await _context.SaveChangesAsync();
                        TempData["mensaje"] = "El cliente se ha creado correctamente.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    TempData["CedulaExiste"] = "No se pudo insertar el cliente, El registro ya existe.";
                    return RedirectToAction(nameof(Index));
                }


            }
            catch (Exception e)
            {
                TempData["error"] = e.ToString();
                ViewBag.error = TempData["error"].ToString();
            }
            ViewData["CiudadId"] = new SelectList(_context.Ciudad.OrderBy(x => x.Nombre), "CiudadId", "Nombre", cliente.CiudadId);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewBag.Provincias = new SelectList(_context.Provincia.OrderBy(x => x.Nombre), "ProvinciaId", "Nombre");
            ViewData["CiudadId"] = new SelectList(_context.Ciudad.OrderBy(x => x.Nombre), "CiudadId", "Nombre", cliente.CiudadId);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Cedula,Nombre,CorreoElectronico,Telefono,CiudadId")] Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["mensaje"] = "El cliente se ha actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Provincias = new SelectList(_context.Provincia.OrderBy(x => x.Nombre), "ProvinciaId", "Nombre");
            ViewData["CiudadId"] = new SelectList(_context.Ciudad.OrderBy(x => x.Nombre), "CiudadId", "Nombre", cliente.CiudadId);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .Include(c => c.Ciudad)
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            TempData["mensaje"] = "El cliente se ha eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.ClienteId == id);
        }


        public JsonResult LocalizarCiudad(int Id)
        {

            var ciudad = _context.Ciudad.Where(x => x.ProvinciaId == Id).ToList();
            return Json(new SelectList(ciudad, "CiudadId", "Nombre"));
        }

    }
}
