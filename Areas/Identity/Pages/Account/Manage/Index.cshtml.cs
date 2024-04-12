// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using INTEX.Models;
using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace INTEX.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IRepo _repo;

        public IndexModel(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            IRepo repo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repo = repo;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            [Required]
            [StringLength(64, ErrorMessage = "First Name must be no more than 64 characters long.")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(64, ErrorMessage = "Last Name must be no more than 64 characters long.")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Birth Date")]
            public DateTime BirthDate { get; set; }

            [Required]
            [StringLength(32, ErrorMessage = "Gender must be no more than 32 characters long.")]
            [Display(Name = "Gender")]
            public string Gender { get; set; }

            #nullable enable
            [StringLength(64, ErrorMessage = "Address Line 1 must be no more than 64 characters")]
            [Display(Name = "Address Line 1")]
            public string? AddressLine1 { get; set; }
    
            [StringLength(64, ErrorMessage = "Address Line 2 must be no more than 64 characters")]
            [Display(Name = "Address Line 2")]
            public string? AddressLine2 { get; set; }
    
            [StringLength(64, ErrorMessage = "City must be no more than 64 characters")]
            [Display(Name = "City")]
            public string? City { get; set; }
    
            [StringLength(64, ErrorMessage = "State must be no more than 64 characters")]
            [Display(Name = "State")]
            public string? State { get; set; }
    
            [StringLength(64, ErrorMessage = "Code must be no more than 64 characters")]
            [Display(Name = "Postal Code")]
            public string? Code { get; set; }
    
            #nullable disable
            [Required(ErrorMessage = "Country is a required field")]
            [StringLength(64, ErrorMessage = "Country must be no more than 64 characters")]
            [Display(Name = "Country")]
            public string Country { get; set; }
        }

        private async Task LoadAsync(Customer user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            user = _repo.GetCustomerById(user.Id);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                AddressLine1 = user.HomeAddress.AddressLine1,
                AddressLine2 = user.HomeAddress.AddressLine2,
                City = user.HomeAddress.City,
                State = user.HomeAddress.State,
                Code = user.HomeAddress.Code,
                Country = user.HomeAddress.Country,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
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
            user = _repo.GetCustomerById(user!.Id);
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
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.BirthDate = DateOnly.FromDateTime(Input.BirthDate);
            user.Gender = Input.Gender;
            if (user.HomeAddress is null)
            {
                user.HomeAddress = new Address
                {
                    AddressLine1 = Input.AddressLine1,
                    AddressLine2 = Input.AddressLine2,
                    City = Input.City,
                    State = Input.State,
                    Code = Input.Code,
                    Country = Input.Country
                };
            }
            else
            {
                user.HomeAddress.AddressLine1 = Input.AddressLine1;
                user.HomeAddress.AddressLine2 = Input.AddressLine2;
                user.HomeAddress.City = Input.City;
                user.HomeAddress.State = Input.State;
                user.HomeAddress.Code = Input.Code;
                user.HomeAddress.Country = Input.Country;
            }
            // replacing _userManager with _repo actions.
            _repo.UpdateCustomer(user);
            // await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
