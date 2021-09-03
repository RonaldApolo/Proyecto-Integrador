using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ecuaRefills2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ecuaRefills2.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public IndexModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager)
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
          
            [DataType(DataType.Text)]
            [Display(Name = "Cédula")]
            public string Cedula { get; set; }

            [Display(Name = "Nombre de Usuario")]
            [DataType(DataType.Text)]
            public string Nombre { get; set; }

           
            [Display(Name = "Nombre Completo")]
            [DataType(DataType.Text)]
            public string NombreCompleto { get; set; }

            
            [DataType(DataType.Text)]
            [Display(Name = "Teléfono Convencional")]
            public string Telefono { get; set; }

            [Phone]
            [Display(Name = "Celular")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Dirección")]
            [DataType(DataType.Text)]
            public string Direccion { get; set; }

        }

        private async Task LoadAsync(Usuario user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Cedula = user.Cedula,
                Nombre = user.Nombre,
                NombreCompleto = user.NombreCompleto,
                Telefono = user.Telefono,
                Direccion = user.Dirección
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
                    //StatusMessage = "Unexpected error when trying to set phone number.";
                    //return RedirectToPage();
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }
            if (Input.Cedula != user.Cedula)
            {
                user.Cedula = Input.Cedula;
            }
            if (Input.Nombre != user.Nombre)
            {
                user.Nombre = Input.Nombre;
            }

            if (Input.NombreCompleto != user.NombreCompleto)
            {
                user.NombreCompleto = Input.NombreCompleto;
            }

            if (Input.Telefono != user.Telefono)
            {
                user.Telefono = Input.Telefono;
            }

            if (Input.Direccion != user.Dirección)
            {
                user.Dirección = Input.Direccion;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Su perfil se ha actualizado correctamente!";
            return RedirectToPage();
        }
    }
}
