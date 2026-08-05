using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheaterAdmin.Api.Models;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly TheaterAdminApiContext _context;
    public MovieController(TheaterAdminApiContext context)
    {
        _context = context;
    }

    // GET: api/Movie
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
    {
        return await _context.Movie.ToListAsync();
    }

    // GET: api/Movie/5
    [HttpGet("{movieid}")]
    public async Task<ActionResult<Movie>> GetMovie(int movieid)
    {
        var movie = await _context.Movie.FindAsync(movieid);

        if (movie == null)
        {
            return NotFound();
        }

        return movie;
    }

    // PUT: api/Movie/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{movieid}")]
    public async Task<IActionResult> PutMovie(int? movieid, Movie movie)
    {
        if (movieid != movie.MovieId)
        {
            return BadRequest();
        }

        _context.Entry(movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(movieid))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Movie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Movie>> PostMovie(Movie movie)
    {
        _context.Movie.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMovie", new { movieid = movie.MovieId }, movie);
    }

    // DELETE: api/Movie/5
    [HttpDelete("{movieid}")]
    public async Task<IActionResult> DeleteMovie(int? movieid)
    {
        var movie = await _context.Movie.FindAsync(movieid);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movie.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int? movieid)
    {
        return _context.Movie.Any(e => e.MovieId == movieid);
    }
}
