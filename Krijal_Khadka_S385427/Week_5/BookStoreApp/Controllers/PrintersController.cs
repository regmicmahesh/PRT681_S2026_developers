using BookStoreApp.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Controllers;

[Authorize]
public class PrintersController : Controller
{
    private readonly EfBookStoreContext _context;

    public PrintersController(EfBookStoreContext context)
    {
        _context = context;
    }

    // Anyone can view the list of printers.
    // GET: Printers
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var printers = await _context.Printers.ToListAsync();

        return View(printers);
    }

    // Anyone can view printer details.
    // GET: Printers/Details/5
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var printer = await _context.Printers
            .FirstOrDefaultAsync(printer => printer.Id == id);

        if (printer == null)
        {
            return NotFound();
        }

        return View(printer);
    }

    // Only authenticated users can open this action.
    // GET: Printers/Create
    public IActionResult Create()
    {
        return View();
    }

    // Only authenticated users can create a printer.
    // POST: Printers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,Brand,Model")] Printer printer)
    {
        if (ModelState.IsValid)
        {
            _context.Printers.Add(printer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(printer);
    }

    // Only authenticated users can open this action.
    // GET: Printers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var printer = await _context.Printers.FindAsync(id);

        if (printer == null)
        {
            return NotFound();
        }

        return View(printer);
    }

    // Only authenticated users can update a printer.
    // POST: Printers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,Brand,Model")] Printer printer)
    {
        if (id != printer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Printers.Update(printer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrinterExists(printer.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(printer);
    }

    // Only authenticated users can open this action.
    // GET: Printers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var printer = await _context.Printers
            .FirstOrDefaultAsync(printer => printer.Id == id);

        if (printer == null)
        {
            return NotFound();
        }

        return View(printer);
    }

    // Only authenticated users can delete a printer.
    // POST: Printers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var printer = await _context.Printers.FindAsync(id);

        if (printer != null)
        {
            _context.Printers.Remove(printer);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PrinterExists(int id)
    {
        return _context.Printers.Any(
            printer => printer.Id == id);
    }
}