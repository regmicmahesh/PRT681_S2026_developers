using BookStoreApp.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Controllers;

public class PrintersController : Controller
{
    private readonly EfBookStoreContext _context;

    public PrintersController(EfBookStoreContext context)
    {
        _context = context;
    }

    // GET: Printers
    public async Task<IActionResult> Index()
    {
        var printers = await _context.Printers.ToListAsync();

        return View(printers);
    }

    // GET: Printers/Details/5
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

    // GET: Printers/Create
    public IActionResult Create()
    {
        return View();
    }

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