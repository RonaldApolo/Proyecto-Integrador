using ecuaRefills2.Data;
using ecuaRefills2.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecuaRefills2.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProvinciasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProvinciasController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult ProvinciaIndex( string searchString)
        {
            var provincias = _context.Provincia.OrderBy(s => s.Nombre);
            var provincia = from s in _context.Provincia
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                provincia = provincia.OrderBy(s => s.Nombre).Where(s => s.Nombre.Contains(searchString));
                return View( provincia.ToList());
            }

            return View( provincias.ToList());
        }
        [HttpGet]
        public IActionResult CrearProvincia()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CrearProvincia(Provincia provincia)
        {
            if (ModelState.IsValid)
            {
                _context.Provincia.Add(provincia);
                _context.SaveChanges();
                TempData["mensaje"] = "La provincia se ha agregado correctamente.";
                return RedirectToAction("ProvinciaIndex");
            }
            return View(provincia);
        }

        // GET: Provincias/Details/5
        public async Task<IActionResult> DetalleProvincia(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provincia = await _context.Provincia
                .FirstOrDefaultAsync(m => m.ProvinciaId == id);
            if (provincia == null)
            {
                return NotFound();
            }

            return View(provincia);
        }


        // GET: Provincias/Edit/5
        public async Task<IActionResult> EditarProvincia(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provincia = await _context.Provincia.FindAsync(id);
            if (provincia == null)
            {
                return NotFound();
            }
            return View(provincia);
        }

        // POST: Provincias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProvincia(int id, [Bind("ProvinciaId,Nombre")] Provincia provincia)
        {
            if (id != provincia.ProvinciaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(provincia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvinciaExists(provincia.ProvinciaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["mensaje"] = "La provincia se ha actualizado correctamente.";
                return RedirectToAction(nameof(ProvinciaIndex));
            }
            return View(provincia);
        }

        // GET: Provincias/Delete/5
        public async Task<IActionResult> BorrarProvincia(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provincia = await _context.Provincia
                .FirstOrDefaultAsync(m => m.ProvinciaId == id);
            if (provincia == null)
            {
                return NotFound();
            }

            return View(provincia);
        }

        // POST: Provincias/Delete/5
        [HttpPost, ActionName("BorrarProvincia")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmacionBorrarProvincia(int id)
        {
            var provincia = await _context.Provincia.FindAsync(id);
            _context.Provincia.Remove(provincia);
            await _context.SaveChangesAsync();
            TempData["mensaje"] = "La provincia se ha eliminado correctamente.";
            return RedirectToAction(nameof(ProvinciaIndex));
        }

        private bool ProvinciaExists(int id)
        {
            return _context.Provincia.Any(e => e.ProvinciaId == id);
        }

    }
}
