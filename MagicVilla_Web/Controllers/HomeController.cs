using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaService _villaService;
        public HomeController(IMapper mapper, IVillaService villaService)
        {
            _mapper = mapper;
            _villaService = villaService;

        }
        public async Task<IActionResult> Index()
        {
            List<VillaDto> list = new();
            var reponse = await _villaService.GetAllAsync<APIResponse>();
            if (reponse != null && reponse.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(reponse.Result));

            }
            return View(list);

        }

    }
}