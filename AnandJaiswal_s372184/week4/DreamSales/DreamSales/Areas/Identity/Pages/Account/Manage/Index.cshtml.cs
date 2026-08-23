using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DreamSales.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DreamSales.Models;

namespace DreamSales.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Full Name")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            public string FullName { get; set; }

            [Required]
            [Range(0, 120, ErrorMessage = "Please enter a valid age.")]
            [Display(Name = "Age")]
            public int Age { get; set; }

            [EmailAddress]
            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Display(Name = "Profile Picture URL")]
            [Url(ErrorMessage = "Please enter a valid URL.")]
            public string ProfilePictureUrl { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "Country")]
            public string Country { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FullName = user.FullName,
                //Age = user.Age,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Address = user.Address,
                City = user.City,
                Country = user.Country
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            // Update FullName
            if (Input.FullName != user.FullName)
            {
                user.FullName = Input.FullName;
            }

            /*// Update Age
            if (Input.Age != user.Age)
            {
                user.Age = Input.Age;
            }*/

            // Update Email
            if (Input.Email != user.Email)
            {
                user.Email = Input.Email;
            }

            // Update ProfilePictureUrl
            if (Input.ProfilePictureUrl != user.ProfilePictureUrl)
            {
                user.ProfilePictureUrl = Input.ProfilePictureUrl;
            }

            // Update Address
            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;
            }

            // Update City
            if (Input.City != user.City)
            {
                user.City = Input.City;
            }

            // Update Country
            if (Input.Country != user.Country)
            {
                user.Country = Input.Country;
            }

            // Apply the updates to the database
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update your profile.";
                return RedirectToPage();
            }

            // Refresh the sign-in session
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
