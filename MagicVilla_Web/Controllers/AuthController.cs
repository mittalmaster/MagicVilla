using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService= authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO requestDTO = new();
            return View(requestDTO);

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            return View(model);

        }

        [HttpGet]
        public IActionResult Register() 
        {
            RegistrationRequestDTO model = new();
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO model)
        {
            APIResponse reg = await _authService.RegisterAsync<APIResponse>(model);
            if(reg!=null && reg.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            return View();

        }
        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AccessDenied() 
        { 
            return View();
        }
    }
}
