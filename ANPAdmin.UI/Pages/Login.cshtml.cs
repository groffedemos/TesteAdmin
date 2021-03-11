﻿using ANPAdmin.Business;
using ANPAdmin.UI.Helpers;
using ANPAdmin.UI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ANPAdmin.UI.Pages
{
    public class LoginModel : PageModel
    {
        [Required]
        [BindProperty]
        public string Email { get; set; }
        [Required]
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {

        }

        private IAuth _auth;

        public LoginModel(IAuth auth)
        {
            _auth = auth;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await Task.Run(() =>
            {
                _auth.Login(Email, Password);
                var user = new UserModel(Email);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "USER_LOGIN", user);
            });

            return RedirectToPage("./Index");
        }
    }
}