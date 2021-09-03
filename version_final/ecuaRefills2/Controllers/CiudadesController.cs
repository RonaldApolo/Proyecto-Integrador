using ecuaRefills2.Data;
using ecuaRefills2.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecuaRefills2.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CiudadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CiudadesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CiudadIndex( string searchString)
        {
            var applicationDbContext = _context.Ciudad.OrderBy(s => s.Nombre).Include(c => c.Provincia);
            var ciudades = from s in _context.Ciudad
                           select s;
            //Si el string no es nulo o no esta vacio se realiza la consulta.
            if (!String.IsNullOrEmpty(searchString))
            {
                ciudades = ciudades.OrderBy(s => s.Nombre).Include(c => c.Provincia).Where(s => s.Nombre.Contains(searchString) || s.Provincia.Nombre.Contains(searchString));
                return View(await ciudades.ToListAsync());
            }
            return View(await applicationDbContext.ToListAsync());
        }
        public IActionResult CrearCiudad()
        {
            ViewBag.Provincias = new SelectList(_context.Provincia.OrderBy(x => x.Nombre), "ProvinciaId", "Nombre");
            return View();
        }

        [HttpPost]
        //Si algo sale mal agregar el Bind
        public IActionResult CrearCiudad(Ciudad ciudad)
        {
            //se agrego la condicion y el view data.
            if (ModelState.IsValid)
            {
                _context.Ciudad.Add(ciudad);
                _context.SaveChanges();
                TempData["mensaje"] = "La ciudad se ha creado correctamente.";
                return RedirectToAction("CiudadIndex");
            }
            ViewBag.Provincias = new SelectList(_context.Provincia.OrderBy(x => x.Nombre), "ProvinciaId", "Nombre", ciudad.ProvinciaId);
            return View(ciudad);
        }

        public async Task<IActionResult> DetalleCiudad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ciudad = await _context.Ciudad
                .Include(c => c.Provincia)
                .FirstOrDefaultAsync(m => m.CiudadId == id);
            if (ciudad == null)
            {
                return NotFound();
            }

            return View(ciudad);
        }

        public async Task<IActionResult> EditarCiudad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ciudad = await _context.Ciudad.FindAsync(id);
            if (ciudad == null)
            {
                return NotFound();
            }
            ViewBag.Provincias = new SelectList(_context.Provincia.OrderBy(x => x.Nombre), "ProvinciaId", "Nombre", ciudad.ProvinciaId);
            return View(ciudad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCiudad(int id, [Bind("CiudadId,Nombre,ProvinciaId")] Ciudad ciudad)
        {
            if (id != ciudad.CiudadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ciudad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CiudadExists(ciudad.CiudadId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["mensaje"] = "La ciudad se ha actualizado correctamente.";
                return RedirectToAction(nameof(CiudadIndex));
            }
            ViewBag.Provincias = new SelectList(_context.Provincia.OrderBy(x => x.Nombre), "ProvinciaId", "Nombre", ciudad.ProvinciaId);
            return View(ciudad);
        }


        // GET: Ciudades/Delete/5
        public async Task<IActionResult> BorrarCiudad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ciudad = await _context.Ciudad
                .Include(c => c.Provincia)
                .FirstOrDefaultAsync(m => m.CiudadId == id);
            if (ciudad == null)
            {
                return NotFound();
            }

            return View(ciudad);
        }

        // POST: Ciudades/Delete/5
        [HttpPost, ActionName("BorrarCiudad")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarBorrar(int id)
        {
            var ciudad = await _context.Ciudad.FindAsync(id);
            _context.Ciudad.Remove(ciudad);
            await _context.SaveChangesAsync();
            TempData["mensaje"] = "La ciudad se ha eliminado correctamente.";
            return RedirectToAction(nameof(CiudadIndex));
        }

        //Revisar si una ciudad existe.
        private bool CiudadExists(int id)
        {
            return _context.Ciudad.Any(e => e.CiudadId == id);
        }


    }
}
