using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaService _villaService;
        public VillaController(IMapper mapper,IVillaService villaService) 
        { 
            _mapper = mapper;
            _villaService = villaService;
        
        }
        public async Task<IActionResult> IndexVilla()
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
