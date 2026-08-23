using BookStoreApp.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Controllers;

[Authorize]
public class BooksController : Controller
{
    private readonly EfBookStoreContext _context;

    public BooksController(EfBookStoreContext context)
    {
        _context = context;
    }

    // Anyone can view the list of books.
    // GET: Books
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.ToListAsync();

        return View(books);
    }

    // Anyone can view book details.
    // GET: Books/Details/5
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .FirstOrDefaultAsync(book => book.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // Only authenticated users can open this action.
    // GET: Books/Create
    public IActionResult Create()
    {
        return View();
    }

    // Only authenticated users can create a book.
    // POST: Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(book);
    }

    // Only authenticated users can open this action.
    // GET: Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // Only authenticated users can update a book.
    // POST: Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name")] Book book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(book);
    }

    // Only authenticated users can open this action.
    // GET: Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .FirstOrDefaultAsync(book => book.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // Only authenticated users can delete a book.
    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(book => book.Id == id);
    }
}