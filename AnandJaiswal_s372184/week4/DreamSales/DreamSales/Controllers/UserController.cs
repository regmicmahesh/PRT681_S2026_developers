using DreamSales.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: User/Items - List all items the user has put up for sale
    public async Task<IActionResult> Items()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var items = await _context.Items.Where(i => i.SellerId == userId).ToListAsync();
        return View(items);
    }

    // GET: User/Customers - List all customers and how much they have spent on the user's items
    public async Task<IActionResult> Customers()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var customerPurchases = await _context.CartItems
            .Where(c => c.Item.SellerId == userId)
            .GroupBy(c => c.UserId)
            .Select(group => new
            {
                CustomerId = group.Key,
                TotalSpent = group.Sum(g => g.Quantity * g.Item.Price)
            })
            .ToListAsync();

        return View(customerPurchases);
    }

    // GET: User/Profile - Display the profile page for the current user
    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId); // Use UserManager to find the user
        return View(user);
    }

    // POST: User/Profile - Handle profile updates for the current user
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ApplicationUser user)
    {
        if (ModelState.IsValid)
        {
            var currentUser = await _userManager.FindByIdAsync(user.Id); // Use UserManager to find the user

            if (currentUser != null)
            {
                // Update user properties
                currentUser.FullName = user.FullName;
                currentUser.Address = user.Address;
                currentUser.City = user.City;
                currentUser.Country = user.Country;
                currentUser.DOB = user.DOB;
                currentUser.ProfilePictureUrl = user.ProfilePictureUrl;

                // Update user information
                var result = await _userManager.UpdateAsync(currentUser);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Profile));
                }
                // Add errors to ModelState if update failed
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        return View(user);
    }
}
